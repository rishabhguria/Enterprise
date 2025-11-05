CREATE TABLE [dbo].[T_CompanyFundFeederFundAssociation] (
    [CompanyMasterSubAccountID] INT             IDENTITY (1, 1) NOT NULL,
    [CompanyFundID]             INT             NOT NULL,
    [CompanyFeederFundID]       INT             NOT NULL,
    [AllocatedAmount]           DECIMAL (10, 2) CONSTRAINT [DF_T_Comp_MasterFeederFundSubAccountAssociation_AllocatedAmount] DEFAULT ((0)) NOT NULL,
    [CurrencyID]                INT             DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_CompanyFundFeederFundAssociation] PRIMARY KEY CLUSTERED ([CompanyMasterSubAccountID] ASC)
);

