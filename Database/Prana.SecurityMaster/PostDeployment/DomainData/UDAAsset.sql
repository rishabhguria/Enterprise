-----------------------------------------------------------------------------------------------------------------------
--Created By: Disha Sharma
--Date: 06-10-2015
--Purpose: Script to add UDA Asset Name(which are not in T_UDAAsset ) which exist in T_Asset table to T_UDAAsset table
-----------------------------------------------------------------------------------------------------------------------

DECLARE	@AssetID INT
SELECT	@AssetID = isnull((MAX(AssetID) + 1),1) FROM T_UDAAssetClass

CREATE TABLE #TempAssetTable
(
	AssetID		INT	IDENTITY(1,1) NOT NULL,
	AssetName	VARCHAR(200)
)
DBCC CHECKIDENT (#TempAssetTable, RESEED, @AssetID)

INSERT INTO #TempAssetTable
(
	AssetName
)
SELECT	TA.AssetName
FROM	T_Asset TA
WHERE	AssetName NOT IN (SELECT	TUA.AssetName
							FROM	T_UDAAssetClass TUA)

INSERT INTO T_UDAAssetClass
(
	AssetID,
	AssetName
)
SELECT	AssetID, AssetName
FROM	#TempAssetTable

DROP TABLE #TempAssetTable