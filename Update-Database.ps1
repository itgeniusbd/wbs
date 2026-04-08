# ===================================================================
# WBS Database Update Script
# This script will update your database with new migrations
# ===================================================================

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "WBS Database Update Script" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Get current directory
$currentDir = Get-Location
Write-Host "Current Directory: $currentDir" -ForegroundColor Yellow

# Check if we're in the right directory
if (Test-Path ".\WBS.Web\WBS.Web.csproj") {
    Write-Host "? Project file found!" -ForegroundColor Green
    Set-Location ".\WBS.Web"
}
elseif (Test-Path ".\WBS.Web.csproj") {
    Write-Host "? Already in project directory!" -ForegroundColor Green
}
else {
    Write-Host "? Error: WBS.Web.csproj not found!" -ForegroundColor Red
    Write-Host "Please run this script from the WBS root directory (F:\WBS)" -ForegroundColor Yellow
    pause
    exit
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 1: Checking EF Core Tools" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

# Check if dotnet-ef is installed
$efInstalled = dotnet tool list --global | Select-String "dotnet-ef"

if ($efInstalled) {
    Write-Host "? EF Core Tools already installed" -ForegroundColor Green
} else {
    Write-Host "Installing EF Core Tools..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? EF Core Tools installed successfully!" -ForegroundColor Green
    } else {
        Write-Host "? Failed to install EF Core Tools" -ForegroundColor Red
        pause
        exit
    }
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 2: Restoring Tools" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

dotnet tool restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "? Tools restored successfully!" -ForegroundColor Green
} else {
    Write-Host "? Tool restore had warnings (this is usually okay)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 3: Building Project" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "? Project built successfully!" -ForegroundColor Green
} else {
    Write-Host "? Build failed! Please fix build errors first." -ForegroundColor Red
    pause
    exit
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Step 4: Updating Database" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

Write-Host "Running migration..." -ForegroundColor Yellow
dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Green
    Write-Host "??? SUCCESS! ???" -ForegroundColor Green
    Write-Host "=====================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Database updated successfully!" -ForegroundColor Green
    Write-Host "New columns added to SiteSettings table:" -ForegroundColor Cyan
    Write-Host "  - OrganizationFullName" -ForegroundColor White
    Write-Host "  - OrganizationFullNameBn" -ForegroundColor White
    Write-Host "  - RegistrationNumber" -ForegroundColor White
    Write-Host "  - RegistrationType" -ForegroundColor White
    Write-Host "  - EstablishedYear" -ForegroundColor White
    Write-Host "  - OrganizationType" -ForegroundColor White
    Write-Host "  - OrganizationTypeBn" -ForegroundColor White
    Write-Host "  - ManagementInfo" -ForegroundColor White
    Write-Host "  - ManagementInfoBn" -ForegroundColor White
    Write-Host "  - RefundPolicyTimeframe" -ForegroundColor White
    Write-Host "  - RefundPolicyTimeframeBn" -ForegroundColor White
    Write-Host "  - PaymentGatewayBanner" -ForegroundColor White
    Write-Host ""
    Write-Host "Next Steps:" -ForegroundColor Cyan
    Write-Host "1. Run your application" -ForegroundColor Yellow
    Write-Host "2. Go to Admin Panel ? Settings" -ForegroundColor Yellow
    Write-Host "3. Fill in the Organization and Policies tabs" -ForegroundColor Yellow
    Write-Host "4. Upload Payment Gateway Banner" -ForegroundColor Yellow
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Red
    Write-Host "? FAILED!" -ForegroundColor Red
    Write-Host "=====================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Database update failed!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Common solutions:" -ForegroundColor Yellow
    Write-Host "1. Check if SQL Server is running" -ForegroundColor White
    Write-Host "2. Verify connection string in appsettings.json" -ForegroundColor White
    Write-Host "3. Make sure you have permission to modify the database" -ForegroundColor White
    Write-Host "4. Try running as Administrator" -ForegroundColor White
    Write-Host ""
    Write-Host "Alternative: Use the SQL script instead" -ForegroundColor Cyan
    Write-Host "Run the 'DATABASE_UPDATE_SCRIPT.sql' file in SSMS" -ForegroundColor Cyan
    Write-Host ""
}

# Return to original directory
Set-Location $currentDir

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
