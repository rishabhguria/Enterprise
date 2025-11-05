CREATE TABLE [dbo].[T_UserRow] (
    [UserRowID] INT NOT NULL,
    [RowID]     INT NOT NULL,
    [UserID]    INT NOT NULL,
    CONSTRAINT [PK_T_UserRow] PRIMARY KEY CLUSTERED ([UserRowID] ASC),
    CONSTRAINT [FK_T_UserRow_T_CompanyUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID]),
    CONSTRAINT [FK_T_UserRow_T_Rows] FOREIGN KEY ([RowID]) REFERENCES [dbo].[T_Rows] ([RowID])
);

