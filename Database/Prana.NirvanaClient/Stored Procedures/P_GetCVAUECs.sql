
/****** Object:  Stored Procedure dbo.P_GetCVAUECs    Script Date: 12/23/2005 1:30:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECs
	(
		@counterPartyVenueID int		
	)
AS
	
	/*SELECT CounterPartyVenueID, AUECID, A.AssetID, A.UnderLyingID, A.ExchangeID 	
	FROM	T_CVAUEC, T_AUEC A
	Where  CounterPartyVenueID = @counterPartyVenueID */
	
	Select AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName From T_AUEC Where
AUECID in (Select AUECID from T_CVAUEC Where CounterPartyVenueID = @counterPartyVenueID)