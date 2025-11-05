CREATE VIEW [dbo].[V_PMSnapshot_Margin]
AS
SELECT *
	,(
		CASE 
			WHEN [Net Exposure (Base)] NOT LIKE '%-%'
				THEN CAST([Net Exposure (Base)] AS FLOAT)
			ELSE 0
			END
		) AS [Long Exposure Base]
	,(
		CASE 
			WHEN [Net Exposure (Base)] LIKE '%-%'
				THEN CAST([Net Exposure (Base)] AS FLOAT)
			ELSE 0
			END
		) AS [Short Exposure Base]
	,(
		CASE 
			WHEN [market value (Base)] NOT LIKE '%-%'
				THEN CAST([market value (Base)] AS FLOAT)
			ELSE 0
			END
		) AS [Long Market Value Base]
	,(
		CASE 
			WHEN [market value (Base)] LIKE '%-%'
				THEN CAST([market value (Base)] AS FLOAT)
			ELSE 0
			END
		) AS [Short Market Value Base]
FROM dbo.T_PMDataDump
WHERE (CreatedON) = (SELECT Max(CreatedON) FROM T_PMDataDump)