CREATE TABLE [dbo].[T_CountryFlag] (
    [CountryFlagID]    INT          IDENTITY (1, 1) NOT NULL,
    [CountryFlagName]  VARCHAR (50) NOT NULL,
    [CountryFlagImage] IMAGE        NOT NULL,
    CONSTRAINT [PK_T_Flag] PRIMARY KEY CLUSTERED ([CountryFlagID] ASC)
);

