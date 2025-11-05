-- =============================================
-- Description:	Creates a Data Set used for Margin Calculation
/* 
Exec [P_WNOMarginReporting]
Exec [P_WNOMarginReporting]
*/
-- Production
-- =============================================
Create Procedure [dbo].[P_WNOMarginReporting] AS
	
	Select Distinct [Underlying Symbol]
	Into #OptionsUnderlying_CTE
	FROM V_PMSnapshot_Margin
	WHERE [Asset Class] = 'EquityOption'
	
	Select Distinct 
	[Symbol] 
	Into #EquitySymbol_CTE
	FROM V_PMSnapshot_Margin
	WHERE [Asset Class] = 'Equity'


	SELECT [Underlying Symbol]
	InTo #NakedOptions_CTE 
	FROM #OptionsUnderlying_CTE 
	LEFT OUTER JOIN #EquitySymbol_CTE ON #OptionsUnderlying_CTE.[Underlying Symbol] = #EquitySymbol_CTE.Symbol
	WHERE #EquitySymbol_CTE.Symbol is NULL

	SELECT
	A.Account AS [Account Number],
	[Security Name], 
	[Underlying Symbol],
	REPLACE([CUSIP],'Undefined','N/A')CUSIP,
	REPLACE([ISIN],'Undefined','N/A')ISIN,
	REPLACE([SEDOL],'Undefined','N/A')SEDOL,
	[Asset Class],
	CAST(Position AS FLOAT) AS [Position QTY],
	CASE [Asset Class] 
		WHEN 'EquityOption' THEN [Underlying Price] 
		ELSE [Px Selected Feed (local)] 
	END AS Price,
	[Net Exposure (Base)] as [Exposure of Underlyer],
	[Market Value (Base)] as [Position Value],
	[NAV (Touch)] as [Account Equity], 
	B.[Long Exposure] as [Gross Long Exposure], 
	B.[Short Exposure] as [Gross Short Exposure],
	REPLACE([Security Type],'Undefined',0)  as [90 Day ADTV], 
	REPLACE([sub sector],'Undefined',0) as [Underlying Volatility], 
	B.[Long Market Value], 
	B.[Short Market Value] 
	INTO #Temp 
	FROM V_PMSnapshot_Margin A,
			(SELECT 
			 SUM([Long Exposure Base]) as [Long Exposure], 
			 ABS(SUM([Short Exposure Base])) as [Short Exposure],
			 ABS(SUM([Short Market Value Base])) as [Short Market Value],
			 SUM([Long Market Value Base]) as [Long Market Value],
			 Account 
			 FROM V_PMSnapshot_Margin GROUP BY Account
			) B 
	WHERE  A.account = B.account 
		AND (a.account NOT LIKE 'TEST%' AND A.Account <> 'Undefined' AND A.Account <> 'Unallocated') 
		AND A.[Underlying Price]<>'Multiple' 
		AND (
				[Asset Class]<>'EquityOption' 
				OR A.[Underlying Symbol] in (SELECT [Underlying Symbol] FROM #NakedOptions_CTE)
			) 
		AND (CASE WHEN ISNUMERIC([Underlying Price])=1 
			  THEN cast([Underlying Price] AS FLOAT) 
			  ELSE 0 
			 End)>=5
		AND (
			 (A.[Net Exposure (Base)] > 0.10* (CASE WHEN ISNUMERIC(B.[Long Market Value])=1 
														THEN CAST(B.[Long Market Value] AS FLOAT) 
														ELSE 0 
												   END)
			  OR (A.[Net Exposure (Base)] < -0.10* (CASE WHEN ISNUMERIC(B.[Short Market Value])=1 
															 THEN CAST(B.[Short Market Value] AS FLOAT) 
															 ELSE 0 
														  END)
				 )
			  )
			 OR (B.[Long Exposure] > 0.65*(B.[Long Exposure] + ABS(B.[Short Exposure])) AND (B.[Long Exposure] + ABS(B.[Short Exposure])) >5000000.00)
			 OR (B.[Short Exposure] > 0.65*(B.[Long Exposure] + ABS(B.[Short Exposure])) AND (B.[Long Exposure]+ ABS(B.[Short Exposure])) > 5000000.00) 
			 OR CAST(Position AS FLOAT(50)) > 0.25 * (CASE WHEN ISNUMERIC([Security Type])=1 
														   THEN CAST([Security Type] AS FLOAT) 
														   ELSE 0 
													  END)
				
			 OR (CASE WHEN ISNUMERIC([Sub Sector])=1 
					 THEN CAST([Sub Sector] AS FLOAT) 
					 ELSE 0 
				END)>40.0
			)

	SELECT * FROM #Temp
	--Where [Underlying Symbol] LIke '%SPX%'  
	ORDER BY [Account Number]--, [Underlying Symbol]
				 

	DROP TABLE #Temp,#NakedOptions_CTE,#EquitySymbol_CTE,#OptionsUnderlying_CTE
