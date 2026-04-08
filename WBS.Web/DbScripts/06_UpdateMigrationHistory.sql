-- Update EF Migrations History for manually created database
USE WBS_NGO;
GO

-- Check if __EFMigrationsHistory table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory](
        [MigrationId] [nvarchar](150) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
    );
    PRINT '? __EFMigrationsHistory table created';
END
GO

-- Add migration records
IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20251228120231_InitialCreate')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20251228120231_InitialCreate', '8.0.0');
END

IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20251230062324_FixBengaliTextSupport')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20251230062324_FixBengaliTextSupport', '8.0.0');
END

IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20251230101005_UpdateBengaliSeedData')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20251230101005_UpdateBengaliSeedData', '8.0.0');
END

IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20251230103814_FixDecimalPrecision')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20251230103814_FixDecimalPrecision', '8.0.0');
END

PRINT '? Migration history updated';

-- Verify
SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId;
GO
