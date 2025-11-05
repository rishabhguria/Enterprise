-- [T_RTPNL_UserWidgetConfigDetails] table for storing Company user's widget details for different modules.

CREATE TABLE [dbo].[T_RTPNL_UserWidgetConfigDetails]
(
  [UserID] int NOT NULL,
  [PageId] varchar(100) NOT NULL DEFAULT'', 
  [WidgetId] varchar(500) NOT NULL DEFAULT '',
  [ViewName] varchar(100) NOT NULL,
  [WidgetName] varchar(100) NOT NULL,
  [WidgetType] varchar(100) NOT NULL,
  [DefaultColumns] varchar(8000) NULL,
  [ColoredColumns] varchar(8000) NULL,
  [GraphType] varchar(100) NOT NULL,
  [IsFlashColorEnabled] bit NOT NULL,
  [ChannelDetail] varchar(100) NOT NULL,
  [LinkedWidget] varchar(100) NOT NULL,
  [PrimaryMetric] varchar(20) NULL,
  [LastSavedTime] [datetime],
  CONSTRAINT PK_T_RTPNL_UserWidgetConfigDetails PRIMARY KEY (WidgetId, PageID)
);
