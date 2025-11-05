CREATE TABLE [dbo].[T_RMCompanyTradAccntOverall] (
    [CompanyTradAccntRMID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyID]            INT          NOT NULL,
    [CompanyTradAccntID]   INT          NOT NULL,
    [TAExposureLimit]      DECIMAL (18) NOT NULL,
    [TAPositivePNL]        DECIMAL (18) NULL,
    [TANegativePNL]        DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_T_CompanyTradAccntRM] PRIMARY KEY CLUSTERED ([CompanyTradAccntRMID] ASC),
    CONSTRAINT [FK_T_RMCompanyTradAccntOverall_T_Company] FOREIGN KEY ([CompanyTradAccntRMID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);

