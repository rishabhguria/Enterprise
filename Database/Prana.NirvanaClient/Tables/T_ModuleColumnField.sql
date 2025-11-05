CREATE TABLE [dbo].[T_ModuleColumnField] (
    [ModuleColumnFieldID] INT      IDENTITY (1, 1) NOT NULL,
    [FieldID]             INT      NOT NULL,
    [ModuleID]            INT      NOT NULL,
    [UserID]              INT      NOT NULL,
    [ColorID]             INT      NOT NULL,
    [SortOrder]           CHAR (4) CONSTRAINT [DF_T_ModuleColumnField_SortOrder] DEFAULT ('Desc') NOT NULL,
    CONSTRAINT [PK_T_ModuleColumnField] PRIMARY KEY CLUSTERED ([ModuleColumnFieldID] ASC)
);

