CREATE TABLE [dbo].[T_ThirdPartyCVIdentifier] (
    [ThirdPartyCVID]                INT          NOT NULL,
    [ThirdPartyID_FK]               INT          NOT NULL,
    [CompanyCounterPartyVenueID_FK] INT          NOT NULL,
    [CVIdentifier]                  VARCHAR (50) NOT NULL,
    FOREIGN KEY ([ThirdPartyID_FK]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

