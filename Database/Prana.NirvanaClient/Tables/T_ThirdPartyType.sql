CREATE TABLE [dbo].[T_ThirdPartyType] (
    [ThirdPartyTypeID]        INT          IDENTITY (1, 1) NOT NULL,
    [ThirdPartyTypeName]      VARCHAR (50) NOT NULL,
    [ThirdPartyTypeShortName] VARCHAR (50) NULL,
    CONSTRAINT [PK_T_ThirdPartyType] PRIMARY KEY CLUSTERED ([ThirdPartyTypeID] ASC)
);

