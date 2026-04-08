-- Update Privacy Policy or Terms & Conditions Content
-- This script allows you to update existing pages without deleting them

USE [WBS_DB];
GO

-- ==============================================
-- UPDATE PRIVACY POLICY PAGE
-- ==============================================

-- Update Privacy Policy English Content
UPDATE Pages
SET 
    Content = N'<h2>Privacy Policy</h2>
<p><strong>Effective Date:</strong> January 2025</p>

<h3>1. Introduction</h3>
<p>Working Bangladesh Society (WBS) is committed to protecting your privacy. This Privacy Policy explains how we collect, use, disclose, and safeguard your information when you visit our website or interact with our services.</p>

<!-- Add your updated English content here -->

<h3>10. Updated Section</h3>
<p>This is an example of how to add new sections or update existing content.</p>',
    
    UpdatedAt = GETDATE(),
    UpdatedBy = 'Admin'
    
WHERE Slug = 'privacy-policy';

PRINT 'Privacy Policy English content updated.';
GO

-- Update Privacy Policy Bangla Content
UPDATE Pages
SET 
    ContentBn = N'<h2>গোপনীয়তা নীতি</h2>
<p><strong>কার্যকর তারিখ:</strong> জানুয়ারি ২০২৫</p>

<h3>১. ভূমিকা</h3>
<p>ওয়ার্কিং বাংলাদেশ সোসাইটি (WBS) আপনার গোপনীয়তা রক্ষায় প্রতিশ্রুতিবদ্ধ।</p>

<!-- এখানে আপনার আপডেট করা বাংলা content যোগ করুন -->

<h3>১০. আপডেট করা বিভাগ</h3>
<p>এটি নতুন বিভাগ যোগ করা বা বিদ্যমান বিষয়বস্তু আপডেট করার একটি উদাহরণ।</p>',
    
    UpdatedAt = GETDATE(),
    UpdatedBy = 'Admin'
    
WHERE Slug = 'privacy-policy';

PRINT 'Privacy Policy Bangla content updated.';
GO

-- ==============================================
-- UPDATE TERMS & CONDITIONS PAGE
-- ==============================================

-- Update Terms & Conditions English Content
UPDATE Pages
SET 
    Content = N'<h2>Terms & Conditions</h2>
<p><strong>Effective Date:</strong> January 2025</p>

<h3>1. Acceptance of Terms</h3>
<p>By accessing and using the Working Bangladesh Society (WBS) website, you accept and agree to be bound by these Terms and Conditions.</p>

<!-- Add your updated English content here -->

<h3>14. New Section</h3>
<p>Add any new terms or conditions here.</p>',
    
    UpdatedAt = GETDATE(),
    UpdatedBy = 'Admin'
    
WHERE Slug = 'terms-conditions';

PRINT 'Terms & Conditions English content updated.';
GO

-- Update Terms & Conditions Bangla Content
UPDATE Pages
SET 
    ContentBn = N'<h2>শর্তাবলী</h2>
<p><strong>কার্যকর তারিখ:</strong> জানুয়ারি ২০২৫</p>

<h3>১. শর্তাবলী গ্রহণ</h3>
<p>ওয়ার্কিং বাংলাদেশ সোসাইটি (WBS) ওয়েবসাইট অ্যাক্সেস এবং ব্যবহার করে, আপনি এই শর্তাবলী দ্বারা আবদ্ধ হতে সম্মত হন।</p>

<!-- এখানে আপনার আপডেট করা বাংলা content যোগ করুন -->

<h3>১৪. নতুন বিভাগ</h3>
<p>এখানে যেকোনো নতুন শর্ত বা নিয়ম যোগ করুন।</p>',
    
    UpdatedAt = GETDATE(),
    UpdatedBy = 'Admin'
    
WHERE Slug = 'terms-conditions';

PRINT 'Terms & Conditions Bangla content updated.';
GO

-- Verify the updates
SELECT 
    Id,
    Title,
    TitleBn,
    Slug,
    CAST(LEFT(Content, 100) AS NVARCHAR(100)) AS ContentPreview,
    CAST(LEFT(ContentBn, 100) AS NVARCHAR(100)) AS ContentBnPreview,
    UpdatedAt,
    UpdatedBy
FROM Pages
WHERE Slug IN ('privacy-policy', 'terms-conditions')
ORDER BY Slug;
GO

PRINT '=====================================================';
PRINT 'Pages updated successfully!';
PRINT 'Remember: Always use N prefix for Unicode text';
PRINT '=====================================================';
