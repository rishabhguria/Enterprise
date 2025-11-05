CREATE TABLE [dbo].[PM_UploadRuns] (
    [UploadID]             INT             IDENTITY (1, 1) NOT NULL,
    [CompanyUploadSetupID] INT             NOT NULL,
    [Status]               INT             NOT NULL,
    [UploadStart]          DATETIME        NULL,
    [UploadEnd]            DATETIME        NULL,
    [TotalRecords]         INT             NULL,
    [StatusDescription]    NVARCHAR (4000) NULL,
    [HeaderRowIndex]       INT             NULL,
    [FirstRecordIndex]     INT             NULL,
    CONSTRAINT [PK_PM_UploadRuns] PRIMARY KEY CLUSTERED ([UploadID] ASC),
    FOREIGN KEY ([CompanyUploadSetupID]) REFERENCES [dbo].[PM_CompanyUploadSetup] ([CompanyUploadSetupID]),
    FOREIGN KEY ([CompanyUploadSetupID]) REFERENCES [dbo].[PM_CompanyUploadSetup] ([CompanyUploadSetupID]) ON DELETE CASCADE
);

