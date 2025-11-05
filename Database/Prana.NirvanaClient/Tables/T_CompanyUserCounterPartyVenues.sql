CREATE TABLE [dbo].[T_CompanyUserCounterPartyVenues] (
    [CompanyUserCounterPartyCVID] INT IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueID]         INT NOT NULL,
    [CompanyUserID]               INT NOT NULL,
    [CompanyCounterPartyCVID]     INT NULL,
    CONSTRAINT [PK_T_CompanyUserCounterParties] PRIMARY KEY CLUSTERED ([CompanyUserCounterPartyCVID] ASC),
    CONSTRAINT [FK_T_CompanyUserCounterParties_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

