Create PROCEDURE [dbo].[P_SaveAssetPermissionForUser]

(

		@companyUserID int		

)

AS
	IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_UserTTAssetPreferences'
			AND COLUMN_NAME = 'UserAssetWisePrefID'
		)
BEGIN

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
	@companyUserID as UserID,
	T_Asset.AssetID as AssetID
	from  T_Asset

	Insert Into T_UserTTAssetPreferences
	(
	     UserID,
		AssetID
	)
	Select
	UserID,
	AssetID
	FROM #Temp_UserAsset TUA
	WHERE NOT EXISTS (
			SELECT 1
			FROM T_UserTTAssetPreferences UA
			WHERE TUA.AssetID = UA.AssetID
				AND TUA.UserID = UA.UserID
			)

	Drop table #Temp_UserAsset
END
RETURN 0