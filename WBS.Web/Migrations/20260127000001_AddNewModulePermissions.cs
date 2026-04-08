using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WBS.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModulePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get current max DisplayOrder
            migrationBuilder.Sql(@"
                DECLARE @MaxOrder INT;
                SELECT @MaxOrder = ISNULL(MAX(DisplayOrder), 0) FROM Permissions;
                
                -- About SDGs
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View About SDGs', 'SDG ???????? ?????', 'About SDGs', 'View', 'View About SDGs content', @MaxOrder + 1),
                    ('Create About SDGs', 'SDG ???????? ???? ????', 'About SDGs', 'Create', 'Create About SDGs content', @MaxOrder + 2),
                    ('Edit About SDGs', 'SDG ???????? ???????? ????', 'About SDGs', 'Edit', 'Edit About SDGs content', @MaxOrder + 3),
                    ('Delete About SDGs', 'SDG ???????? ?????', 'About SDGs', 'Delete', 'Delete About SDGs content', @MaxOrder + 4);
                
                -- History
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View History', '?????? ?????', 'History', 'View', 'View History content', @MaxOrder + 5),
                    ('Create History', '?????? ???? ????', 'History', 'Create', 'Create History content', @MaxOrder + 6),
                    ('Edit History', '?????? ???????? ????', 'History', 'Edit', 'Edit History content', @MaxOrder + 7),
                    ('Delete History', '?????? ?????', 'History', 'Delete', 'Delete History content', @MaxOrder + 8);
                
                -- Legal Status
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Legal Status', '????? ?????? ?????', 'Legal Status', 'View', 'View Legal Status', @MaxOrder + 9),
                    ('Create Legal Status', '????? ?????? ???? ????', 'Legal Status', 'Create', 'Create Legal Status', @MaxOrder + 10),
                    ('Edit Legal Status', '????? ?????? ???????? ????', 'Legal Status', 'Edit', 'Edit Legal Status', @MaxOrder + 11),
                    ('Delete Legal Status', '????? ?????? ?????', 'Legal Status', 'Delete', 'Delete Legal Status', @MaxOrder + 12);
                
                -- Partners & Sponsors
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Partners & Sponsors', '??????? ? ??????? ?????', 'Partners & Sponsors', 'View', 'View Partners & Sponsors', @MaxOrder + 13),
                    ('Create Partners & Sponsors', '??????? ? ??????? ???? ????', 'Partners & Sponsors', 'Create', 'Create Partners & Sponsors', @MaxOrder + 14),
                    ('Edit Partners & Sponsors', '??????? ? ??????? ???????? ????', 'Partners & Sponsors', 'Edit', 'Edit Partners & Sponsors', @MaxOrder + 15),
                    ('Delete Partners & Sponsors', '??????? ? ??????? ?????', 'Partners & Sponsors', 'Delete', 'Delete Partners & Sponsors', @MaxOrder + 16);
                
                -- Districts
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Districts', '???? ?????', 'Districts', 'View', 'View Districts', @MaxOrder + 17),
                    ('Create Districts', '???? ???? ????', 'Districts', 'Create', 'Create Districts', @MaxOrder + 18),
                    ('Edit Districts', '???? ???????? ????', 'Districts', 'Edit', 'Edit Districts', @MaxOrder + 19),
                    ('Delete Districts', '???? ?????', 'Districts', 'Delete', 'Delete Districts', @MaxOrder + 20);
                
                -- Upazilas
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Upazilas', '?????? ?????', 'Upazilas', 'View', 'View Upazilas', @MaxOrder + 21),
                    ('Create Upazilas', '?????? ???? ????', 'Upazilas', 'Create', 'Create Upazilas', @MaxOrder + 22),
                    ('Edit Upazilas', '?????? ???????? ????', 'Upazilas', 'Edit', 'Edit Upazilas', @MaxOrder + 23),
                    ('Delete Upazilas', '?????? ?????', 'Upazilas', 'Delete', 'Delete Upazilas', @MaxOrder + 24);
                
                -- Accounts
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Accounts', '?????????? ?????', 'Accounts', 'View', 'View Accounts', @MaxOrder + 25),
                    ('Create Accounts', '?????????? ???? ????', 'Accounts', 'Create', 'Create Accounts', @MaxOrder + 26),
                    ('Edit Accounts', '?????????? ???????? ????', 'Accounts', 'Edit', 'Edit Accounts', @MaxOrder + 27),
                    ('Delete Accounts', '?????????? ?????', 'Accounts', 'Delete', 'Delete Accounts', @MaxOrder + 28);
                
                -- General Expenses
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View General Expenses', '?????? ??? ?????', 'General Expenses', 'View', 'View General Expenses', @MaxOrder + 29),
                    ('Create General Expenses', '?????? ??? ???? ????', 'General Expenses', 'Create', 'Create General Expenses', @MaxOrder + 30),
                    ('Edit General Expenses', '?????? ??? ???????? ????', 'General Expenses', 'Edit', 'Edit General Expenses', @MaxOrder + 31),
                    ('Delete General Expenses', '?????? ??? ?????', 'General Expenses', 'Delete', 'Delete General Expenses', @MaxOrder + 32);
                
                -- Program Expenses
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Program Expenses', '????????? ??? ?????', 'Program Expenses', 'View', 'View Program Expenses', @MaxOrder + 33),
                    ('Create Program Expenses', '????????? ??? ???? ????', 'Program Expenses', 'Create', 'Create Program Expenses', @MaxOrder + 34),
                    ('Edit Program Expenses', '????????? ??? ???????? ????', 'Program Expenses', 'Edit', 'Edit Program Expenses', @MaxOrder + 35),
                    ('Delete Program Expenses', '????????? ??? ?????', 'Program Expenses', 'Delete', 'Delete Program Expenses', @MaxOrder + 36);
                
                -- Financial Reports
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Financial Reports', '?????? ????????? ?????', 'Financial Reports', 'View', 'View Financial Reports', @MaxOrder + 37),
                    ('Export Financial Reports', '?????? ????????? ????????? ????', 'Financial Reports', 'Export', 'Export Financial Reports', @MaxOrder + 38);
                
                -- Other Incomes
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Other Incomes', '???????? ??? ?????', 'Other Incomes', 'View', 'View Other Incomes', @MaxOrder + 39),
                    ('Create Other Incomes', '???????? ??? ???? ????', 'Other Incomes', 'Create', 'Create Other Incomes', @MaxOrder + 40),
                    ('Edit Other Incomes', '???????? ??? ???????? ????', 'Other Incomes', 'Edit', 'Edit Other Incomes', @MaxOrder + 41),
                    ('Delete Other Incomes', '???????? ??? ?????', 'Other Incomes', 'Delete', 'Delete Other Incomes', @MaxOrder + 42);
                
                -- SMS Management
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View SMS Management', '?????? ??????????? ?????', 'SMS Management', 'View', 'View SMS Management', @MaxOrder + 43),
                    ('Send SMS Management', '?????? ?????', 'SMS Management', 'Send', 'Send SMS', @MaxOrder + 44);
                
                -- Contact Lists
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Contact Lists', '??????? ?????? ?????', 'Contact Lists', 'View', 'View Contact Lists', @MaxOrder + 45),
                    ('Create Contact Lists', '??????? ?????? ???? ????', 'Contact Lists', 'Create', 'Create Contact Lists', @MaxOrder + 46),
                    ('Edit Contact Lists', '??????? ?????? ???????? ????', 'Contact Lists', 'Edit', 'Edit Contact Lists', @MaxOrder + 47),
                    ('Delete Contact Lists', '??????? ?????? ?????', 'Contact Lists', 'Delete', 'Delete Contact Lists', @MaxOrder + 48);
                
                -- Appeals
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Appeals', '????? ?????', 'Appeals', 'View', 'View Appeals', @MaxOrder + 49),
                    ('Create Appeals', '????? ???? ????', 'Appeals', 'Create', 'Create Appeals', @MaxOrder + 50),
                    ('Edit Appeals', '????? ???????? ????', 'Appeals', 'Edit', 'Edit Appeals', @MaxOrder + 51),
                    ('Delete Appeals', '????? ?????', 'Appeals', 'Delete', 'Delete Appeals', @MaxOrder + 52);
                
                -- SDG Programs
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View SDG Programs', 'SDG ????????? ?????', 'SDG Programs', 'View', 'View SDG Programs', @MaxOrder + 53),
                    ('Create SDG Programs', 'SDG ????????? ???? ????', 'SDG Programs', 'Create', 'Create SDG Programs', @MaxOrder + 54),
                    ('Edit SDG Programs', 'SDG ????????? ???????? ????', 'SDG Programs', 'Edit', 'Edit SDG Programs', @MaxOrder + 55),
                    ('Delete SDG Programs', 'SDG ????????? ?????', 'SDG Programs', 'Delete', 'Delete SDG Programs', @MaxOrder + 56);
                
                -- Rohingya Programs
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Rohingya Programs', '??????? ????????? ?????', 'Rohingya Programs', 'View', 'View Rohingya Programs', @MaxOrder + 57),
                    ('Create Rohingya Programs', '??????? ????????? ???? ????', 'Rohingya Programs', 'Create', 'Create Rohingya Programs', @MaxOrder + 58),
                    ('Edit Rohingya Programs', '??????? ????????? ???????? ????', 'Rohingya Programs', 'Edit', 'Edit Rohingya Programs', @MaxOrder + 59),
                    ('Delete Rohingya Programs', '??????? ????????? ?????', 'Rohingya Programs', 'Delete', 'Delete Rohingya Programs', @MaxOrder + 60);
                
                -- Success Stories
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Success Stories', '??????? ??? ?????', 'Success Stories', 'View', 'View Success Stories', @MaxOrder + 61),
                    ('Create Success Stories', '??????? ??? ???? ????', 'Success Stories', 'Create', 'Create Success Stories', @MaxOrder + 62),
                    ('Edit Success Stories', '??????? ??? ???????? ????', 'Success Stories', 'Edit', 'Edit Success Stories', @MaxOrder + 63),
                    ('Delete Success Stories', '??????? ??? ?????', 'Success Stories', 'Delete', 'Delete Success Stories', @MaxOrder + 64);
                
                -- Publications
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Publications', '??????? ?????', 'Publications', 'View', 'View Publications', @MaxOrder + 65),
                    ('Create Publications', '??????? ???? ????', 'Publications', 'Create', 'Create Publications', @MaxOrder + 66),
                    ('Edit Publications', '??????? ???????? ????', 'Publications', 'Edit', 'Edit Publications', @MaxOrder + 67),
                    ('Delete Publications', '??????? ?????', 'Publications', 'Delete', 'Delete Publications', @MaxOrder + 68);
                
                -- Sliders
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Sliders', '???????? ?????', 'Sliders', 'View', 'View Sliders', @MaxOrder + 69),
                    ('Create Sliders', '???????? ???? ????', 'Sliders', 'Create', 'Create Sliders', @MaxOrder + 70),
                    ('Edit Sliders', '???????? ???????? ????', 'Sliders', 'Edit', 'Edit Sliders', @MaxOrder + 71),
                    ('Delete Sliders', '???????? ?????', 'Sliders', 'Delete', 'Delete Sliders', @MaxOrder + 72);
                
                -- Menus
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Menus', '???? ?????', 'Menus', 'View', 'View Menus', @MaxOrder + 73),
                    ('Create Menus', '???? ???? ????', 'Menus', 'Create', 'Create Menus', @MaxOrder + 74),
                    ('Edit Menus', '???? ???????? ????', 'Menus', 'Edit', 'Edit Menus', @MaxOrder + 75),
                    ('Delete Menus', '???? ?????', 'Menus', 'Delete', 'Delete Menus', @MaxOrder + 76);
                
                -- Notification Templates
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Notification Templates', '??????? ???????? ?????', 'Notification Templates', 'View', 'View Notification Templates', @MaxOrder + 77),
                    ('Create Notification Templates', '??????? ???????? ???? ????', 'Notification Templates', 'Create', 'Create Notification Templates', @MaxOrder + 78),
                    ('Edit Notification Templates', '??????? ???????? ???????? ????', 'Notification Templates', 'Edit', 'Edit Notification Templates', @MaxOrder + 79),
                    ('Delete Notification Templates', '??????? ???????? ?????', 'Notification Templates', 'Delete', 'Delete Notification Templates', @MaxOrder + 80);
                
                -- Contact Messages
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Contact Messages', '??????? ????? ?????', 'Contact Messages', 'View', 'View Contact Messages', @MaxOrder + 81),
                    ('Reply Contact Messages', '??????? ????? ????? ???', 'Contact Messages', 'Reply', 'Reply to Contact Messages', @MaxOrder + 82),
                    ('Delete Contact Messages', '??????? ????? ?????', 'Contact Messages', 'Delete', 'Delete Contact Messages', @MaxOrder + 83);
                
                -- Roles Management
                INSERT INTO Permissions (Name, NameBn, Module, Action, Description, DisplayOrder)
                VALUES 
                    ('View Roles', '????? ?????', 'Roles', 'View', 'View Roles', @MaxOrder + 84),
                    ('Create Roles', '????? ???? ????', 'Roles', 'Create', 'Create Roles', @MaxOrder + 85),
                    ('Edit Roles', '????? ???????? ????', 'Roles', 'Edit', 'Edit Roles', @MaxOrder + 86),
                    ('Delete Roles', '????? ?????', 'Roles', 'Delete', 'Delete Roles', @MaxOrder + 87);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove added permissions
            migrationBuilder.Sql(@"
                DELETE FROM Permissions WHERE Module IN (
                    'About SDGs', 'History', 'Legal Status', 'Partners & Sponsors',
                    'Districts', 'Upazilas', 'Accounts', 'General Expenses',
                    'Program Expenses', 'Financial Reports', 'Other Incomes',
                    'SMS Management', 'Contact Lists', 'Appeals', 'SDG Programs',
                    'Rohingya Programs', 'Success Stories', 'Publications', 
                    'Sliders', 'Menus', 'Notification Templates', 'Contact Messages', 'Roles'
                );
            ");
        }
    }
}
