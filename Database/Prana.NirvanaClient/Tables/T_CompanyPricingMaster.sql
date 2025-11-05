CREATE TABLE [dbo].[T_CompanyPricingMaster] (
    [PricingRuleID]     INT NOT NULL,
    [CompanyFundID]     INT CONSTRAINT [DF_T_CompanyPricingMaster_CompanyFundID] DEFAULT ((0)) NOT NULL,
    [AssetClassID]      INT CONSTRAINT [DF_T_CompanyPricingMaster_AssetClassID] DEFAULT ((0)) NOT NULL,
    [ExchangeID]        INT CONSTRAINT [DF_T_CompanyPricingMaster_ExchangeID] DEFAULT ((0)) NOT NULL,
    [PricingDataType]   INT CONSTRAINT [DF_T_CompanyPricingMaster_PricingDataType] DEFAULT ((0)) NOT NULL,
    [SourceID]          INT CONSTRAINT [DF_T_CompanyPricingMaster_SourceID] DEFAULT ((0)) NOT NULL,
    [SecondarySourceID] INT CONSTRAINT [DF_T_CompanyPricingMaster_SecondarySourceID] DEFAULT ((0)) NOT NULL,
    [CompanyID]         INT CONSTRAINT [DF_T_CompanyPricingMaster_CompanyID] DEFAULT ((0)) NOT NULL,
    [RuleType]          INT CONSTRAINT [DF_T_CompanyPricingMaster_RuleType] DEFAULT ((0)) NOT NULL,
    [TimeDuration]      INT CONSTRAINT [DF_T_CompanyPricingMaster_TimeDuration] DEFAULT ((0)) NOT NULL,
    [IsPricingPolicy]   BIT CONSTRAINT [DF_T_CompanyPricingMaster_IsPricingPolicy] DEFAULT ((0)) NULL,
    [PricingPolicyID]   INT NULL,
    CONSTRAINT [PK_PricingRule] PRIMARY KEY CLUSTERED ([CompanyFundID] ASC, [AssetClassID] ASC, [ExchangeID] ASC, [RuleType] ASC, [TimeDuration] ASC, [CompanyID] ASC)
);

