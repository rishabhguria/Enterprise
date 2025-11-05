CREATE TABLE [dbo].[T_Country] (
    [CountryID]   INT           NOT NULL,
    [CountryName] VARCHAR (100) NOT NULL,
    [ISOCode] VARCHAR (100) Default '' NOT NULL
    CONSTRAINT [PK__T_Country__10566F31] PRIMARY KEY CLUSTERED ([CountryID] ASC) WITH (FILLFACTOR = 100)
);

