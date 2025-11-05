CREATE TABLE [dbo].[T_CA_UserReadWritePermission] (
    [CompanyId] INT           NOT NULL,
    [UserId]    INT           NOT NULL,
    [RuleType]  VARCHAR (100) NOT NULL,
    [IsCreate]  BIT           NULL,
    [IsRename]  BIT           NULL,
    [IsDelete]  BIT           NULL,
    [IsEnable]  BIT           NULL,
    [IsExport]  BIT           NULL,
    [IsImport]  BIT           NULL,
    PRIMARY KEY CLUSTERED ([CompanyId] ASC, [UserId] ASC, [RuleType] ASC)
);

