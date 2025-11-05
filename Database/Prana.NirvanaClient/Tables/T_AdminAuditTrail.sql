CREATE TABLE [dbo].[T_AdminAuditTrail] (
    [AuditTrailID]        INT           IDENTITY (1, 1) NOT NULL,
    [CompanyID]           INT           NOT NULL,
    [CompanyFundID]       INT           NOT NULL,
    [UserID]              INT           CONSTRAINT [DF_T_AdminAuditTrail_UserID] DEFAULT ((0)) NOT NULL,
    [ActionID]            INT           NOT NULL,
    [ExecutionTime]       DATETIME      NOT NULL,
    [StatusID]            INT           NULL,
    [Comments]            VARCHAR (100) NULL,
    [ModuleID]            INT           NULL,
    [ActualActionDate]    DATETIME      NULL,
    [AuditDimensionValue] INT           NULL,
    [IsActive]            BIT           DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_AdminAuditTrail] PRIMARY KEY CLUSTERED ([AuditTrailID] ASC)
);

