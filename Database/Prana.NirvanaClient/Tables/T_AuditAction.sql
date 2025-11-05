CREATE TABLE [dbo].[T_AuditAction] (
    [ActionId]          INT            NOT NULL,
    [ActionName]        NVARCHAR (50)  NOT NULL,
    [ActionType]        NVARCHAR (50)  NOT NULL,
    [AuditDimension]    NVARCHAR (50)  NOT NULL,
    [ActionDescription] NVARCHAR (100) NULL
);

