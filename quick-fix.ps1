# Quick Git Fix - Just ignore .vs and commit
Write-Host "Quick Fix: Ignoring .vs folder and committing..." -ForegroundColor Green

# Just add everything except .vs
git add -A
git reset .vs/* 2>$null

# Show what will be committed
Write-Host "`nFiles to be committed:" -ForegroundColor Yellow
git status --short

# Commit
Write-Host "`nCommitting..." -ForegroundColor Yellow
git commit -m "Fix: Updated Activity views and gitignore"

Write-Host "`nDone! To push: git push origin main" -ForegroundColor Green
