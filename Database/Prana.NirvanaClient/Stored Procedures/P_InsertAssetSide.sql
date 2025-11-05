CREATE PROCEDURE [dbo].[P_InsertAssetSide] (
	@assetID INT
	,@SideID INT
	)
AS
INSERT INTO T_AssetSide (
	AssetID
	,SideID
	)
VALUES (
	@assetID
	,@SideID
	)