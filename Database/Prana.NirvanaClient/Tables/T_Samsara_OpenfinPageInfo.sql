-- [T_RTPNL_OpenfinPageInfo] table for storing Company user's Openfin page information.

CREATE TABLE [dbo].[T_Samsara_OpenfinPageInfo] (
  [UserID] int NOT NULL,
  [PageId] varchar(100) NOT NULL,
  [PageName] varchar(100) NOT NULL,
  [PageLayout] varbinary(max) NOT NULL,
  [PageTag] varchar(100) NOT NULL,
  [LastSavedTime] [datetime] NULL
  CONSTRAINT [PK_T_RTPNL_OpenfinPageInfo] PRIMARY KEY ([PageId])
);