CREATE TABLE [dbo].[PM_DataSourceCompanyFund] (
    [DataSourceCompanyFundID] INT           IDENTITY (1, 1) NOT NULL,
    [DataSourceID]            INT           NOT NULL,
    [CompanyID]               INT           NOT NULL,
    [CompanyFundID]           INT           NOT NULL,
    [DataSourceFundName]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PM_DataSourceCompanyFund] PRIMARY KEY CLUSTERED ([DataSourceCompanyFundID] ASC),
    CONSTRAINT [FK_PM_DataSourceCompanyFund_PM_DataSources] FOREIGN KEY ([DataSourceID]) REFERENCES [dbo].[PM_DataSources] ([DataSourceID]),
    CONSTRAINT [FK_PM_DataSourceCompanyFund_T_CompanyFunds] FOREIGN KEY ([CompanyFundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PM_DataSourceCompanyFund]
    ON [dbo].[PM_DataSourceCompanyFund]([CompanyFundID] ASC);

