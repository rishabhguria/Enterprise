
/****** Object:  Stored Procedure dbo.P_GetAllCVAUECs    Script Date: 04/06/2006 10:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetAllCVAUECs
AS
	Select CVAUECID, CounterPartyVenueID, CVA.AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName
	 From T_CVAUEC CVA 
	inner join T_AUEC A on	CVA.AUECID = A.AUECID 
