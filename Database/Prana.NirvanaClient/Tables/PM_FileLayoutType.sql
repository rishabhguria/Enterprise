CREATE TABLE [dbo].[PM_FileLayoutType] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100)  NOT NULL,
    [Description] VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

