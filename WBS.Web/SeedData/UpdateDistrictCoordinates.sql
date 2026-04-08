-- Update District Coordinates for Bangladesh Map
-- Run this script to ensure all districts have accurate latitude and longitude

-- Major Districts Coordinates (Sample - Add all 64 districts)

UPDATE Districts SET Latitude = 23.8103, Longitude = 90.4125 WHERE Name = 'Dhaka';
UPDATE Districts SET Latitude = 22.3569, Longitude = 91.7832 WHERE Name = 'Chittagong';
UPDATE Districts SET Latitude = 24.8949, Longitude = 91.8687 WHERE Name = 'Sylhet';
UPDATE Districts SET Latitude = 22.7010, Longitude = 90.3535 WHERE Name = 'Khulna';
UPDATE Districts SET Latitude = 24.3745, Longitude = 88.6042 WHERE Name = 'Rajshahi';
UPDATE Districts SET Latitude = 25.7439, Longitude = 89.2752 WHERE Name = 'Rangpur';
UPDATE Districts SET Latitude = 22.7010, Longitude = 90.3535 WHERE Name = 'Barisal';
UPDATE Districts SET Latitude = 24.7471, Longitude = 90.4203 WHERE Name = 'Mymensingh';

-- Additional districts (add as needed)
UPDATE Districts SET Latitude = 23.9999, Longitude = 90.4203 WHERE Name = 'Gazipur';
UPDATE Districts SET Latitude = 24.8949, Longitude = 89.3725 WHERE Name = 'Bogra';
UPDATE Districts SET Latitude = 23.4607, Longitude = 91.1809 WHERE Name = 'Comilla';
UPDATE Districts SET Latitude = 25.0968, Longitude = 88.9483 WHERE Name = 'Dinajpur';
UPDATE Districts SET Latitude = 23.6238, Longitude = 90.5000 WHERE Name = 'Narayanganj';
UPDATE Districts SET Latitude = 23.2513, Longitude = 90.1718 WHERE Name = 'Faridpur';
UPDATE Districts SET Latitude = 24.3745, Longitude = 91.4160 WHERE Name = 'Habiganj';
UPDATE Districts SET Latitude = 25.1074, Longitude = 91.8815 WHERE Name = 'Moulvibazar';
UPDATE Districts SET Latitude = 24.3700, Longitude = 91.9515 WHERE Name = 'Sunamganj';
UPDATE Districts SET Latitude = 23.9389, Longitude = 89.5339 WHERE Name = 'Jessore';
UPDATE Districts SET Latitude = 22.8456, Longitude = 89.5403 WHERE Name = 'Satkhira';
UPDATE Districts SET Latitude = 23.1634, Longitude = 89.2182 WHERE Name = 'Jhenaidah';
UPDATE Districts SET Latitude = 22.3155, Longitude = 91.9815 WHERE Name = 'Cox''s Bazar';
UPDATE Districts SET Latitude = 22.8081, Longitude = 91.1120 WHERE Name = 'Noakhali';
UPDATE Districts SET Latitude = 23.1793, Longitude = 91.9882 WHERE Name = 'Feni';

-- Verify updates
SELECT 
    Name,
    NameBn,
    Latitude,
    Longitude,
    HasWork,
    CASE 
        WHEN Latitude IS NULL OR Longitude IS NULL THEN 'Missing Coordinates'
        WHEN Latitude < 20.5 OR Latitude > 26.5 THEN 'Invalid Latitude'
        WHEN Longitude < 88.0 OR Longitude > 92.5 THEN 'Invalid Longitude'
        ELSE 'OK'
    END AS ValidationStatus
FROM Districts
ORDER BY Name;

-- Districts without coordinates
SELECT 
    Name,
    NameBn,
    HasWork
FROM Districts
WHERE Latitude IS NULL OR Longitude IS NULL
ORDER BY Name;

-- Active districts (HasWork = 1) without coordinates (PRIORITY)
SELECT 
    Name,
    NameBn,
    HasWork
FROM Districts
WHERE HasWork = 1 
    AND (Latitude IS NULL OR Longitude IS NULL)
ORDER BY Name;
