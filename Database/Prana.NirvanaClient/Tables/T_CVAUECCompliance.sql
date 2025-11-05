CREATE TABLE [dbo].[T_CVAUECCompliance] (
    [CVAUECComplianceID]    BIGINT       IDENTITY (1, 1) NOT NULL,
    [CVAUECID]              BIGINT       NOT NULL,
    [FollowCompliance]      INT          NOT NULL,
    [ShortSellConfirmation] INT          NOT NULL,
    [IdentifierID]          INT          NOT NULL,
    [ForeignID]             VARCHAR (20) NULL,
    CONSTRAINT [PK_T_CounterPartyVenueCompliance] PRIMARY KEY CLUSTERED ([CVAUECComplianceID] ASC)
);

