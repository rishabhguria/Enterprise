CREATE TABLE [dbo].[T_BTFilterTypes] (
    [FilterTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [FilterName]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_T_BTFilterTypes] PRIMARY KEY CLUSTERED ([FilterTypeID] ASC)
);

