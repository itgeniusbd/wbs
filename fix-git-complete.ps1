# Git Fix Script for WBS Project
# This script fixes the git commit issue by handling locked files

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Git Repository Fix Script" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Step 1: Check if Visual Studio is running
Write-Host "[Step 1] Checking for running processes..." -ForegroundColor Yellow
$vsProcesses = Get-Process | Where-Object { $_.ProcessName -like "*devenv*" }
if ($vsProcesses) {
    Write-Host "WARNING: Visual Studio is running. Please close it first!" -ForegroundColor Red
    Write-Host "Found processes: $($vsProcesses.ProcessName -join ', ')" -ForegroundColor Red
    Write-Host "`nPress any key to exit..." -ForegroundColor Yellow
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}
Write-Host "OK: No Visual Studio processes found`n" -ForegroundColor Green

# Step 2: Configure Git for long paths
Write-Host "[Step 2] Configuring Git for long paths..." -ForegroundColor Yellow
try {
    git config core.longpaths true
    Write-Host "OK: Git configured for long paths`n" -ForegroundColor Green
} catch {
    Write-Host "Warning: Could not configure long paths`n" -ForegroundColor Yellow
}

# Step 3: Add .gitignore first
Write-Host "[Step 3] Adding .gitignore..." -ForegroundColor Yellow
try {
    git add .gitignore
    Write-Host "OK: .gitignore added`n" -ForegroundColor Green
} catch {
    Write-Host "Error adding .gitignore: $_`n" -ForegroundColor Red
}

# Step 4: Remove .vs folder from git tracking (if it exists)
Write-Host "[Step 4] Removing .vs folder from Git tracking..." -ForegroundColor Yellow
if (Test-Path ".vs") {
    try {
        # Try to remove from git cache
        git rm -r --cached .vs/ 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "OK: .vs removed from Git tracking`n" -ForegroundColor Green
        } else {
            Write-Host "Info: .vs not in Git tracking (this is good)`n" -ForegroundColor Cyan
        }
    } catch {
        Write-Host "Info: .vs folder handling complete`n" -ForegroundColor Cyan
    }
}

# Step 5: Remove bin and obj folders
Write-Host "[Step 5] Removing bin/obj folders from Git tracking..." -ForegroundColor Yellow
try {
    git rm -r --cached WBS.Web/bin/ 2>$null
    git rm -r --cached WBS.Web/obj/ 2>$null
    Write-Host "OK: Build folders removed from tracking`n" -ForegroundColor Green
} catch {
    Write-Host "Info: Build folders not tracked`n" -ForegroundColor Cyan
}

# Step 6: Add your actual code changes
Write-Host "[Step 6] Adding your code changes..." -ForegroundColor Yellow
try {
    git add *.cs 2>$null
    git add *.cshtml 2>$null
    git add *.csproj 2>$null
    git add *.json 2>$null
    git add *.ps1 2>$null
    git add *.bat 2>$null
    Write-Host "OK: Code files staged for commit`n" -ForegroundColor Green
} catch {
    Write-Host "Warning: Some files could not be added`n" -ForegroundColor Yellow
}

# Step 7: Show status
Write-Host "[Step 7] Current Git status:" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Gray
git status --short
Write-Host "----------------------------------------`n" -ForegroundColor Gray

# Step 8: Ready to commit
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "READY TO COMMIT!" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Now you can commit with:" -ForegroundColor Yellow
Write-Host '  git commit -m "Fix: Updated views and gitignore"' -ForegroundColor White
Write-Host "`nOr run this command now? (Y/N): " -ForegroundColor Yellow -NoNewline

$response = Read-Host
if ($response -eq 'Y' -or $response -eq 'y') {
    Write-Host "`nCommitting changes..." -ForegroundColor Yellow
    git commit -m "Fix: Updated views, removed temp files, and updated gitignore"
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n? SUCCESS! Changes committed!" -ForegroundColor Green
        Write-Host "`nTo push to GitHub, run: git push origin main" -ForegroundColor Cyan
    } else {
        Write-Host "`n? Commit failed. Check the error above." -ForegroundColor Red
    }
} else {
    Write-Host "`nOK. You can commit manually when ready." -ForegroundColor Cyan
}

Write-Host "`nPress any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
