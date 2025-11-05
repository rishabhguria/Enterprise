-- [T_Samsara_CompanyUserLayouts] table for storing Company user's layouts for different modules.
/*
-- Existing [T_PranaUserPrefs] table stores both company user's preferences(ex. TT_Preference, QTT_Preference file) and company user's layouts(ex. Blotter, PM layouts).
-- Purpose to create [T_Samsara_CompanyUserLayouts] table is to store layouts separately.
-- Currently this table is used to save layouts for Web application modules.
*/

CREATE TABLE [dbo].[T_Samsara_CompanyUserLayouts] (
  [UserID] int NOT NULL,  
  [ViewId] varchar(100) NULL,
  [ViewName] varchar(100) NULL,
  [ViewLayout] varbinary(max) NULL,
  [ModuleName] varchar(100),
  [Description] varchar(2000) NOT NULL  DEFAULT '',
  [LastSavedTime] datetime NOT NULL,
  --CONSTRAINT [PK_T_Samsara_CompanyUserLayouts] PRIMARY KEY ([FileName], [PageId]),
  --CONSTRAINT [FK_T_Samsara_CompanyUserLayouts_PageId] FOREIGN KEY ([PageId]) REFERENCES [dbo].[T_RTPNL_OpenfinPageInfo]([PageId]) ON DELETE CASCADE
);