
/****** Object:  Stored Procedure dbo.P_GetAUECByAssetAndUnderLying    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetAUECByAssetAndUnderLying
	(
		@assetID int,
		@underlyingID int
	)
AS

SELECT DISTINCT AUECID, assetID, underlyingID, ExchangeID, DisplayName
FROM	T_AUEC
WHERE assetID = @assetID 
	AND underlyingID = @underlyingID 
		

/* SELECT DISTINCT A.AUECID, A.assetID, A.underlyingID, A.AUECExchangeID, AE.ExchangeID
FROM	T_AUEC A, T_AUECExchange AE 
WHERE A.assetID = @assetID 
	AND A.underlyingID = @underlyingID 
		AND AE.AUECExchangeID = A.AUECExchangeID */
		
	/* SELECT DISTINCT T_AUEC.AUECID, T_AUEC.assetID, T_AUEC.underlyingID, T_AUEC.AUECExchangeID
FROM	T_AUEC 
WHERE T_AUEC.assetID = @assetID 
	AND T_AUEC.underlyingID = @underlyingID  */

