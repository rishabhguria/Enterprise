CREATE TABLE [dbo].[T_ThirdPartyFFRunFunds] (
    [ThirdPartyFFRunFundID]  INT IDENTITY (1, 1) NOT NULL,
    [CompanyThirdPartyID_FK] INT NOT NULL,
    [ThirdPartyFFRunID_FK]   INT NOT NULL,
    [CompanyFundID]          INT NOT NULL,
    CONSTRAINT [PK_T_ThirdPartyFFRunFunds] PRIMARY KEY CLUSTERED ([ThirdPartyFFRunFundID] ASC),
    CONSTRAINT [FK_T_ThirdPartyFFRunFunds_T_ThirdPartyFFRunReport] FOREIGN KEY ([ThirdPartyFFRunID_FK]) REFERENCES [dbo].[T_ThirdPartyFFRunReport] ([ThirdPartyFFRunID])
);

