-- Cleanup Duplicate Permissions Script
-- This script removes duplicate permissions and keeps only one instance of each

BEGIN TRANSACTION;

PRINT 'Starting cleanup of duplicate permissions...';

-- Step 1: Check for duplicates
PRINT '';
PRINT '=== Checking for duplicate permissions ===';
SELECT 
    Module, 
    Action, 
    COUNT(*) as DuplicateCount
FROM Permissions
GROUP BY Module, Action
HAVING COUNT(*) > 1
ORDER BY Module, Action;

-- Step 2: Create a temporary table to store the IDs we want to keep
IF OBJECT_ID('tempdb..#PermissionsToKeep') IS NOT NULL DROP TABLE #PermissionsToKeep;

SELECT 
    MIN(Id) as IdToKeep,
    Module,
    Action
INTO #PermissionsToKeep
FROM Permissions
GROUP BY Module, Action;

PRINT '';
PRINT '=== Permissions to keep (lowest ID for each Module-Action pair) ===';
SELECT * FROM #PermissionsToKeep ORDER BY Module, Action;

-- Step 3: Find all duplicate IDs that need to be deleted
IF OBJECT_ID('tempdb..#PermissionsToDelete') IS NOT NULL DROP TABLE #PermissionsToDelete;

SELECT p.Id, p.Module, p.Action, p.Name
INTO #PermissionsToDelete
FROM Permissions p
WHERE NOT EXISTS (
    SELECT 1 
    FROM #PermissionsToKeep ptk 
    WHERE ptk.IdToKeep = p.Id
)
AND EXISTS (
    SELECT 1
    FROM Permissions p2
    WHERE p2.Module = p.Module 
    AND p2.Action = p.Action
    AND p2.Id < p.Id
);

PRINT '';
PRINT '=== Permissions marked for deletion (duplicates) ===';
SELECT * FROM #PermissionsToDelete ORDER BY Module, Action;

DECLARE @DuplicateCount INT;
SELECT @DuplicateCount = COUNT(*) FROM #PermissionsToDelete;
PRINT '';
PRINT 'Total duplicate permissions to delete: ' + CAST(@DuplicateCount AS VARCHAR(10));

-- Step 4: Update RolePermissions to point to the kept permission
PRINT '';
PRINT '=== Updating RolePermissions references ===';

UPDATE rp
SET rp.PermissionId = ptk.IdToKeep
FROM RolePermissions rp
INNER JOIN #PermissionsToDelete ptd ON rp.PermissionId = ptd.Id
INNER JOIN #PermissionsToKeep ptk ON ptd.Module = ptk.Module AND ptd.Action = ptk.Action
WHERE rp.PermissionId = ptd.Id;

DECLARE @UpdatedRolePermissions INT = @@ROWCOUNT;
PRINT 'Updated ' + CAST(@UpdatedRolePermissions AS VARCHAR(10)) + ' RolePermissions references';

-- Step 5: Remove duplicate RolePermissions (same role-permission combination)
PRINT '';
PRINT '=== Removing duplicate RolePermissions ===';

-- First, let's see if there are any duplicates
SELECT 
    RoleId,
    PermissionId,
    COUNT(*) as DuplicateCount
INTO #DuplicateRolePermissions
FROM RolePermissions
GROUP BY RoleId, PermissionId
HAVING COUNT(*) > 1;

DECLARE @DuplicateRPCount INT;
SELECT @DuplicateRPCount = COUNT(*) FROM #DuplicateRolePermissions;

IF @DuplicateRPCount > 0
BEGIN
    PRINT 'Found ' + CAST(@DuplicateRPCount AS VARCHAR(10)) + ' duplicate RolePermission combinations';
    
    -- Delete duplicates, keeping only one of each RoleId-PermissionId pair
    DELETE rp
    FROM RolePermissions rp
    INNER JOIN (
        SELECT 
            RoleId,
            PermissionId,
            ROW_NUMBER() OVER (PARTITION BY RoleId, PermissionId ORDER BY (SELECT NULL)) as RowNum
        FROM RolePermissions
    ) AS numbered
    ON rp.RoleId = numbered.RoleId 
    AND rp.PermissionId = numbered.PermissionId
    WHERE numbered.RowNum > 1;
    
    DECLARE @DeletedRolePermissions INT = @@ROWCOUNT;
    PRINT 'Deleted ' + CAST(@DeletedRolePermissions AS VARCHAR(10)) + ' duplicate RolePermissions';
END
ELSE
BEGIN
    DECLARE @DeletedRolePermissions INT = 0;
    PRINT 'No duplicate RolePermissions found';
END

DROP TABLE #DuplicateRolePermissions;

-- Step 6: Delete the duplicate permissions
PRINT '';
PRINT '=== Deleting duplicate permissions ===';

DELETE FROM Permissions
WHERE Id IN (SELECT Id FROM #PermissionsToDelete);

DECLARE @DeletedPermissions INT = @@ROWCOUNT;
PRINT 'Deleted ' + CAST(@DeletedPermissions AS VARCHAR(10)) + ' duplicate permissions';

-- Step 7: Verify cleanup
PRINT '';
PRINT '=== Verification: Checking for remaining duplicates ===';
SELECT 
    Module, 
    Action, 
    COUNT(*) as Count
FROM Permissions
GROUP BY Module, Action
HAVING COUNT(*) > 1;

DECLARE @RemainingDuplicates INT;
SELECT @RemainingDuplicates = COUNT(*)
FROM (
    SELECT Module, Action
    FROM Permissions
    GROUP BY Module, Action
    HAVING COUNT(*) > 1
) AS Duplicates;

IF @RemainingDuplicates = 0
BEGIN
    PRINT '';
    PRINT '✓ SUCCESS! No duplicate permissions found.';
    PRINT '';
    PRINT '=== Summary ===';
    PRINT 'Deleted permissions: ' + CAST(@DeletedPermissions AS VARCHAR(10));
    PRINT 'Updated RolePermissions: ' + CAST(@UpdatedRolePermissions AS VARCHAR(10));
    PRINT 'Deleted duplicate RolePermissions: ' + CAST(@DeletedRolePermissions AS VARCHAR(10));
    
    -- Show current permission count
    DECLARE @TotalPermissions INT;
    SELECT @TotalPermissions = COUNT(*) FROM Permissions;
    PRINT 'Total permissions remaining: ' + CAST(@TotalPermissions AS VARCHAR(10));
    
    COMMIT TRANSACTION;
    PRINT '';
    PRINT '✓ Transaction committed successfully!';
END
ELSE
BEGIN
    PRINT '';
    PRINT '✗ WARNING! Still found ' + CAST(@RemainingDuplicates AS VARCHAR(10)) + ' duplicate permission(s).';
    PRINT 'Rolling back transaction...';
    ROLLBACK TRANSACTION;
    PRINT '✗ Transaction rolled back. Please investigate manually.';
END

-- Cleanup temporary tables
DROP TABLE #PermissionsToKeep;
DROP TABLE #PermissionsToDelete;

PRINT '';
PRINT 'Cleanup script completed.';
