
CREATE PROCEDURE [dbo].[P_ImportBBUtilityInClientDB]
(
	@FilePath varchar(max)
)
AS
BEGIN
-- Transaction start-----------
BEGIN TRANSACTION [Tran1]
--Start try block statement ---
BEGIN TRY

--Declare @filePath Varchar(Max)
--Set @filePath = 'D:\TigerVeda_Final.CSV'

---- Create a temp table
CREATE TABLE #TempDataTable 
(
	[Security] VARCHAR(300) Not Null -- COL A
	,TODAY_DT VARCHAR(100) Null -- COL B
	,PX_LAST VARCHAR(100) Null -- COL C
	,ID_SEDOL1 VARCHAR(100) Null -- COL D
	,ID_ISIN VARCHAR(100) Null -- COL E
	,ID_CUSIP VARCHAR(100) Null -- COL F
	,VOLUME_AVG_90D VARCHAR(100) Null -- COL G
	,CNTRY_OF_INCORPORATION VARCHAR(100) Null -- COL H
	,COUNTRY_FULL_NAME Varchar(500) -- COL I: CNTRY_OF_RISK		
	,GICS_SECTOR_NAME VARCHAR(100) Null -- COL J: INDUSTRY_SECTOR		
	,CUR_MKT_CAP VARCHAR(100) Null -- COL K
	,EXPECTED_REPORT_DT VARCHAR(100) null -- COL L
	,EQY_SH_OUT VARCHAR(100) Null -- COL M Daily Out Standings shares
	,EQY_BETA VARCHAR(100) Null -- COL N
	,CHG_PCT_MTD VARCHAR(100) Null -- COL O
	,CHG_PCT_YTD VARCHAR(100) Null -- COL P
	,RT_PX_CHG_PCT_1D VARCHAR(100) Null -- COL Q
 ,CHG_PCT_1D VARCHAR(100) Null -- COL R,
 ,Sedol1_Country_ISO  VARCHAR(100) Null -- COL S     

 ,PX_YEST_CLOSE VARCHAR(100) Null -- COL T  
,PX_OFFICIAL_CLOSE VARCHAR(100) Null -- COL U  
,PX_CLOSE_1D VARCHAR(100) Null -- COL V,  
,PX_CLOSE_DT  VARCHAR(100) Null -- COL W            
          
)

