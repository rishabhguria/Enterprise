CREATE TABLE [dbo].[PM_SymbolConvention] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (200)  NOT NULL,
    [Description] VARCHAR (1000) NULL,
    CONSTRAINT [PK_PM_SymbolConvention] PRIMARY KEY CLUSTERED ([ID] ASC)
);

