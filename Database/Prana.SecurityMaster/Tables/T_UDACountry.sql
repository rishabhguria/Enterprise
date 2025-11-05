CREATE TABLE [dbo].[T_UDACountry] (
    [CountryID]   INT           NOT NULL,
    [CountryName] VARCHAR (100) NULL,
    CONSTRAINT [T_UDACountry_Unique_CountryName] UNIQUE NONCLUSTERED ([CountryName] ASC)
);

