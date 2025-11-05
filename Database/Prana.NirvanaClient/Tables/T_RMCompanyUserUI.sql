CREATE TABLE [dbo].[T_RMCompanyUserUI] (
    [RMCompanyUserUIID]              INT          IDENTITY (1, 1) NOT NULL,
    [CompanyID]                      INT          NOT NULL,
    [CompanyUserID]                  INT          NOT NULL,
    [TicketSize]                     DECIMAL (18) NOT NULL,
    [PriceDeviation]                 DECIMAL (18) NOT NULL,
    [AllowUsertoOverwrite]           INT          NULL,
    [NotifyUserWhenLiveFeedsAreDown] INT          NULL,
    [CompanyUserAUECID]              INT          NOT NULL,
    CONSTRAINT [PK_T_RMCompanyUserUI] PRIMARY KEY CLUSTERED ([RMCompanyUserUIID] ASC),
    CONSTRAINT [FK_T_RMCompanyUserUI_T_CompanyUser] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

