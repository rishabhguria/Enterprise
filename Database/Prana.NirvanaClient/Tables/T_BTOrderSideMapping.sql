CREATE TABLE [dbo].[T_BTOrderSideMapping] (
    [OrderSideMappingID] INT           IDENTITY (1, 1) NOT NULL,
    [TemplateID]         VARCHAR (200) NOT NULL,
    [SideMappingString]  VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_T_BTOrderSideMapping_1] PRIMARY KEY CLUSTERED ([OrderSideMappingID] ASC),
    FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[T_BTTemplateList] ([TemplateID])
);

