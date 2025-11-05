CREATE TABLE [dbo].[T_CompanyUserFunds] (
    [CompanyUserFundID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyFundID]     INT NOT NULL,
    [CompanyUserID]     INT NOT NULL,
    CONSTRAINT [PK_T_CompanyUserFunds] PRIMARY KEY CLUSTERED ([CompanyUserFundID] ASC)
);

