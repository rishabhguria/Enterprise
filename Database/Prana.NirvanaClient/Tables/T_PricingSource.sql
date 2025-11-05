CREATE TABLE [dbo].[T_PricingSource] (
    [SourceID]          INT          IDENTITY (1, 1) NOT NULL,
    [SourceName]        VARCHAR (50) NOT NULL,
    [PricingSourceType] INT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_PricingSource] PRIMARY KEY CLUSTERED ([SourceID] ASC)
);

