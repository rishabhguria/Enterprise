CREATE TABLE [dbo].[T_TTRiskNValidationSetting] (
    [CompanyUserID]           INT            NULL,
    [PK]                      INT            IDENTITY (1, 1) NOT NULL,
    [IsRiskChecked]           VARCHAR (6)    NOT NULL,
    [IsValidateSymbolChecked] VARCHAR (6)    NOT NULL,
    [RiskValue]               FLOAT (53)     NOT NULL,
    [LimitPriceChecked]       VARCHAR (6)    NOT NULL,
    [SetExecutedQtytoZero]    NVARCHAR (100) DEFAULT ('False') NOT NULL,
    CONSTRAINT [PK_T_TTRiskNValidationSetting] PRIMARY KEY CLUSTERED ([PK] ASC)
);

