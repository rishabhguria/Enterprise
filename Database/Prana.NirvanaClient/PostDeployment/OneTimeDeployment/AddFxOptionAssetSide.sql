IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyTTAssetPreferences' AND COLUMN_NAME = 'CompanyAssetWisePrefID')
BEGIN
 
    --TRUNCATE table T_CompanyTTAssetPreferences

	DECLARE @assetID INT
	DECLARE @sideID INT
	DECLARE @companyID INT
	
	SET @companyID = (SELECT TOP 1 CompanyID FROM T_Company WHERE CompanyID > 0)

	---FXOption Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'FXOption')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
		IF NOT EXISTS (select * from T_CompanyTTAssetPreferences where AssetID=10)
		BEGIN
		INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
		VALUES (@companyID,@assetID,@sideID)
		END
	END
END