CREATE TABLE [dbo].[T_CA_CompliancePreferences] (
    [CompanyId]          INT           NOT NULL,
    [ImportExportPath]   VARCHAR (MAX) NOT NULL,
    [PrePostCrossImport] BIT           NOT NULL,
    [InMarket]           BIT           DEFAULT ('False') NOT NULL,
    [InStage]            BIT           DEFAULT ('False') NOT NULL,
    [PostInMarket]		 BIT           DEFAULT ('False') NOT NULL,
    [PostInStage]        BIT           DEFAULT ('False') NOT NULL,
    [BlockTradeOnComplianceFaliure] BIT     DEFAULT ('True')  NOT NULL, 
	[StageValueFromField] [bit]   DEFAULT ('False') NOT NULL,
	[StageValueFromFieldString] [varchar](100)  DEFAULT (' ') NOT NULL,
    [IsBasketComplianceEnabledCompany]		 BIT           DEFAULT ('False') NOT NULL,
 CONSTRAINT [PK_T_CA_CompliancePreferences] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);

