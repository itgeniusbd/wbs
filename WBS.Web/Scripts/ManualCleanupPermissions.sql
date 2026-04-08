-- MANUAL Duplicate Cleanup - Execute this AFTER reviewing duplicates
-- This script will keep the FIRST occurrence (lowest ID) of each permission

-- ?? WARNING: Review the duplicates before running this!
-- Run CheckDuplicatePermissions.sql first to see what will be deleted

-- Uncomment the following line to enable deletion
-- SET NOCOUNT ON;

PRINT 'Starting manual cleanup...';
PRINT '';

-- Show what will be deleted
PRINT '=== Permissions that will be DELETED ===';
SELECT 
    p.Id,
    p.Module,
    p.Action,
    p.Name,
    p.DisplayOrder
FROM Permissions p
WHERE EXISTS (
    SELECT 1
    FROM Permissions p2
    WHERE p2.Module = p.Module
    AND p2.Action = p.Action
    AND p2.Id < p.Id  -- Keep the one with lower ID
)
ORDER BY p.Module, p.Action, p.Id;

DECLARE @ToDeleteCount INT;
SELECT @ToDeleteCount = COUNT(*)
FROM Permissions p
WHERE EXISTS (
    SELECT 1
    FROM Permissions p2
    WHERE p2.Module = p.Module
    AND p2.Action = p.Action
    AND p2.Id < p.Id
);

PRINT '';
PRINT 'Total permissions to delete: ' + CAST(@ToDeleteCount AS VARCHAR(10));
PRINT '';
PRINT '=== Permissions that will be KEPT ===';
SELECT 
    p.Id,
    p.Module,
    p.Action,
    p.Name,
    p.DisplayOrder
FROM Permissions p
WHERE EXISTS (
    SELECT 1
    FROM Permissions p2
    WHERE p2.Module = p.Module
    AND p2.Action = p.Action
    AND p2.Id <> p.Id
)
AND NOT EXISTS (
    SELECT 1
    FROM Permissions p3
    WHERE p3.Module = p.Module
    AND p3.Action = p.Action
    AND p3.Id < p.Id
)
ORDER BY p.Module, p.Action;

PRINT '';
PRINT '?????? REVIEW THE ABOVE LISTS CAREFULLY ??????';
PRINT '';
PRINT '>>> If everything looks correct, uncomment the DELETE section below and run again <<<';
PRINT '';

/*
-- ============================================
-- UNCOMMENT THIS SECTION TO EXECUTE CLEANUP
-- ============================================

BEGIN TRANSACTION;

BEGIN TRY
    -- Step 1: Update RolePermissions to point to the permissions we're keeping
    PRINT 'Updating RolePermissions references...';
    
    UPDATE rp
    SET rp.PermissionId = (
        SELECT MIN(p2.Id)
        FROM Permissions p2
        WHERE p2.Module = p.Module
        AND p2.Action = p.Action
    )
    FROM RolePermissions rp
    INNER JOIN Permissions p ON rp.PermissionId = p.Id
    WHERE EXISTS (
        SELECT 1
        FROM Permissions p3
        WHERE p3.Module = p.Module
        AND p3.Action = p.Action
        AND p3.Id < p.Id
    );
    
    PRINT '? Updated ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' RolePermissions';
    
    -- Step 2: Delete duplicate RolePermissions
    PRINT 'Removing duplicate RolePermissions...';
    
    WITH DuplicateRP AS (
        SELECT 
            RoleId,
            PermissionId,
            ROW_NUMBER() OVER (PARTITION BY RoleId, PermissionId ORDER BY (SELECT NULL)) as RowNum
        FROM RolePermissions
    )
    DELETE rp
    FROM RolePermissions rp
    INNER JOIN DuplicateRP dup 
        ON rp.RoleId = dup.RoleId 
        AND rp.PermissionId = dup.PermissionId
    WHERE dup.RowNum > 1;
    
    PRINT '? Deleted ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' duplicate RolePermissions';
    
    -- Step 3: Delete duplicate permissions
    PRINT 'Deleting duplicate permissions...';
    
    DELETE FROM Permissions
    WHERE EXISTS (
        SELECT 1
        FROM Permissions p2
        WHERE p2.Module = Permissions.Module
        AND p2.Action = Permissions.Action
        AND p2.Id < Permissions.Id
    );
    
    PRINT '? Deleted ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' duplicate permissions';
    
    -- Verify
    DECLARE @StillDuplicates INT;
    SELECT @StillDuplicates = COUNT(*)
    FROM (
        SELECT Module, Action
        FROM Permissions
        GROUP BY Module, Action
        HAVING COUNT(*) > 1
    ) AS Dup;
    
    IF @StillDuplicates = 0
    BEGIN
        COMMIT TRANSACTION;
        PRINT '';
        PRINT '??? SUCCESS! All duplicates removed! ???';
        
        DECLARE @FinalCount INT;
        SELECT @FinalCount = COUNT(*) FROM Permissions;
        PRINT 'Final permission count: ' + CAST(@FinalCount AS VARCHAR(10));
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION;
        PRINT '? Still have duplicates! Rolling back...';
    END
    
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '? ERROR: ' + ERROR_MESSAGE();
    PRINT '? Transaction rolled back.';
END CATCH

-- ============================================
-- END OF CLEANUP SECTION
-- ============================================
*/
