-- [T_CompanyUserLayouts] table for storing Company user's layouts for different modules.
/*
-- Existing [T_PranaUserPrefs] table stores both company user's preferences(ex. TT_Preference, QTT_Preference file) and company user's layouts(ex. Blotter, PM layouts).
-- Purpose to create [T_CompanyUserLayouts] table is to store layouts separately.
-- Currently this table is used to save layouts for Web application modules.
*/

CREATE TABLE [dbo].[T_CompanyUserLayouts] (
  [UserID] int NOT NULL,
  [FileName] varchar(100) NOT NULL,
  [FileData] image NULL,
  [LastSaveTime] datetime NOT NULL,
  [ModuleName] varchar(100),
  [Description] varchar(2000) NOT NULL  DEFAULT '',
  [PageId] varchar(100) NOT NULL  DEFAULT '',
);