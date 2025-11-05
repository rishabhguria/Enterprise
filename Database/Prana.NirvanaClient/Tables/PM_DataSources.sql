CREATE TABLE [dbo].[PM_DataSources] (
    [DataSourceID]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]                     NVARCHAR (50)  NOT NULL,
    [ShortName]                VARCHAR (50)   NOT NULL,
    [StatusID]                 INT            NULL,
    [SourceTypeID]             INT            NULL,
    [Address1]                 NVARCHAR (100) NULL,
    [Address2]                 NVARCHAR (100) NULL,
    [StateID]                  INT            NULL,
    [Zip]                      NVARCHAR (50)  NULL,
    [CountryID]                INT            NULL,
    [WorkNumber]               VARCHAR (50)   NULL,
    [FaxNumber]                VARCHAR (50)   NULL,
    [PrimaryContactFirstName]  VARCHAR (50)   NULL,
    [PrimaryContactLastName]   VARCHAR (50)   NULL,
    [PrimaryContactTitle]      VARCHAR (50)   NULL,
    [PrimaryContactEMail]      VARCHAR (100)  NULL,
    [PrimaryContactWorkNumber] VARCHAR (50)   NULL,
    [PrimaryContactCellNumber] VARCHAR (50)   NULL,
    [ImportMethod]             TINYINT        NULL,
    [ImportFormat]             TINYINT        NULL,
    [DataSourceTableName]      NCHAR (50)     NULL,
    CONSTRAINT [PK_PM_DataSources] PRIMARY KEY CLUSTERED ([DataSourceID] ASC),
    CONSTRAINT [FK_PM_DataSources_PM_DataSourceType] FOREIGN KEY ([SourceTypeID]) REFERENCES [dbo].[PM_DataSourceType] ([PM_DataSourceTypeID]),
    CONSTRAINT [FK_PM_DataSources_T_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[T_Country] ([CountryID]),
    CONSTRAINT [FK_PM_DataSources_T_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[T_State] ([StateID]),
    CONSTRAINT [DATASOURCES_UNIQUENAME] UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [DATASOURCES_UNIQUESHORTNAME] UNIQUE NONCLUSTERED ([ShortName] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => ftp,
1 => fix

To fetch values at frontend use ImportMethod Enum from Nirvana.PM.BLL.ImportMethod', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_DataSources', @level2type = N'COLUMN', @level2name = N'ImportMethod';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => xls,
1 => csv,
2 => tsv

To fetch values at frontend use ImportFormat Enum from Nirvana.PM.BLL.ImportFormat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_DataSources', @level2type = N'COLUMN', @level2name = N'ImportFormat';

