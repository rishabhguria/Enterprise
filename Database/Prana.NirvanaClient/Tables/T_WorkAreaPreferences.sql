CREATE TABLE [dbo].[T_WorkAreaPreferences] (
    [IsUseWorkAreaPreferences] BIT           DEFAULT ((1)) NOT NULL,
    [CounterPartyID]           INT           NOT NULL,
    [VenueID]                  INT           NOT NULL,
    [TIFID]                    INT           NOT NULL,
    [TradingAccountID]         INT           NOT NULL,
    [CommaSeparatedFunds] VARCHAR(MAX) NOT NULL DEFAULT '', 
    [NonScalableMasterFunds] VARCHAR(MAX) NOT NULL DEFAULT '', 
    [CalculationStrategy] INT NOT NULL DEFAULT 0, 
    [IsIncludeExcludeAllowed] BIT NOT NULL DEFAULT 1
);

