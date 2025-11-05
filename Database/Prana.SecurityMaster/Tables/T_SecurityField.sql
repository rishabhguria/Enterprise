CREATE TABLE [dbo].[T_SecurityField] (
    [FieldID]      INT           NOT NULL,
    [FieldName]    VARCHAR (50)  NOT NULL,
    [IsRealTime]   BIT           DEFAULT ((0)) NOT NULL,
    [IsHistorical] BIT           DEFAULT ((0)) NOT NULL,
    [Esignal]      VARCHAR (100) DEFAULT ((0)) NOT NULL,
    [Bloomberg]    VARCHAR (100) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_SecurityField] PRIMARY KEY CLUSTERED ([FieldID] ASC),
    UNIQUE NONCLUSTERED ([FieldName] ASC)
);

