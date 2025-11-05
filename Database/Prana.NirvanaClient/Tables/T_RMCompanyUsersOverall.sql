CREATE TABLE [dbo].[T_RMCompanyUsersOverall] (
    [RMCompanyUserID]      INT          IDENTITY (1, 1) NOT NULL,
    [CompanyUserID]        INT          NOT NULL,
    [UserExposureLimit]    DECIMAL (18) NOT NULL,
    [MaximumPNLLoss]       DECIMAL (18) NOT NULL,
    [MaximumSizePerOrder]  DECIMAL (18) NOT NULL,
    [MaximumSizePerBasket] DECIMAL (18) NOT NULL,
    [CompanyID]            INT          NOT NULL,
    CONSTRAINT [PK_T_RMCompanyUsersOverall] PRIMARY KEY CLUSTERED ([RMCompanyUserID] ASC),
    CONSTRAINT [FK_T_RMCompanyUsersOverall_T_CompanyUser1] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

