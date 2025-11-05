/* =============================================
-- Author:<Nuwan Panditaratne>
-- Create date: <07/25/2015>
-- Description:	Creates a Data Set used for Margin Calculation
-- Usage: Exec P_MarginReporting_v110
-- Production
-- Modified By: Sumit Kakra
-- Modified Date: 23 October 2019
-- Desc: https://jira.nirvanasolutions.com:8443/browse/CI-2059
-- ============================================= */
CREATE PROCEDURE [dbo].[P_MarginReporting_v110]
AS
--select a.Fund as [Account Number],[Security Name], Symbol,' ' as Cusip, ' ' as Sedol,Position as [Position QTY],[PX Last] as Price,[Net Exposure (Base)] as [Net Delta],[Market Value Base] as [Position Value], (1000*cast([Market Value Base] as float)) as [90 Day ADTV], (0.31) as [Underlying Volatility],b.[Gross Market Value] as [Gross Market Value], b.[Gross Long Market Value] as [Gross Long Market Value], b.[Gross Short Market Value] as [Gross Short Market Value] into #temp from V_pmsnapshot A,
--(select sum([Gross Market Value Base]) as [Gross Market Value], sum([Long Market Value Base]) as [Gross Long Market Value], sum(abs([Short Market Value Base])) as [Gross Short Market Value], fund from V_PMSnapshot group by fund) B where a.fund =b.fund and  (a.[Net Exposure (Base)] > 0.10*cast([Gross Market Value] as float) or cast([PX Last]as float)<5 or [Gross Long Market Value] > 0.65 * cast([Gross Market Value] as float) or [Gross Short Market Value] > 0.65 * cast([Gross Market Value] as float) or a.Position > 0.25 * (1000*cast([Market Value Base] as float)) or (cast([PX Last] as float)*10)>40)
--09/21/2014
--select a.Fund as [Account Number],[Security Name], [Underlying Symbol],replace([CUSIP],'Undefined','N/A')CUSIP,replace([SEDOL],'Undefined','N/A')SEDOL,Position as [Position QTY],case [Asset Class] when 'EquityOption' Then [Underlying Price] Else [Px Selected Feed] End as Price,[Net Exposure (Base)] as [Net Delta],[Market Value Base] as [Position Value],b.[Gross Market Value] as [Gross Market Value], b.[Gross Long Market Value] as [Gross Long Market Value], b.[Gross Short Market Value] as [Gross Short Market Value] into #temp from V_pmsnapshot A,
--(select sum([Gross Market Value Base]) as [Gross Market Value], sum([Long Market Value Base]) as [Gross Long Market Value], sum(abs([Short Market Value Base])) as [Gross Short Market Value], fund from V_PMSnapshot group by fund) B where  a.fund =b.fund and (a.fund='TEST-888-400000' or a.fund='TEST-888-400100-P' or a.fund='TEST-888-400200-P' or a.fund='TEST-888-400300-P' or a.fund='TEST-888-400400-P' or a.fund='TEST-888-400500-P' or a.fund='TEST-888-400600-P' or a.fund='TEST-888-400700-P') and (a.[Net Exposure (Base)] > 0.10*cast([Gross Market Value] as float) or cast([PX Last]as float)<5 or a.Position > 0.25 * (1000*cast([Market Value Base] as float)) or (cast([PX Last] as float)*10)>40)
--select a.Fund as [Account Number],[Security Name], [Underlying Symbol],replace([CUSIP],'Undefined','N/A')CUSIP,replace([SEDOL],'Undefined','N/A')SEDOL,Position as [Position QTY],case [Asset Class] when 'EquityOption' Then [Underlying Price] Else [Px Selected Feed] End as Price,[Net Exposure (Base)] as [Net Delta],[Market Value Base] as [Position Value],[Fund NAV] as [Account Equity], b.[Long Exposure] as [Gross Long Exposure], b.[Short Exposure] as [Gross Short Exposure],replace(SecurityType,'Undefined',0)  as [90 Day ADTV], replace(subsector,'Undefined',0) as [Underlying Volatility]  into #temp from V_pmsnapshot A,
--(select case when max([Net Exposure (Base)]) >  0  then sum(cast([Net Exposure (Base)] as float(50))) else 0 end as [Long Exposure], case when max([Net Exposure (Base)]) <  0  then sum(abs(cast([Net Exposure (Base)] as float(50)))) else 0 end as [Short Exposure], fund from V_PMSnapshot group by fund) B where  a.fund =b.fund and (a.fund='TEST-888-400000' or a.fund='TEST-888-400100-P' or a.fund='TEST-888-400200-P' or a.fund='TEST-888-400300-P' or a.fund='TEST-888-400400-P' or a.fund='TEST-888-400500-P' or a.fund='TEST-888-400600-P' or a.fund='TEST-888-400700-P') and [PX Last]<>'Multiple' and (a.[Net Exposure (Base)] > 0.10*cast([Fund NAV] as float) or cast([PX Last]as float)<5 or (B.[Long Exposure] > 0.65*(B.[Long Exposure]+ abs(B.[Short Exposure])) and [Long Exposure Base] >1000000)   or (B.[Short Exposure] > 0.65*(B.[Long Exposure]+ abs(B.[Short Exposure])) and abs([Short Exposure Base]) > 1000000) or cast(position as float(50)) > 0.25 * cast(SecurityType as float(50)) or subsector > 40.0  )
----position > 0.25* case ISNumeric ([Security Type]) when 1 then convert(float(50),SecurityType) else 0 end)--
SELECT a.account AS [Account Number]
	,[Security Name]
	,[Underlying Symbol]
	,replace([CUSIP], 'Undefined', 'N/A') CUSIP
	,replace([ISIN], 'Undefined', 'N/A') ISIN
	,replace([SEDOL], 'Undefined', 'N/A') SEDOL
	,cast(Position AS FLOAT) AS [Position QTY]
	,CASE [Asset Class]
		WHEN 'EquityOption'
			THEN [Underlying Price]
		ELSE [Px Selected Feed (local)]
		END AS Price
	,[net Exposure (Base)] AS [Exposure of Underlyer]
	,[Market Value (Base)] AS [Position Value]
	,[NAV (Touch)] AS [Account Equity]
	,b.[Long Exposure] AS [Gross Long Exposure]
	,b.[Short Exposure] AS [Gross Short Exposure]
	,replace([Security Type], 'Undefined', 0) AS [90 Day ADTV]
	,replace([sub sector], 'Undefined', 0) AS [Underlying Volatility]
	,B.[Long Market Value]
	,B.[Short Market Value]
