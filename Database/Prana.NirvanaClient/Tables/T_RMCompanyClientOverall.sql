CREATE TABLE [dbo].[T_RMCompanyClientOverall] (
    [CompanyClientRMID]   INT          IDENTITY (1, 1) NOT NULL,
    [ClientID]            INT          NOT NULL,
    [ClientExposureLimit] DECIMAL (18) NOT NULL,
    [CompanyID]           INT          NOT NULL,
    CONSTRAINT [PK_Table1_2] PRIMARY KEY CLUSTERED ([CompanyClientRMID] ASC),
    CONSTRAINT [FK_T_RMCompanyClientOverall_T_CompanyClient] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID])
);

