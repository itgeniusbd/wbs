-- =======================================================
-- WBS_NGO Database - Complete Manual Setup Script
-- Run this ENTIRE script in SQL Server Management Studio
-- Server: DESKTOP-JUNKIQI\LA_SATTAR-PC
-- Database: WBS_NGO
-- 
-- This script will:
-- 1. Create all tables
-- 2. Insert seed data (Donation Types, SDGs)
-- 3. Setup Identity tables
-- =======================================================

USE WBS_NGO;
GO

PRINT '========================================='
PRINT 'WBS_NGO Database Setup Starting...'
PRINT '========================================='
PRINT ''

-- =======================================================
-- PART 1: ASP.NET Identity Tables
-- =======================================================

PRINT '1. Creating ASP.NET Identity Tables...'

-- AspNetRoles
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoles](
        [Id] [nvarchar](450) NOT NULL,
        [Name] [nvarchar](256) NULL,
        [NormalizedName] [nvarchar](256) NULL,
        [ConcurrencyStamp] [nvarchar](max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);
    PRINT '   ? AspNetRoles created';
END

-- AspNetUsers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUsers](
        [Id] [nvarchar](450) NOT NULL,
        [UserName] [nvarchar](256) NULL,
        [NormalizedUserName] [nvarchar](256) NULL,
        [Email] [nvarchar](256) NULL,
        [NormalizedEmail] [nvarchar](256) NULL,
        [EmailConfirmed] [bit] NOT NULL,
        [PasswordHash] [nvarchar](max) NULL,
        [SecurityStamp] [nvarchar](max) NULL,
        [ConcurrencyStamp] [nvarchar](max) NULL,
        [PhoneNumber] [nvarchar](max) NULL,
        [PhoneNumberConfirmed] [bit] NOT NULL,
        [TwoFactorEnabled] [bit] NOT NULL,
        [LockoutEnd] [datetimeoffset](7) NULL,
        [LockoutEnabled] [bit] NOT NULL,
        [AccessFailedCount] [int] NOT NULL,
        [Discriminator] [nvarchar](21) NOT NULL DEFAULT N'IdentityUser',
        [FirstName] [nvarchar](100) NULL,
        [LastName] [nvarchar](100) NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);
    CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);
    PRINT '   ? AspNetUsers created';
END

-- AspNetUserRoles
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserRoles](
        [UserId] [nvarchar](450) NOT NULL,
        [RoleId] [nvarchar](450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]([RoleId] ASC);
    PRINT '   ? AspNetUserRoles created';
END

-- AspNetUserClaims
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [nvarchar](450) NOT NULL,
        [ClaimType] [nvarchar](max) NULL,
        [ClaimValue] [nvarchar](max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]([UserId] ASC);
    PRINT '   ? AspNetUserClaims created';
END

-- AspNetUserLogins
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins](
        [LoginProvider] [nvarchar](450) NOT NULL,
        [ProviderKey] [nvarchar](450) NOT NULL,
        [ProviderDisplayName] [nvarchar](max) NULL,
        [UserId] [nvarchar](450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]([UserId] ASC);
    PRINT '   ? AspNetUserLogins created';
END

-- AspNetUserTokens
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserTokens](
        [UserId] [nvarchar](450) NOT NULL,
        [LoginProvider] [nvarchar](450) NOT NULL,
        [Name] [nvarchar](450) NOT NULL,
        [Value] [nvarchar](max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT '   ? AspNetUserTokens created';
END

-- AspNetRoleClaims
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoleClaims](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [RoleId] [nvarchar](450) NOT NULL,
        [ClaimType] [nvarchar](max) NULL,
        [ClaimValue] [nvarchar](max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]([RoleId] ASC);
    PRINT '   ? AspNetRoleClaims created';
END

PRINT '   ? Identity tables completed'
PRINT ''

-- =======================================================
-- PART 2: Application Tables
-- =======================================================

PRINT '2. Creating Application Tables...'

-- DonationTypes
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DonationTypes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DonationTypes](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [NameBn] [nvarchar](100) NULL,
        [Description] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        CONSTRAINT [PK_DonationTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? DonationTypes created';
END

-- SDGs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SDGs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SDGs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Number] [int] NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [NameBn] [nvarchar](200) NULL,
        [Description] [nvarchar](max) NULL,
        [Color] [nvarchar](50) NULL,
        [Icon] [nvarchar](200) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_SDGs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? SDGs created';
END

-- Appeals
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appeals]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Appeals](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](300) NOT NULL,
        [TitleBn] [nvarchar](300) NULL,
        [Slug] [nvarchar](300) NOT NULL,
        [Summary] [nvarchar](max) NULL,
        [SummaryBn] [nvarchar](max) NULL,
        [Content] [nvarchar](max) NULL,
        [ContentBn] [nvarchar](max) NULL,
        [FeaturedImage] [nvarchar](500) NULL,
        [BannerImage] [nvarchar](500) NULL,
        [TargetAmount] [decimal](18, 2) NULL,
        [RaisedAmount] [decimal](18, 2) NOT NULL DEFAULT 0,
        [StartDate] [datetime2](7) NULL,
        [EndDate] [datetime2](7) NULL,
        [AppealType] [nvarchar](100) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsFeatured] [bit] NOT NULL DEFAULT 0,
        [IsUrgent] [bit] NOT NULL DEFAULT 0,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_Appeals] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Appeals created';
