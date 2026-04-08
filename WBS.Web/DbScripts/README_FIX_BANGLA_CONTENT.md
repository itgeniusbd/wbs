# Fix for Bangla Content Displaying as Question Marks (???????)

## Problem
Bangla text in Privacy Policy and Terms & Conditions pages is displaying as question marks (?????????) instead of actual Bangla characters.

## Root Cause
The issue is caused by **character encoding problem**. When inserting Unicode characters (like Bangla text) into SQL Server, you must use:
- **NVARCHAR** data type (not VARCHAR)
- **N prefix** before string literals (N'?????')

## Solution
Run the fixed SQL script that uses proper Unicode support.

## How to Fix

### Step 1: Run the Fix Script

#### Option 1: Using SQL Server Management Studio (SSMS)
1. Open SQL Server Management Studio
2. Connect to your database server
3. Open: `WBS.Web\DbScripts\FixBanglaContentInPages.sql`
4. Make sure you're connected to **WBS_DB**
5. Click "Execute" (F5)

#### Option 2: Using Azure Data Studio
1. Open Azure Data Studio
2. Connect to your database
3. Open: `WBS.Web\DbScripts\FixBanglaContentInPages.sql`
4. Click "Run" (F5)

#### Option 3: Command Line
```bash
sqlcmd -S your_server_name -d WBS_DB -i "WBS.Web\DbScripts\FixBanglaContentInPages.sql"
```

### Step 2: Verify the Fix
1. Go to: `http://localhost:5001/page/privacy-policy`
2. Click "???" (Bangla) button in the navigation
3. You should see proper Bangla text (not ????????)
4. Test Terms & Conditions page too: `http://localhost:5001/page/terms-conditions`

## What the Fix Script Does

1. **Deletes** existing pages with encoding issues
2. **Re-inserts** pages with proper Unicode support:
   - Uses `N` prefix before all Bangla strings
   - Example: `N'????????? ????'` instead of `'????????? ????'`
3. **Verifies** the content is stored correctly

## Key Differences from Previous Script

### Previous Script (WRONG):
```sql
INSERT INTO Pages (Title, TitleBn, ...)
VALUES (
    'Privacy Policy',
    '????????? ????',  -- ? NO N prefix
    ...
```

### Fixed Script (CORRECT):
```sql
INSERT INTO Pages (Title, TitleBn, ...)
VALUES (
    N'Privacy Policy',
    N'????????? ????',  -- ? WITH N prefix
    ...
```

## Database Column Requirements

For storing Unicode text (Bangla, Arabic, Chinese, etc.), columns must be:
- **NVARCHAR** (not VARCHAR)
- **NTEXT** (not TEXT) for large text

Check your Pages table columns:
```sql
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Pages'
    AND COLUMN_NAME IN ('TitleBn', 'ContentBn');
```

Should show:
- `TitleBn` ? `nvarchar(200)`
- `ContentBn` ? `nvarchar(max)` or `ntext`

## Testing Checklist

After running the fix script:

- [ ] Privacy Policy page loads without errors
- [ ] English content displays correctly
- [ ] Bangla content displays correctly (not ????????)
- [ ] Language switching works properly
- [ ] Terms & Conditions page works the same way
- [ ] Footer links work

## If Still Showing Question Marks

### Check 1: Verify Database Content
Run this query:
```sql
SELECT TOP 1 TitleBn, CAST(LEFT(ContentBn, 50) AS NVARCHAR(50)) AS Preview
FROM Pages 
WHERE Slug = 'privacy-policy';
```

If you see actual Bangla text, the database is OK. If you see ????????, rerun the fix script.

### Check 2: Check Database Collation
```sql
SELECT DATABASEPROPERTYEX('WBS_DB', 'Collation') AS DatabaseCollation;
```

Should be something like: `Latin1_General_CI_AS` or `SQL_Latin1_General_CP1_CI_AS`

### Check 3: Verify Page View
The view file `Views\Page\Index.cshtml` should have language detection:
```csharp
var currentCulture = Context.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
bool isBangla = currentCulture == "bn";
var pageContent = isBangla && !string.IsNullOrEmpty(Model.ContentBn) ? Model.ContentBn : Model.Content;
```

## Prevention for Future

When inserting any Bangla/Unicode text in SQL:
1. Always use **N prefix**: `N'????? ??????'`
2. Use **NVARCHAR** columns, not VARCHAR
3. Test with actual Bangla characters before deploying

## Additional Resources

- [SQL Server Unicode Support](https://docs.microsoft.com/en-us/sql/relational-databases/collations/collation-and-unicode-support)
- [NVARCHAR vs VARCHAR](https://docs.microsoft.com/en-us/sql/t-sql/data-types/nchar-and-nvarchar-transact-sql)

## Notes

- This fix deletes and recreates the pages, so any manual edits will be lost
- If you've made custom changes to these pages, backup them first from Admin Panel
- After fix, you can edit pages through `/Admin/Pages` if needed
