# Fix Git Permission Issues
Write-Host "Fixing Git repository..." -ForegroundColor Green

# Remove .vs folder from Git tracking
Write-Host "Removing .vs folder from Git tracking..." -ForegroundColor Yellow
git rm -r --cached .vs/ 2>$null

# Remove bin and obj folders from Git tracking
Write-Host "Removing bin and obj folders from Git tracking..." -ForegroundColor Yellow
git rm -r --cached WBS.Web/bin/ 2>$null
git rm -r --cached WBS.Web/obj/ 2>$null

# Add .gitignore
Write-Host "Adding .gitignore..." -ForegroundColor Yellow
git add .gitignore

# Commit changes
Write-Host "Committing changes..." -ForegroundColor Yellow
git commit -m "Fix: Remove temporary files and update .gitignore"

Write-Host "Done! You can now commit your other changes." -ForegroundColor Green
Write-Host "Note: If Visual Studio is running, close it first." -ForegroundColor Cyan