END

-- Donations
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Donations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Donations](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [DonorName] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Phone] [nvarchar](20) NULL,
        [Address] [nvarchar](200) NULL,
        [Amount] [decimal](18, 2) NOT NULL,
        [Currency] [nvarchar](10) NOT NULL DEFAULT 'BDT',
        [DonationTypeId] [int] NOT NULL,
        [AppealId] [int] NULL,
        [PaymentMethod] [nvarchar](50) NOT NULL,
        [TransactionId] [nvarchar](100) NULL,
        [Status] [int] NOT NULL DEFAULT 0,
        [IsRecurring] [bit] NOT NULL DEFAULT 0,
        [RecurringFrequency] [nvarchar](50) NULL,
        [IsAnonymous] [bit] NOT NULL DEFAULT 0,
        [Notes] [nvarchar](max) NULL,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [PaidAt] [datetime2](7) NULL,
        CONSTRAINT [PK_Donations] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Donations_DonationTypes_DonationTypeId] FOREIGN KEY([DonationTypeId]) REFERENCES [dbo].[DonationTypes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Donations_Appeals_AppealId] FOREIGN KEY([AppealId]) REFERENCES [dbo].[Appeals] ([Id])
    );
    CREATE NONCLUSTERED INDEX [IX_Donations_DonationTypeId] ON [dbo].[Donations]([DonationTypeId] ASC);
    CREATE NONCLUSTERED INDEX [IX_Donations_AppealId] ON [dbo].[Donations]([AppealId] ASC);
    PRINT '   ? Donations created';
END

-- Menus
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Menus]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Menus](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [NameBn] [nvarchar](100) NULL,
        [Url] [nvarchar](500) NULL,
        [ParentMenuId] [int] NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [OpenInNewTab] [bit] NOT NULL DEFAULT 0,
        [Icon] [nvarchar](100) NULL,
        CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Menus_Menus_ParentMenuId] FOREIGN KEY([ParentMenuId]) REFERENCES [dbo].[Menus] ([Id])
    );
    CREATE NONCLUSTERED INDEX [IX_Menus_ParentMenuId] ON [dbo].[Menus]([ParentMenuId] ASC);
    PRINT '   ? Menus created';
END

-- Pages
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Pages](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](200) NOT NULL,
        [TitleBn] [nvarchar](200) NULL,
        [Slug] [nvarchar](200) NOT NULL,
        [Content] [nvarchar](max) NULL,
        [ContentBn] [nvarchar](max) NULL,
        [MetaTitle] [nvarchar](200) NULL,
        [MetaDescription] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Pages created';
END

-- Continue with remaining tables...
-- (This is getting long, so I'll create them in groups)

PRINT '   Creating content tables...'

-- News
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[News]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[News](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](300) NOT NULL,
        [TitleBn] [nvarchar](300) NULL,
        [Slug] [nvarchar](300) NOT NULL,
        [Summary] [nvarchar](max) NULL,
        [SummaryBn] [nvarchar](max) NULL,
        [Content] [nvarchar](max) NULL,
        [ContentBn] [nvarchar](max) NULL,
        [FeaturedImage] [nvarchar](500) NULL,
        [PublishedDate] [datetime2](7) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsFeatured] [bit] NOT NULL DEFAULT 0,
        [ViewCount] [int] NOT NULL DEFAULT 0,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? News created';
END

-- Events
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Events](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](300) NOT NULL,
        [TitleBn] [nvarchar](300) NULL,
        [Slug] [nvarchar](300) NOT NULL,
        [Description] [nvarchar](max) NULL,
        [DescriptionBn] [nvarchar](max) NULL,
        [FeaturedImage] [nvarchar](500) NULL,
        [EventDate] [datetime2](7) NOT NULL,
        [Location] [nvarchar](300) NULL,
        [LocationBn] [nvarchar](300) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsFeatured] [bit] NOT NULL DEFAULT 0,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Events created';
END

-- Stories
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Stories]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Stories](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](300) NOT NULL,
        [TitleBn] [nvarchar](300) NULL,
        [Slug] [nvarchar](300) NOT NULL,
        [Content] [nvarchar](max) NULL,
        [ContentBn] [nvarchar](max) NULL,
        [FeaturedImage] [nvarchar](500) NULL,
        [PersonName] [nvarchar](200) NULL,
        [PersonNameBn] [nvarchar](200) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsFeatured] [bit] NOT NULL DEFAULT 0,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Stories] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Stories created';
