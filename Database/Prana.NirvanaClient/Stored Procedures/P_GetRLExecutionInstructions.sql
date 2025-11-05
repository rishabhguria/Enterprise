

CREATE PROCEDURE dbo.P_GetRLExecutionInstructions
(
		@AUECID int
)
AS
SELECT DISTINCT T_ExecutionInstructions.ExecutionInstructionsID, T_ExecutionInstructions.ExecutionInstructions
FROM         T_ExecutionInstructions INNER JOIN
                      T_AUECExecutionInstruction ON T_ExecutionInstructions.ExecutionInstructionsID = T_AUECExecutionInstruction.ExecutionInstructionID
WHERE     (T_AUECExecutionInstruction.AUECID = @AUECID)
ORDER BY T_ExecutionInstructions.ExecutionInstructions

