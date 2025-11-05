CREATE TABLE [dbo].[T_StockloanReport] (
    [StockLoanReportID] INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]        VARCHAR (100) NULL,
    [Account]           VARCHAR (50)  NULL,
    [SecurityType]      VARCHAR (100) NULL,
    [Security]          VARCHAR (200) NULL,
    [Date]              DATETIME      NULL,
    [ContractValue]     FLOAT (53)    NULL,
    [Quantity]          FLOAT (53)    NULL,
    [ContractPrice]     FLOAT (53)    NULL,
    [FedRate]           FLOAT (53)    NULL,
    [BorrowRate]        FLOAT (53)    NULL,
    [BorrowInterest]    FLOAT (53)    NULL,
    CONSTRAINT [PK_T_StockloanReport] PRIMARY KEY CLUSTERED ([StockLoanReportID] ASC)
);

