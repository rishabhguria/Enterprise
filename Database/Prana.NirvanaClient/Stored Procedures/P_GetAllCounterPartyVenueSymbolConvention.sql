create procedure [dbo].[P_GetAllCounterPartyVenueSymbolConvention]
as

select CounterPartyID, VenueID,  SymbolConventionShortName 

from t_counterpartyvenue c join T_SymbolConvention s on c.SymbolConventionID = s.SymbolConventionID
