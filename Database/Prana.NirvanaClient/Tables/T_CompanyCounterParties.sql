CREATE TABLE [dbo].[T_CompanyCounterParties] (
    [CompanyCounterPartyID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyID]             INT NOT NULL,
    [CounterPartyID]        INT NOT NULL,
    CONSTRAINT [PK_T_CompanyCounterParties] PRIMARY KEY CLUSTERED ([CompanyCounterPartyID] ASC),
    CONSTRAINT [FK_T_CompanyCounterParties_T_CounterParty] FOREIGN KEY ([CounterPartyID]) REFERENCES [dbo].[T_CounterParty] ([CounterPartyID])
);

