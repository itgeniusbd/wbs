# Fix Career Page Error - Database Setup Guide

## Problem
The `/getinvolved/Career` page is showing an error because the `Careers` table doesn't exist or has missing columns in the database.

## Error Message
```
SqlException: Invalid column name 'ApplicationEmail'. 
Invalid column name 'ApplicationUrl'. 
Invalid column name 'Benefits'.
...and other column errors
```

## Solution

### Step 1: Run the SQL Script

1. Open **SQL Server Management Studio (SSMS)** or any SQL client
2. Connect to your database
3. Open the file: `WBS.Web\DbScripts\CreateCareersTable.sql`
4. Execute the script

This script will:
- Create the `Careers` table if it doesn't exist
- Add any missing columns if the table exists but is incomplete
- Insert 2 sample career postings (Program Manager and Field Officer)

### Step 2: Verify the Table

After running the script, verify the table structure:

```sql
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Careers'
ORDER BY ORDINAL_POSITION;
```

You should see these columns:
- Id
- Title
- TitleBn
- Slug
- Department
- Location
- JobType
- Description
- DescriptionBn
- Requirements
- RequirementsBn
- Benefits
- SalaryRange
- Deadline
- ApplicationUrl
- ApplicationEmail
- IsActive
- CreatedAt
- UpdatedAt

### Step 3: Test the Career Page

1. Restart your application (Stop debugging and press F5)
2. Navigate to `/getinvolved/Career`
3. The page should now load without errors
4. You should see the 2 sample career postings

## Alternative: Entity Framework Migration (If Preferred)

If you prefer using EF migrations instead of SQL scripts:

```bash
# Navigate to WBS.Web directory
cd F:\WBS\WBS.Web

# Create migration
dotnet ef migrations add CreateCareersTable

# Update database
dotnet ef database update
```

## Sample Data

The script includes 2 sample career postings:

### 1. Program Manager
- Location: Dhaka, Bangladesh
- Department: Programs
- Salary: ?40,000 - ?60,000
- Deadline: 1 month from now

### 2. Field Officer
- Location: Sylhet, Bangladesh  
- Department: Field Operations
- Salary: ?25,000 - ?35,000
- Deadline: 2 months from now

## Notes

- The script is **safe to run multiple times** - it checks if the table exists before creating it
- The script uses **NVARCHAR** columns for proper Bangla (Unicode) support
- All existing data will be preserved if the table already exists
- The script only adds missing columns, it won't delete existing data

## Troubleshooting

### Error: "Table 'Careers' already exists"
This is normal - the script detected the existing table and added missing columns instead.

### Error: "Invalid object name 'Careers'"
The table doesn't exist yet. Make sure you're connected to the correct database and run the script.

### Career page still shows errors after running script
1. Clear browser cache
2. Restart the application
3. Check if you ran the script on the correct database
4. Verify the connection string in `appsettings.json`

## Next Steps

After fixing the database:
- Visit `/admin/careers` to manage career postings
- Add more job postings
- The Career page will support both Bangla and English languages automatically
