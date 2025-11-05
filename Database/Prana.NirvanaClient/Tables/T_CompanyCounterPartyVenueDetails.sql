CREATE TABLE [dbo].[T_CompanyCounterPartyVenueDetails] (
    [CompanyCounterPartyVenueDetailsID]  INT          IDENTITY (1, 1) NOT NULL,
    [CompanyCounterPartyVenueID]         INT          NOT NULL,
    [CompanyClearingFirmsPrimeBrokersID] INT          NOT NULL,
    [CompanyMPID]                        INT          NULL,
    [DeliverToCompanyID]                 VARCHAR (20) NULL,
    [SenderCompanyID]                    VARCHAR (20) NOT NULL,
    [TargetCompID]                       VARCHAR (20) NOT NULL,
    [ClearingFirm]                       VARCHAR (20) NULL,
    [CompanyFundID]                      INT          NULL,
    [CompanyStrategyID]                  INT          NULL,
    [OnBehalfOfSubID]                    VARCHAR (20) NULL,
    CONSTRAINT [PK_T_CompanyCounterPartyVenueDetails] PRIMARY KEY CLUSTERED ([CompanyCounterPartyVenueDetailsID] ASC)
);

