CREATE TABLE [dbo].[T_State] (
    [StateID]   INT          IDENTITY (1, 1) NOT NULL,
    [State]     VARCHAR (50) NOT NULL,
    [CountryID] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_State] PRIMARY KEY CLUSTERED ([StateID] ASC)
);

