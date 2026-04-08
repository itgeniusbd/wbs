-- ========================================
-- Sample Notification Templates
-- For SMS & Email Template Management System
-- ========================================

-- This script adds sample templates for different donation types
-- Run this after the database migration is complete

USE [WBS_Database] -- Change to your database name
GO

-- ========================================
-- 1. ZAKAT TEMPLATES
-- ========================================

-- Zakat SMS Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, SmsContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Zakat Donation Receipt SMS',
    'SMS',
    'DonationReceipt',
    2, -- Zakat DonationTypeId
    'Dear {DonorName}, JazakAllah for your Zakat of BDT {Amount}. TXN: {TransactionId}. May Allah purify your wealth and bless you abundantly. - WBS',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- Zakat Email Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, EmailSubject, EmailContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Zakat Donation Receipt Email',
    'Email',
    'DonationReceipt',
    2, -- Zakat DonationTypeId
    'Zakat Receipt #{TransactionId} - JazakAllah for Your Donation',
    '<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #2c5f2d 0%, #1a3a1b 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .logo { max-width: 120px; margin-bottom: 15px; }
        .content { background: #f8f9fa; padding: 30px; }
        .receipt-box { background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #28a745; }
        .amount { font-size: 32px; font-weight: bold; color: #28a745; }
        .zakat-info { background: #fff3cd; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #ffc107; }
        .footer { text-align: center; padding: 20px; color: #666; font-size: 14px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <img src="https://yourwebsite.com/images/logo.png" alt="WBS Logo" class="logo" />
            <h1>WBS</h1>
            <p>Zakat Receipt - May Allah Accept Your Charity</p>
        </div>
        
        <div class="content">
            <h2>Dear {DonorName},</h2>
            <p><strong>Assalamu Alaikum wa Rahmatullahi wa Barakatuh!</strong></p>
            <p>JazakAllah Khairan for fulfilling your sacred obligation of Zakat. May Allah purify your wealth and bless you with abundance.</p>
            
            <div class="receipt-box">
                <h3>Zakat Donation Receipt</h3>
                <table style="width: 100%;">
                    <tr><td><strong>Transaction ID:</strong></td><td>{TransactionId}</td></tr>
                    <tr><td><strong>Donation Type:</strong></td><td>{DonationType}</td></tr>
                    <tr><td><strong>Amount:</strong></td><td><span class="amount">?{Amount}</span></td></tr>
                    <tr><td><strong>Date:</strong></td><td>{Date}</td></tr>
                </table>
            </div>
            
            <div class="zakat-info">
                <h4 style="margin-top: 0; color: #856404;">About Your Zakat</h4>
                <p style="margin-bottom: 0; color: #856404;">Your Zakat will be distributed to those in need according to Islamic guidelines. It will help provide food, shelter, education, and healthcare to the most vulnerable members of our community.</p>
            </div>
            
            <p><strong>"The example of those who spend their wealth in the way of Allah is like a seed [of grain] which grows seven spikes; in each spike is a hundred grains." (Quran 2:261)</strong></p>
            
            <p style="margin-top: 30px;">If you have any questions about your Zakat donation, please don''t hesitate to contact us.</p>
        </div>
        
        <div class="footer">
            <p><strong>WBS</strong></p>
            <p>Working for Humanity</p>
            <p>Email: info@wbs.org | Phone: +880 1XXX-XXXXXX</p>
            <p>&copy; 2024 WBS. All rights reserved.</p>
        </div>
    </div>
</body>
</html>',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}, {Date}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- ========================================
-- 2. WINTER APPEAL TEMPLATES
-- ========================================

-- Winter Appeal SMS Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, SmsContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Winter Appeal Receipt SMS',
    'SMS',
    'DonationReceipt',
    4, -- Winter Appeal DonationTypeId
    'Dear {DonorName}, Thank you for your Winter Appeal donation of BDT {Amount}. TXN: {TransactionId}. Your kindness will warm hearts this winter. - WBS',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- Winter Appeal Email Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, EmailSubject, EmailContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Winter Appeal Receipt Email',
    'Email',
    'DonationReceipt',
    4, -- Winter Appeal DonationTypeId
    'Thank You for Your Winter Appeal Donation - Receipt #{TransactionId}',
    '<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #0056b3 0%, #003d82 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .logo { max-width: 120px; margin-bottom: 15px; }
        .content { background: #f8f9fa; padding: 30px; }
        .receipt-box { background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #0056b3; }
        .amount { font-size: 32px; font-weight: bold; color: #0056b3; }
        .winter-impact { background: #e7f3ff; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #0056b3; }
        .footer { text-align: center; padding: 20px; color: #666; font-size: 14px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <img src="https://yourwebsite.com/images/logo.png" alt="WBS Logo" class="logo" />
            <h1>WBS</h1>
            <p>?? Winter Appeal - Warming Hearts Together</p>
        </div>
        
        <div class="content">
            <h2>Dear {DonorName},</h2>
            <p><strong>Assalamu Alaikum!</strong></p>
            <p>Thank you for your generous Winter Appeal donation. Your compassion will bring warmth and comfort to families struggling through the cold winter months.</p>
            
            <div class="receipt-box">
                <h3>Donation Receipt</h3>
                <table style="width: 100%;">
                    <tr><td><strong>Transaction ID:</strong></td><td>{TransactionId}</td></tr>
                    <tr><td><strong>Campaign:</strong></td><td>{DonationType}</td></tr>
                    <tr><td><strong>Amount:</strong></td><td><span class="amount">?{Amount}</span></td></tr>
                    <tr><td><strong>Date:</strong></td><td>{Date}</td></tr>
                </table>
            </div>
            
            <div class="winter-impact">
                <h4 style="margin-top: 0; color: #004085;">Your Impact This Winter</h4>
                <p style="margin-bottom: 0; color: #004085;">Your donation will provide warm blankets, winter clothing, and heating support to vulnerable families. Together, we''re ensuring no one faces the harsh winter alone.</p>
            </div>
            
            <p><strong>May Allah reward you abundantly for your kindness and compassion.</strong></p>
            
            <p style="margin-top: 30px;">We will keep you updated on the impact of your donation. Thank you for standing with us.</p>
        </div>
        
        <div class="footer">
            <p><strong>WBS</strong></p>
            <p>Working for Humanity</p>
            <p>Email: info@wbs.org | Phone: +880 1XXX-XXXXXX</p>
            <p>&copy; 2024 WBS. All rights reserved.</p>
        </div>
    </div>
</body>
</html>',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}, {Date}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- ========================================
-- 3. EMERGENCY APPEAL TEMPLATES
-- ========================================

-- Emergency Appeal SMS Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, SmsContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Emergency Appeal Receipt SMS',
    'SMS',
    'DonationReceipt',
    5, -- Emergency Appeal DonationTypeId
    'Dear {DonorName}, Your urgent donation of BDT {Amount} is deeply appreciated. TXN: {TransactionId}. You are saving lives. May Allah bless you. - WBS',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- Emergency Appeal Email Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, EmailSubject, EmailContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Emergency Appeal Receipt Email',
    'Email',
    'DonationReceipt',
    5, -- Emergency Appeal DonationTypeId
    'URGENT: Thank You for Your Emergency Response - Receipt #{TransactionId}',
    '<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #dc3545 0%, #bd2130 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .logo { max-width: 120px; margin-bottom: 15px; }
        .content { background: #f8f9fa; padding: 30px; }
        .receipt-box { background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #dc3545; }
        .amount { font-size: 32px; font-weight: bold; color: #dc3545; }
        .emergency-impact { background: #f8d7da; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #dc3545; }
        .footer { text-align: center; padding: 20px; color: #666; font-size: 14px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <img src="https://yourwebsite.com/images/logo.png" alt="WBS Logo" class="logo" />
            <h1>WBS</h1>
            <p>?? Emergency Response - Every Second Counts</p>
        </div>
        
        <div class="content">
            <h2>Dear {DonorName},</h2>
            <p><strong>Assalamu Alaikum!</strong></p>
            <p>Thank you for your immediate response to our emergency appeal. Your swift action is making a life-saving difference right now.</p>
            
            <div class="receipt-box">
                <h3>Emergency Donation Receipt</h3>
                <table style="width: 100%;">
                    <tr><td><strong>Transaction ID:</strong></td><td>{TransactionId}</td></tr>
                    <tr><td><strong>Appeal:</strong></td><td>{DonationType}</td></tr>
                    <tr><td><strong>Amount:</strong></td><td><span class="amount">?{Amount}</span></td></tr>
                    <tr><td><strong>Date:</strong></td><td>{Date}</td></tr>
                </table>
            </div>
            
            <div class="emergency-impact">
                <h4 style="margin-top: 0; color: #721c24;">Your Immediate Impact</h4>
                <p style="margin-bottom: 0; color: #721c24;">Your donation will be deployed immediately to provide urgent relief - food, water, medical aid, and emergency shelter to those affected by crisis. You are literally saving lives right now.</p>
            </div>
            
            <p><strong>"Whoever saves a life, it will be as if they saved all of humanity." (Quran 5:32)</strong></p>
            
            <p style="margin-top: 30px;">We will send you updates on how your emergency donation is making an impact. Thank you for being a hero to those in desperate need.</p>
        </div>
        
        <div class="footer">
            <p><strong>WBS</strong></p>
            <p>Working for Humanity</p>
            <p>Email: info@wbs.org | Phone: +880 1XXX-XXXXXX</p>
            <p>&copy; 2024 WBS. All rights reserved.</p>
        </div>
    </div>
</body>
</html>',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}, {Date}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- ========================================
-- 4. SADAQAH JARIYAH TEMPLATES
-- ========================================

-- Sadaqah Jariyah SMS Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, SmsContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Sadaqah Jariyah Receipt SMS',
    'SMS',
    'DonationReceipt',
    3, -- Sadaqah Jariyah DonationTypeId
    'Dear {DonorName}, JazakAllah for your Sadaqah Jariyah of BDT {Amount}. TXN: {TransactionId}. Your continuous charity will benefit you forever. - WBS',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- Sadaqah Jariyah Email Template
INSERT INTO NotificationTemplates (Name, TemplateType, Category, DonationTypeId, EmailSubject, EmailContent, AvailablePlaceholders, IsActive, IsDefault, CreatedAt, CreatedBy)
VALUES (
    'Sadaqah Jariyah Receipt Email',
    'Email',
    'DonationReceipt',
    3, -- Sadaqah Jariyah DonationTypeId
    'Sadaqah Jariyah Receipt #{TransactionId} - Your Eternal Reward',
    '<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #17a2b8 0%, #138496 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .logo { max-width: 120px; margin-bottom: 15px; }
        .content { background: #f8f9fa; padding: 30px; }
        .receipt-box { background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #17a2b8; }
        .amount { font-size: 32px; font-weight: bold; color: #17a2b8; }
        .jariyah-info { background: #d1ecf1; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #17a2b8; }
        .footer { text-align: center; padding: 20px; color: #666; font-size: 14px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <img src="https://yourwebsite.com/images/logo.png" alt="WBS Logo" class="logo" />
            <h1>WBS</h1>
            <p>?? Sadaqah Jariyah - Continuous Charity, Eternal Reward</p>
        </div>
        
        <div class="content">
            <h2>Dear {DonorName},</h2>
            <p><strong>Assalamu Alaikum wa Rahmatullahi wa Barakatuh!</strong></p>
            <p>JazakAllah Khairan for your beautiful Sadaqah Jariyah. May Allah accept this continuous charity and grant you rewards that flow like a river, even after your time on earth.</p>
            
            <div class="receipt-box">
                <h3>Sadaqah Jariyah Receipt</h3>
                <table style="width: 100%;">
                    <tr><td><strong>Transaction ID:</strong></td><td>{TransactionId}</td></tr>
                    <tr><td><strong>Donation Type:</strong></td><td>{DonationType}</td></tr>
                    <tr><td><strong>Amount:</strong></td><td><span class="amount">?{Amount}</span></td></tr>
                    <tr><td><strong>Date:</strong></td><td>{Date}</td></tr>
                </table>
            </div>
            
            <div class="jariyah-info">
                <h4 style="margin-top: 0; color: #0c5460;">The Power of Sadaqah Jariyah</h4>
                <p style="margin-bottom: 0; color: #0c5460;">Your donation will fund sustainable projects like water wells, education programs, and community development initiatives. Every time someone benefits from these projects, you continue to earn rewards. This is a charity that never stops giving.</p>
            </div>
            
            <p><strong>"When a person dies, all their deeds end except three: a continuing charity, beneficial knowledge, and a child who prays for them." (Prophet Muhammad ?)</strong></p>
            
            <p style="margin-top: 30px;">Your Sadaqah Jariyah is an investment in both this world and the hereafter. May Allah multiply your rewards infinitely.</p>
        </div>
        
        <div class="footer">
            <p><strong>WBS</strong></p>
            <p>Working for Humanity</p>
            <p>Email: info@wbs.org | Phone: +880 1XXX-XXXXXX</p>
            <p>&copy; 2024 WBS. All rights reserved.</p>
        </div>
    </div>
</body>
</html>',
    '{DonorName}, {Amount}, {DonationType}, {TransactionId}, {Date}',
    1,
    0,
    GETUTCDATE(),
    'Admin'
);

-- ========================================
-- VERIFICATION QUERY
-- ========================================

-- Check all notification templates
SELECT 
    Id,
    Name,
    TemplateType,
    Category,
    DonationTypeId,
    IsActive,
    IsDefault,
    CreatedAt
FROM NotificationTemplates
ORDER BY TemplateType, DonationTypeId, Name;

-- Check templates by donation type
SELECT 
    dt.Name AS DonationType,
    nt.Name AS TemplateName,
    nt.TemplateType,
    nt.IsActive
FROM DonationTypes dt
LEFT JOIN NotificationTemplates nt ON dt.Id = nt.DonationTypeId
ORDER BY dt.DisplayOrder, nt.TemplateType;

GO

PRINT '? Sample templates inserted successfully!'
PRINT '?? You now have templates for:'
PRINT '   - Zakat (SMS + Email)'
PRINT '   - Winter Appeal (SMS + Email)'
PRINT '   - Emergency Appeal (SMS + Email)'
PRINT '   - Sadaqah Jariyah (SMS + Email)'
PRINT ''
PRINT '?? TIP: You can edit these templates from Admin Panel ? Notification Test ? Templates'
PRINT '?? TIP: Logo URL needs to be updated: https://yourwebsite.com/images/logo.png'
