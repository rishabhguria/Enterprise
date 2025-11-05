CREATE TABLE [dbo].[T_AuditTrailOtherPermissions] (
    [CompanyId]    INT           NOT NULL,
    [UserId]       INT           NOT NULL,
    [IgnoredUsers] VARCHAR (200) NULL,
    CONSTRAINT [FK_T_AuditTrailOtherPermissions_T_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [FK_T_AuditTrailOtherPermissions_T_CompanyUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

