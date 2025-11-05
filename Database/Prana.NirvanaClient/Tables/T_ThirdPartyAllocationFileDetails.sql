CREATE TABLE [dbo].[T_ThirdPartyAllocationFileDetails] (
    [ThirdPartyAllocationFileDetailId] INT           IDENTITY (1, 1) NOT NULL,
    [FileId]                           BIGINT        NOT NULL,
    [TaxlotId]                         VARCHAR (100) NOT NULL,
    [IsL1Data]                         BIT           NOT NULL
);

