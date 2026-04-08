-- Check for Duplicate Permissions
-- This script only checks and reports duplicates without making any changes

PRINT '=== Checking for Duplicate Permissions ===';
PRINT '';

-- Check for duplicate permissions
SELECT 
    Module, 
    Action, 
    Name,
    COUNT(*) as DuplicateCount,
    STRING_AGG(CAST(Id AS VARCHAR(10)), ', ') as DuplicateIds
FROM Permissions
GROUP BY Module, Action, Name
HAVING COUNT(*) > 1
ORDER BY Module, Action;

DECLARE @DuplicateModules INT;
SELECT @DuplicateModules = COUNT(*)
FROM (
    SELECT Module, Action
    FROM Permissions
    GROUP BY Module, Action
    HAVING COUNT(*) > 1
) AS Duplicates;

PRINT '';
IF @DuplicateModules = 0
BEGIN
    PRINT '? No duplicate permissions found!';
    
    -- Show total permission count
    DECLARE @TotalPermissions INT;
    SELECT @TotalPermissions = COUNT(*) FROM Permissions;
    PRINT 'Total unique permissions: ' + CAST(@TotalPermissions AS VARCHAR(10));
END
ELSE
BEGIN
    PRINT '? Found ' + CAST(@DuplicateModules AS VARCHAR(10)) + ' duplicate permission(s)!';
    PRINT '';
    PRINT '=== Detailed List of All Permissions (with duplicates highlighted) ===';
    
    SELECT 
        p.Id,
        p.Module,
        p.Action,
        p.Name,
        p.DisplayOrder,
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM Permissions p2 
                WHERE p2.Module = p.Module 
                AND p2.Action = p.Action 
                AND p2.Id <> p.Id
            )
            THEN '?? DUPLICATE'
            ELSE '? Unique'
        END as Status
    FROM Permissions p
    ORDER BY p.Module, p.Action, p.Id;
    
    PRINT '';
    PRINT '?? Run CleanupDuplicatePermissions.sql to fix these duplicates.';
END

PRINT '';
PRINT '=== Module Summary ===';
SELECT 
    Module,
    COUNT(*) as PermissionCount,
    CASE 
        WHEN COUNT(*) = COUNT(DISTINCT Action) 
        THEN '? No duplicates'
        ELSE '?? Has duplicates'
    END as DuplicateStatus
FROM Permissions
GROUP BY Module
ORDER BY Module;
