CREATE TABLE [dbo].[T_CompanyFeederFunds] (
    [FeederFundID]        INT             IDENTITY (1, 1) NOT NULL,
    [FeederFundName]      VARCHAR (100)   NOT NULL,
    [FeederFundShortName] VARCHAR (50)    NOT NULL,
    [CompanyID]           INT             NOT NULL,
    [FundTypeID]          INT             NULL,
    [UIOrder]             INT             CONSTRAINT [DF_T_CompanyFeederFunds_UIOrder] DEFAULT ((9999)) NOT NULL,
    [Amount]              DECIMAL (10, 2) NOT NULL,
    [AllocatedAmount]     DECIMAL (10, 2) CONSTRAINT [DF_T_CompanyFeederFunds_TotalCapital] DEFAULT ((0.0)) NULL,
    [RemainingAmount]     DECIMAL (10, 2) NULL,
    [Currency]            INT             NOT NULL,
    [IsActive]            BIT             DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_CompanyFeederFunds] PRIMARY KEY CLUSTERED ([FeederFundID] ASC)
);

