IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_UserTTAssetPreferences'
			AND COLUMN_NAME = 'UserAssetWisePrefID'
		)
BEGIN

     TRUNCATE table T_UserTTAssetPreferences

     CREATE TABLE #Temp_UserAsset
	(
	     UserID INT,
		AssetID INT
	)

	INSERT INTO #Temp_UserAsset
	(
	     UserID,
		AssetID
	)
	Select
	T_CompanyUser.UserID as UserID,
	T_Asset.AssetID as AssetID
	from T_CompanyUser, T_Asset

	Insert Into T_UserTTAssetPreferences
	(
	     UserID,
		AssetID
	)
	Select
	UserID,
	AssetID
	from #Temp_UserAsset

	Drop table #Temp_UserAsset
END
