CREATE TABLE [dbo].[T_XmlDataNew2] (
    [CompanyCVID]       VARCHAR (50) NULL,
    [TradingAccount]    VARCHAR (50) NULL,
    [ThirdPartyFFRunID] INT          NULL,
    [FFUserID]          INT          NULL,
    [StatusID]          INT          NULL,
    [FirstID]           INT          NULL,
    [SecondID]          INT          IDENTITY (1, 1) NOT NULL
);

