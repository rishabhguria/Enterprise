CREATE TABLE [dbo].[T_AllocationScheme] (
    [AllocationSchemeID]   INT           IDENTITY (30000, 1) NOT NULL,
    [Date]                 DATETIME      NOT NULL,
    [AllocationSchemeName] VARCHAR (100) NOT NULL,
    [AllocationScheme]     XML           NOT NULL,
	[IsPrefVisible]		   BIT			DEFAULT 1 NOT NULL,
	[CreationSource]	   INT			DEFAULT 1 NOT NULL
);

