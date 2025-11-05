


CREATE PROCEDURE [dbo].[P_GetRLAUECExchange]
(
		@CompanyID int,
		@AssetID int,
		@UnderLyingID int
)
AS

SELECT     T_AUEC.AUECID, T_AUEC.DisplayName
FROM         T_AUEC INNER JOIN
                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID 
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_AUEC.AssetID = @AssetID) AND (T_AUEC.UnderLyingID = @UnderLyingID)
ORDER BY T_AUEC.DisplayName 


