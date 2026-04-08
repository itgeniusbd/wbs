# ?? Production Deployment Guide - WBS Bangladesh

## ?? Table of Contents
1. [Pre-Deployment Checklist](#pre-deployment-checklist)
2. [Database Migration](#database-migration)
3. [Application Deployment](#application-deployment)
4. [Post-Deployment Verification](#post-deployment-verification)
5. [Rollback Procedure](#rollback-procedure)

---

## ? Pre-Deployment Checklist

### 1. Backup Current System
- [ ] Backup production database
- [ ] Backup application files
- [ ] Document current system state
- [ ] Backup IIS/Server configuration

### 2. Test in Staging
- [ ] Deploy to staging environment
- [ ] Run migration script in staging database
- [ ] Test all functionality
- [ ] Verify SSLCommerz integration

### 3. Prepare Production Environment
- [ ] Verify server requirements (.NET 8 Runtime)
- [ ] Verify SQL Server access
- [ ] Prepare SSL certificate
- [ ] Configure firewall rules

---

## ??? Database Migration

### Step 1: Create Manual Backup

```sql
-- In SQL Server Management Studio (SSMS)
USE master;
GO

BACKUP DATABASE WBS_NGO 
TO DISK = 'D:\Backups\WBS_NGO_PreSSLCommerz_20250125.bak'
WITH FORMAT, 
     NAME = 'WBS Pre-SSLCommerz Full Backup',
     COMPRESSION;
GO

-- Verify backup
RESTORE VERIFYONLY 
FROM DISK = 'D:\Backups\WBS_NGO_PreSSLCommerz_20250125.bak';
GO
```

### Step 2: Run Migration Script

**Option A: Using SSMS**

1. Open SQL Server Management Studio
2. Connect to your production server
3. Open file: `WBS.Web/Database/Migration_20250125_SSLCommerz.sql`
4. Review the script carefully
5. Execute the script
6. Review output for any errors

**Option B: Using sqlcmd**

```bash
# From command prompt
sqlcmd -S YOUR_SERVER_NAME -d WBS_NGO -E -i "Migration_20250125_SSLCommerz.sql" -o "migration_output.log"

# Review the log
type migration_output.log
```

### Step 3: Verify Migration

```sql
-- Check new columns exist
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Donations'
AND COLUMN_NAME IN ('TransactionId', 'BankTransactionId', 'CardType', 'PaidAt', 'Currency');
GO

-- Check data integrity
SELECT 
    COUNT(*) AS TotalDonations,
    COUNT(TransactionId) AS WithTransactionId,
    COUNT(CASE WHEN Status = 1 THEN 1 END) AS CompletedDonations
FROM Donations;
GO

-- Check backup tables exist
SELECT name, create_date 
FROM sys.tables 
WHERE name LIKE '%Backup%';
GO
```

### Expected Results:

```
? All 5 new columns should exist
? All donations should have TransactionId
? Backup tables should exist with today's date
? No data loss (compare counts with backup)
```

---

## ?? Application Deployment

### Method 1: Visual Studio Publish (Recommended)

#### Step 1: Configure Publish Profile

1. Right-click `WBS.Web` project ? **Publish**
2. Click **New** ? Choose deployment target:
   - **Azure App Service** (for cloud)
   - **IIS** (for on-premise server)
   - **Folder** (for manual deployment)

#### Step 2: Configure Settings

**For IIS:**
```xml
<!-- PublishProfiles/Production.pubxml -->
<Project>
  <PropertyGroup>
    <WebPublishMethod>MSDeploy</WebPublishMethod>
    <PublishProvider>MSDeploy</PublishProvider>
    <SiteUrlToLaunchAfterPublish>https://wbs-bd.org</SiteUrlToLaunchAfterPublish>
    <MSDeployServiceURL>YOUR_SERVER</MSDeployServiceURL>
    <DeployIisAppPath>WBS-BD</DeployIisAppPath>
    <TargetFramework>net8.0</TargetFramework>
    <EnvironmentName>Production</EnvironmentName>
  </PropertyGroup>
</Project>
```

#### Step 3: Update Production Configuration

Create `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_PRODUCTION_SERVER;Database=WBS_NGO;User Id=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;"
  },
  "SSLCommerz": {
    "StoreId": "wbsbdorg0live",
    "StorePassword": "6974AA318444C52997",
    "IsLive": true,
    "SessionUrl": "https://securepay.sslcommerz.com/gwprocess/v4/api.php",
    "ValidationUrl": "https://securepay.sslcommerz.com/validator/api/validationserverAPI.php",
    "Currency": "BDT"
  },
  "Cloudinary": {
    "CloudName": "YOUR_CLOUD_NAME",
    "ApiKey": "YOUR_API_KEY",
    "ApiSecret": "YOUR_API_SECRET"
  },
  "GreenwebSms": {
    "Enabled": true,
    "ApiToken": "YOUR_SMS_TOKEN"
  },
  "EmailSettings": {
    "Enabled": true,
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "FromEmail": "noreply@wbs-bd.org",
    "FromName": "WBS Bangladesh",
    "Username": "YOUR_EMAIL",
    "Password": "YOUR_APP_PASSWORD",
    "EnableSsl": true
  }
}
```

?? **IMPORTANT**: Never commit `appsettings.Production.json` to Git!

Add to `.gitignore`:
```
appsettings.Production.json
appsettings.*.local.json
```

#### Step 4: Publish

```bash
# In Package Manager Console or Terminal
dotnet publish -c Release -o ./publish

# Or click "Publish" button in Visual Studio
```

### Method 2: Manual IIS Deployment

#### Step 1: Prepare Server

```powershell
# Install .NET 8 Hosting Bundle
# Download from: https://dotnet.microsoft.com/download/dotnet/8.0

# After installation, restart IIS
iisreset

# Verify installation
dotnet --list-runtimes
```

#### Step 2: Configure IIS

```powershell
# Create Application Pool
New-WebAppPool -Name "WBS-Bangladesh" -Force

# Configure Application Pool
Set-ItemProperty IIS:\AppPools\WBS-Bangladesh -Name managedRuntimeVersion -Value ""
Set-ItemProperty IIS:\AppPools\WBS-Bangladesh -Name processModel.identityType -Value 4

# Create Website
New-Website -Name "WBS-Bangladesh" `
            -PhysicalPath "C:\inetpub\wwwroot\WBS" `
            -ApplicationPool "WBS-Bangladesh" `
            -Port 443 `
            -Ssl `
            -Force

# Bind SSL Certificate
# (Do this via IIS Manager or use certutil)
```

#### Step 3: Copy Files

```powershell
# Stop site
Stop-Website "WBS-Bangladesh"

# Backup current files
Copy-Item "C:\inetpub\wwwroot\WBS" "C:\Backups\WBS_$(Get-Date -Format 'yyyyMMdd')" -Recurse

# Copy new files
Copy-Item ".\publish\*" "C:\inetpub\wwwroot\WBS\" -Recurse -Force

# Set permissions
icacls "C:\inetpub\wwwroot\WBS" /grant "IIS_IUSRS:(OI)(CI)F" /T

# Start site
Start-Website "WBS-Bangladesh"
```

### Method 3: Azure App Service

#### Using Azure CLI:

```bash
# Login to Azure
az login

# Create Resource Group (if not exists)
az group create --name WBS-RG --location "Southeast Asia"

# Create App Service Plan
az appservice plan create --name WBS-Plan --resource-group WBS-RG --sku B1 --is-linux

# Create Web App
az webapp create --name wbs-bangladesh --resource-group WBS-RG --plan WBS-Plan --runtime "DOTNET|8.0"

# Configure connection string
az webapp config connection-string set \
  --name wbs-bangladesh \
  --resource-group WBS-RG \
  --connection-string-type SQLAzure \
  --settings DefaultConnection="YOUR_CONNECTION_STRING"

# Configure app settings
az webapp config appsettings set \
  --name wbs-bangladesh \
  --resource-group WBS-RG \
  --settings SSLCommerz__StoreId="wbsbdorg0live" \
             SSLCommerz__StorePassword="6974AA318444C52997" \
             SSLCommerz__IsLive="true"

# Deploy from local Git
az webapp deployment source config-local-git \
  --name wbs-bangladesh \
  --resource-group WBS-RG

# Get deployment URL
az webapp deployment source show \
  --name wbs-bangladesh \
  --resource-group WBS-RG

# Push code
git remote add azure <DEPLOYMENT_URL>
git push azure main:master

# Or deploy ZIP file
az webapp deployment source config-zip \
  --resource-group WBS-RG \
  --name wbs-bangladesh \
  --src ./publish.zip
```

---

## ? Post-Deployment Verification

### 1. Health Checks

```bash
# Check if application is running
curl https://wbs-bd.org/
curl https://wbs-bd.org/Health

# Check database connectivity
curl https://wbs-bd.org/Health/Database
```

### 2. Database Verification

```sql
-- Check recent migrations
SELECT TOP 10 * 
FROM Donations 
ORDER BY CreatedAt DESC;

-- Verify indexes
SELECT 
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName,
    i.type_desc AS IndexType
FROM sys.indexes i
WHERE OBJECT_NAME(i.object_id) = 'Donations'
AND i.name IS NOT NULL;

-- Check stored procedures
SELECT name, create_date, modify_date 
FROM sys.procedures 
WHERE name LIKE 'sp_%';
```

### 3. Functional Testing

**Test Checklist:**

- [ ] Homepage loads correctly
- [ ] Donation form accessible
- [ ] Sandbox payment works
- [ ] Live payment test (small amount)
- [ ] Payment success callback
- [ ] Thank you page displays
- [ ] Database records created
- [ ] Account balance updated
- [ ] Admin panel accessible
- [ ] Reports display correctly

### 4. SSLCommerz Configuration

**Update Callback URLs in SSLCommerz Merchant Panel:**

1. Login: https://merchant.sslcommerz.com/
2. Go to: **Menu ? My Store ? IPN Settings**
3. Set IPN URL: `https://wbs-bd.org/Donation/PaymentIPN`
4. Save changes

**Verify Integration:**

```bash
# Test payment initiation
curl -X POST https://wbs-bd.org/Donation/Index \
  -H "Content-Type: application/json" \
  -d '{"Amount": 10, "PaymentMethod": "Online", ...}'
```

### 5. Monitor Logs

**In Azure:**
```bash
# Stream logs
az webapp log tail --name wbs-bangladesh --resource-group WBS-RG

# Download logs
az webapp log download --name wbs-bangladesh --resource-group WBS-RG
```

**In IIS:**
```powershell
# Check Event Viewer
Get-EventLog -LogName Application -Source "ASP.NET Core*" -Newest 50

# Check IIS logs
Get-Content "C:\inetpub\logs\LogFiles\W3SVC1\*.log" -Tail 50
```

---

## ?? Rollback Procedure

### If Migration Fails:

#### Option 1: Rollback Script

```sql
-- Run rollback script
-- Edit script and set @Confirm = 'YES'
-- Execute: WBS.Web/Database/Rollback_20250125_SSLCommerz.sql
```

#### Option 2: Restore from Backup

```sql
-- Restore database backup
USE master;
GO

ALTER DATABASE WBS_NGO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE WBS_NGO 
FROM DISK = 'D:\Backups\WBS_NGO_PreSSLCommerz_20250125.bak'
WITH REPLACE, RECOVERY;
GO

ALTER DATABASE WBS_NGO SET MULTI_USER;
GO
```

### If Application Deployment Fails:

#### For IIS:

```powershell
# Stop website
Stop-Website "WBS-Bangladesh"

# Delete new files
Remove-Item "C:\inetpub\wwwroot\WBS\*" -Recurse -Force

# Restore backup
Copy-Item "C:\Backups\WBS_20250125\*" "C:\inetpub\wwwroot\WBS\" -Recurse -Force

# Start website
Start-Website "WBS-Bangladesh"
```

#### For Azure:

```bash
# Swap deployment slots (if using slots)
az webapp deployment slot swap \
  --name wbs-bangladesh \
  --resource-group WBS-RG \
  --slot staging

# Or redeploy previous version
git reset --hard HEAD~1
git push azure main:master --force
```

---

## ?? Success Criteria

After deployment, verify:

? **Database**
- Migration script executed without errors
- All new columns exist
- Data integrity maintained
- Backup tables created

? **Application**
- Application running on production URL
- HTTPS enabled
- All pages accessible
- No errors in logs

? **SSLCommerz**
- Test transaction successful
- Callback URLs working
- Payment status updates correctly
- Transaction logs created

? **Performance**
- Response time < 2 seconds
- Database queries optimized
- No memory leaks
- CPU usage normal

---

## ?? Support Contacts

**In Case of Emergency:**

### SSLCommerz Support:
- **Email**: integration@sslcommerz.com
- **Phone**: +88096122 26969
- **Merchant Panel**: https://merchant.sslcommerz.com/

### Database Issues:
- Review migration output log
- Check backup tables exist
- Contact DBA if needed

### Application Issues:
- Check application logs
- Verify appsettings.Production.json
- Review IIS/Azure configuration

---

## ?? Post-Deployment Tasks

After successful deployment:

1. [ ] Update documentation
2. [ ] Notify stakeholders
3. [ ] Monitor for 24 hours
4. [ ] Schedule data cleanup (optional):
   ```sql
   -- After 7 days, if everything is stable
   DROP TABLE Donations_Backup_20250125;
   DROP TABLE Accounts_Backup_20250125;
   ```
5. [ ] Update disaster recovery plan
6. [ ] Train users on new features

---

## ?? Congratulations!

If you've reached this point successfully, your WBS donation system with SSLCommerz integration is now live in production! ??

**Next Steps:**
- Monitor first real donations
- Gather user feedback
- Plan next features
- Celebrate success! ??

---

**Document Version**: 1.0.0  
**Last Updated**: January 25, 2025  
**Deployment Date**: _____________  
**Deployed By**: _____________  
**Verified By**: _____________
