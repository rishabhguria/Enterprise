CREATE TABLE [dbo].[T_Identifier] (
    [IdentifierID]   INT          IDENTITY (1, 1) NOT NULL,
    [IdentifierName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_Identifier] PRIMARY KEY CLUSTERED ([IdentifierID] ASC)
);

