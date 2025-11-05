-- [T_UserWidgetConfigDetails] table for storing Company user's widget details for different modules.

CREATE TABLE [dbo].[T_UserWidgetConfigDetails]
(
  [UserID] int NOT NULL,
  [WidgetKey] varchar(500) NOT NULL DEFAULT '',
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
);
