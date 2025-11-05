CREATE TABLE [dbo].[T_State] (
    [StateID]   INT          NOT NULL,
    [State]     VARCHAR (50) NOT NULL,
    [CountryID] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK__T_State__1332DBDC] PRIMARY KEY CLUSTERED ([StateID] ASC) WITH (FILLFACTOR = 100)
);

