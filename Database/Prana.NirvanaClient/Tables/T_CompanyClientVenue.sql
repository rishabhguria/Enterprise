CREATE TABLE [dbo].[T_CompanyClientVenue] (
    [CompanyClientVenueID] INT NOT NULL,
    [CompanyClientID]      INT NOT NULL,
    [CompanyVenueID]       INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientVenue] PRIMARY KEY CLUSTERED ([CompanyClientVenueID] ASC),
    CONSTRAINT [FK_T_CompanyClientVenue_T_CompanyClient] FOREIGN KEY ([CompanyClientID]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID]),
    CONSTRAINT [FK_T_CompanyClientVenue_T_CompanyVenue] FOREIGN KEY ([CompanyVenueID]) REFERENCES [dbo].[T_CompanyVenue] ([CompanyVenueID])
);

