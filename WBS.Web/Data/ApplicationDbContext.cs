using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Models;

namespace WBS.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationType> DonationTypes { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<VideoGallery> VideoGalleries { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<AnnualReport> AnnualReports { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<SDG> SDGs { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<SDGProgram> SDGPrograms { get; set; }
        public DbSet<SDGProject> SDGProjects { get; set; }
        public DbSet<SDGProjectImage> SDGProjectImages { get; set; }
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<CVApplication> CVApplications { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<SmsBalance> SmsBalances { get; set; }
        public DbSet<SmsLog> SmsLogs { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<LegalStatus> LegalStatuses { get; set; }
        public DbSet<RegistrationInfo> RegistrationInfos { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<AboutSDG> AboutSDGs { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Upazila> Upazilas { get; set; }
        public DbSet<DonorTypeCategory> DonorTypeCategories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<OtherIncome> OtherIncomes { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<GeneralExpense> GeneralExpenses { get; set; }
        public DbSet<ProgramExpense> ProgramExpenses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        
        // SMS Management
        public DbSet<ContactGroup> ContactGroups { get; set; }
        public DbSet<ContactListItem> ContactListItems { get; set; }
        public DbSet<SMSCampaign> SMSCampaigns { get; set; }
        public DbSet<SMSCampaignRecipient> SMSCampaignRecipients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Slider entity to use NVARCHAR for Unicode support
            builder.Entity<Slider>(entity =>
            {
                entity.Property(s => s.TitleBn).HasColumnType("nvarchar(200)");
                entity.Property(s => s.SubtitleBn).HasColumnType("nvarchar(500)");
                entity.Property(s => s.ButtonTextBn).HasColumnType("nvarchar(100)");
            });

            // Configure SDG entity to use NVARCHAR for Unicode support
            builder.Entity<SDG>(entity =>
            {
                entity.Property(s => s.NameBn).HasColumnType("nvarchar(200)");
                entity.Property(s => s.DescriptionBn).HasColumnType("nvarchar(max)");
            });

            // Configure Sector entity to use NVARCHAR for Unicode support
            builder.Entity<Sector>(entity =>
            {
                entity.Property(s => s.NameBn).HasColumnType("nvarchar(200)");
                entity.Property(s => s.DescriptionBn).HasColumnType("nvarchar(max)");
            });

            // Configure News entity
            builder.Entity<News>(entity =>
            {
                entity.Ignore(n => n.PublishedAt);
            });

            // Configure decimal precision for Appeal amounts
            builder.Entity<Appeal>(entity =>
            {
                entity.Property(a => a.TargetAmount)
                    .HasPrecision(18, 2);
                entity.Property(a => a.RaisedAmount)
                    .HasPrecision(18, 2);
            });

            // Configure decimal precision for Donation amounts
            builder.Entity<Donation>(entity =>
            {
                entity.Property(d => d.Amount)
                    .HasPrecision(18, 2);
            });

            // Configure decimal precision for SmsLog amounts
            builder.Entity<SmsLog>(entity =>
            {
                entity.Property(s => s.Amount)
                    .HasPrecision(18, 2);
            });

            // Configure decimal precision for Event ticket prices
            builder.Entity<Event>(entity =>
            {
                entity.Property(e => e.TicketPrice)
                    .HasPrecision(18, 2);
            });

            // Configure decimal precision for EventRegistration amounts
            builder.Entity<EventRegistration>(entity =>
            {
                entity.Property(e => e.AmountPaid)
                    .HasPrecision(18, 2);
            });

            // Menu self-referencing relationship
            builder.Entity<Menu>()
                .HasOne(m => m.ParentMenu)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(m => m.ParentMenuId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed default donation types
            builder.Entity<DonationType>().HasData(
                new DonationType { Id = 1, Name = "Lillah", NameBn = "লিল্লাহ", Description = "Voluntary charity", IsActive = true, DisplayOrder = 1 },
                new DonationType { Id = 2, Name = "Zakat", NameBn = "যাকাত", Description = "Obligatory charity", IsActive = true, DisplayOrder = 2 },
                new DonationType { Id = 3, Name = "Sadaqah Jariyah", NameBn = "সাদাকাহ জারিয়াহ", Description = "Continuous charity", IsActive = true, DisplayOrder = 3 },
                new DonationType { Id = 4, Name = "Winter Appeal", NameBn = "শীতকালীন আবেদন", Description = "Winter Appeal", IsActive = true, DisplayOrder = 4 },
                new DonationType { Id = 5, Name = "Emergency Appeal", NameBn = "জরুরী আবেদন", Description = "Emergency Appeal", IsActive = true, DisplayOrder = 5 }
            );

            // Seed SDGs
            builder.Entity<SDG>().HasData(
                new SDG { Id = 1, Number = 1, Name = "No Poverty", NameBn = "দারিদ্র্য নিরসন", Color = "#E5243B", IsActive = true },
                new SDG { Id = 2, Number = 2, Name = "Zero Hunger", NameBn = "ক্ষুধামুক্তি", Color = "#DDA63A", IsActive = true },
                new SDG { Id = 3, Number = 3, Name = "Good Health and Well-being", NameBn = "সুস্বাস্থ্য ও কল্যাণ", Color = "#4C9F38", IsActive = true },
                new SDG { Id = 4, Number = 4, Name = "Quality Education", NameBn = "মানসম্মত শিক্ষা", Color = "#C5192D", IsActive = true },
                new SDG { Id = 6, Number = 6, Name = "Clean Water and Sanitation", NameBn = "নিরাপদ পানি ও স্যানিটেশন", Color = "#26BDE2", IsActive = true },
                new SDG { Id = 7, Number = 7, Name = "Affordable and Clean Energy", NameBn = "সাশ্রয়ী ও টেকসই জ্বালানি", Color = "#FCC30B", IsActive = true },
                new SDG { Id = 11, Number = 11, Name = "Sustainable Cities and Communities", NameBn = "টেকসই শহর ও সম্প্রদায়", Color = "#FD9D24", IsActive = true },
                new SDG { Id = 13, Number = 13, Name = "Climate Action", NameBn = "জলবায়ু পদক্ষেপ", Color = "#3F7E44", IsActive = true },
                new SDG { Id = 14, Number = 14, Name = "Life Below Water", NameBn = "জলজ জীবন রক্ষা", Color = "#0A97D9", IsActive = true },
                new SDG { Id = 15, Number = 15, Name = "Life on Land", NameBn = "স্থলজ জীবন", Color = "#56C02B", IsActive = true }
            );

            // Seed initial SMS balance
            builder.Entity<SmsBalance>().HasData(
                new SmsBalance 
                { 
                    Id = 1, 
                    AvailableBalance = 0, 
                    LastUpdated = DateTime.UtcNow, 
                    UpdatedBy = "System",
                    Notes = "Initial SMS balance setup. Please update with actual balance."
                }
            );

            // Seed Donor Types
            builder.Entity<DonorTypeCategory>().HasData(
                new DonorTypeCategory { Id = 1, Name = "Regular", NameBn = "নিয়মিত দাতা", Description = "Regular Donor", IsActive = true, IsVisible = true, DisplayOrder = 1 },
                new DonorTypeCategory { Id = 2, Name = "Monthly", NameBn = "মাসিক দাতা", Description = "Monthly recurring donor", IsActive = true, IsVisible = true, DisplayOrder = 2 },
                new DonorTypeCategory { Id = 3, Name = "Daily", NameBn = "দৈনিক দাতা", Description = "Daily recurring donor", IsActive = true, IsVisible = true, DisplayOrder = 3 },
                new DonorTypeCategory { Id = 4, Name = "Yearly", NameBn = "বার্ষিক দাতা", Description = "Yearly recurring donor", IsActive = true, IsVisible = true, DisplayOrder = 4 },
                new DonorTypeCategory { Id = 5, Name = "Lifetime", NameBn = "আজীবন দাতা", Description = "Lifetime donor", IsActive = true, IsVisible = true, DisplayOrder = 5 },
                new DonorTypeCategory { Id = 6, Name = "Corporate", NameBn = "প্রাতিষ্ঠানিক দাতা", Description = "Corporate or institutional donor", IsActive = true, IsVisible = true, DisplayOrder = 6 },
                new DonorTypeCategory { Id = 7, Name = "One Time", NameBn = "একবারের দাতা", Description = "One-time donor", IsActive = true, IsVisible = true, DisplayOrder = 7 }
            );

            // Seed Payment Methods
            builder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { Id = 1, Name = "Cash", NameBn = "নগদ টাকা", Description = "Cash payment", Icon = "fas fa-money-bill-wave", IsActive = true, DisplayOrder = 1 },
                new PaymentMethod { Id = 2, Name = "bKash", NameBn = "বিকাশ", Description = "bKash mobile banking", Icon = "fab fa-bitcoin", IsActive = true, DisplayOrder = 2 },
                new PaymentMethod { Id = 3, Name = "Nagad", NameBn = "নগদ", Description = "Nagad mobile banking", Icon = "fas fa-mobile-alt", IsActive = true, DisplayOrder = 3 },
                new PaymentMethod { Id = 4, Name = "Bank Transfer", NameBn = "ব্যাংক স্থানান্তর", Description = "Bank transfer", Icon = "fas fa-university", IsActive = true, DisplayOrder = 4 },
                new PaymentMethod { Id = 5, Name = "Cheque", NameBn = "চেক", Description = "Cheque payment", Icon = "fas fa-money-check", IsActive = true, DisplayOrder = 5 },
                new PaymentMethod { Id = 6, Name = "Credit/Debit Card", NameBn = "ক্রেডিট/ডেবিট কার্ড", Description = "Card payment", Icon = "fas fa-credit-card", IsActive = true, DisplayOrder = 6 }
            );

            // Seed Default Account
            builder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    AccountName = "Main Account",
                    AccountNameBn = "মূল একাউন্ট",
                    AccountType = "Cash",
                    Description = "Default main account for donations",
                    DescriptionBn = "দানের জন্য ডিফল্ট উত্তরাধিকার একাউন্ট",
                    AccountBalance = 0,
                    Total_IN = 0,
                    Total_OUT = 0,
                    Total_Income = 0,
                    Total_Expense = 0,
                    Deleted_Income = 0,
                    Deleted_Expense = 0,
                    Default_Status = true,
                    IsActive = true,
                    DisplayOrder = 1,
                    AccountCreateDate = DateTime.UtcNow
                }
            );

            // Configure RolePermission composite key
            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Permissions
            SeedPermissions(builder);
        }

        private void SeedPermissions(ModelBuilder builder)
        {
            var permissions = new List<Permission>();
            int id = 1;

            // Dashboard - Note: Dashboard View is available for all authenticated users by default
            // This permission is optional and can be used for reporting purposes
            permissions.Add(new Permission { Id = id++, Module = "Dashboard", Action = "View", Name = "View Dashboard", NameBn = "ড্যাশবোর্ড দেখুন", Description = "Optional - Dashboard is accessible to all authenticated users", DisplayOrder = 1 });

            // Users
            permissions.Add(new Permission { Id = id++, Module = "Users", Action = "View", Name = "View Users", NameBn = "ব্যবহারকারী দেখুন", DisplayOrder = 2 });
            permissions.Add(new Permission { Id = id++, Module = "Users", Action = "Create", Name = "Create User", NameBn = "ব্যবহারকারী তৈরি করুন", DisplayOrder = 3 });
            permissions.Add(new Permission { Id = id++, Module = "Users", Action = "Edit", Name = "Edit User", NameBn = "ব্যবহারকারী সম্পাদনা করুন", DisplayOrder = 4 });
            permissions.Add(new Permission { Id = id++, Module = "Users", Action = "Delete", Name = "Delete User", NameBn = "ব্যবহারকারী মুছুন", DisplayOrder = 5 });

            // Roles
            permissions.Add(new Permission { Id = id++, Module = "Roles", Action = "View", Name = "View Roles", NameBn = "ভূমিকা দেখুন", DisplayOrder = 6 });
            permissions.Add(new Permission { Id = id++, Module = "Roles", Action = "Create", Name = "Create Role", NameBn = "ভূমিকা তৈরি করুন", DisplayOrder = 7 });
            permissions.Add(new Permission { Id = id++, Module = "Roles", Action = "Edit", Name = "Edit Role", NameBn = "ভূমিকা সম্পাদনা করুন", DisplayOrder = 8 });
            permissions.Add(new Permission { Id = id++, Module = "Roles", Action = "Delete", Name = "Delete Role", NameBn = "ভূমিকা মুছুন", DisplayOrder = 9 });

            // Donations
            permissions.Add(new Permission { Id = id++, Module = "Donations", Action = "View", Name = "View Donations", NameBn = "দান দেখুন", DisplayOrder = 10 });
            permissions.Add(new Permission { Id = id++, Module = "Donations", Action = "Create", Name = "Create Donation", NameBn = "দান তৈরি করুন", DisplayOrder = 11 });
            permissions.Add(new Permission { Id = id++, Module = "Donations", Action = "Edit", Name = "Edit Donation", NameBn = "দান সম্পাদনা করুন", DisplayOrder = 12 });
            permissions.Add(new Permission { Id = id++, Module = "Donations", Action = "Delete", Name = "Delete Donation", NameBn = "দান মুছুন", DisplayOrder = 13 });

            // Pages
            permissions.Add(new Permission { Id = id++, Module = "Pages", Action = "View", Name = "View Pages", NameBn = "পৃষ্ঠা দেখুন", DisplayOrder = 14 });
            permissions.Add(new Permission { Id = id++, Module = "Pages", Action = "Create", Name = "Create Page", NameBn = "পৃষ্ঠা তৈরি করুন", DisplayOrder = 15 });
            permissions.Add(new Permission { Id = id++, Module = "Pages", Action = "Edit", Name = "Edit Page", NameBn = "পৃষ্ঠা সম্পাদনা করুন", DisplayOrder = 16 });
            permissions.Add(new Permission { Id = id++, Module = "Pages", Action = "Delete", Name = "Delete Page", NameBn = "পৃষ্ঠা মুছুন", DisplayOrder = 17 });

            // News
            permissions.Add(new Permission { Id = id++, Module = "News", Action = "View", Name = "View News", NameBn = "সংবাদ দেখুন", DisplayOrder = 18 });
            permissions.Add(new Permission { Id = id++, Module = "News", Action = "Create", Name = "Create News", NameBn = "সংবাদ তৈরি করুন", DisplayOrder = 19 });
            permissions.Add(new Permission { Id = id++, Module = "News", Action = "Edit", Name = "Edit News", NameBn = "সংবাদ সম্পাদনা করুন", DisplayOrder = 20 });
            permissions.Add(new Permission { Id = id++, Module = "News", Action = "Delete", Name = "Delete News", NameBn = "সংবাদ মুছুন", DisplayOrder = 21 });

            // Events
            permissions.Add(new Permission { Id = id++, Module = "Events", Action = "View", Name = "View Events", NameBn = "ইভেন্ট দেখুন", DisplayOrder = 22 });
            permissions.Add(new Permission { Id = id++, Module = "Events", Action = "Create", Name = "Create Event", NameBn = "ইভেন্ট তৈরি করুন", DisplayOrder = 23 });
            permissions.Add(new Permission { Id = id++, Module = "Events", Action = "Edit", Name = "Edit Event", NameBn = "ইভেন্ট সম্পাদনা করুন", DisplayOrder = 24 });
            permissions.Add(new Permission { Id = id++, Module = "Events", Action = "Delete", Name = "Delete Event", NameBn = "ইভেন্ট মুছুন", DisplayOrder = 25 });

            // Volunteers
            permissions.Add(new Permission { Id = id++, Module = "Volunteers", Action = "View", Name = "View Volunteers", NameBn = "স্বেচ্ছাসেবক দেখুন", DisplayOrder = 26 });
            permissions.Add(new Permission { Id = id++, Module = "Volunteers", Action = "Edit", Name = "Edit Volunteer", NameBn = "স্বেচ্ছাসেবক সম্পাদনা করুন", DisplayOrder = 27 });
            permissions.Add(new Permission { Id = id++, Module = "Volunteers", Action = "Delete", Name = "Delete Volunteer", NameBn = "স্বেচ্ছাসেবক মুছুন", DisplayOrder = 28 });

            // Galleries
            permissions.Add(new Permission { Id = id++, Module = "Galleries", Action = "View", Name = "View Galleries", NameBn = "গ্যালারি দেখুন", DisplayOrder = 29 });
            permissions.Add(new Permission { Id = id++, Module = "Galleries", Action = "Create", Name = "Create Gallery", NameBn = "গ্যালারি তৈরি করুন", DisplayOrder = 30 });
            permissions.Add(new Permission { Id = id++, Module = "Galleries", Action = "Edit", Name = "Edit Gallery", NameBn = "গ্যালারি সম্পাদনা করুন", DisplayOrder = 31 });
            permissions.Add(new Permission { Id = id++, Module = "Galleries", Action = "Delete", Name = "Delete Gallery", NameBn = "গ্যালারি মুছুন", DisplayOrder = 32 });

            // Settings
            permissions.Add(new Permission { Id = id++, Module = "Settings", Action = "View", Name = "View Settings", NameBn = "সেটিংস দেখুন", DisplayOrder = 33 });
            permissions.Add(new Permission { Id = id++, Module = "Settings", Action = "Edit", Name = "Edit Settings", NameBn = "সেটিংস সম্পাদনা করুন", DisplayOrder = 34 });

            builder.Entity<Permission>().HasData(permissions);
        }
    }
}
