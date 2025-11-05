CREATE TABLE [dbo].[PM_CompanyCashReconColumns] (
    [CompanyCashReconSetupID] INT NOT NULL,
    [DataSourceColumnID]      INT NOT NULL,
    [Classification]          INT NOT NULL,
    CONSTRAINT [PK_PM_CompanyCashReconColumns] PRIMARY KEY CLUSTERED ([CompanyCashReconSetupID] ASC, [DataSourceColumnID] ASC),
    CONSTRAINT [FK_PM_CompanyCashReconColumns_PM_CompanyCashReconSetup] FOREIGN KEY ([CompanyCashReconSetupID]) REFERENCES [dbo].[PM_CompanyCashReconSetup] ([CompanyCashReconSetupID]),
    CONSTRAINT [FK_PM_CompanyCashReconColumns_PM_DataSourceColumns] FOREIGN KEY ([DataSourceColumnID]) REFERENCES [dbo].[PM_DataSourceColumns] ([DataSourceColumnID])
);

