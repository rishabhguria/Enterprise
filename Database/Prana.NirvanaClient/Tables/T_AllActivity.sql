CREATE TABLE [dbo].[T_AllActivity] (
    [ActivityID]                 VARCHAR (50)   NOT NULL,
    [ActivityTypeId_FK]          INT            NOT NULL,
    [FKID]                       VARCHAR (50)   NOT NULL,
    [TransactionSource]          VARCHAR (300)  NULL,
    [FundID]                     INT            NULL,
    [Symbol]                     VARCHAR (300)  NULL,
    [TradeDate]                  DATETIME       NULL,
    [SettlementDate]             DATETIME       NULL,
    [CurrencyID]                 INT            NULL,
    [LeadCurrencyID]             INT            NULL,
    [VsCurrencyID]               INT            NULL,
    [ClosedQty]                  VARCHAR (50)   NULL,
    [Amount]                     VARCHAR (50)   NULL,
	[PnL]                        VARCHAR (50)   NULL,
	[FXPnL]                      VARCHAR (50)   NULL,
    [Commission]                 FLOAT (53)     CONSTRAINT [DF_T_AllActivity_Commission] DEFAULT ((0)) NULL,
    [OtherBrokerFees]            FLOAT (53)     CONSTRAINT [DF_T_AllActivity_OtherBrokerFees] DEFAULT ((0)) NULL,
    [StampDuty]                  FLOAT (53)     CONSTRAINT [DF_T_AllActivity_StampDuty] DEFAULT ((0)) NULL,
    [TransactionLevy]            FLOAT (53)     CONSTRAINT [DF_T_AllActivity_TransactionLevy] DEFAULT ((0)) NULL,
    [ClearingFee]                FLOAT (53)     CONSTRAINT [DF_T_AllActivity_ClearingFee] DEFAULT ((0)) NULL,
    [TaxOnCommissions]           FLOAT (53)     CONSTRAINT [DF_T_AllActivity_TaxOnCommissions] DEFAULT ((0)) NULL,
    [MiscFees]                   FLOAT (53)     CONSTRAINT [DF_T_AllActivity_MiscFees] DEFAULT ((0)) NULL,
    [FXRate]                     FLOAT (53)     NULL,
    [ActivityNumber]             INT            NULL,
    [Description]                VARCHAR (3000) NULL,
    [Subactivity]                VARCHAR (50)   NULL,
    [UniqueKey]                  VARCHAR (300)  NULL,
    [BalanceType]                INT            NOT NULL,
    [ActivitySource]             VARCHAR (50)   NULL,
    [FXConversionMethodOperator] VARCHAR (3)    NULL,
    [SideMultiplier]             INT            NULL,
    [SecFee]                     FLOAT (53)     CONSTRAINT [DF_T_AllActivity_SecFee] DEFAULT ((0)) NULL,
    [OccFee]                     FLOAT (53)     CONSTRAINT [DF_T_AllActivity_OccFee] DEFAULT ((0)) NULL,
    [OrfFee]                     FLOAT (53)     CONSTRAINT [DF_T_AllActivity_OrfFee] DEFAULT ((0)) NULL,
    [ClearingBrokerFee]          FLOAT (53)     CONSTRAINT [DF_T_AllActivity_ClearingBrokerFee] DEFAULT ((0)) NULL,
    [SoftCommission]             FLOAT (53)     CONSTRAINT [DF_T_AllActivity_SoftCommission] DEFAULT ((0)) NULL,
    [SettlCurrency]              INT            NULL,
    [OptionPremiumAdjustment]    FLOAT (53)     CONSTRAINT [DF_T_AllActivity_OptionPremiumAdjustment] DEFAULT ((0)) NULL,
    [BaseAmount]                 FLOAT (53)     DEFAULT ((0)) NULL,
    [ModifyDate]                 DATETIME       CONSTRAINT [DF_ALLMODIFYDATE] DEFAULT (getdate()) NULL,
    [EntryDate]                  DATETIME       CONSTRAINT [DF_ALLENTRYDATE] DEFAULT (getdate()) NULL,
    [UserId]                     INT            NULL,
    CONSTRAINT [PK_T_AllActivity] PRIMARY KEY CLUSTERED ([ActivityID] ASC),
    CONSTRAINT [FK_T_AllActivity_T_ActivityType] FOREIGN KEY ([ActivityTypeId_FK]) REFERENCES [dbo].[T_ActivityType] ([ActivityTypeId])
);

GO
CREATE INDEX [FKID_NonClustered]
	ON [dbo].[T_AllActivity](FKID);

	GO
CREATE INDEX [UniqueKey_NonClustered]
	ON [dbo].[T_AllActivity](UniqueKey);

GO
CREATE NONCLUSTERED INDEX [NonClustered_ACTID_TDATE_SDATE] ON [dbo].[T_AllActivity]
(
	[ActivityID] ASC,
	[TradeDate] ASC,
	[SettlementDate] ASC
);

