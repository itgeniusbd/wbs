-- Add New Module Permissions
-- Run this SQL script directly in SQL Server Management Studio or Azure Data Studio

DECLARE @MaxOrder INT;
SELECT @MaxOrder = ISNULL(MAX(DisplayOrder), 0) FROM Permissions;

-- About SDGs
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View About SDGs', 'SDG সম্পর্কে দেখুন', 'About SDGs', 'View', 'View About SDGs content', @MaxOrder + 1),
    ('Create About SDGs', 'SDG সম্পর্কে তৈরি করুন', 'About SDGs', 'Create', 'Create About SDGs content', @MaxOrder + 2),
    ('Edit About SDGs', 'SDG সম্পর্কে সম্পাদনা করুন', 'About SDGs', 'Edit', 'Edit About SDGs content', @MaxOrder + 3),
    ('Delete About SDGs', 'SDG সম্পর্কে মুছুন', 'About SDGs', 'Delete', 'Delete About SDGs content', @MaxOrder + 4);

-- History
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View History', 'ইতিহাস দেখুন', 'History', 'View', 'View History content', @MaxOrder + 5),
    ('Create History', 'ইতিহাস তৈরি করুন', 'History', 'Create', 'Create History content', @MaxOrder + 6),
    ('Edit History', 'ইতিহাস সম্পাদনা করুন', 'History', 'Edit', 'Edit History content', @MaxOrder + 7),
    ('Delete History', 'ইতিহাস মুছুন', 'History', 'Delete', 'Delete History content', @MaxOrder + 8);

-- Legal Status
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Legal Status', 'আইনগত অবস্থা দেখুন', 'Legal Status', 'View', 'View Legal Status', @MaxOrder + 9),
    ('Create Legal Status', 'আইনগত অবস্থা তৈরি করুন', 'Legal Status', 'Create', 'Create Legal Status', @MaxOrder + 10),
    ('Edit Legal Status', 'আইনগত অবস্থা সম্পাদনা করুন', 'Legal Status', 'Edit', 'Edit Legal Status', @MaxOrder + 11),
    ('Delete Legal Status', 'আইনগত অবস্থা মুছুন', 'Legal Status', 'Delete', 'Delete Legal Status', @MaxOrder + 12);

-- Partners & Sponsors
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Partners & Sponsors', 'অংশীদার ও স্পন্সর দেখুন', 'Partners & Sponsors', 'View', 'View Partners & Sponsors', @MaxOrder + 13),
    ('Create Partners & Sponsors', 'অংশীদার ও স্পন্সর তৈরি করুন', 'Partners & Sponsors', 'Create', 'Create Partners & Sponsors', @MaxOrder + 14),
    ('Edit Partners & Sponsors', 'অংশীদার ও স্পন্সর সম্পাদনা করুন', 'Partners & Sponsors', 'Edit', 'Edit Partners & Sponsors', @MaxOrder + 15),
    ('Delete Partners & Sponsors', 'অংশীদার ও স্পন্সর মুছুন', 'Partners & Sponsors', 'Delete', 'Delete Partners & Sponsors', @MaxOrder + 16);

-- Districts
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Districts', 'জেলা দেখুন', 'Districts', 'View', 'View Districts', @MaxOrder + 17),
    ('Create Districts', 'জেলা তৈরি করুন', 'Districts', 'Create', 'Create Districts', @MaxOrder + 18),
    ('Edit Districts', 'জেলা সম্পাদনা করুন', 'Districts', 'Edit', 'Edit Districts', @MaxOrder + 19),
    ('Delete Districts', 'জেলা মুছুন', 'Districts', 'Delete', 'Delete Districts', @MaxOrder + 20);

-- Upazilas
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Upazilas', 'উপজেলা দেখুন', 'Upazilas', 'View', 'View Upazilas', @MaxOrder + 21),
    ('Create Upazilas', 'উপজেলা তৈরি করুন', 'Upazilas', 'Create', 'Create Upazilas', @MaxOrder + 22),
    ('Edit Upazilas', 'উপজেলা সম্পাদনা করুন', 'Upazilas', 'Edit', 'Edit Upazilas', @MaxOrder + 23),
    ('Delete Upazilas', 'উপজেলা মুছুন', 'Upazilas', 'Delete', 'Delete Upazilas', @MaxOrder + 24);

