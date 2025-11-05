CREATE TABLE [dbo].[T_RMUserTradingAccount] (
    [RMUserTradingAccntID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyID]            INT          NOT NULL,
    [CompanyUserID]        INT          NOT NULL,
    [UserTradingAccntID]   INT          NOT NULL,
    [UserTAExposureLimit]  DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_T_RMUserTradingAccount] PRIMARY KEY CLUSTERED ([RMUserTradingAccntID] ASC)
);

