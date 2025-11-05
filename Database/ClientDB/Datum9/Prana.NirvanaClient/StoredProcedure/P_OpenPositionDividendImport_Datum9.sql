
/*
Desc: script for the Datum9 client which would intake the Bloomberg Symbols and Ex-Date from the Bloomberg prices file.
And match it with Nirvana holding for the security on the previous working date.And generate the file for Dividend Import.
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-443

EXEC [P_OpenPositionDividendImport_Datumm9] 'C:\Users\Public\Desktop\DATUM9\Datum9_Dividend_BBG\Datum9BBData.csv'
*/

CREATE Procedure [dbo].[P_OpenPositionDividendImport_Datumm9]
 (    
   @FilePath varchar(max)    
 )  
As     

--Declare @FilePath varchar(1000) 
--Set @FilePath = 'C:\Users\Public\Desktop\DATUM9\Datum9_Dividend_BBG\ConstellationBBData.csv'   
                             
Begin  
  
SET NOCOUNT on

Create TABLE [dbo].[#TempTableCSVData]
(	    
	[Security] VARCHAR(200) 	
	,DVD_SH_LAST float
	,DVD_CRNCY VARCHAR(20)
	,DVD_EX_DT VARCHAR(50)
	,DVD_PAY_DT VARCHAR(50)
	,EQY_RAW_BETA_6M VARCHAR(200)
	,DELTA VARCHAR(20)
	,PX_LAST VARCHAR(200)
	,FIXED_CLOSING_PRICE_NY VARCHAR(200)
	,COUNTRY_FULL_NAME VARCHAR(200)
	,GICS_SECTOR_NAME VARCHAR(200)
	,GICS_SUB_INDUSTRY_NAME VARCHAR(200)
	,LAST_DPS_GROSS VARCHAR(200)
	,CNTRY_OF_RISK VARCHAR(200)
	,MID VARCHAR(200)
	,SECURITY_TYP2 VARCHAR(200)
	,BS_SH_OUT VARCHAR(200)
	,EQY_SH_OUT_Actual VARCHAR(200)
	,ID_CUSIP VARCHAR(20)
	,ID_ISIN VARCHAR(20)
	,ID_SEDOL1 VARCHAR(20)
	
)


DECLARE @IndexInputQuery VARCHAR(MAX)

SET @IndexInputQuery ='
Bulk Insert [dbo].[#TempTableCSVData]
From ''' + @FilePath + ''' with (FirstRow = 2,Fieldterminator = '','', Rowterminator = ''\n'')'

exec (@IndexInputQuery)

------remove records where Security_DT Is Null Or Security_DT = ''
Delete From #TempTableCSVData
	Where ([Security] Is Null Or [Security] = '') 
	Or (DVD_EX_DT Is Null Or DVD_EX_DT = '' Or DVD_EX_DT in ('N/A' ,'#N/A N/A'))
	Or (DVD_PAY_DT Is Null Or DVD_PAY_DT = '' Or DVD_PAY_DT In ('N/A','#N/A N/A'))
	

	
-- remove extra spaces from beginning and ending
UPDATE #TempTableCSVData 
	SET [Security] = LTRIM(RTRIM([Security])),	
	DVD_SH_LAST = LTRIM(RTRIM(DVD_SH_LAST)),
	DVD_CRNCY = LTRIM(RTRIM(DVD_CRNCY)),
	DVD_EX_DT = LTRIM(RTRIM(DVD_EX_DT)),
	DVD_PAY_DT = LTRIM(RTRIM(DVD_PAY_DT)),
	EQY_RAW_BETA_6M = LTRIM(RTRIM(EQY_RAW_BETA_6M)),
	DELTA = LTRIM(RTRIM(DELTA)),
	PX_LAST = LTRIM(RTRIM(PX_LAST)),
	FIXED_CLOSING_PRICE_NY = LTRIM(RTRIM(FIXED_CLOSING_PRICE_NY)),
	COUNTRY_FULL_NAME = LTRIM(RTRIM(COUNTRY_FULL_NAME)),
	GICS_SECTOR_NAME = LTRIM(RTRIM(GICS_SECTOR_NAME)),
	GICS_SUB_INDUSTRY_NAME = LTRIM(RTRIM(GICS_SUB_INDUSTRY_NAME)),
	LAST_DPS_GROSS = LTRIM(RTRIM(LAST_DPS_GROSS)),
	CNTRY_OF_RISK = LTRIM(RTRIM(CNTRY_OF_RISK)),
	MID = LTRIM(RTRIM(MID)),
	SECURITY_TYP2 = LTRIM(RTRIM(SECURITY_TYP2)),
	BS_SH_OUT = LTRIM(RTRIM(BS_SH_OUT)),
	EQY_SH_OUT_Actual = LTRIM(RTRIM(EQY_SH_OUT_Actual)),
	ID_CUSIP = LTRIM(RTRIM(ID_CUSIP)),
	ID_ISIN = LTRIM(RTRIM(ID_ISIN)),
	ID_SEDOL1 = LTRIM(RTRIM(ID_SEDOL1))
	

  
--- Remove if Duplicate Symbol
;With SecMasterCTE(Security_DT, Ranking) AS
(
	Select [Security],
	Ranking = DENSE_RANK() Over(Partition by [Security] order by NewID() ASC)
	From #TempTableCSVData
)
Delete from SecMasterCTE
Where Ranking > 1



---- Alter temp table and add symbol and business adjusted ex date "BusinessAdjustedEXDate"
---- and update their values

Alter Table #TempTableCSVData
Add Symbol Varchar(200),
    BusinessAdjustedEXDate DateTime

Update Temp
Set Temp.Symbol = SM.TickerSymbol,
Temp.BusinessAdjustedEXDate = dbo.AdjustBusinessDays(DVD_EX_DT,-1,SM.AUECID) 
From #TempTableCSVData Temp
Inner Join V_SecMasterData SM With(NOLOCK) on SM.BloombergSymbol = Temp.[Security]

---- Remove extra records which are not in V_SecMasterData
Delete From #TempTableCSVData
Where (Symbol = '' Or Symbol IS Null)

;With TaxlotPK_CTE AS
(
	Select 
	Max(PT.Taxlot_PK) As Taxlot_PK
	From PM_Taxlots PT With(NOLOCK)
	inner join [#TempTableCSVData] Temp on Temp.Symbol = PT.Symbol
	Where DateDiff(Day,PT.AUECModifiedDate,Temp.BusinessAdjustedEXDate) >= 0		
	Group by TaxlotID
)

--- Get Fund, Symbol and Side wise open positions and keep in a temp table

Select 
 TC.FundName As AccountName ,
 PT.Symbol,
 Sum(PT.TaxlotOpenQty) as OpenQty,
 Case dbo.GetSideMultiplier(PT.OrderSideTagValue) 
	When 1 
	Then 'Dividend Income'
	Else 'Dividend Expense'
End As ActivityType,
Max(Temp.DVD_EX_DT) as ExDate,
Max(Temp.DVD_PAY_DT) as PayoutDate,
Max(Temp.DVD_CRNCY) As Currency,
Sum(PT.TaxlotOpenQty * Cast(Temp.LAST_DPS_GROSS As Float)) As DividendAmount
From PM_Taxlots PT With(NOLOCK) 
Inner Join TaxlotPK_CTE TaxlotPK On TaxlotPK.Taxlot_PK = PT.TaxLot_PK 
Inner Join T_Companyfunds TC With(NOLOCK) on TC.CompanyFundID = PT.FundID
Inner Join T_Currency CUR on CUR.CurrencyID = TC.LocalCurrency
inner join [#TempTableCSVData] Temp on Temp.Symbol = PT.Symbol
Where PT.TaxLotOpenQty > 0 
Group by PT.Symbol,PT.OrderSideTagValue,TC.FundName
Order By TC.FundName,PT.Symbol

Drop Table #TempTableCSVData

END

