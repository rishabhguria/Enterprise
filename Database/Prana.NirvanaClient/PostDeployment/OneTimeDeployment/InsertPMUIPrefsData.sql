TRUNCATE TABLE T_PMUIPrefs

SELECT T_Company.CompanyID
	,50 AS NumberOfCustomViewsAllowed
	,200 AS NumberOfVisibleColumnsAllowed
	,0 AS FetchData
INTO #temp_PMUIPrefs
FROM T_Company

INSERT INTO T_PMUIPrefs
SELECT *
FROM #temp_PMUIPrefs

DROP TABLE #temp_PMUIPrefs