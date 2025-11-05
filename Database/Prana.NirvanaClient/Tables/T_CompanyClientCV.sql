CREATE TABLE [dbo].[T_CompanyClientCV] (
    [CompanyClientCVID]       INT IDENTITY (1, 1) NOT NULL,
    [CompanyClientID]         INT NOT NULL,
    [CompanyCounterPartyCVID] INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientCV] PRIMARY KEY CLUSTERED ([CompanyClientCVID] ASC),
    CONSTRAINT [FK_T_CompanyClientCV_T_CompanyCounterPartyVenues] FOREIGN KEY ([CompanyCounterPartyCVID]) REFERENCES [dbo].[T_CompanyCounterPartyVenues] ([CompanyCounterPartyCVID]) ON DELETE CASCADE
);

