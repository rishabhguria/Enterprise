CREATE TABLE [dbo].[PM_UploadedData] (
    [UploadID]             INT             NOT NULL,
    [DataSourceColumnID]   INT             NOT NULL,
    [DataRowID]            INT             NOT NULL,
    [Value]                NVARCHAR (4000) NULL,
    [PMCompanyID]          INT             NOT NULL,
    [UploadedColumnDataID] INT             IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_PM_UPloadedData] PRIMARY KEY CLUSTERED ([UploadedColumnDataID] ASC),
    FOREIGN KEY ([UploadID]) REFERENCES [dbo].[PM_UploadRuns] ([UploadID]),
    CONSTRAINT [FK_PM_UploadedData_PM_DataSourceColumns] FOREIGN KEY ([DataSourceColumnID]) REFERENCES [dbo].[PM_DataSourceColumns] ([DataSourceColumnID])
);

