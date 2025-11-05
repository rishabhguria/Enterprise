CREATE TABLE [dbo].[T_CompanyClientFund] (
    [CompanyClientFundID]        INT          IDENTITY (1, 1) NOT NULL,
    [CompanyClientFundName]      VARCHAR (50) NOT NULL,
    [CompanyClientFundShortName] VARCHAR (50) NOT NULL,
    [CompanyClientID]            INT          NOT NULL,
    CONSTRAINT [PK_T_CompanyClientFund] PRIMARY KEY CLUSTERED ([CompanyClientFundID] ASC),
    CONSTRAINT [FK_T_CompanyClientFund_T_CompanyClient] FOREIGN KEY ([CompanyClientID]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID])
);

