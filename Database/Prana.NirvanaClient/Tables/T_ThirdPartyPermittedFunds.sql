CREATE TABLE [dbo].[T_ThirdPartyPermittedFunds] (
    [ThirdPartyFundID_PK] INT IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]        INT NOT NULL,
    [CopanyFundID]        INT NOT NULL,
    CONSTRAINT [PK_T_ThirdPartyPermittedFunds] PRIMARY KEY CLUSTERED ([ThirdPartyFundID_PK] ASC)
);

