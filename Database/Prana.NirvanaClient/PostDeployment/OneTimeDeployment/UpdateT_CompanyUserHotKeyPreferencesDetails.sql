--------------------------------------------------
-- UpdateT_CompanyUserHotKeyPreferencesDetails
--------------------------------------------------

-- Add 'Module' column if it does not exist
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'T_CompanyUserHotKeyPreferencesDetails' 
      AND COLUMN_NAME = 'Module'
)
BEGIN
    ALTER TABLE [dbo].[T_CompanyUserHotKeyPreferencesDetails]
    ADD [Module] VARCHAR(MAX) NULL;
END

-- Set default for 'Module'
UPDATE [dbo].[T_CompanyUserHotKeyPreferencesDetails]
SET [Module] = 'TT'
WHERE [Module] IS NULL OR [Module] = '';


-- Add 'HotButtonType' column if it does not exist
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'T_CompanyUserHotKeyPreferencesDetails' 
      AND COLUMN_NAME = 'HotButtonType'
)
BEGIN
    ALTER TABLE [dbo].[T_CompanyUserHotKeyPreferencesDetails]
    ADD [HotButtonType] VARCHAR(MAX) NULL;
END

-- Set default for 'HotButtonType'
UPDATE [dbo].[T_CompanyUserHotKeyPreferencesDetails]
SET [HotButtonType] = 'Create New Order'
WHERE [HotButtonType] IS NULL OR [HotButtonType] = '';


-- Add 'LastSavedTime' column if it does not exist
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'T_CompanyUserHotKeyPreferencesDetails' 
      AND COLUMN_NAME = 'LastSavedTime'
)
BEGIN
    ALTER TABLE [dbo].[T_CompanyUserHotKeyPreferencesDetails]
    ADD [LastSavedTime] DATETIME NULL;
END

-- Set default for 'LastSavedTime'
UPDATE [dbo].[T_CompanyUserHotKeyPreferencesDetails]
SET [LastSavedTime] = GETDATE()
WHERE [LastSavedTime] IS NULL;
