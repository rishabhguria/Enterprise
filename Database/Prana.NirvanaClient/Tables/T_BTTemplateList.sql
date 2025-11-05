CREATE TABLE [dbo].[T_BTTemplateList] (
    [TemplateID]        VARCHAR (200) NOT NULL,
    [TemplateName]      VARCHAR (20)  NULL,
    [Columns]           VARCHAR (500) NULL,
    [IsDefaultTemplate] VARCHAR (5)   NULL,
    PRIMARY KEY CLUSTERED ([TemplateID] ASC) WITH (FILLFACTOR = 100)
);

