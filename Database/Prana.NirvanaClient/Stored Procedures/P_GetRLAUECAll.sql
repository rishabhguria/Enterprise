

CREATE PROCEDURE dbo.P_GetRLAUECAll
(
		@CompanyID int
)
AS
SELECT DISTINCT AUECID
FROM         T_CompanyAUEC
WHERE     (CompanyID = @CompanyID)


