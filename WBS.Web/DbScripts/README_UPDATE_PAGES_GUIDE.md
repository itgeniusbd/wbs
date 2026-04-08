# How to Update Privacy Policy and Terms & Conditions Pages

## ?? Table of Contents
1. [Update via Admin Panel (Recommended)](#method-1-admin-panel)
2. [Update via SQL Script](#method-2-sql-script)
3. [Best Practices](#best-practices)
4. [Common Issues](#common-issues)

---

## Method 1: Admin Panel (Recommended) ?

### Step-by-Step Guide:

#### 1. Login to Admin Panel
- Go to: `http://localhost:5001/Account/Login` (or your domain)
- Login with your admin credentials

#### 2. Navigate to Pages Section
- Click on **"Pages"** in the sidebar menu
- Or directly go to: `http://localhost:5001/Admin/Pages`

#### 3. Find the Page to Edit
You'll see a list of all pages:
- **Privacy Policy** (privacy-policy)
- **Terms & Conditions** (terms-conditions)

#### 4. Click Edit Button
- Click the **Edit** button next to the page you want to update

#### 5. Update Content
You can edit the following fields:

**Basic Information:**
- **Title**: English title (e.g., "Privacy Policy")
- **Title (Bangla)**: ????? title (e.g., "????????? ????")
- **Slug**: URL-friendly name (don't change: "privacy-policy")

**Content:**
- **Content**: Full English content (HTML supported)
- **Content (Bangla)**: Full ????? content (HTML supported)

**SEO:**
- **Meta Title**: For search engines
- **Meta Description**: Brief description for search results
- **Meta Keywords**: Related keywords

**Settings:**
- **Is Active**: ? Keep this checked
- **Show In Footer**: ? Keep this checked

#### 6. Save Changes
- Click **"Save"** or **"Update"** button
- You'll see a success message

#### 7. Verify Changes
- Go to the page: `/page/privacy-policy` or `/page/terms-conditions`
- Switch language to test both English and Bangla content

### Advantages of Admin Panel Method:
? No SQL knowledge required
? User-friendly interface
? Can preview before saving
? Automatic UpdatedAt timestamp
? Can upload images if needed
? No risk of breaking database

---

## Method 2: SQL Script (Advanced) ??

### When to Use SQL:
- Bulk updates
- Automated deployments
- Complex content changes
- Direct database access needed

### Step-by-Step:

#### 1. Create or Use Update Script
File: `WBS.Web\DbScripts\UpdatePrivacyAndTermsPages.sql`

#### 2. Edit the Script
Open the file and modify the content:

```sql
-- Update English Content
UPDATE Pages
SET 
    Content = N'<h2>Your Updated Title</h2>
    <p>Your updated content here...</p>
    
    <!-- Add more sections as needed -->',
    
    UpdatedAt = GETDATE(),
    UpdatedBy = 'YourName'
    
WHERE Slug = 'privacy-policy';
```

**Important Notes:**
- ?? Always use `N` prefix before strings: `N'text'`
- ?? For Bangla text, must use: `N'????? ??????'`
- ?? HTML is allowed in Content fields

#### 3. Run the Script

**Option A: SQL Server Management Studio (SSMS)**
1. Open SSMS
2. Connect to your database
3. Open the script file
4. Press F5 to execute

**Option B: Azure Data Studio**
1. Open Azure Data Studio
2. Connect to database
3. Open script
4. Click "Run"

**Option C: Command Line**
```bash
sqlcmd -S server_name -d WBS_DB -i "WBS.Web\DbScripts\UpdatePrivacyAndTermsPages.sql"
```

#### 4. Verify the Update
```sql
SELECT Title, TitleBn, Slug, UpdatedAt, UpdatedBy
FROM Pages
WHERE Slug IN ('privacy-policy', 'terms-conditions');
```

---

## Best Practices ??

### 1. Content Writing

**HTML Structure:**
```html
<h2>Main Title</h2>
<p><strong>Effective Date:</strong> January 2025</p>

<h3>1. Section Title</h3>
<p>Paragraph content here...</p>

<h4>1.1 Subsection</h4>
<p>More detailed information...</p>

<ul>
    <li>List item 1</li>
    <li>List item 2</li>
</ul>
```

**Bangla Content:**
```html
<h2>?????? ???????</h2>
<p><strong>??????? ?????:</strong> ????????? ????</p>

<h3>?. ??????? ???????</h3>
<p>?????????? ??????????...</p>
```

### 2. Testing Checklist

After updating, test:
- [ ] English content displays correctly
- [ ] Bangla content displays correctly
- [ ] Language switching works
- [ ] Page formatting is correct
- [ ] Links work (if any)
- [ ] Mobile responsive
- [ ] No special characters turn into ???

### 3. Backup Before Major Changes

**Via Admin Panel:**
1. Copy current content to a text file
2. Save it with date: `privacy-policy-backup-2025-01-27.txt`

**Via SQL:**
```sql
-- Backup to another table
SELECT * INTO Pages_Backup_20250127
FROM Pages
WHERE Slug IN ('privacy-policy', 'terms-conditions');
```

### 4. Version Control

Keep track of changes:
```sql
-- Add to UpdatedBy field who made changes
UpdatedBy = 'Admin - Added GDPR compliance section'
```

---

## Common Issues and Solutions ??

### Issue 1: Bangla Text Shows as ?????????

**Solution:**
Make sure you're using `N` prefix:
```sql
-- ? Wrong
Content = '?????'

-- ? Correct
Content = N'?????'
```

### Issue 2: HTML Not Rendering

**Check:**
- View uses `@Html.Raw(Model.Content)` not `@Model.Content`
- HTML tags are properly closed
- No syntax errors in HTML

### Issue 3: Changes Not Showing

**Try:**
1. Clear browser cache (Ctrl + F5)
2. Restart the application
3. Check if page is set to `IsActive = true`
4. Verify database was actually updated

### Issue 4: Page Not Found

**Check:**
- Slug is exactly: `privacy-policy` or `terms-conditions`
- No extra spaces in slug
- Page is marked as `IsActive = true`
- Route is configured: `/page/{slug}`

---

## Quick Reference ??

### Admin Panel URLs:
- Pages List: `/Admin/Pages`
- Edit Privacy: `/Admin/Pages/Edit/{id}`
- Edit Terms: `/Admin/Pages/Edit/{id}`

### Frontend URLs:
- Privacy Policy: `/page/privacy-policy`
- Terms & Conditions: `/page/terms-conditions`

### SQL Update Template:
```sql
UPDATE Pages
SET 
    Content = N'<your HTML content>',
    ContentBn = N'<????? ????? content>',
    UpdatedAt = GETDATE(),
    UpdatedBy = 'Your Name'
WHERE Slug = 'page-slug';
```

### Important Fields:
| Field | Type | Required | Notes |
|-------|------|----------|-------|
| Title | NVARCHAR | Yes | English title |
| TitleBn | NVARCHAR | No | Bangla title |
| Slug | NVARCHAR | Yes | Don't change |
| Content | NVARCHAR(MAX) | Yes | English HTML |
| ContentBn | NVARCHAR(MAX) | No | Bangla HTML |
| IsActive | BIT | Yes | Must be 1 (true) |
| ShowInFooter | BIT | No | 1 to show in footer |

---

## Examples ??

### Example 1: Add New Section

**Via Admin Panel:**
1. Go to Edit page
2. Scroll to Content field
3. Add at the end:
```html
<h3>10. New Section</h3>
<p>Your new content here...</p>
```
4. Save

**Via SQL:**
```sql
UPDATE Pages
SET Content = CONCAT(Content, N'
<h3>10. New Section</h3>
<p>Your new content here...</p>
')
WHERE Slug = 'privacy-policy';
```

### Example 2: Update Effective Date

```sql
UPDATE Pages
SET 
    Content = REPLACE(Content, 'January 2025', 'February 2025'),
    ContentBn = REPLACE(ContentBn, N'????????? ????', N'??????????? ????')
WHERE Slug = 'privacy-policy';
```

### Example 3: Add Contact Email

```sql
UPDATE Pages
SET Content = REPLACE(
    Content, 
    'info@wbs-bd.org', 
    'contact@wbs-bd.org'
)
WHERE Slug IN ('privacy-policy', 'terms-conditions');
```

---

## Tips for Success ??

1. **Always test in development first**
2. **Keep backups of content**
3. **Use N prefix for all Bangla text**
4. **Test both languages after updating**
5. **Clear cache after major changes**
6. **Document what you changed and when**
7. **Use consistent HTML formatting**
8. **Keep legal team informed of changes**

---

## Need Help? ??

If you encounter issues:

1. **Check logs**: `/Admin/Logs` or server logs
2. **Verify database**: Run `CheckBanglaContent.sql`
3. **Test locally**: Before deploying to production
4. **Contact support**: For technical assistance

---

## Summary ??

**For Quick Updates:** Use Admin Panel ?
**For Complex Changes:** Use SQL Script ??
**Always Remember:** Use `N` prefix for Bangla text ??
**After Changes:** Test both languages ??
