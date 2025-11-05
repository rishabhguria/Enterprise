CREATE TABLE [dbo].[PM_DataSourceType] (
    [PM_DataSourceTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Description]         VARCHAR (500) NULL,
    [Text]                VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_PM_DataSourceType] PRIMARY KEY CLUSTERED ([PM_DataSourceTypeID] ASC)
);

