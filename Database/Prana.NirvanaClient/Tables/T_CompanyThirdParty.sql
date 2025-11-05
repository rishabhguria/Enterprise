CREATE TABLE [dbo].[T_CompanyThirdParty] (
    [CompanyThirdPartyID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyID]           INT NOT NULL,
    [ThirdPartyID]        INT NOT NULL,
    CONSTRAINT [PK_T_CompanyThirdParty] PRIMARY KEY CLUSTERED ([CompanyThirdPartyID] ASC),
    FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID]),
    CONSTRAINT [FK_T_CompanyThirdParty_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);

