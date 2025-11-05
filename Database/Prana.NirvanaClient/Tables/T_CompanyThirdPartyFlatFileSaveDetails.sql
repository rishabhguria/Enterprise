CREATE TABLE [dbo].[T_CompanyThirdPartyFlatFileSaveDetails] (
    [CompanyThirdPartyID]           INT           NOT NULL,
    [SaveGeneratedFileIn]           VARCHAR (100) NOT NULL,
    [NamingConvention]              VARCHAR (50)  NULL,
    [CompanyThirdPartySaveDetailID] INT           IDENTITY (1, 1) NOT NULL,
    [CompanyIdentifier]             NCHAR (15)    NOT NULL,
    CONSTRAINT [PK_T_CompanyThirdPartyFlatFileSaveDetails] PRIMARY KEY CLUSTERED ([CompanyThirdPartySaveDetailID] ASC)
);

