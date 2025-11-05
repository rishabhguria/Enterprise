CREATE TABLE [dbo].[T_Country] (
    [CountryID]   INT           IDENTITY (1, 1) NOT NULL,
    [CountryName] VARCHAR (100) NOT NULL,
    [ISOCode] VARCHAR (100) Default '' NOT NULL,
    [BloombergCountryCode] VARCHAR (100) Default '' NOT NULL
    CONSTRAINT [PK_T_Country] PRIMARY KEY CLUSTERED ([CountryID] ASC)
);

