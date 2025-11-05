CREATE TABLE [dbo].[T_CounterPartyType] (
    [CounterPartyTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [CounterPartyType]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_T_CounterPartyType] PRIMARY KEY CLUSTERED ([CounterPartyTypeID] ASC)
);

