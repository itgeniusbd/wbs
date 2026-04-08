-- Remove the incorrect migration entry
DELETE FROM __EFMigrationsHistory 
WHERE MigrationId = '20260108081815_AddNewsColumns';

-- Verify deletion
SELECT * FROM __EFMigrationsHistory 
ORDER BY MigrationId DESC;
