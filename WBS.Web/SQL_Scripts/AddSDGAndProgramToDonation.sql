-- Add SDGId and ProgramId columns to Donations table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Donations' AND COLUMN_NAME = 'SDGId')
BEGIN
    ALTER TABLE Donations ADD SDGId INT NULL;
    
    -- Add foreign key constraint
    ALTER TABLE Donations
    ADD CONSTRAINT FK_Donations_SDGs_SDGId
    FOREIGN KEY (SDGId) REFERENCES SDGs(Id);
    
    -- Add index
    CREATE INDEX IX_Donations_SDGId ON Donations(SDGId);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Donations' AND COLUMN_NAME = 'ProgramId')
BEGIN
    ALTER TABLE Donations ADD ProgramId INT NULL;
    
    -- Add foreign key constraint
    ALTER TABLE Donations
    ADD CONSTRAINT FK_Donations_SDGPrograms_ProgramId
    FOREIGN KEY (ProgramId) REFERENCES SDGPrograms(Id);
    
    -- Add index
    CREATE INDEX IX_Donations_ProgramId ON Donations(ProgramId);
END

PRINT 'Migration completed: Added SDGId and ProgramId to Donations table';