---- insert bulk data in temp table
DECLARE @bulkinsert NVARCHAR(2000)
SET @bulkinsert = N'BULK INSERT #TempDataTable FROM ''' + @filePath + N''' WITH (FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
EXEC sp_executesql @bulkinsert
	
---- Delete column headers row
DELETE TOP (1)FROM #TempDataTable

---- we are getting spaces left and reght sider, so updating before saving in the database
UPDATE #TempDataTable 
SET 
	[Security] = RTRIM(LTRIM([Security])),
	[TODAY_DT] = RTRIM(LTRIM([TODAY_DT])),
	[PX_LAST] = RTRIM(LTRIM([PX_LAST])),
	[ID_SEDOL1] = RTRIM(LTRIM([ID_SEDOL1])),
	[ID_ISIN] = RTRIM(LTRIM([ID_ISIN])),
	[ID_CUSIP] = RTRIM(LTRIM([ID_CUSIP])),
	[VOLUME_AVG_90D] = RTRIM(LTRIM([VOLUME_AVG_90D])),
	[CNTRY_OF_INCORPORATION] = RTRIM(LTRIM([CNTRY_OF_INCORPORATION])),
	[COUNTRY_FULL_NAME] = RTRIM(LTRIM([COUNTRY_FULL_NAME])),
	[GICS_SECTOR_NAME] = RTRIM(LTRIM([GICS_SECTOR_NAME])),
	[CUR_MKT_CAP] = RTRIM(LTRIM([CUR_MKT_CAP])),
	[EXPECTED_REPORT_DT] = RTRIM(LTRIM([EXPECTED_REPORT_DT])),
	[EQY_SH_OUT] = RTRIM(LTRIM([EQY_SH_OUT])),
	[EQY_BETA] = RTRIM(LTRIM([EQY_BETA])),
	[CHG_PCT_MTD] = RTRIM(LTRIM([CHG_PCT_MTD])),
	[CHG_PCT_YTD] = RTRIM(LTRIM([CHG_PCT_YTD])),
	[RT_PX_CHG_PCT_1D] = RTRIM(LTRIM([RT_PX_CHG_PCT_1D])),
 [CHG_PCT_1D] = RTRIM(LTRIM([CHG_PCT_1D])),
 [Sedol1_Country_ISO] = RTRIM(LTRIM([Sedol1_Country_ISO])),

[PX_YEST_CLOSE] = RTRIM(LTRIM([PX_YEST_CLOSE])),  
 [PX_OFFICIAL_CLOSE] = RTRIM(LTRIM([PX_OFFICIAL_CLOSE])),  
 [PX_CLOSE_1D] = RTRIM(LTRIM([PX_CLOSE_1D])),  
 [PX_CLOSE_DT] = RTRIM(LTRIM([PX_CLOSE_DT]))           

----TODO: replace [SecurityMasterDB].DBO.T_SMSymbolLookUpTable with V_SecMasterData_WithUnderlying

-------------------- Daily Beta ----------------------

DELETE PM_DailyBeta
From PM_DailyBeta 
----Inner Join [SecurityMasterDB].DBO.T_SMSymbolLookUpTable SM On SM.TickerSymbol = PM_DailyBeta.Symbol
Inner Join V_SecMasterData_WithUnderlying SM On SM.TickerSymbol = PM_DailyBeta.Symbol
Inner Join #TempDataTable Temp On Temp.Security = SM.BloombergSymbol
Where DateDiff(Day,Convert(VARCHAR(15), Temp.TODAY_DT, 110),PM_DailyBeta.Date) = 0  

INSERT INTO     
PM_DailyBeta                        
(                        
	Symbol,                     
	Date,                      
	Beta                         
)                   
SELECT                         
	SM.TickerSymbol,                      
	Cast(Temp.TODAY_DT As Datetime),
	Cast(Temp.EQY_BETA As Float)       
FROM #TempDataTable Temp 
--Inner Join [SecurityMasterDB].DBO.T_SMSymbolLookUpTable SM On SM.BloombergSymbol = Temp.Security 
Inner Join V_SecMasterData_WithUnderlying SM On SM.BloombergSymbol = Temp.Security 
Where Temp.EQY_BETA <> '' And Temp.EQY_BETA Is Not Null And (Temp.TODAY_DT <> '' And Temp.TODAY_DT Is Not Null)

-------------------- Daily Beta ----------------------

-------------------- Trading Volume ----------------------

DELETE PM_DailyTradingVol
From PM_DailyTradingVol 
--Inner Join [SecurityMasterDB].DBO.T_SMSymbolLookUpTable SM On SM.TickerSymbol = PM_DailyTradingVol.Symbol
Inner Join V_SecMasterData SM On SM.TickerSymbol = PM_DailyTradingVol.Symbol
Inner Join #TempDataTable Temp On Temp.Security = SM.BloombergSymbol
Where DateDiff(Day,Convert(VARCHAR(15), Temp.TODAY_DT, 110),PM_DailyTradingVol.Date) = 0  

--Insert data into PM_DailyTradingVol
INSERT INTO     
PM_DailyTradingVol                        
(  	
	 Symbol
	,Date
	,TradingVolume                       
)                   
SELECT                         
	SM.TickerSymbol                      
	,Cast(Temp.TODAY_DT As Datetime)
	,Cast(Temp.VOLUME_AVG_90D As float)       
FROM #TempDataTable Temp 
--Inner Join [SecurityMasterDB].DBO.T_SMSymbolLookUpTable SM On SM.BloombergSymbol = Temp.Security 
Inner Join V_SecMasterData SM On SM.BloombergSymbol = Temp.Security 
Where  Temp.TODAY_DT <> '' And Temp.VOLUME_AVG_90D <> '' And (Temp.TODAY_DT <> '' And Temp.TODAY_DT Is Not Null)

-------------------- Trading Volume ----------------------

-------------------- Daily Out Standings shares ----------------------

DELETE PM_DailyOutStandings
From PM_DailyOutStandings 
--Inner Join [SecurityMasterDB].DBO.T_SMSymbolLookUpTable SM On SM.TickerSymbol = PM_DailyOutStandings.Symbol
Inner Join V_SecMasterData SM On SM.TickerSymbol = PM_DailyOutStandings.Symbol
Inner Join #TempDataTable Temp On Temp.Security = SM.BloombergSymbol
Where DateDiff(Day,Convert(VARCHAR(15), Temp.TODAY_DT, 110),PM_DailyOutStandings.Date) = 0  

---- Gennaro asked to multiply by 1000 
---- http://jira.nirvanasolutions.com:8080/browse/PRANA-10425 
--Insert into PM_DailyOutStandings
INSERT INTO     
PM_DailyOutStandings                        
( 
	 Symbol
	,Date
	,OutStandings                       
)                   
SELECT                         
	SM.TickerSymbol                      
	,Cast(Temp.TODAY_DT As Datetime)
 ,Cast(Temp.EQY_SH_OUT As float) * 1000               
FROM #TempDataTable Temp 
--Inner Join [SecurityMasterDB].DBO.T_SMSymbolLookUpTable SM On SM.BloombergSymbol = Temp.Security 
Inner Join V_SecMasterData SM On SM.BloombergSymbol = Temp.Security
Where  Temp.TODAY_DT <> '' and  Temp.EQY_SH_OUT <> '' And (Temp.TODAY_DT <> '' And Temp.TODAY_DT Is Not Null)
-------------------- Daily Out Standings shares ----------------------

----
-------------------------- PriceChange : DayPriceChange,MonthPriceChange,YearPriceChange----------------------
----CREATE TABLE #TempCurrencyAndIndexData
----(
----	BBSymbol VARCHAR(100)
----	,[Date] Datetime
----	,DayPriceChange FLOAT
----	,MonthPriceChange FLOAT
----	,YearPriceChange FLOAT
----)
----
----INSERT INTO #TempCurrencyAndIndexData
----SELECT [Security] 
----    ,TODAY_DT 
----	,ISNULL([CHG_PCT_1D], 0) 
----	,ISNULL([CHG_PCT_MTD], 0) 
----    ,ISNULL([CHG_PCT_YTD], 0) 
----	FROM #TempDataTable  where ( CHARINDEX('CURNCY',[Security] ) > 3 and CHARINDEX('CURNCY',[Security] ) < 7  ) or CHARINDEX('INDEX',[Security] ) > 3
----
----
----DELETE T_TV_pricechange
----From T_TV_pricechange 
----Inner Join #TempCurrencyAndIndexData Temp On Temp.BBSymbol =T_TV_pricechange.BBSymbol
----Where DateDiff(Day,Convert(VARCHAR(15), Temp.[date], 110),T_TV_pricechange.Date) = 0  
----
------Insert into T_TV_pricechange
----INSERT INTO     
----T_TV_pricechange                        
----( 
----	 BBSymbol
----	,Date
----	,DayPriceChange
----	,MonthPriceChange                       
----	,YearPriceChange
----)                   
----SELECT                         
----	Temp.BBSymbol,
----	Temp.Date,
----	Temp.DayPriceChange,
----	Temp.MonthPriceChange,
----	Temp.YearPriceChange
----FROM #TempCurrencyAndIndexData Temp 
----Where  Temp.Date <> '' 
--drop table #TempCurrencyAndIndexData
---------------------- PriceChange: DayPriceChange,MonthPriceChange,YearPriceChange----------------------


Drop Table #TempDataTable

--if all records saved successfully then commit 
Commit TRANSACTION [Tran1]
END TRY
BEGIN CATCH
-- roll back all record if anyone query failed
  ROLLBACK TRANSACTION [Tran1]
END CATCH  

End
