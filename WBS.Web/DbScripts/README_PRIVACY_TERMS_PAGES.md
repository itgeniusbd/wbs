# Privacy Policy and Terms & Conditions Pages Setup

## Problem
The footer links `/page/privacy-policy` and `/page/terms-conditions` are showing 404 errors because these pages don't exist in the database.

## Solution
Run the SQL script to create these pages in the database.

## How to Run

### Option 1: Using SQL Server Management Studio (SSMS)
1. Open SQL Server Management Studio
2. Connect to your database server
3. Open the file: `WBS.Web\DbScripts\InsertPrivacyAndTermsPages.sql`
4. Make sure you're connected to the correct database (WBS_DB)
5. Click "Execute" or press F5

### Option 2: Using Azure Data Studio
1. Open Azure Data Studio
2. Connect to your database server
3. Open the file: `WBS.Web\DbScripts\InsertPrivacyAndTermsPages.sql`
4. Make sure you're connected to the correct database (WBS_DB)
5. Click "Run" or press F5

### Option 3: Using Command Line (sqlcmd)
```bash
sqlcmd -S your_server_name -d WBS_DB -i "WBS.Web\DbScripts\InsertPrivacyAndTermsPages.sql"
```

### Option 4: Using Admin Panel (Alternative)
If you prefer to create pages manually through the admin panel:

1. Go to `/Admin/Pages/Create`
2. For Privacy Policy:
   - Title: Privacy Policy
   - Title (Bangla): গোপনীয়তা নীতি
   - Slug: privacy-policy
   - Add content from the SQL script or write your own
   - Check "Is Active" and "Show In Footer"
   - Click Save

3. For Terms & Conditions:
   - Title: Terms & Conditions
   - Title (Bangla): শর্তাবলী
   - Slug: terms-conditions
   - Add content from the SQL script or write your own
   - Check "Is Active" and "Show In Footer"
   - Click Save

## What This Script Does

1. Checks if the Pages table exists
2. Inserts a "Privacy Policy" page with:
   - English and Bangla content
   - Slug: `privacy-policy`
   - Shows in footer
   - Comprehensive privacy policy content

3. Inserts a "Terms & Conditions" page with:
   - English and Bangla content
   - Slug: `terms-conditions`
   - Shows in footer
   - Comprehensive terms and conditions content

4. Verifies the insertion by selecting the created pages

## After Running the Script

The following URLs will work:
- Privacy Policy: `http://localhost:5001/page/privacy-policy`
- Terms & Conditions: `http://localhost:5001/page/terms-conditions`

These links are already configured in the footer in `_Layout.cshtml`.

## Verification

After running the script, check:
1. Go to `/Admin/Pages` to see if the pages appear in the list
2. Click on the footer links to verify they work
3. Test both English and Bangla versions by switching language

## Notes

- The script checks if pages already exist before inserting to avoid duplicates
- Pages are marked as active and visible in footer
- Content includes both English and Bangla versions
- Content is comprehensive and covers common legal requirements

## Customization

You can edit these pages later through:
1. Go to `/Admin/Pages`
2. Find the page you want to edit
3. Click "Edit"
4. Update content as needed
5. Save changes

The pages support rich HTML content, so you can format them as needed.
