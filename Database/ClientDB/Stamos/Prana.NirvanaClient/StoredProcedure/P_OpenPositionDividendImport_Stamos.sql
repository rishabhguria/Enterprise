/*
Desc: script for the Stamos client which would intake the Bloomberg Symbols and Ex-Date from the Bloomberg prices file.
And match it with Nirvana holding for the security on the previous working date.And generate the file for Dividend Import.
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-205

EXEC [P_OpenPositionDividendImport_Stamos] 'C:\Users\Administrator\Desktop\DividendImportFor Stamos\Prices.csv'
*/

create Procedure [dbo].[P_OpenPositionDividendImport_Stamos]
 (    
   @FilePath varchar(max)    
 )  
As     

--Declare @FilePath varchar(1000) 
--Set @FilePath = 'C:\Users\kuldeep\Desktop\DividendImportFor Stamos\Prices.csv'   
                             
Begin  
  
SET NOCOUNT on

Create TABLE [dbo].[#TempTableCSVData]
(
	TODAY_DT VARCHAR(20)     
	,Security_DT VARCHAR(200) 
	,ID_CUSIP VARCHAR(20)
	,ID_ISIN VARCHAR(20)
	,ID_SEDOL1 VARCHAR(20)
	,PX_MID float
	,PX_LAST float
	,GICS_SECTOR_NAME VARCHAR(200)
	,GICS_SUB_INDUSTRY_NAME VARCHAR(200)
	,GICS_INDUSTRY_NAME VARCHAR(200)
	,GICS_INDUSTRY_GROUP_NAME VARCHAR(200)
	,FUND_TYP VARCHAR(200)
	,CUR_MKT_CAP VARCHAR(200)
	,VOLUME_AVG_100D VARCHAR(200)
	,EQY_SH_OUT VARCHAR(200)
	,COUNTRY_FULL_NAME VARCHAR(200)
	,DVD_EX_DT VARCHAR(50)
	,DVD_PAY_DT VARCHAR(50)
	,DVD_RECORD_DT VARCHAR(50)
	,EQY_RAW_BETA_6M VARCHAR(100)
	,DELTA_MID VARCHAR(20)
	,DVD_SH_LAST VARCHAR(50)
)


DECLARE @IndexInputQuery VARCHAR(MAX)

SET @IndexInputQuery ='
Bulk Insert [dbo].[#TempTableCSVData]
From ''' + @FilePath + ''' with (FirstRow = 2,Fieldterminator = '','', Rowterminator = ''\n'')'

exec (@IndexInputQuery)

------remove records where Security_DT Is Null Or Security_DT = ''
Delete From #TempTableCSVData
	Where (Security_DT Is Null Or Security_DT = '') 
	Or (DVD_EX_DT Is Null Or DVD_EX_DT = '' Or DVD_EX_DT in ('N/A' ,'#N/A N/A'))
	Or (DVD_EX_DT Is Null Or DVD_EX_DT = '' Or DVD_EX_DT In ('N/A','#N/A N/A'))
	Or (DVD_SH_LAST Is Null Or DVD_SH_LAST = '' Or DVD_SH_LAST In ('N/A','#N/A N/A'))

-- remove extra spaces from beginning and ending
UPDATE #TempTableCSVData 
	SET Security_DT = LTRIM(RTRIM(Security_DT)),
	DVD_EX_DT = LTRIM(RTRIM(DVD_EX_DT)),
	DVD_PAY_DT = LTRIM(RTRIM(DVD_PAY_DT)),
	DVD_RECORD_DT = LTRIM(RTRIM(DVD_RECORD_DT)),
	DVD_SH_LAST = LTRIM(RTRIM(DVD_SH_LAST))

  
--- Remove if Duplicate Symbol
;With SecMasterCTE(Security_DT, Ranking) AS
(
	Select Security_DT,
	Ranking = DENSE_RANK() Over(Partition by Security_DT order by NewID() ASC)
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
Inner Join V_SecMasterData SM With(NOLOCK) on SM.BloombergSymbol = Temp.Security_DT

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
Max(Temp.DVD_RECORD_DT) As RecordDate,
Sum(PT.TaxlotOpenQty * Cast(Temp.DVD_SH_LAST As Float)) As DividendAmount
From PM_Taxlots PT With(NOLOCK) 
Inner Join TaxlotPK_CTE TaxlotPK On TaxlotPK.Taxlot_PK = PT.TaxLot_PK 
Inner Join T_Companyfunds TC With(NOLOCK) on TC.CompanyFundID = PT.FundID
inner join [#TempTableCSVData] Temp on Temp.Symbol = PT.Symbol
Where PT.TaxLotOpenQty > 0 
Group by PT.Symbol,PT.OrderSideTagValue,TC.FundName
Order By TC.FundName,PT.Symbol

Drop Table #TempTableCSVData

END

