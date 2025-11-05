CREATE TABLE [dbo].[T_BTConventionMapping] (
    [ConventionMappingID] INT           IDENTITY (1, 1) NOT NULL,
    [Percentage]          VARCHAR (10)  NOT NULL,
    [RoundLot]            VARCHAR (10)  NOT NULL,
    [TemplateID]          VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_T_BTOrderSideMapping] PRIMARY KEY CLUSTERED ([ConventionMappingID] ASC),
    FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[T_BTTemplateList] ([TemplateID])
);