INTO #temp
FROM V_PMSnapshot_Margin A
	,(
		SELECT SUM([Long Exposure Base]) AS [Long Exposure]
			,ABS(SUM([Short Exposure Base])) AS [Short Exposure]
			,ABS(SUM([Short Market Value Base])) AS [Short Market Value]
			,SUM([Long Market Value Base]) AS [Long Market Value]
			,Account
		FROM V_PMSnapshot_Margin
		GROUP BY Account
		) B
WHERE A.account = B.account
	AND (
		a.account NOT LIKE 'TEST%'
		AND A.Account <> 'Undefined'
		AND A.Account <> 'Unallocated'
		)
	AND A.[Underlying Price] <> 'Multiple'
	AND [Asset Class] <> 'EquityOption'
	AND (
		CASE 
			WHEN ISNUMERIC([Underlying Price]) = 1
				THEN cast([Underlying Price] AS FLOAT)
			ELSE 0
			END
		) >= 5
	AND (
		(
			A.[Net Exposure (Base)] > 0.10 * (
				CASE 
					WHEN ISNUMERIC(B.[Long Market Value]) = 1
						THEN CAST(B.[Long Market Value] AS FLOAT)
					ELSE 0
					END
				)
			OR (
				A.[Net Exposure (Base)] < - 0.10 * (
					CASE 
						WHEN ISNUMERIC(B.[Short Market Value]) = 1
							THEN CAST(B.[Short Market Value] AS FLOAT)
						ELSE 0
						END
					)
				)
			)
		OR (
			B.[Long Exposure] > 0.65 * (B.[Long Exposure] + ABS(B.[Short Exposure]))
			AND (B.[Long Exposure] + ABS(B.[Short Exposure])) > 5000000.00
			)
		OR (
			B.[Short Exposure] > 0.65 * (B.[Long Exposure] + ABS(B.[Short Exposure]))
			AND (B.[Long Exposure] + ABS(B.[Short Exposure])) > 5000000.00
			)
		OR CAST(Position AS FLOAT(50)) > 0.25 * (
			CASE 
				WHEN ISNUMERIC([Security Type]) = 1
					THEN CAST([Security Type] AS FLOAT)
				ELSE 0
				END
			)
		OR (
			CASE 
				WHEN ISNUMERIC([Sub Sector]) = 1
					THEN CAST([Sub Sector] AS FLOAT)
				ELSE 0
				END
			) > 40.0
		)

SELECT *
FROM #temp
ORDER BY [Account Number]

DROP TABLE #temp