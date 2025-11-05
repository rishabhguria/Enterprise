CREATE TABLE [dbo].[T_FundWorkflowStats] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Date]         DATETIME      NULL,
    [FundID]       INT           NULL,
    [TaskID]       INT           NULL,
    [StateID]      INT           NULL,
    [Comments]     NCHAR (500)   NULL,
    [ContextID]    INT           NULL,
    [ContextValue] VARCHAR (100) NULL,
    [TaskRunTime]  DATETIME      NULL
);

