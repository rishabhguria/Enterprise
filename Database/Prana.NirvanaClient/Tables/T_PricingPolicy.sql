CREATE TABLE [dbo].[T_PricingPolicy] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [PolicyName]      VARCHAR (200) NOT NULL,
    [SPName]          VARCHAR (200) NOT NULL,
    [IsFileAvailable] BIT           NOT NULL,
    [FilePath]        VARCHAR (200) NULL,
    [FolderPath]      VARCHAR (200) NULL,
    CONSTRAINT [PK_T_PricingPolicy] PRIMARY KEY CLUSTERED ([PolicyName] ASC)
);

