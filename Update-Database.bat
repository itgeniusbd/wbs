@echo off
REM ===================================================================
REM WBS Database Update - Simple Batch Script
REM Double-click this file to update your database
REM ===================================================================

echo =====================================
echo WBS Database Update
echo =====================================
echo.

REM Check if we're in the right directory
if exist "WBS.Web\WBS.Web.csproj" (
    echo [OK] Project found!
    cd WBS.Web
) else if exist "WBS.Web.csproj" (
    echo [OK] Already in project directory!
) else (
    echo [ERROR] WBS.Web.csproj not found!
    echo Please run this file from the WBS root directory (F:\WBS)
    pause
    exit /b 1
)

echo.
echo Step 1: Installing/Checking EF Core Tools...
echo =====================================
dotnet tool install --global dotnet-ef 2>nul
if errorlevel 1 (
    echo EF Tools already installed or update available
) else (
    echo EF Tools installed successfully!
)

echo.
echo Step 2: Restoring Tools...
echo =====================================
dotnet tool restore

echo.
echo Step 3: Building Project...
echo =====================================
dotnet build
if errorlevel 1 (
    echo [ERROR] Build failed!
    pause
    exit /b 1
)

echo.
echo Step 4: Updating Database...
echo =====================================
dotnet ef database update

if errorlevel 1 (
    echo.
    echo =====================================
    echo [ERROR] Database Update Failed!
    echo =====================================
    echo.
    echo Try these solutions:
    echo 1. Make sure SQL Server is running
    echo 2. Check your connection string
    echo 3. Run as Administrator
    echo 4. Use DATABASE_UPDATE_SCRIPT.sql instead
    echo.
) else (
    echo.
    echo =====================================
    echo [SUCCESS] Database Updated!
    echo =====================================
    echo.
    echo New features available:
    echo - Organization information
    echo - Registration certificate display
    echo - Refund policy with timeframe
    echo - Payment gateway banner
    echo.
    echo Next: Go to Admin -^> Settings to configure
    echo.
)

pause
