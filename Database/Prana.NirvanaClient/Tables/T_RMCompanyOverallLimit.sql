CREATE TABLE [dbo].[T_RMCompanyOverallLimit] (
    [RMCompanyOverallLimitID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyID]               INT          NOT NULL,
    [RMBaseCurrencyID]        INT          NOT NULL,
    [CalculateRiskLimit]      INT          NOT NULL,
    [ExposureLimit]           DECIMAL (18) NOT NULL,
    [PositivePNL]             DECIMAL (18) NULL,
    [NegativePNL]             DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_T_CompanyOverallLimit] PRIMARY KEY CLUSTERED ([RMCompanyOverallLimitID] ASC),
    CONSTRAINT [FK_T_RMCompanyOverallLimit_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);

