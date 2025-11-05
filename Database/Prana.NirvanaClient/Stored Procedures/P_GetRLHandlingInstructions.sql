

CREATE PROCEDURE dbo.P_GetRLHandlingInstructions
(
		@AUECID int
)
AS
SELECT DISTINCT T_HandlingInstructions.HandlingInstructionsID, T_HandlingInstructions.HandlingInstructions
FROM         T_CVAUEC INNER JOIN
                      T_CVAUECHandlingInstructions ON T_CVAUEC.CVAUECID = T_CVAUECHandlingInstructions.CVAUECID INNER JOIN
                      T_HandlingInstructions ON T_CVAUECHandlingInstructions.HandlingInstructionsID = T_HandlingInstructions.HandlingInstructionsID
WHERE     (T_CVAUEC.AUECID = @AUECID)

