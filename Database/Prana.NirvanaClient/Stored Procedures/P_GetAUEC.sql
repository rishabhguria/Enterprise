
/****** Object:  Stored Procedure dbo.P_GetAUEC    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetAUEC	
(
	@auecID int		
)
AS
	Select T_AUEC.AUECID, T_AUEC.assetID, T_AUEC.underlyingID, T_AUEC.ExchangeID, T_AUEC.BaseCurrencyID, T_AUEC.DisplayName
	From T_AUEC
	Where auecID = @auecID
