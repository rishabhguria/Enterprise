CREATE TABLE [dbo].[T_CompanyClientTrader] (
    [TraderID]        INT          IDENTITY (1, 1) NOT NULL,
    [FirstName]       VARCHAR (50) NULL,
    [LastName]        VARCHAR (50) NULL,
    [ShortName]       VARCHAR (50) NULL,
    [Title]           VARCHAR (50) NULL,
    [EMail]           VARCHAR (50) NULL,
    [TelephoneWork]   VARCHAR (50) NULL,
    [TelephoneCell]   VARCHAR (50) NULL,
    [Pager]           VARCHAR (50) NULL,
    [TelephoneHome]   VARCHAR (50) NULL,
    [Fax]             VARCHAR (50) NULL,
    [CompanyClientID] INT          NOT NULL,
    CONSTRAINT [PK_T_CompanyTrader] PRIMARY KEY CLUSTERED ([TraderID] ASC),
    CONSTRAINT [FK_T_CompanyClientTrader_T_CompanyClient] FOREIGN KEY ([CompanyClientID]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID])
);

