CREATE TABLE [dbo].[T_Rows] (
    [RowID]        INT          NOT NULL,
    [RowColor]     VARCHAR (2)  NULL,
    [RowTextColor] VARCHAR (2)  NULL,
    [RowName]      VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_T_Rows] PRIMARY KEY CLUSTERED ([RowID] ASC)
);

