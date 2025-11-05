CREATE TABLE [dbo].[T_ReportTemplate] (
    [ReportID]       INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]     VARCHAR (100) NOT NULL,
    [StartDate]      DATETIME      NULL,
    [EndDate]        DATETIME      NULL,
    [ThirdPartyID]   VARCHAR (MAX) NOT NULL,
    [Funds]          VARCHAR (MAX) NOT NULL,
    [Columns]        VARCHAR (MAX) NOT NULL,
    [GroupingBy]     VARCHAR (MAX) NULL,
    [CronExpression] VARCHAR (100) NOT NULL,
    [FormatType]     VARCHAR (10)  NOT NULL,
    [WhereClause]    VARCHAR (MAX) NULL,
    [LastRunTime]    DATETIME      NULL,
    CONSTRAINT [PK_T_ReportTemplate] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);