END

-- Sliders
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sliders]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sliders](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](200) NULL,
        [TitleBn] [nvarchar](200) NULL,
        [Subtitle] [nvarchar](500) NULL,
        [SubtitleBn] [nvarchar](500) NULL,
        [ImageUrl] [nvarchar](500) NOT NULL,
        [LinkUrl] [nvarchar](500) NULL,
        [ButtonText] [nvarchar](100) NULL,
        [ButtonTextBn] [nvarchar](100) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Sliders] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Sliders created';
END

-- Galleries
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Galleries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Galleries](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](200) NOT NULL,
        [TitleBn] [nvarchar](200) NULL,
        [Description] [nvarchar](500) NULL,
        [DescriptionBn] [nvarchar](500) NULL,
        [CoverImage] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Galleries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Galleries created';
END

-- GalleryImages
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GalleryImages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[GalleryImages](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [GalleryId] [int] NOT NULL,
        [ImageUrl] [nvarchar](500) NOT NULL,
        [Caption] [nvarchar](300) NULL,
        [CaptionBn] [nvarchar](300) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        CONSTRAINT [PK_GalleryImages] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_GalleryImages_Galleries_GalleryId] FOREIGN KEY([GalleryId]) REFERENCES [dbo].[Galleries] ([Id]) ON DELETE CASCADE
    );
    CREATE NONCLUSTERED INDEX [IX_GalleryImages_GalleryId] ON [dbo].[GalleryImages]([GalleryId] ASC);
    PRINT '   ? GalleryImages created';
END

-- Publications
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Publications]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Publications](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](300) NOT NULL,
        [TitleBn] [nvarchar](300) NULL,
        [Description] [nvarchar](max) NULL,
        [DescriptionBn] [nvarchar](max) NULL,
        [FileUrl] [nvarchar](500) NOT NULL,
        [CoverImage] [nvarchar](500) NULL,
        [PublishedDate] [datetime2](7) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Publications] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Publications created';
END

-- AnnualReports
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AnnualReports]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AnnualReports](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Year] [int] NOT NULL,
        [Title] [nvarchar](200) NOT NULL,
        [TitleBn] [nvarchar](200) NULL,
        [FileUrl] [nvarchar](500) NOT NULL,
        [CoverImage] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_AnnualReports] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? AnnualReports created';
END

-- Partners
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Partners]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Partners](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [NameBn] [nvarchar](200) NULL,
        [LogoUrl] [nvarchar](500) NULL,
        [Website] [nvarchar](500) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Partners] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Partners created';
END

-- Sectors
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sectors]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sectors](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [NameBn] [nvarchar](200) NULL,
        [Description] [nvarchar](max) NULL,
        [DescriptionBn] [nvarchar](max) NULL,
        [Icon] [nvarchar](200) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Sectors] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Sectors created';
END

-- Volunteers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Volunteers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Volunteers](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [FirstName] [nvarchar](100) NOT NULL,
        [LastName] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Phone] [nvarchar](20) NOT NULL,
        [Address] [nvarchar](500) NULL,
        [Skills] [nvarchar](500) NULL,
        [Message] [nvarchar](max) NULL,
        [Status] [nvarchar](50) NOT NULL DEFAULT 'Pending',
        [AppliedDate] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Volunteers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Volunteers created';
END

-- Careers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Careers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Careers](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](200) NOT NULL,
        [TitleBn] [nvarchar](200) NULL,
        [Description] [nvarchar](max) NOT NULL,
        [DescriptionBn] [nvarchar](max) NULL,
        [Location] [nvarchar](200) NULL,
        [JobType] [nvarchar](100) NULL,
        [Deadline] [datetime2](7) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [PostedDate] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Careers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? Careers created';
END

-- ContactMessages
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactMessages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ContactMessages](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Phone] [nvarchar](20) NULL,
        [Subject] [nvarchar](200) NULL,
        [Message] [nvarchar](max) NOT NULL,
        [IsRead] [bit] NOT NULL DEFAULT 0,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_ContactMessages] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? ContactMessages created';
END

