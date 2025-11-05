CREATE TABLE [dbo].[T_OPRAUnderlyingMapping] (
    [OPRASymbol]       VARCHAR (50)  NOT NULL,
    [UnderlyingSymbol] VARCHAR (50)  NULL,
    [CompanyName]      VARCHAR (255) NULL,
    [Exchange]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_T_OPRAUnderlyingMapping] PRIMARY KEY CLUSTERED ([OPRASymbol] ASC)
);

