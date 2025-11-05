CREATE TABLE [dbo].[T_Blotter] (
    [BlotterID]   INT          IDENTITY (1, 1) NOT NULL,
    [BlotterType] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_Blotter] PRIMARY KEY CLUSTERED ([BlotterID] ASC)
);

