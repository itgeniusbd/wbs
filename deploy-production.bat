@echo off
echo ==========================================
echo WBS Production Deployment Helper Script
echo ==========================================
echo.

echo Step 1: Backing up current appsettings.Production.json...
if exist "WBS.Web\appsettings.Production.json" (
    copy "WBS.Web\appsettings.Production.json" "WBS.Web\appsettings.Production.json.backup"
    echo Backup created: appsettings.Production.json.backup
) else (
    echo No existing appsettings.Production.json found
)
echo.

echo Step 2: Checking database connection...
echo IMPORTANT: Please update the ConnectionString in appsettings.Production.json
echo Current connection string is pointing to local server (DESKTOP-3UN61QI)
echo.
echo You need to update it with your production server details:
echo   - Server name
echo   - Database name
echo   - Username
echo   - Password
echo.
pause

echo Step 3: Building the project...
dotnet clean
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo Build failed! Please fix the errors and try again.
    pause
    exit /b %errorlevel%
)
echo Build successful!
echo.

echo Step 4: Running database migrations...
echo.
echo Make sure your production database connection string is correct!
echo Press Ctrl+C to cancel or any key to continue...
pause
dotnet ef database update --project WBS.Web --configuration Release
if %errorlevel% neq 0 (
    echo Migration failed! Please check your connection string.
    pause
    exit /b %errorlevel%
)
echo Migration successful!
echo.

echo Step 5: Publishing the application...
dotnet publish WBS.Web -c Release -o ./publish
if %errorlevel% neq 0 (
    echo Publish failed!
    pause
    exit /b %errorlevel%
)
echo Publish successful!
echo.

echo Step 6: Deployment checklist...
echo.
echo [ ] 1. Update appsettings.Production.json with correct database connection
echo [ ] 2. Verify database migrations are applied
echo [ ] 3. Test email/SMS settings
echo [ ] 4. Copy 'publish' folder to production server
echo [ ] 5. Configure IIS or web server
echo [ ] 6. Test donation submission
echo [ ] 7. Monitor logs for any errors
echo.
echo Published files are in the 'publish' folder
echo.

echo Deployment preparation complete!
echo Please follow the checklist above and refer to PRODUCTION_DEPLOYMENT_GUIDE.md for details.
pause
