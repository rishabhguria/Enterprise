IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyTTAssetPreferences' AND COLUMN_NAME = 'CompanyAssetWisePrefID')
BEGIN
 
     TRUNCATE table T_CompanyTTAssetPreferences
	DECLARE @assetID INT
	DECLARE @sideID INT
	DECLARE @companyID INT
	
	SET @companyID = (SELECT TOP 1 CompanyID FROM T_Company WHERE CompanyID > 0)

	---Equity Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'Equity')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---PrivateEquity Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'PrivateEquity')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---Indices Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'Indices')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---Futures Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'Future')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---FX Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'FX')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---FXForward Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'FXForward')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---FixedIncome Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'FixedIncome')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---CreditDefaultSwap Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'CreditDefaultSwap')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---ConvertibleBond Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'ConvertibleBond')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---EquityOption Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'EquityOption')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy to Open')

	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

	---FutureOption Sides Begin
	SET @assetID = (SELECT AssetID FROM T_Asset WHERE AssetName = 'FutureOption')
	SET @sideID = (SELECT SideID FROM T_Side WHERE Side = 'Buy to Open')
	
	IF EXISTS (SELECT AssetID FROM T_CompanyAUEC CA INNER JOIN T_AUEC A ON CA.AUECID = A.AUECID WHERE AssetID = @assetID)
	BEGIN
	INSERT INTO T_CompanyTTAssetPreferences (CompanyID,AssetID,SideID)
	VALUES (@companyID,@assetID,@sideID)
	END

    SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Cash'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_CompanyTTAssetPreferences (
		CompanyID
		,AssetID
		,SideID
		)
	VALUES (
		@companyID
		,@assetID
		,@sideID
		)

    SET @assetID = (
			SELECT AssetID
			FROM T_Asset
			WHERE AssetName = 'Forex'
			)
	SET @sideID = (
			(
				SELECT SideID
				FROM T_Side
				WHERE Side = 'Buy'
				)
			)

	INSERT INTO T_CompanyTTAssetPreferences (
		CompanyID
		,AssetID
		,SideID
		)
	VALUES (
		@companyID
		,@assetID
		,@sideID
		)
END
