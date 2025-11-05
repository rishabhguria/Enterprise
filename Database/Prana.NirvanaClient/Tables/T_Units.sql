CREATE TABLE [dbo].[T_Units] (
    [UnitID]   INT          IDENTITY (1, 1) NOT NULL,
    [UnitName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_Units] PRIMARY KEY CLUSTERED ([UnitID] ASC)
);

