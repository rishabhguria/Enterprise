CREATE TABLE [dbo].[T_RMCompanyFundAccntOverall] (
    [CompanyFundAccntRMID]        INT          IDENTITY (1, 1) NOT NULL,
    [ExposureLimitRMBaseCurrency] DECIMAL (18) NOT NULL,
    [CompanyID]                   INT          NOT NULL,
    [FAPositivePNL]               DECIMAL (18) NULL,
    [FANegativePNL]               DECIMAL (18) NOT NULL,
    [CompanyFundAccntID]          INT          NOT NULL,
    CONSTRAINT [PK_T_RMCompanyFundaccntOverall] PRIMARY KEY CLUSTERED ([CompanyFundAccntRMID] ASC),
    CONSTRAINT [FK_T_RMCompanyFundAccntOverall_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);

