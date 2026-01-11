# Featured Programs - Database Migration Guide

## Overview
This update adds the `IsFeatured` column to the `SDGPrograms` table, allowing programs to be featured on the home page.

## Changes Made

### 1. Database Changes
- Added `IsFeatured` (BIT) column to `SDGPrograms` table
- Default value: `false` (0)

### 2. Code Changes
- **Model**: Added `IsFeatured` property to `SDGProgram.cs`
- **Service**: Updated `ContentService.GetFeaturedProgramsAsync()` to filter by `IsFeatured = true`
- **Views**: 
  - Added IsFeatured checkbox in Create/Edit forms
  - Display Featured badge in Index list

## How to Apply Database Changes

### Option 1: Using SQL Script (Recommended for Production)
Run the SQL script located at:
```
WBS.Web\DbScripts\AddIsFeaturedToSDGPrograms.sql
```

You can execute this script:
1. Open SQL Server Management Studio (SSMS)
2. Connect to your database
3. Open the script file
4. Execute the script

### Option 2: Using Entity Framework Migrations (Development)
```bash
# Create migration
dotnet ef migrations add AddIsFeaturedToSDGPrograms --project WBS.Web

# Update database
dotnet ef database update --project WBS.Web
```

## Testing

### 1. Mark Programs as Featured
After applying the database changes, go to:
- **Admin Panel** → **SDG Programs** → **Edit any program**
- Check the **"Show on Home Page"** checkbox
- Save the program

### 2. Verify on Home Page
- Visit the home page
- The "Featured Programs" (প্রধান প্রোগ্রাম) section should now display the programs you marked as featured
- Up to 6 programs will be shown

## Troubleshooting

### Home page shows no programs
**Solution**: Make sure at least one program has:
- `IsActive = true`
- `IsFeatured = true`

### SQL Script Error
If you get an error that the column already exists, it's safe to ignore - it means the migration was already applied.

## Rollback (If Needed)
To remove the IsFeatured column:
```sql
ALTER TABLE [dbo].[SDGPrograms]
DROP COLUMN [IsFeatured];
```

## Notes
- The featured programs section replaces the "Ways to Give" section on the home page
- Programs are sorted by `DisplayOrder`
- Only active and featured programs are shown
- Bilingual support (English/বাংলা) is maintained
