
CREATE VIEW [dbo].[V_PMSnapshotFundDayPNL]
AS
SELECT [Account] AS [Fund]
	,SUM(CAST([Day P&L (Base)] AS FLOAT)) AS [Day P&L Base (Fund)]
FROM dbo.T_PMDataDump AS T_PMDataDump_3
WHERE (
		CreatedOn = (
			SELECT MAX(CreatedOn) AS Expr1
			FROM dbo.T_PMDataDump AS T_PMDataDump_2
			WHERE (
					CreatedOn < (
						SELECT MAX(CreatedOn) AS Expr1
						FROM dbo.T_PMDataDump AS T_PMDataDump_1
						)
					)
			)
		)
GROUP BY [Account]