-- SiteSettings
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteSettings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SiteSettings](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [SiteName] [nvarchar](200) NULL,
        [SiteNameBn] [nvarchar](200) NULL,
        [LogoUrl] [nvarchar](500) NULL,
        [FaviconUrl] [nvarchar](500) NULL,
        [Email] [nvarchar](100) NULL,
        [Phone] [nvarchar](20) NULL,
        [Address] [nvarchar](500) NULL,
        [AddressBn] [nvarchar](500) NULL,
        [FacebookUrl] [nvarchar](500) NULL,
        [TwitterUrl] [nvarchar](500) NULL,
        [InstagramUrl] [nvarchar](500) NULL,
        [YoutubeUrl] [nvarchar](500) NULL,
        [LinkedInUrl] [nvarchar](500) NULL,
        [FooterText] [nvarchar](1000) NULL,
        [FooterTextBn] [nvarchar](1000) NULL,
        CONSTRAINT [PK_SiteSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT '   ? SiteSettings created';
END

-- EF Migrations History
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory](
        [MigrationId] [nvarchar](150) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
    );
    PRINT '   ? __EFMigrationsHistory created';
END

PRINT '   ? Application tables completed'
PRINT ''

-- =======================================================
-- PART 3: Seed Data
-- =======================================================

PRINT '3. Inserting Seed Data...'

-- Seed DonationTypes
IF NOT EXISTS (SELECT * FROM DonationTypes)
BEGIN
    SET IDENTITY_INSERT [dbo].[DonationTypes] ON;
    
    INSERT INTO [dbo].[DonationTypes] ([Id], [Name], [NameBn], [Description], [IsActive], [DisplayOrder])
    VALUES 
        (1, N'Lillah', N'???????', N'Voluntary charity', 1, 1),
        (2, N'Zakat', N'?????', N'Obligatory charity', 1, 2),
        (3, N'Sadaqah Jariyah', N'??????? ????????', N'Continuous charity', 1, 3),
        (4, N'Winter Appeal', N'???????? ?????', N'Winter Appeal', 1, 4),
        (5, N'Emergency Appeal', N'????? ?????', N'Emergency Appeal', 1, 5);
    
    SET IDENTITY_INSERT [dbo].[DonationTypes] OFF;
    PRINT '   ? 5 Donation Types inserted';
END
ELSE
    PRINT '   ? Donation Types already exist';

-- Seed SDGs
IF NOT EXISTS (SELECT * FROM SDGs)
BEGIN
    SET IDENTITY_INSERT [dbo].[SDGs] ON;
    
    INSERT INTO [dbo].[SDGs] ([Id], [Number], [Name], [NameBn], [Color], [IsActive])
    VALUES 
        (1, 1, N'No Poverty', N'????????? ??????', N'#E5243B', 1),
        (2, 2, N'Zero Hunger', N'?????? ??????', N'#DDA63A', 1),
        (3, 3, N'Good Health and Well-being', N'??????????? ? ??????', N'#4C9F38', 1),
        (4, 4, N'Quality Education', N'???????? ??????', N'#C5192D', 1),
        (5, 5, N'Gender Equality', N'????? ????', N'#FF3A21', 1),
        (6, 6, N'Clean Water and Sanitation', N'??????? ???? ? ??????????', N'#26BDE2', 1);
    
    SET IDENTITY_INSERT [dbo].[SDGs] OFF;
    PRINT '   ? 6 SDG Goals inserted';
END
ELSE
    PRINT '   ? SDG Goals already exist';

-- Insert Migration History
IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20251230120000_ManualSetup')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20251230120000_ManualSetup', '8.0.0');
    PRINT '   ? Migration history recorded';
END

PRINT '   ? Seed data completed'
PRINT ''

-- =======================================================
-- PART 4: Verification
-- =======================================================

PRINT '4. Verifying Database Setup...'
PRINT ''

DECLARE @TableCount INT
SELECT @TableCount = COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'

PRINT '   Total Tables Created: ' + CAST(@TableCount AS VARCHAR)
PRINT ''

-- List all tables
PRINT '   Tables List:'
SELECT 
    ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS [#],
    TABLE_NAME AS [Table Name]
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

PRINT ''

-- Check seed data
DECLARE @DonationTypeCount INT, @SDGCount INT
SELECT @DonationTypeCount = COUNT(*) FROM DonationTypes
SELECT @SDGCount = COUNT(*) FROM SDGs

PRINT '   Seed Data:'
PRINT '   - Donation Types: ' + CAST(@DonationTypeCount AS VARCHAR)
PRINT '   - SDG Goals: ' + CAST(@SDGCount AS VARCHAR)
PRINT ''

-- Summary
PRINT '========================================='
PRINT '? Database Setup Complete!'
PRINT '========================================='
PRINT ''
PRINT '?? Database: WBS_NGO'
PRINT '?? Tables: ' + CAST(@TableCount AS VARCHAR)
PRINT '?? Seed Data: Populated'
PRINT ''
PRINT '?? Next Steps:'
PRINT '1. Run your application (dotnet run or F5)'
PRINT '2. Admin user will be created automatically'
PRINT '3. Login at: /Account/Login'
PRINT '   Email: admin@wbs.org'
PRINT '   Password: Admin@123'
PRINT ''
PRINT '? Setup completed successfully!'
GO
