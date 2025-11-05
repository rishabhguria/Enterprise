CREATE TABLE [dbo].[T_Fields] (
    [FieldID]       INT          IDENTITY (1, 1) NOT NULL,
    [FieldName]     VARCHAR (50) NOT NULL,
    [IsDefault]     BIT          CONSTRAINT [DF_T_Fields_IsDefault] DEFAULT (1) NOT NULL,
    [IsColumnField] BIT          CONSTRAINT [DF_T_Fields_IsColumnField] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_T_Fields] PRIMARY KEY CLUSTERED ([FieldID] ASC)
);

