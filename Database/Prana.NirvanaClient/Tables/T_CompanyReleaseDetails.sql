CREATE TABLE [dbo].[T_CompanyReleaseDetails] (
    [CompanyReleaseID] INT           IDENTITY (1, 1) NOT NULL,
    [ReleaseName]      VARCHAR (100) NOT NULL,
    [IP]               VARCHAR (100) NULL,
    [ReleasePath]      VARCHAR (500) NULL,
    [ClientDB_Name]    VARCHAR (100) NULL,
    [SMDB_Name]        VARCHAR (100) NULL,
    CONSTRAINT [PK_T_CompanyReleaseDetails] PRIMARY KEY CLUSTERED ([CompanyReleaseID] ASC)
);

