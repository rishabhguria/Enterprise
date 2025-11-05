

CREATE PROCEDURE dbo.P_GetRLUnderLying
(
		@CompanyID int,
		@AssetId int
)
AS

SELECT     T_UnderLying.UnderLyingID, T_UnderLying.UnderLyingName
FROM         T_AUEC INNER JOIN
                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID INNER JOIN
                      T_UnderLying ON T_AUEC.UnderLyingID = T_UnderLying.UnderLyingID
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_AUEC.AssetID = @AssetId)
ORDER BY T_UnderLying.UnderLyingName 


