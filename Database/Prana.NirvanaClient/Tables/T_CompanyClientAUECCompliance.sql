CREATE TABLE [dbo].[T_CompanyClientAUECCompliance] (
    [CompanyClientAUECComplianceID] INT          NOT NULL,
    [CompanyClientAUECID]           INT          NOT NULL,
    [FollowComplaince]              BIT          NOT NULL,
    [ShortSellConfirmation]         BIT          NOT NULL,
    [ForeignID]                     VARCHAR (50) NULL,
    CONSTRAINT [PK_T_CompanyClientAUECCompliance] PRIMARY KEY CLUSTERED ([CompanyClientAUECComplianceID] ASC)
);