-- Accounts
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Accounts', 'অ্যাকাউন্ট দেখুন', 'Accounts', 'View', 'View Accounts', @MaxOrder + 25),
    ('Create Accounts', 'অ্যাকাউন্ট তৈরি করুন', 'Accounts', 'Create', 'Create Accounts', @MaxOrder + 26),
    ('Edit Accounts', 'অ্যাকাউন্ট সম্পাদনা করুন', 'Accounts', 'Edit', 'Edit Accounts', @MaxOrder + 27),
    ('Delete Accounts', 'অ্যাকাউন্ট মুছুন', 'Accounts', 'Delete', 'Delete Accounts', @MaxOrder + 28);

-- General Expenses
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View General Expenses', 'সাধারণ খরচ দেখুন', 'General Expenses', 'View', 'View General Expenses', @MaxOrder + 29),
    ('Create General Expenses', 'সাধারণ খরচ তৈরি করুন', 'General Expenses', 'Create', 'Create General Expenses', @MaxOrder + 30),
    ('Edit General Expenses', 'সাধারণ খরচ সম্পাদনা করুন', 'General Expenses', 'Edit', 'Edit General Expenses', @MaxOrder + 31),
    ('Delete General Expenses', 'সাধারণ খরচ মুছুন', 'General Expenses', 'Delete', 'Delete General Expenses', @MaxOrder + 32);

-- Program Expenses
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Program Expenses', 'প্রোগ্রাম খরচ দেখুন', 'Program Expenses', 'View', 'View Program Expenses', @MaxOrder + 33),
    ('Create Program Expenses', 'প্রোগ্রাম খরচ তৈরি করুন', 'Program Expenses', 'Create', 'Create Program Expenses', @MaxOrder + 34),
    ('Edit Program Expenses', 'প্রোগ্রাম খরচ সম্পাদনা করুন', 'Program Expenses', 'Edit', 'Edit Program Expenses', @MaxOrder + 35),
    ('Delete Program Expenses', 'প্রোগ্রাম খরচ মুছুন', 'Program Expenses', 'Delete', 'Delete Program Expenses', @MaxOrder + 36);

-- Financial Reports
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Financial Reports', 'আর্থিক প্রতিবেদন দেখুন', 'Financial Reports', 'View', 'View Financial Reports', @MaxOrder + 37),
    ('Export Financial Reports', 'আর্থিক প্রতিবেদন এক্সপোর্ট করুন', 'Financial Reports', 'Export', 'Export Financial Reports', @MaxOrder + 38);

-- Other Incomes
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Other Incomes', 'অন্যান্য আয় দেখুন', 'Other Incomes', 'View', 'View Other Incomes', @MaxOrder + 39),
    ('Create Other Incomes', 'অন্যান্য আয় তৈরি করুন', 'Other Incomes', 'Create', 'Create Other Incomes', @MaxOrder + 40),
    ('Edit Other Incomes', 'অন্যান্য আয় সম্পাদনা করুন', 'Other Incomes', 'Edit', 'Edit Other Incomes', @MaxOrder + 41),
    ('Delete Other Incomes', 'অন্যান্য আয় মুছুন', 'Other Incomes', 'Delete', 'Delete Other Incomes', @MaxOrder + 42);

-- SMS Management
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View SMS Management', 'এসএমএস ব্যবস্থাপনা দেখুন', 'SMS Management', 'View', 'View SMS Management', @MaxOrder + 43),
    ('Send SMS Management', 'এসএমএস পাঠান', 'SMS Management', 'Send', 'Send SMS', @MaxOrder + 44);

-- Contact Lists
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Contact Lists', 'যোগাযোগ তালিকা দেখুন', 'Contact Lists', 'View', 'View Contact Lists', @MaxOrder + 45),
    ('Create Contact Lists', 'যোগাযোগ তালিকা তৈরি করুন', 'Contact Lists', 'Create', 'Create Contact Lists', @MaxOrder + 46),
    ('Edit Contact Lists', 'যোগাযোগ তালিকা সম্পাদনা করুন', 'Contact Lists', 'Edit', 'Edit Contact Lists', @MaxOrder + 47),
    ('Delete Contact Lists', 'যোগাযোগ তালিকা মুছুন', 'Contact Lists', 'Delete', 'Delete Contact Lists', @MaxOrder + 48);

