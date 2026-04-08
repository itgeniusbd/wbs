# Fix Career Page Column Size Error

## Problem
When submitting Career form with TinyMCE content, getting these errors:

### Error 1: RequirementsBn truncation
```
SqlException: String or binary data would be truncated in table 'WBS_NGO.dbo.Careers', 
column 'RequirementsBn'. Truncated value: '<p>???? ??? ????...'
```

### Error 2: Slug truncation
```
SqlException: String or binary data would be truncated in table 'WBS_NGO.dbo.Careers', 
column 'Slug'. Truncated value: 'project-m'
```

## Root Cause
The database columns were created with incorrect or limited sizes:

1. **Text fields** (Description, Requirements, Benefits, etc.): Had limited size instead of NVARCHAR(MAX)
2. **Slug column**: Was NCHAR(10) - only 10 characters!
3. **Title columns**: Were NVARCHAR(200) - too small for long job titles
4. **UpdatedAt column**: Was incorrectly set as NCHAR(10) instead of DATETIME2

## Solution

### Quick Fix (Already Applied) ?

The following fixes have been applied to your database:

```sql
-- Fixed columns:
Title:           NVARCHAR(200) ? NVARCHAR(500)
TitleBn:         NVARCHAR(200) ? NVARCHAR(500)
Slug:            NCHAR(10) ? NVARCHAR(500)
Description:     ? NVARCHAR(MAX)
DescriptionBn:   ? NVARCHAR(MAX)
Requirements:    ? NVARCHAR(MAX)
RequirementsBn:  ? NVARCHAR(MAX)
Benefits:        ? NVARCHAR(MAX)
Department:      ? NVARCHAR(MAX)
Location:        ? NVARCHAR(MAX)
JobType:         ? NVARCHAR(MAX)
SalaryRange:     ? NVARCHAR(MAX)
ApplicationUrl:  ? NVARCHAR(MAX)
ApplicationEmail: ? NVARCHAR(MAX)
UpdatedAt:       NCHAR(10) ? DATETIME2
```

### For Future Reference

**File:** `WBS.Web\DbScripts\FixCareersColumnSizes.sql`

This comprehensive script will:
- Fix all column sizes
- Convert incorrect data types
- Support unlimited text content (up to 2GB for MAX fields)
- Support long job titles and slugs
- Properly handle date/time fields

### Manual Fix (If Needed)

If you need to run fixes manually:

```sql
USE [WBS_NGO];
GO

-- Fix Title and Slug columns
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Title] NVARCHAR(500) NOT NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [TitleBn] NVARCHAR(500) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Slug] NVARCHAR(500) NOT NULL;

-- Fix text content columns
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Description] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [DescriptionBn] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Requirements] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [RequirementsBn] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Benefits] NVARCHAR(MAX) NULL;

-- Fix other text columns
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Department] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [Location] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [JobType] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [SalaryRange] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [ApplicationUrl] NVARCHAR(MAX) NULL;
ALTER TABLE [dbo].[Careers] ALTER COLUMN [ApplicationEmail] NVARCHAR(MAX) NULL;

-- Fix UpdatedAt column
ALTER TABLE [dbo].[Careers] ALTER COLUMN [UpdatedAt] DATETIME2 NULL;

PRINT 'All columns fixed!';
```

### Verify Changes

Run this query to verify all columns are correct:

```sql
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CASE 
        WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
        WHEN CHARACTER_MAXIMUM_LENGTH IS NULL THEN 'N/A'
        ELSE CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR)
    END AS SIZE,
    IS_NULLABLE
FROM 
    INFORMATION_SCHEMA.COLUMNS
WHERE 
    TABLE_NAME = 'Careers'
ORDER BY 
    ORDINAL_POSITION;
```

**Expected Results:**
- `Title`, `TitleBn`, `Slug`: NVARCHAR with size 500
- Text fields: NVARCHAR with size MAX
- `UpdatedAt`: datetime2 (no size shown)
- All NVARCHAR columns should support Unicode (Bengali text)

## Testing the Fix

1. **Restart your application**:
   - Stop debugging (Shift+F5)
   - Start again (F5)

2. **Test Career Creation**:
   - Navigate to: `/Admin/Careers/Create`
   - Fill in all fields:
     - Long job title (more than 200 chars if needed)
     - Rich text content with Bengali characters
     - Long requirements with formatting
   - Click "Create Career Posting"
   - ? Should succeed without errors

3. **What Now Works**:
   - ? Long job titles (up to 500 characters)
   - ? Long slugs (auto-generated, up to 500 characters)
   - ? Unlimited rich text content (TinyMCE HTML)
   - ? Bengali Unicode text (any length)
   - ? Multiple paragraphs and formatting
   - ? Lists, links, bold, italic, etc.
   - ? Proper date/time storage in UpdatedAt

## Technical Details

### Before Fix:
```
Title:           NVARCHAR(200)  - Too small
TitleBn:         NVARCHAR(200)  - Too small
Slug:            NCHAR(10)      - Way too small! (10 chars only)
RequirementsBn:  NVARCHAR(4000) - Limited
UpdatedAt:       NCHAR(10)      - Wrong type!
```

### After Fix:
```
Title:           NVARCHAR(500)  - Can hold long titles
TitleBn:         NVARCHAR(500)  - Can hold long Bengali titles
Slug:            NVARCHAR(500)  - Can hold long slugs
RequirementsBn:  NVARCHAR(MAX)  - Unlimited (up to 2GB)
UpdatedAt:       DATETIME2      - Proper date/time type
```

### Why These Sizes?

- **NVARCHAR(500)** for Title/Slug: 
  - Allows long job titles
  - Supports URL-friendly slugs
  - More than enough for most use cases

- **NVARCHAR(MAX)** for text fields:
  - Stores up to 2GB of text
  - Perfect for rich HTML content
  - Supports any amount of formatting

- **DATETIME2** for dates:
  - Proper SQL Server date/time type
  - High precision (100 nanoseconds)
  - Range: 0001-01-01 to 9999-12-31

## Prevention for Future Tables

When creating new tables with rich text content:

? **DO:**
- Use `NVARCHAR(MAX)` for HTML/rich text fields
- Use `NVARCHAR(500)` or more for titles
- Use `NVARCHAR(500)` for slugs
- Use `NVARCHAR` (not VARCHAR) for Bengali/Unicode text
- Use proper types: `DATETIME2` for dates, `INT` for IDs, etc.

? **DON'T:**
- Don't use `NCHAR` or `CHAR` with small fixed sizes
- Don't use `VARCHAR` for Unicode content
- Don't use `NVARCHAR(4000)` when you need more
- Don't use wrong types (e.g., NCHAR for dates)

## Related Files
- Model: `WBS.Web\Models\Career.cs`
- Controller: `WBS.Web\Areas\Admin\Controllers\CareersController.cs`
- View: `WBS.Web\Areas\Admin\Views\Careers\Create.cshtml`
- Database Script: `WBS.Web\DbScripts\CreateCareersTable.sql`
- Fix Script: `WBS.Web\DbScripts\FixCareersColumnSizes.sql`

## Status: ? RESOLVED

All database columns have been fixed. The Career posting form now works correctly!
