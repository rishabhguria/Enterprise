


CREATE PROCEDURE [dbo].[P_GetRLAllAUECAssetUnderLyingExchange]
(
	@CompanyID int
)
AS
SELECT DISTINCT 
                      T_AUEC.AUECID, T_Asset.AssetID, T_Asset.AssetName, T_UnderLying.UnderLyingID, 
					  T_UnderLying.UnderLyingName, T_AUEC.ExchangeID, 
					  T_AUEC.DisplayName AS ExchangeName
FROM         T_AUEC INNER JOIN
                      T_Asset ON T_AUEC.AssetID = T_Asset.AssetID INNER JOIN
                      T_UnderLying ON T_AUEC.UnderLyingID = T_UnderLying.UnderLyingID INNER JOIN
                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID)
ORDER BY T_Asset.AssetName, T_UnderLying.UnderLyingName, T_AUEC.DisplayName
	RETURN 



