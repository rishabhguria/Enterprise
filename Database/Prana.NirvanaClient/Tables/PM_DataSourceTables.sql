CREATE TABLE [dbo].[PM_DataSourceTables] (
    [DataSourceTableID] INT          IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]      INT          NOT NULL,
    [TableName]         VARCHAR (50) NOT NULL,
    [TableTypeID]       INT          NOT NULL
);

