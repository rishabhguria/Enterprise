CREATE TABLE [dbo].[T_FundDefault] (
    [DefaultID]         INT              NOT NULL,
    [DefaultName]       VARCHAR (500)    NULL,
    [DefaultAllocation] VARBINARY (8000) NULL,
    CONSTRAINT [PK_T_FundDefault] PRIMARY KEY CLUSTERED ([DefaultID] ASC)
);

