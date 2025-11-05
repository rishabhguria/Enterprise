CREATE TABLE [dbo].[T_InterestReport] (
    [InterestReportID] INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]       VARCHAR (100) NULL,
    [Account]          VARCHAR (50)  NULL,
    [InterestMode]     VARCHAR (100) NULL,
    [Date]             DATETIME      NULL,
    [LongShort]        VARCHAR (20)  NULL,
    [Balance]          FLOAT (53)    NULL,
    [InterestRate]     FLOAT (53)    NULL,
    [DailyInterest]    FLOAT (53)    NULL,
    CONSTRAINT [PK_T_InterestReport] PRIMARY KEY CLUSTERED ([InterestReportID] ASC)
);

