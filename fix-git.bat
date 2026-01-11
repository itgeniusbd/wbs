@echo off
echo ====================================
echo Git Permission Issue Fix
echo ====================================
echo.

echo Step 1: Removing .vs folder from Git...
git rm -r --cached .vs/ 2>nul

echo Step 2: Removing bin/obj folders from Git...
git rm -r --cached WBS.Web/bin/ 2>nul
git rm -r --cached WBS.Web/obj/ 2>nul

echo Step 3: Adding .gitignore...
git add .gitignore

echo Step 4: Adding all other changes...
git add *.cs *.cshtml *.csproj *.json

echo Step 5: Checking status...
git status

echo.
echo ====================================
echo Ready to commit!
echo ====================================
echo Run: git commit -m "Your commit message"
echo.
pause
