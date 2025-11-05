CREATE TABLE [dbo].[T_CompanyThirdPartyCVIdentifier] (
    [ThirdPartyCVID]                INT          IDENTITY (1, 1) NOT NULL,
    [CompanyThirdPartyID_FK]        INT          NOT NULL,
    [CompanyCounterPartyVenueID_FK] INT          NOT NULL,
    [CVIdentifier]                  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ThirdPartyCVIdentifier] PRIMARY KEY CLUSTERED ([ThirdPartyCVID] ASC),
    CONSTRAINT [FK_T_CompanyThirdPartyCVIdentifier_T_CompanyThirdParty] FOREIGN KEY ([CompanyThirdPartyID_FK]) REFERENCES [dbo].[T_CompanyThirdParty] ([CompanyThirdPartyID])
);

