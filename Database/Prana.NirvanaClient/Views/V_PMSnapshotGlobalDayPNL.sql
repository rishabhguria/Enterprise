
CREATE VIEW [dbo].[V_PMSnapshotGlobalDayPNL]
AS
SELECT SUM(CAST([Day P&L (Base)] AS FLOAT)) AS [Day P&L Base (Global)]
FROM dbo.T_PMDataDump
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