-- Appeals
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Appeals', 'আবেদন দেখুন', 'Appeals', 'View', 'View Appeals', @MaxOrder + 49),
    ('Create Appeals', 'আবেদন তৈরি করুন', 'Appeals', 'Create', 'Create Appeals', @MaxOrder + 50),
    ('Edit Appeals', 'আবেদন সম্পাদনা করুন', 'Appeals', 'Edit', 'Edit Appeals', @MaxOrder + 51),
    ('Delete Appeals', 'আবেদন মুছুন', 'Appeals', 'Delete', 'Delete Appeals', @MaxOrder + 52);

-- SDG Programs
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View SDG Programs', 'SDG প্রোগ্রাম দেখুন', 'SDG Programs', 'View', 'View SDG Programs', @MaxOrder + 53),
    ('Create SDG Programs', 'SDG প্রোগ্রাম তৈরি করুন', 'SDG Programs', 'Create', 'Create SDG Programs', @MaxOrder + 54),
    ('Edit SDG Programs', 'SDG প্রোগ্রাম সম্পাদনা করুন', 'SDG Programs', 'Edit', 'Edit SDG Programs', @MaxOrder + 55),
    ('Delete SDG Programs', 'SDG প্রোগ্রাম মুছুন', 'SDG Programs', 'Delete', 'Delete SDG Programs', @MaxOrder + 56);

-- Rohingya Programs
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Rohingya Programs', 'রোহিঙ্গা প্রোগ্রাম দেখুন', 'Rohingya Programs', 'View', 'View Rohingya Programs', @MaxOrder + 57),
    ('Create Rohingya Programs', 'রোহিঙ্গা প্রোগ্রাম তৈরি করুন', 'Rohingya Programs', 'Create', 'Create Rohingya Programs', @MaxOrder + 58),
    ('Edit Rohingya Programs', 'রোহিঙ্গা প্রোগ্রাম সম্পাদনা করুন', 'Rohingya Programs', 'Edit', 'Edit Rohingya Programs', @MaxOrder + 59),
    ('Delete Rohingya Programs', 'রোহিঙ্গা প্রোগ্রাম মুছুন', 'Rohingya Programs', 'Delete', 'Delete Rohingya Programs', @MaxOrder + 60);

-- Success Stories
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Success Stories', 'সফলতার গল্প দেখুন', 'Success Stories', 'View', 'View Success Stories', @MaxOrder + 61),
    ('Create Success Stories', 'সফলতার গল্প তৈরি করুন', 'Success Stories', 'Create', 'Create Success Stories', @MaxOrder + 62),
    ('Edit Success Stories', 'সফলতার গল্প সম্পাদনা করুন', 'Success Stories', 'Edit', 'Edit Success Stories', @MaxOrder + 63),
    ('Delete Success Stories', 'সফলতার গল্প মুছুন', 'Success Stories', 'Delete', 'Delete Success Stories', @MaxOrder + 64);

-- Publications
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Publications', 'প্রকাশনা দেখুন', 'Publications', 'View', 'View Publications', @MaxOrder + 65),
    ('Create Publications', 'প্রকাশনা তৈরি করুন', 'Publications', 'Create', 'Create Publications', @MaxOrder + 66),
    ('Edit Publications', 'প্রকাশনা সম্পাদনা করুন', 'Publications', 'Edit', 'Edit Publications', @MaxOrder + 67),
    ('Delete Publications', 'প্রকাশনা মুছুন', 'Publications', 'Delete', 'Delete Publications', @MaxOrder + 68);

-- Sliders
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Sliders', 'স্লাইডার দেখুন', 'Sliders', 'View', 'View Sliders', @MaxOrder + 69),
    ('Create Sliders', 'স্লাইডার তৈরি করুন', 'Sliders', 'Create', 'Create Sliders', @MaxOrder + 70),
    ('Edit Sliders', 'স্লাইডার সম্পাদনা করুন', 'Sliders', 'Edit', 'Edit Sliders', @MaxOrder + 71),
    ('Delete Sliders', 'স্লাইডার মুছুন', 'Sliders', 'Delete', 'Delete Sliders', @MaxOrder + 72);

