/*
--------------------------------------------------------------------------------------
 Delete Duplicated data from T_UserTTAssetPreferences for every user			
--------------------------------------------------------------------------------------
*/

;WITH CTE AS(
   SELECT [AssetID], [UserID],
       RN = ROW_NUMBER()OVER(PARTITION BY UserID,AssetID ORDER BY UserID)
   FROM dbo.T_UserTTAssetPreferences
)
DELETE FROM CTE WHERE RN > 1;
