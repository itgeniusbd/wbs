@echo off
echo ========================================
echo WBS Project - Git Fix Script
echo ========================================
echo.

REM Check if Visual Studio is running
tasklist /FI "IMAGENAME eq devenv.exe" 2>NUL | find /I /N "devenv.exe">NUL
if "%ERRORLEVEL%"=="0" (
    echo ERROR: Visual Studio is running!
    echo Please close Visual Studio first and run this script again.
    echo.
    pause
    exit /b 1
)

echo Step 1: Configuring Git...
git config core.longpaths true
echo OK
echo.

echo Step 2: Adding .gitignore...
git add .gitignore
echo OK
echo.

echo Step 3: Removing .vs from git tracking...
git rm -r --cached .vs/ 2>nul
echo OK (or already removed)
echo.

echo Step 4: Removing bin/obj from git tracking...
git rm -r --cached WBS.Web/bin/ 2>nul
git rm -r --cached WBS.Web/obj/ 2>nul
echo OK (or already removed)
echo.

echo Step 5: Adding your code files...
git add *.cs 2>nul
git add *.cshtml 2>nul
git add *.csproj 2>nul
git add *.json 2>nul
git add fix-*.ps1 2>nul
git add quick-fix.ps1 2>nul
echo OK
echo.

echo Step 6: Current status...
echo ========================================
git status --short
echo ========================================
echo.

echo Ready to commit!
echo.
set /p commit_msg="Enter commit message (or press Enter for default): "

if "%commit_msg%"=="" (
    set commit_msg=Fix: Updated Activity views and gitignore
)

echo.
echo Committing with message: %commit_msg%
git commit -m "%commit_msg%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo SUCCESS! Changes committed!
    echo ========================================
    echo.
    echo To push to GitHub, run:
    echo    git push origin main
    echo.
) else (
    echo.
    echo ========================================
    echo ERROR: Commit failed!
    echo ========================================
    echo Check the error messages above.
    echo.
)

pause
