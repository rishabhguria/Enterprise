
CREATE PROCEDURE [dbo].[P_ImportBBUtilityInSMDB]
(
  @FilePath Varchar(max)
)
AS
BEGIN
-- Transaction start-----------
BEGIN TRANSACTION [Tran1]
--Start try block statement ---
BEGIN TRY
--Declare @filePath Varchar(Max)
--Set @filePath = 'D:\TigerVeda_Final.CSV'

CREATE TABLE #TempDataTable 
	(
		[Security] VARCHAR(300) Not Null -- COL A
		,TODAY_DT VARCHAR(100) Null -- COL B
		,PX_LAST VARCHAR(100) Null -- COL C
		,ID_SEDOL1 VARCHAR(100) Null -- COL D
		,ID_ISIN VARCHAR(100) Null -- COL E
		,ID_CUSIP VARCHAR(100) Null -- COL F
		,VOLUME_AVG_90D VARCHAR(100) Null -- COL G
		,CNTRY_OF_INCORPORATION VARCHAR(100) Null -- COL H UDA Country
		,COUNTRY_FULL_NAME Varchar(500) -- COL I: CNTRY_OF_RISK		
		,GICS_SECTOR_NAME VARCHAR(100) Null -- COL J: INDUSTRY_SECTOR		
		,CUR_MKT_CAP VARCHAR(100) Null -- COL K
		,EXPECTED_REPORT_DT VARCHAR(100) null -- COL L
		,EQY_SH_OUT VARCHAR(100) Null -- COL M
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


---- Join with SM database table to update only existing symbols data
Select *
InTo #TempSMDataTable
from #TempDataTable
Inner Join [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM On SM.BloombergSymbol = #TempDataTable.Security

-------------------- Update SEDOL ----------------------

Update SM
Set SM.SedolSymbol = Temp.ID_SEDOL1
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.BloombergSymbol = Temp.Security 
Where Temp.ID_SEDOL1 Is Not Null And Temp.ID_SEDOL1 <> ''

-------------------- Update SEDOL ----------------------

-------------------- Update ISIN ----------------------
Update SM
Set SM.ISINSymbol = Temp.ID_ISIN
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.BloombergSymbol = Temp.Security 
Where Temp.ID_ISIN Is Not Null And Temp.ID_ISIN <> ''
-------------------- Update ISIN ----------------------

-------------------- Update CUSIP ----------------------
Update SM
Set SM.CUSIPSymbol = Temp.ID_CUSIP
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.BloombergSymbol = Temp.Security 
Where Temp.ID_CUSIP Is Not Null And Temp.ID_CUSIP <> ''
-------------------- Update CUSIP ----------------------

-------------------- Update UDACountry ----------------------

CREATE TABLE #Temp_UDACountry
(
UDACountryID Int IDENTITY(1,1) NOT NULL,
UDACountryName Varchar(100) Null
)

Declare @MaxUDACountryID Int
Select @MaxUDACountryID = Max(CountryID) From [$(SecurityMaster)].dbo.T_UDACountry	

---- Get UDA Country which are not available in UDA Master Table
Insert Into #Temp_UDACountry
Select Distinct
CNTRY_OF_INCORPORATION As UDACountryName
From #TempSMDataTable 
Where CNTRY_OF_INCORPORATION Not In (Select CountryName From [$(SecurityMaster)].dbo.T_UDACountry)
And CNTRY_OF_INCORPORATION Is Not Null And CNTRY_OF_INCORPORATION <> ''

---- Insert UDA Country in Master Table
Insert Into [$(SecurityMaster)].dbo.T_UDACountry 
Select 
UDACountryID + @MaxUDACountryID,
UDACountryName
From #Temp_UDACountry

---- Update UDA Country in SM table

Update SM
Set SM.UDACountryID = UDACountry.CountryID
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
--Inner Join #TempSMDataTable Temp On Temp.Security = SM.BloombergSymbol
Inner Join #TempSMDataTable Temp On Temp.UnderlyingSymbol = SM.UnderlyingSymbol
Inner Join [$(SecurityMaster)].dbo.T_UDACountry UDACountry On UDACountry.CountryName = Temp.CNTRY_OF_INCORPORATION
Where Temp.CNTRY_OF_INCORPORATION Is Not Null And Temp.CNTRY_OF_INCORPORATION <> ''

-------------------- Update UDACountry ----------------------

-------- Update UDA Sector -------------------
CREATE TABLE #Temp_UDASector
(
UDASectorID Int IDENTITY(1,1) NOT NULL,
UDASectorName Varchar(100) Null
)

Declare @MaxUDASectorID Int
Select @MaxUDASectorID = Max(SectorID) From [$(SecurityMaster)].dbo.T_UDASector	

---- Get UDA Sector which are not available in UDA Master Table
Insert Into #Temp_UDASector
Select Distinct
GICS_SECTOR_NAME As UDASectorName
From #TempSMDataTable 
Where GICS_SECTOR_NAME Not In (Select SectorName From [$(SecurityMaster)].dbo.T_UDASector)
And GICS_SECTOR_NAME Is Not Null And GICS_SECTOR_NAME <> ''

---- Insert UDA Sector in Master Table
Insert Into [$(SecurityMaster)].dbo.T_UDASector (SectorID,SectorName)
Select 
UDASectorID + @MaxUDASectorID,
UDASectorName
From #Temp_UDASector

---- Update UDA Country in SM table

Update SM
Set SM.UDASectorID = UDASector.SectorID
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
--Inner Join #TempSMDataTable Temp On Temp.Security = SM.BloombergSymbol
Inner Join #TempSMDataTable Temp On Temp.UnderlyingSymbol = SM.UnderlyingSymbol
Inner Join [$(SecurityMaster)].dbo.T_UDASector UDASector On UDASector.SectorName = Temp.GICS_SECTOR_NAME
Where Temp.GICS_SECTOR_NAME Is Not Null And Temp.GICS_SECTOR_NAME <> ''

-------- Update UDA Sector -------------------

-----Update Dynamic UDA: EXPECTED_REPORT_DT value in CustomUDA5, Market Cap value in CustomUDA6 and CountryOfRisk ----------
--- CountryOfRisk if symbol_Pk already exists
Update DUDA
Set CountryOfRisk = Temp.COUNTRY_FULL_NAME 
From #TempSMDataTable Temp 
Inner Join [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Inner Join  [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData DUDA On SM.Symbol_PK = DUDA.Symbol_PK
Where (Temp.COUNTRY_FULL_NAME Is Not Null And Temp.COUNTRY_FULL_NAME <> '')



---- Insert CountryOfRisk if symbol_Pk does not exist in Dynamic UDA table
Insert InTo [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData 
(Symbol_PK,UDAData,FundID,Analyst,CountryOfRisk,
CustomUDA1,CustomUDA2,CustomUDA3,CustomUDA4,CustomUDA5,CustomUDA6,CustomUDA7,
Issuer,LiquidTag,MarketCap,Region,RiskCurrency,UCITSEligibleTag)

Select 
SM.Symbol_PK,
'<DynamicUDAs />',
0 as fundid,
'Undefined' As Analyst,
COUNTRY_FULL_NAME As CountryOfRisk,
'Undefined' As CustomUDA1,
'Undefined' As CustomUDA2,
'Undefined' As CustomUDA3,
'Undefined' As CustomUDA4,
'Undefined' As CustomUDA5,
'Undefined' As CustomUDA6,
'Undefined' As CustomUDA7,
'Undefined' As Issuer,
'Undefined' As LiquidTag,
'Undefined' As MarketCap,
'Undefined' As Region,
'Undefined' As RiskCurrency,
'Undefined' As UCITSEligibleTag
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Where ( Temp.Symbol_PK Not In (Select Symbol_PK From [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData))
And (Temp.COUNTRY_FULL_NAME Is Not Null And Temp.COUNTRY_FULL_NAME <> '')




---- Update CustomUDA6 from CUR_MKT_CAP if symbol_Pk already exists
Update DUDA
Set CustomUDA6 = Temp.CUR_MKT_CAP
From #TempSMDataTable Temp 
Inner Join [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Inner Join  [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData DUDA On SM.Symbol_PK = DUDA.Symbol_PK
Where (Temp.CUR_MKT_CAP Is Not Null And Temp.CUR_MKT_CAP <> '')

---- Insert CustomUDA6 (from file CUR_MKT_CAP) if symbol_Pk does not exist in Dynamic UDA table
Insert InTo [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData 
(Symbol_PK,UDAData,FundID,Analyst,CountryOfRisk,
CustomUDA1,CustomUDA2,CustomUDA3,CustomUDA4,CustomUDA5,CustomUDA6,CustomUDA7,
Issuer,LiquidTag,MarketCap,Region,RiskCurrency,UCITSEligibleTag)

Select 
SM.Symbol_PK,
'<DynamicUDAs />',
0 As FundID,
'Undefined' As Analyst,
'Undefined' As CountryOfRisk,
'Undefined' As CustomUDA1,
'Undefined' As CustomUDA2,
'Undefined' As CustomUDA3,
'Undefined' As CustomUDA4,
'Undefined' As CustomUDA5,
 CUR_MKT_CAP As CustomUDA6,
'Undefined' As CustomUDA7,
'Undefined' As Issuer,
'Undefined' As LiquidTag,
'Undefined' As MarketCap,
'Undefined' As Region,
'Undefined' As RiskCurrency,
'Undefined' As UCITSEligibleTag
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Where Temp.Symbol_PK Not In (Select Symbol_PK From [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData)
And  (Temp.CUR_MKT_CAP Is Not Null And Temp.CUR_MKT_CAP <> '' )

---- Update CustomUDA5 from EXPECTED_REPORT_DT (file) if symbol_Pk already exists
Update DUDA
Set CustomUDA5 = Temp.EXPECTED_REPORT_DT
From #TempSMDataTable Temp 
Inner Join [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Inner Join  [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData DUDA On SM.Symbol_PK = DUDA.Symbol_PK
Where (Temp.EXPECTED_REPORT_DT Is Not Null And Temp.EXPECTED_REPORT_DT <> '')

---- Insert CustomUDA5 (from file EXPECTED_REPORT_DT) if symbol_Pk does not exist in Dynamic UDA table
Insert InTo [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData 
(Symbol_PK,UDAData,FundID,Analyst,CountryOfRisk,
CustomUDA1,CustomUDA2,CustomUDA3,CustomUDA4,CustomUDA5,CustomUDA6,CustomUDA7,
Issuer,LiquidTag,MarketCap,Region,RiskCurrency,UCITSEligibleTag)

Select 
SM.Symbol_PK,
'<DynamicUDAs />',
0 As FundID,
'Undefined' As Analyst,
'Undefined' As CountryOfRisk,
'Undefined' As CustomUDA1,
'Undefined' As CustomUDA2,
'Undefined' As CustomUDA3,
'Undefined' As CustomUDA4,
EXPECTED_REPORT_DT As CustomUDA5,
'Undefined' As CustomUDA6,
'Undefined' As CustomUDA7,
'Undefined' As Issuer,
'Undefined' As LiquidTag,
'Undefined' As MarketCap,
'Undefined' As Region,
'Undefined' As RiskCurrency,
'Undefined' As UCITSEligibleTag
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Where Temp.Symbol_PK Not In (Select Symbol_PK From [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData)
And (Temp.EXPECTED_REPORT_DT Is Not Null And Temp.EXPECTED_REPORT_DT <> '')

---- Update CustomUDA1 from Sedol1_Country_ISO (file) if symbol_Pk already exists

Update DUDA
Set CustomUDA1 = Temp.Sedol1_Country_ISO
From #TempSMDataTable Temp 
Inner Join [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Inner Join  [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData DUDA On SM.Symbol_PK = DUDA.Symbol_PK
Where ( Temp.Sedol1_Country_ISO Is Not Null And Temp.Sedol1_Country_ISO <> '')


---- Insert CustomUDA1 9Sedol1_Country_ISO (file)) if symbol_Pk does not exist in Dynamic UDA table
Insert InTo [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData 
(Symbol_PK,UDAData,FundID,Analyst,CountryOfRisk,
CustomUDA1,CustomUDA2,CustomUDA3,CustomUDA4,CustomUDA5,CustomUDA6,CustomUDA7,
Issuer,LiquidTag,MarketCap,Region,RiskCurrency,UCITSEligibleTag)

Select 
SM.Symbol_PK,
'<DynamicUDAs />',
0 As FundID,
'Undefined' As Analyst,
'Undefined' As CountryOfRisk,
Sedol1_Country_ISO As CustomUDA1,
'Undefined' As CustomUDA2,
'Undefined' As CustomUDA3,
'Undefined' As CustomUDA4,
'Undefined' As CustomUDA5,
'Undefined' As CustomUDA6,
'Undefined' As CustomUDA7,
'Undefined' As Issuer,
'Undefined' As LiquidTag,
'Undefined' As MarketCap,
'Undefined' As Region,
'Undefined' As RiskCurrency,
'Undefined' As UCITSEligibleTag
From [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM
Inner Join #TempSMDataTable Temp On SM.UnderlyingSymbol = Temp.UnderlyingSymbol
Where Temp.Symbol_PK Not In (Select Symbol_PK From [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData)
And (Temp.Sedol1_Country_ISO Is Not Null And Temp.Sedol1_Country_ISO <> '')


-----Update Dynamic UDA: EXPECTED_REPORT_DT value in CustomUDA5, Market Cap value in CustomUDA6 and CountryOfRisk ----------

--------------------------------Dropping Temporary tables----------------------------
DROP TABLE #TempDataTable,#TempSMDataTable,#Temp_UDACountry ,#Temp_UDASector

--if all records saved successfully then commit 
Commit TRANSACTION [Tran1]
END TRY
BEGIN CATCH
-- roll back all record if anyone query failed
  ROLLBACK TRANSACTION [Tran1]
END CATCH  

End

