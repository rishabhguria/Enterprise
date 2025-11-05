
/****** Object:  Stored Procedure dbo.P_GetSymbolMapping    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetSymbolMapping
	(
		@counterPartyVenueID int
	)
AS
	SELECT     CVSymbolMappingID, CVSM.CVAUECID, Symbol, MappedSymbol, CVA.CounterPartyVenueID
	FROM         T_CVSymbolMapping CVSM inner join T_CVAUEC CVA on CVSM.CVAUECID = CVA.CVAUECID 
					Where CVA.CounterPartyVenueID = @counterPartyVenueID	

