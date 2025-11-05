CREATE TABLE [dbo].[T_PBWiseTaxlotState] (
    [TableID]           INT           PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [PBID]              INT           NOT NULL,
    [TaxLotID]          VARCHAR (50)  NOT NULL,
    [TaxLotState]       INT           NOT NULL,
    [PBTaxlotID]        VARCHAR (100) NOT NULL,
    [FileFormatID]      INT           CONSTRAINT [DF_T_PBWiseTaxlotState_FileFormatID] DEFAULT ((0)) NULL
);

GO
CREATE INDEX [PBWise_NonClust_TaxlotID_Format] ON [dbo].[T_PBWiseTaxlotState]
(
	[TaxLotID] ASC,
	[FileFormatID] ASC
)
INCLUDE ([TaxLotState])