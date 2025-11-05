CREATE TABLE [dbo].[T_PranaKeyValuePreferences] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [PreferenceKey]   VARCHAR (200) NOT NULL,
    [PreferenceValue] SQL_VARIANT   NULL,
    [Description]     NCHAR (500)   NULL,
    CONSTRAINT [PK_T_PranaKeyValuePreferences] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UQ__T_PranaKeyValueP__02F4B477] UNIQUE NONCLUSTERED ([PreferenceKey] ASC)
);

