CREATE TABLE [dbo].[PM_ReconDataSourceXSLT] (
    [ReconThirdPartyID] INT          IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]      INT          NOT NULL,
    [ReconType]         INT          NOT NULL,
    [FormatType]        VARCHAR (50) NULL,
    [XSLTID]            INT          NULL
);

