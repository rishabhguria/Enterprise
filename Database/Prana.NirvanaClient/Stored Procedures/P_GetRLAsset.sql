

CREATE PROCEDURE dbo.P_GetRLAsset
(
		@CompanyID int
)
AS

SELECT     T_Asset.AssetID, T_Asset.AssetName
FROM         T_AUEC INNER JOIN
                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID INNER JOIN
                      T_Asset ON T_AUEC.AssetID = T_Asset.AssetID
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID)
ORDER BY T_Asset.AssetName 

