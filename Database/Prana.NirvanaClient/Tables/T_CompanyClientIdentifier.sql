CREATE TABLE [dbo].[T_CompanyClientIdentifier] (
    [CompanyClientIdentifierID] VARCHAR (100) NOT NULL,
    [CompanyClientID]           INT           NOT NULL,
    [IdentifierID]              INT           NOT NULL,
    [Identifier]                VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_T_CompanyClientIdentifier] PRIMARY KEY CLUSTERED ([CompanyClientIdentifierID] ASC),
    CONSTRAINT [FK_T_CompanyClientIdentifier_T_Identifier] FOREIGN KEY ([IdentifierID]) REFERENCES [dbo].[T_Identifier] ([IdentifierID])
);

