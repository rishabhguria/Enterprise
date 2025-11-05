CREATE TABLE [dbo].[PM_CompanyReconColumns] (
    [DataSourceColumnID]   INT     NOT NULL,
    [IncludeAsCash]        BIT     NOT NULL,
    [Type]                 BIT     NOT NULL,
    [AcceptableDeviation]  TINYINT CONSTRAINT [DF_PM_CompanyReconColumns_AcceptableDeviation] DEFAULT ((0)) NULL,
    [CompanyReconColumnID] INT     IDENTITY (1, 1) NOT NULL,
    [DeviationSign]        TINYINT CONSTRAINT [DF_PM_CompanyReconColumns_DeviationSign] DEFAULT ((0)) NULL,
    [AcceptDataFrom]       BIT     CONSTRAINT [DF_PM_CompanyReconColumns_AcceptData] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PM_CompanyReconSetup_1] PRIMARY KEY CLUSTERED ([CompanyReconColumnID] ASC, [DataSourceColumnID] ASC),
    CONSTRAINT [FK_PM_CompanyReconColumns_PM_DataSourceColumns] FOREIGN KEY ([DataSourceColumnID]) REFERENCES [dbo].[PM_DataSourceColumns] ([DataSourceColumnID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => Optional,
1 => Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_CompanyReconColumns', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_CompanyReconColumns', @level2type = N'COLUMN', @level2name = N'AcceptableDeviation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => N/A
1 => +
2 => -
3 => +/-', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_CompanyReconColumns', @level2type = N'COLUMN', @level2name = N'DeviationSign';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => Source,
1 => Application', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_CompanyReconColumns', @level2type = N'COLUMN', @level2name = N'AcceptDataFrom';

