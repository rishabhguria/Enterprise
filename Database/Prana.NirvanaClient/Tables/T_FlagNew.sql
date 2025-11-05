CREATE TABLE [dbo].[T_FlagNew] (
    [CountryFlagiID]  INT           IDENTITY (1, 1) NOT NULL,
    [CountryFlagName] VARCHAR (50)  NOT NULL,
    [CountryFlagURL]  VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_T_FlagNew] PRIMARY KEY CLUSTERED ([CountryFlagiID] ASC)
);

