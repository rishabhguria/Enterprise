CREATE TABLE [dbo].[T_CVSymbolConvention] (
    [CVSymbolConventionID] INT IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueID]  INT NOT NULL,
    [SymbolConventionID]   INT NOT NULL,
    CONSTRAINT [PK_T_CounterPartySymbolConvention] PRIMARY KEY CLUSTERED ([CVSymbolConventionID] ASC)
);

