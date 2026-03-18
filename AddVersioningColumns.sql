-- Check and add Version column if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MutualFundKnowledge') AND name = 'Version')
BEGIN
    ALTER TABLE MutualFundKnowledge
    ADD Version INT DEFAULT 1;
END

-- Check and add IsActive column if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MutualFundKnowledge') AND name = 'IsActive')
BEGIN
    ALTER TABLE MutualFundKnowledge
    ADD IsActive BIT DEFAULT 1;
END

-- Update existing records to have default values
UPDATE MutualFundKnowledge
SET Version = 1
WHERE Version IS NULL;

UPDATE MutualFundKnowledge
SET IsActive = 1
WHERE IsActive IS NULL;

-- Create index for performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MutualFundKnowledge_IsActive')
BEGIN
    CREATE INDEX IX_MutualFundKnowledge_IsActive ON MutualFundKnowledge(IsActive);
END
