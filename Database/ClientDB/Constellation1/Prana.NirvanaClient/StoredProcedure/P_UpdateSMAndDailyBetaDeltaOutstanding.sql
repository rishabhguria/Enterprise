CREATE Procedure [dbo].[P_UpdateSMAndDailyBetaDeltaOutstanding]
 (    
   @FilePath varchar(max),
   @InputDate DATETIME     
 )  
As  

--Declare @FilePath varchar(1000) 
--Set @FilePath = 'D:\Constellation\ConstellationBBData 20190906.csv' 
--Declare  @InputDate Datetime
--set @InputDate='2019-07-12'
                             
Begin 

   
	If (@InputDate = '' Or @InputDate Is Null)    
		Begin    
		 Set @InputDate = dbo.AdjustBusinessDays(GetDate(),-1,1)    
		End    

	SET NOCOUNT ON

	CREATE TABLE #TempDataTable             
	(   	
		[Security] VARCHAR(300)     
		,DVD_SH_LAST VARCHAR(300) 
		,DVD_CRNCY VARCHAR(300)
		,DVD_EX_DT VARCHAR(300)
		,DVD_PAY_DT VARCHAR(300)
		,EQY_RAW_BETA_6M VARCHAR(300)
		,DELTA VARCHAR(300)
		,PX_LAST VARCHAR(300)
		,FIXED_CLOSING_PRICE_NY VARCHAR(300)
		,COUNTRY_FULL_NAME VARCHAR(300)
		,GICS_SECTOR_NAME VARCHAR(300)
		,GICS_SUB_INDUSTRY_NAME VARCHAR(300)
		,LAST_DPS_GROSS VARCHAR(300)
		,CNTRY_OF_RISK VARCHAR(300)	
		,MID VARCHAR(300)
		,SECURITY_TYP2 VARCHAR(300)
		,BS_SH_OUT VARCHAR(300)
		,EQY_SH_OUT_Actual VARCHAR(300)	
	)

	DECLARE @IndexInputQuery VARCHAR(MAX)

	SET @IndexInputQuery ='
	Bulk Insert [dbo].[#TempDataTable]
	From ''' + @FilePath + ''' with (FirstRow = 2,Fieldterminator = '','', Rowterminator = ''\n'')'

	EXEC (@IndexInputQuery)

	Delete #TempDataTable   
	Where [Security] Is Null Or [Security] In ('Security')  

	UPDATE #TempDataTable                         
	SET 	                  
		[Security] = RTRIM(LTRIM([Security]))
		,DVD_SH_LAST = RTRIM(LTRIM(DVD_SH_LAST))
		,DVD_CRNCY = RTRIM(LTRIM(DVD_CRNCY))
		,DVD_EX_DT = RTRIM(LTRIM(DVD_EX_DT))
		,DVD_PAY_DT = RTRIM(LTRIM(DVD_PAY_DT))
		,EQY_RAW_BETA_6M = RTRIM(LTRIM(EQY_RAW_BETA_6M))
		,DELTA = RTRIM(LTRIM(DELTA))
		,PX_LAST = RTRIM(LTRIM(PX_LAST))
		,FIXED_CLOSING_PRICE_NY = RTRIM(LTRIM(FIXED_CLOSING_PRICE_NY))
		,COUNTRY_FULL_NAME = RTRIM(LTRIM(COUNTRY_FULL_NAME))
		,GICS_SECTOR_NAME = RTRIM(LTRIM(GICS_SECTOR_NAME))	
		,GICS_SUB_INDUSTRY_NAME = RTRIM(LTRIM(GICS_SUB_INDUSTRY_NAME))
		,LAST_DPS_GROSS = RTRIM(LTRIM(LAST_DPS_GROSS))
		,CNTRY_OF_RISK = RTRIM(LTRIM(CNTRY_OF_RISK))	
		,MID = RTRIM(LTRIM(MID))
		,SECURITY_TYP2 = RTRIM(LTRIM(SECURITY_TYP2))
		,BS_SH_OUT = RTRIM(LTRIM(BS_SH_OUT))	
		,EQY_SH_OUT_Actual = RTRIM(LTRIM(EQY_SH_OUT_Actual))	


	--- Remove if Duplicate Symbol
	;With SecMasterCTE(Security, Ranking) AS
	(
		Select Security,
		Ranking = DENSE_RANK() Over(Partition by Security order by NewID() ASC)
		From #TempDataTable
	)
	Delete from SecMasterCTE
	Where Ranking > 1

	-- Need to replace null or blank value to 0. Otherwise it throw error on when we convert varchar to Decimal
	UPDATE #TempDataTable 
	SET EQY_SH_OUT_Actual = 0 
	WHERE (EQY_SH_OUT_Actual = '' OR EQY_SH_OUT_Actual IS NULL)

	UPDATE #TempDataTable 
	SET EQY_RAW_BETA_6M = 0 
	WHERE (EQY_RAW_BETA_6M = '' OR EQY_RAW_BETA_6M IS NULL)

	UPDATE #TempDataTable 
	SET DELTA = 0 
	WHERE (DELTA = '' OR DELTA IS NULL)

	-- Create a final table for update values	
	Create Table #TempFinalDataTable
	(
		TickerSymbol varchar(255),
		BBGSymbol varchar(255),
		Delta varchar(100),
		Beta varchar(100),
		Outstanding varchar(100)
	)

	Insert into #TempFinalDataTable
	Select SM.TickerSymbol, SM.BloombergSymbol,Temp.DELTA,Temp.EQY_RAW_BETA_6M,Temp.EQY_SH_OUT_Actual
	From #TempDataTable Temp  
	Inner Join V_SecMasterData_WithUnderlying SM With(NOLOCK) On SM.BloombergSymbol = Temp.Security

	Delete From #TempFinalDataTable
	Where TickerSymbol Is Null Or TickerSymbol = ''
----------------------------------------------------------------------------------------------------------------------
	--Select * from #TempFinalDataTable
	--Where BBGSymbol = 'SPT LN EQUITY'

	--------------- Delete and inerts in Daily OutStandings for the given date ----------------------------------

	DELETE OutStanding 
	FROM PM_DailyOutStandings OutStanding
	Inner Join #TempFinalDataTable Temp On Temp.TickerSymbol = OutStanding.Symbol
	WHERE Datediff(DAY, OutStanding.Date, @InputDate) = 0
 
--	--------INSERT INTO [PM_DailyOutStandings]---------

	INSERT INTO PM_DailyOutStandings
	SELECT 
	@InputDate,
	Temp.TickerSymbol,
	Cast(Temp.Outstanding As Float)     
	From #TempFinalDataTable Temp

--	--------------------------------Update Out Standing In PI-----------------------------------------------
	--UPDATE PIN SET UserSharesOutstanding = (Cast(Temp.EQY_SH_OUT_Actual as float) / 1000), UserSharesOutstandingUsed =1 FROM [T_UserOptionModelInput] PIN
	--INNER JOIN #TempDataTable Temp ON PIN.Symbol = Temp.Symbol
	--WHERE Temp.EQY_SH_OUT_Actual <> '' AND Temp.EQY_SH_OUT_Actual IS NOT NULL 
--	------------------------------------------------------------------------------------------------

--	---------------Delete and Insert Daily Beta for a date----------------------------------------------
	DELETE Beta 
	FROM PM_DailyBeta Beta 
	Inner Join #TempFinalDataTable Temp On Temp.TickerSymbol = Beta.Symbol
	WHERE Datediff(DAY, Beta.Date, @InputDate) = 0

	INSERT INTO PM_DailyBeta
	SELECT
	@InputDate, 
	Temp.TickerSymbol, 
	Cast (Temp.Beta As Float)
	From #TempFinalDataTable Temp

--	---------------------------------------------------------------------------------------

--	---------------Delete and Insert Daily Delta----------------------------------------------
	DELETE Delta 
	FROM PM_DailyDelta Delta
	Inner Join #TempFinalDataTable Temp On Temp.TickerSymbol = Delta.Symbol
	WHERE Datediff(d, Delta.Date, @InputDate)=0

	INSERT INTO PM_DailyDelta
	SELECT 
	@InputDate,
	Temp.TickerSymbol, 
	Cast (Temp.DELTA As Float)
	From #TempFinalDataTable Temp
	---------------------------------------------------------------------------------------
--------------- Update UDA Sector ----------------------------------------------
     
CREATE TABLE #Temp_UDASector      
(      
UDASectorID Int IDENTITY(1,1) NOT NULL,      
UDASectorName Varchar(100) Null      
)      
      
Declare @MaxUDASectorID Int      
Select @MaxUDASectorID = Max(SectorID) From [ConstellationSMV1.7.1].DBO.T_UDASector       
      
---- Get UDA Sector which are not available in UDA Master Table      
Insert Into #Temp_UDASector      
Select Distinct      
GICS_SECTOR_NAME As UDASectorName      
From #TempDataTable       
Where GICS_SECTOR_NAME Not In (Select SectorName From [ConstellationSMV1.7.1].DBO.T_UDASector)      
And GICS_SECTOR_NAME Is Not Null And GICS_SECTOR_NAME <> ''      
      
---- Insert UDA Sector in Master Table      
Insert Into [ConstellationSMV1.7.1].DBO.T_UDASector (SectorID,SectorName)      
Select       
UDASectorID + @MaxUDASectorID,      
UDASectorName      
From #Temp_UDASector      
      
Update SM      
Set SM.UDASectorID = UDASector.SectorID      
From [ConstellationSMV1.7.1].DBO.T_SMSymbolLookUpTable SM      
Inner Join #TempDataTable Temp On Temp.Security = SM.BloombergSymbol      
Inner Join [ConstellationSMV1.7.1].DBO.T_UDASector UDASector On UDASector.SectorName = Temp.GICS_SECTOR_NAME   
Where Temp.GICS_SECTOR_NAME Is Not Null And Temp.GICS_SECTOR_NAME <> ''

-------------------------------------------------------------------------------------- 
---------------------------Update UDA Sub Sector-------------------------------------   
CREATE TABLE #Temp_UDASubSector      
(      
UDASubSectorID Int IDENTITY(1,1) NOT NULL,      
UDASubSectorName Varchar(100) Null      
)      
      
Declare @MaxUDASubSectorID Int      
Select @MaxUDASubSectorID = Max(SubSectorID) From [ConstellationSMV1.7.1].DBO.T_UDASubSector       
      
---- Get UDA Sub Sector which are not available in UDA Master Table      
Insert Into #Temp_UDASubSector      
Select Distinct      
GICS_SUB_INDUSTRY_NAME As UDASubSectorName      
From #TempDataTable       
Where GICS_SUB_INDUSTRY_NAME Not In (Select SubSectorName From [ConstellationSMV1.7.1].DBO.T_UDASubSector)      
And GICS_SUB_INDUSTRY_NAME Is Not Null And GICS_SUB_INDUSTRY_NAME <> ''      
      
---- Insert UDA Sub Sector in Master Table      
Insert Into [ConstellationSMV1.7.1].DBO.T_UDASubSector (SubSectorID,SubSectorName)      
Select       
UDASubSectorID + @MaxUDASubSectorID,      
UDASubSectorName      
From #Temp_UDASubSector      
      
Update SM      
Set SM.UDASubSectorID = UDASubSector.SubSectorID      
From [ConstellationSMV1.7.1].DBO.T_SMSymbolLookUpTable SM      
Inner Join #TempDataTable Temp On Temp.Security = SM.BloombergSymbol      
Inner Join [ConstellationSMV1.7.1].DBO.T_UDASubSector UDASubSector On UDASubSector.SubSectorName = Temp.GICS_SUB_INDUSTRY_NAME   
Where Temp.GICS_SUB_INDUSTRY_NAME Is Not Null And Temp.GICS_SUB_INDUSTRY_NAME <> ''

---------------------------------------------------------------------------------------
-------- Update UDA Country -------------------    
CREATE TABLE #Temp_UDACountry     
(      
UDACountryID Int IDENTITY(1,1) NOT NULL,      
UDACountryName Varchar(100) Null      
)      
      
Declare @MaxUDACountryID Int      
Select @MaxUDACountryID = Max(CountryID) From [ConstellationSMV1.7.1].DBO.T_UDACountry       

---- Get UDA Country which are not available in UDA Master Table      
Insert Into #Temp_UDACountry      
Select Distinct      
COUNTRY_FULL_NAME  As UDACountryName      
From #TempDataTable     
Where COUNTRY_FULL_NAME Not In (Select CountryName From [ConstellationSMV1.7.1].DBO.T_UDACountry)      
And COUNTRY_FULL_NAME Is Not Null And COUNTRY_FULL_NAME <> ''  
        
------ Insert UDA Country in Master Table      
Insert Into [ConstellationSMV1.7.1].DBO.T_UDACountry (CountryID,CountryName)      
Select       
UDACountryID + @MaxUDACountryID,      
UDACountryName      
From #Temp_UDACountry    

Update SM      
Set SM.UDACountryID = UDACountry.CountryID      
From [ConstellationSMV1.7.1].DBO.T_SMSymbolLookUpTable SM      
Inner Join #TempDataTable Temp On Temp.Security = SM.BloombergSymbol      
Inner Join [ConstellationSMV1.7.1].DBO.T_UDACountry UDACountry On UDACountry.CountryName = Temp.COUNTRY_FULL_NAME   
Where Temp.COUNTRY_FULL_NAME Is Not Null And Temp.COUNTRY_FULL_NAME <> '' 

Drop Table #TempDataTable,#TempFinalDataTable,#Temp_UDACountry,#Temp_UDASubSector,#Temp_UDASector                       
END 