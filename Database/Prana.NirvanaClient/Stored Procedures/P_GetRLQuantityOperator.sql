CREATE PROCEDURE dbo.P_GetRLQuantityOperator
(
	@Dummy int
)
AS
SELECT DISTINCT OperatorID, Name
FROM         T_Operator
ORDER BY Name
