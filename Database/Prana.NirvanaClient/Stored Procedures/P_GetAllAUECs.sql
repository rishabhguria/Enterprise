
/****** Object:  Stored Procedure dbo.P_GetAllAUECs    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetAllAUECs
AS
	SELECT   AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName
FROM         T_AUEC order by AssetID, UnderLyingID, ExchangeID, BaseCurrencyID
