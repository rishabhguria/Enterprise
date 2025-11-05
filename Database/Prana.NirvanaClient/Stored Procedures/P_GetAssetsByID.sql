CREATE PROCEDURE [dbo].[P_GetAssetsByCompanyID] (@companyID INT)
AS
SELECT DISTINCT T_Asset.AssetID
	,T_Asset.AssetName
FROM T_CompanyAUEC
INNER JOIN T_AUEC
	ON T_AUEC.AUECID = T_CompanyAUEC.AUECID
INNER JOIN T_Asset
	ON T_Asset.AssetID = T_AUEC.AssetID
WHERE T_CompanyAUEC.CompanyID = @companyID