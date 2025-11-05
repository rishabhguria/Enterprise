CREATE TABLE [dbo].[PM_DataSourceColumns] (
    [DataSourceColumnID]  INT            IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]        INT            NOT NULL,
    [ColumnName]          NVARCHAR (50)  NOT NULL,
    [Description]         NVARCHAR (500) NULL,
    [Type]                TINYINT        NOT NULL,
    [SampleValue]         NVARCHAR (100) NULL,
    [Notes]               NVARCHAR (500) NULL,
    [ApplicationColumnId] INT            NULL,
    [Locked]              BIT            CONSTRAINT [DF_PM_DataSourceColumns_Locked] DEFAULT ((0)) NULL,
    [RequiredInUpload]    BIT            CONSTRAINT [DF_PM_DataSourceColumns_RequiredInUpload] DEFAULT ((0)) NOT NULL,
    [ColumnSequenceNo]    INT            NULL,
    [TableTypeID]         INT            NULL,
    CONSTRAINT [PK_PM_DataSourceColumns] PRIMARY KEY CLUSTERED ([DataSourceColumnID] ASC),
    CONSTRAINT [FK_PM_DataSourceColumns_PM_ApplicationColumns] FOREIGN KEY ([ApplicationColumnId]) REFERENCES [dbo].[PM_ApplicationColumns] ([ApplicationColumnID]),
    CONSTRAINT [FK_PM_DataSourceColumns_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID]),
    CONSTRAINT [CK_PM_DataSourceColumns_Unique_DataSourceID_ColumnName] UNIQUE NONCLUSTERED ([ThirdPartyID] ASC, [ColumnName] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => Numeric,
1 => Text,
2 => Both       

To fetch values at frontend use SelectColumnsType Enum from Nirvana.PM.BLL.SelectColumnsType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_DataSourceColumns';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Added Rajat (24 Nov 2006) , Stores the sequence no of column which comes in the upload file from data source', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_DataSourceColumns', @level2type = N'COLUMN', @level2name = N'ColumnSequenceNo';

