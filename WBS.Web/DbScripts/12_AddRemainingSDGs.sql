-- Add remaining SDGs matching UN official goals

-- Check if SDG 5 exists (Gender Equality)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 5)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (5, 5, 'Gender Equality', '????? ????', '#FF3A21', 1);
END

-- Check if SDG 7 exists (Affordable and Clean Energy)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 7)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (7, 7, 'Affordable and Clean Energy', '???????? ? ????????? ?????', '#FCC30B', 1);
END
ELSE
BEGIN
    -- Update if it exists with wrong name
    UPDATE SDGs SET Name = 'Affordable and Clean Energy', NameBn = '???????? ? ????????? ?????', Color = '#FCC30B' WHERE Id = 7;
END

-- Check if SDG 8 exists (Decent Work and Economic Growth)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 8)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (8, 8, 'Decent Work and Economic Growth', '??????? ??? ? ????????? ?????????', '#A21942', 1);
END
ELSE
BEGIN
    -- Update if it exists with wrong name
    UPDATE SDGs SET Name = 'Decent Work and Economic Growth', NameBn = '??????? ??? ? ????????? ?????????', Color = '#A21942' WHERE Id = 8;
END

-- Check if SDG 9 exists (Industry, Innovation and Infrastructure)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 9)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (9, 9, 'Industry, Innovation and Infrastructure', '?????, ??????? ? ????????', '#FD6925', 1);
END
ELSE
BEGIN
    -- Update if it exists with wrong name
    UPDATE SDGs SET Name = 'Industry, Innovation and Infrastructure', NameBn = '?????, ??????? ? ????????', Color = '#FD6925' WHERE Id = 9;
END

-- Check if SDG 10 exists (Reduced Inequalities)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 10)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (10, 10, 'Reduced Inequalities', '????? ?????', '#DD1367', 1);
END
ELSE
BEGIN
    -- Update if it exists with wrong name
    UPDATE SDGs SET Name = 'Reduced Inequalities', NameBn = '????? ?????', Color = '#DD1367' WHERE Id = 10;
END

-- Check if SDG 11 exists (Sustainable Cities and Communities)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 11)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (11, 11, 'Sustainable Cities and Communities', '????? ??? ? ????????', '#FD9D24', 1);
END

-- Check if SDG 12 exists (Responsible Consumption and Production)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 12)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (12, 12, 'Responsible Consumption and Production', '??????????? ??? ? ??????', '#BF8B2E', 1);
END

-- Check if SDG 13 exists (Climate Action)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 13)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (13, 13, 'Climate Action', '??????? ???????', '#3F7E44', 1);
END

-- Check if SDG 14 exists (Life Below Water)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 14)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (14, 14, 'Life Below Water', '????? ???? ????', '#0A97D9', 1);
END

-- Check if SDG 15 exists (Life on Land)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 15)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (15, 15, 'Life on Land', '?????? ????', '#56C02B', 1);
END

-- Check if SDG 16 exists (Peace, Justice and Strong Institutions)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 16)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (16, 16, 'Peace, Justice and Strong Institutions', '??????, ??????????? ? ????????? ??????????', '#00689D', 1);
END

-- Check if SDG 17 exists (Partnerships for the Goals)
IF NOT EXISTS (SELECT 1 FROM SDGs WHERE Id = 17)
BEGIN
    INSERT INTO SDGs (Id, Number, Name, NameBn, Color, IsActive)
    VALUES (17, 17, 'Partnerships for the Goals', '??????? ???????????', '#19486A', 1);
END

-- Update existing SDG colors to match UN standards
UPDATE SDGs SET Color = '#E5243B' WHERE Number = 1;
UPDATE SDGs SET Color = '#DDA63A' WHERE Number = 2;
UPDATE SDGs SET Color = '#4C9F38' WHERE Number = 3;
UPDATE SDGs SET Color = '#C5192D' WHERE Number = 4;
UPDATE SDGs SET Color = '#FF3A21' WHERE Number = 5;
UPDATE SDGs SET Color = '#26BDE2' WHERE Number = 6;
UPDATE SDGs SET Color = '#FCC30B' WHERE Number = 7;
UPDATE SDGs SET Color = '#A21942' WHERE Number = 8;
UPDATE SDGs SET Color = '#FD6925' WHERE Number = 9;
UPDATE SDGs SET Color = '#DD1367' WHERE Number = 10;
UPDATE SDGs SET Color = '#FD9D24' WHERE Number = 11;
UPDATE SDGs SET Color = '#BF8B2E' WHERE Number = 12;
UPDATE SDGs SET Color = '#3F7E44' WHERE Number = 13;
UPDATE SDGs SET Color = '#0A97D9' WHERE Number = 14;
UPDATE SDGs SET Color = '#56C02B' WHERE Number = 15;
UPDATE SDGs SET Color = '#00689D' WHERE Number = 16;
UPDATE SDGs SET Color = '#19486A' WHERE Number = 17;

GO
