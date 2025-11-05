CREATE TABLE [dbo].[PM_ApplicationColumns] (
    [ApplicationColumnID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (50) NOT NULL,
    [Description]         VARCHAR (500) NULL,
    [Type]                TINYINT       NOT NULL,
    [TableName]           VARCHAR (200) NULL,
    CONSTRAINT [PK_PM_ApplicationColumns] PRIMARY KEY CLUSTERED ([ApplicationColumnID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 => Numeric,
1 => Text,
2 => Both       

To fetch values at frontend use SelectColumnsType Enum from Nirvana.PM.BLL.SelectColumnsType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_ApplicationColumns';

