-- ========================================
-- Fix Email Templates
-- 1. Remove broken logo
-- 2. Replace ? with Tk
-- ========================================

USE [WBS_Database] -- Change to your database name
GO

PRINT 'Fixing email templates...'
GO

-- Update all email templates to:
-- 1. Remove logo image (broken)
-- 2. Replace ? with Tk

UPDATE NotificationTemplates
SET EmailContent = REPLACE(
    REPLACE(
        EmailContent,
        '<img src="https://yourwebsite.com/images/logo.png" alt="WBS Logo" class="logo" />',
        '<!-- Logo removed -->'
    ),
    '?{Amount}',
    'Tk {Amount}'
),
UpdatedAt = GETUTCDATE()
WHERE TemplateType = 'Email';

PRINT '? Email templates updated!'
GO

-- Verify the changes
SELECT 
    Id,
    Name,
    TemplateType,
    CASE 
        WHEN EmailContent LIKE '%yourwebsite.com/images/logo.png%' THEN '? Still has broken logo'
        WHEN EmailContent LIKE '%?%' THEN '? Still has Bengali symbol'
        ELSE '? Fixed'
    END AS Status,
    UpdatedAt
FROM NotificationTemplates
WHERE TemplateType = 'Email';

PRINT ''
PRINT '?? TIP: Test by sending a donation receipt email'
PRINT '?? Amount should now show as: Tk 5,000.00'