-- Menus
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Menus', 'মেনু দেখুন', 'Menus', 'View', 'View Menus', @MaxOrder + 73),
    ('Create Menus', 'মেনু তৈরি করুন', 'Menus', 'Create', 'Create Menus', @MaxOrder + 74),
    ('Edit Menus', 'মেনু সম্পাদনা করুন', 'Menus', 'Edit', 'Edit Menus', @MaxOrder + 75),
    ('Delete Menus', 'মেনু মুছুন', 'Menus', 'Delete', 'Delete Menus', @MaxOrder + 76);

-- Notification Templates
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট দেখুন', 'Notification Templates', 'View', 'View Notification Templates', @MaxOrder + 77),
    ('Create Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট তৈরি করুন', 'Notification Templates', 'Create', 'Create Notification Templates', @MaxOrder + 78),
    ('Edit Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট সম্পাদনা করুন', 'Notification Templates', 'Edit', 'Edit Notification Templates', @MaxOrder + 79),
    ('Delete Notification Templates', 'বিজ্ঞপ্তি টেমপ্লেট মুছুন', 'Notification Templates', 'Delete', 'Delete Notification Templates', @MaxOrder + 80);

-- Contact Messages
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Contact Messages', 'যোগাযোগ বার্তা দেখুন', 'Contact Messages', 'View', 'View Contact Messages', @MaxOrder + 81),
    ('Reply Contact Messages', 'যোগাযোগ বার্তা উত্তর দিন', 'Contact Messages', 'Reply', 'Reply to Contact Messages', @MaxOrder + 82),
    ('Delete Contact Messages', 'যোগাযোগ বার্তা মুছুন', 'Contact Messages', 'Delete', 'Delete Contact Messages', @MaxOrder + 83);

-- Roles Management
INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
VALUES 
    ('View Roles', 'ভূমিকা দেখুন', 'Roles', 'View', 'View Roles', @MaxOrder + 84),
    ('Create Roles', 'ভূমিকা তৈরি করুন', 'Roles', 'Create', 'Create Roles', @MaxOrder + 85),
    ('Edit Roles', 'ভূমিকা সম্পাদনা করুন', 'Roles', 'Edit', 'Edit Roles', @MaxOrder + 86),
    ('Delete Roles', 'ভূমিকা মুছুন', 'Roles', 'Delete', 'Delete Roles', @MaxOrder + 87);

PRINT 'Successfully added 87 new permissions!';
PRINT 'Total permissions added:';
PRINT '  - About SDGs: 4 permissions';
PRINT '  - History: 4 permissions';
PRINT '  - Legal Status: 4 permissions';
PRINT '  - Partners & Sponsors: 4 permissions';
PRINT '  - Districts: 4 permissions';
PRINT '  - Upazilas: 4 permissions';
PRINT '  - Accounts: 4 permissions';
PRINT '  - General Expenses: 4 permissions';
PRINT '  - Program Expenses: 4 permissions';
PRINT '  - Financial Reports: 2 permissions';
PRINT '  - Other Incomes: 4 permissions';
PRINT '  - SMS Management: 2 permissions';
PRINT '  - Contact Lists: 4 permissions';
PRINT '  - Appeals: 4 permissions';
PRINT '  - SDG Programs: 4 permissions';
PRINT '  - Rohingya Programs: 4 permissions';
PRINT '  - Success Stories: 4 permissions';
PRINT '  - Publications: 4 permissions';
PRINT '  - Sliders: 4 permissions';
PRINT '  - Menus: 4 permissions';
PRINT '  - Notification Templates: 4 permissions';
PRINT '  - Contact Messages: 3 permissions';
PRINT '  - Roles: 4 permissions';
PRINT '  - Contact Lists: 4 permissions';

-- Verify the additionspa
SELECT Module, Action, Name, NameBn 
FROM Permissions 
WHERE Module IN (
    'About SDGs', 'History', 'Legal Status', 'Partners & Sponsors',
    'Districts', 'Upazilas', 'Accounts', 'General Expenses',
    'Program Expenses', 'Financial Reports', 'Other Incomes',
    'SMS Management', 'Contact Lists'
)
ORDER BY DisplayOrder;
