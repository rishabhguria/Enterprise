CREATE TABLE [dbo].[T_ContractListingType] (
    [ContractListingTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [ContractListingType]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_T_ContractListingType] PRIMARY KEY CLUSTERED ([ContractListingTypeID] ASC)
);

