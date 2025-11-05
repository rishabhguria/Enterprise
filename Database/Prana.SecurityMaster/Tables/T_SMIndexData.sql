CREATE TABLE [dbo].[T_SMIndexData] (
    [Symbol_PK]       BIGINT        NULL,
    [LongName]        VARCHAR (200) NULL,
    [ShortName]       VARCHAR (100) NULL,
    [PK]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [LeveragedFactor] FLOAT (53)    NULL,
    CONSTRAINT [PK_T_SMIndexData] PRIMARY KEY CLUSTERED ([PK] ASC),
    CONSTRAINT [FK_T_SMIndexData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);

