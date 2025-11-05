DECLARE @assetID INT
DECLARE @sideID INT
DECLARE @companyID INT

SET @companyID = (SELECT TOP 1 CompanyID FROM T_Company WHERE CompanyID > 0)

SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'FixedIncome')
SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

IF NOT EXISTS (SELECT AssetID FROM T_CompanyTTAssetPreferences WHERE AssetID = @assetID)
BEGIN
INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
VALUES (@companyID,@assetID,@sideID)
END