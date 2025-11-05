CREATE TABLE [dbo].[PM_TableTypes] (
    [TableTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [TableTypeName] VARCHAR (50) NOT NULL,
    [Acronym]       VARCHAR (50) NULL,
    CONSTRAINT [PK_PM_TableTypes] PRIMARY KEY CLUSTERED ([TableTypeID] ASC)
);

