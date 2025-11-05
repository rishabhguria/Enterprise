
/*
exec [P_GetOpenPos_Crestline_BBG] @thirdPartyID=86,@companyFundIDs=N'1257',
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

CREATE Procedure [dbo].[P_GetOpenPos_Crestline_BBG]                        
(                                 
@ThirdPartyID int,                                            
@CompanyFundIDs varchar(max),                                                                                                                                                                          
@InputDate datetime,                                                                                                                                                                      
@CompanyID int,                                                                                                                                      
@AUECIDs varchar(max),                                                                            
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
@FileFormatID int                                  
)                                  
AS

--Declare @InputDate DateTime    
--Set @InputDate = '02-03-2021' 

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund                                                                                                    
Select CompanyFundID From T_CompanyFunds
--Cast(Items as int) from dbo.Split(@companyFundIDs,',') 

SELECT PT.Taxlot_PK    
InTo #TempTaxlotPK         
FROM PM_Taxlots PT          
Inner Join @Fund Fund on Fund.FundID = PT.FundID              
Where PT.Taxlot_PK in                                       
(                                                                                                
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                   
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                      
 group by TaxlotId                                                                      
)                                                                                          
And PT.TaxLotOpenQty > 0 

SELECT BloombergSymbol
		,CUSIPSymbol
		,SEDOLSymbol
		,TickerSymbol
		,ISINSymbol
		,ReutersSymbol
		,UnderlyingSymbol
		,AssetID
		,LeadCurrency
		,VsCurrency
		,AUECID
		,CurrencyID
	INTO #TempSMTABLE
	FROM V_SecmasterData

Create Table #TempOpenUniqueSymbol 
(
BloombergSymbol Varchar(100) Default '',
CUSIPSymbol Varchar(100) Default '',
SEDOLSymbol Varchar(100) Default '',
TickerSymbol Varchar(100) Default '',
ISINSymbol Varchar(100) Default '',
NirvanaTicker Varchar(100) Default '',
TradeCurrency Varchar(100) Default ''
)

Insert Into #TempOpenUniqueSymbol

	SELECT DISTINCT 
		 SM.BloombergSymbol
		,SM.CUSIPSymbol
		,SM.SEDOLSymbol
		,SM.TickerSymbol
		,SM.ISINSymbol
		,SM.TickerSymbol As NirvanaTicker
		,Curr.CurrencySymbol As TradeCurrency
	FROM #TempSMTABLE SM
	INNER JOIN PM_Taxlots ON PM_Taxlots.Symbol = SM.TickerSymbol
	INNER JOIN #TempTaxlotPK Temp ON PM_Taxlots.Taxlot_PK = Temp.Taxlot_PK
	Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID
	WHERE (
			PM_Taxlots.TaxlotOpenQty <> 0
			OR Datediff(d, PM_Taxlots.AUECModifiedDate, @InputDate) <= 0
			)
	
	UNION
	
	-- get underlying symbol 
	SELECT DISTINCT 
		 SMUnder.BloombergSymbol
		,SMUnder.CUSIPSymbol
		,SMUnder.SEDOLSymbol
		,SMUnder.TickerSymbol
		,SMUnder.ISINSymbol
		,SMUnder.TickerSymbol As NirvanaTicker
		,Curr.CurrencySymbol As TradeCurrency
	FROM PM_Taxlots
	INNER JOIN #TempTaxlotPK Temp ON PM_Taxlots.Taxlot_PK = Temp.Taxlot_PK
	INNER JOIN #TempSMTABLE SM ON PM_Taxlots.Symbol = SM.TickerSymbol
	INNER JOIN V_SecMasterData_WithUnderlying SMUnder ON SM.UnderlyingSymbol = SMUnder.TickerSymbol
	Inner Join T_Currency Curr On Curr.CurrencyID = SMUnder.CurrencyID
	WHERE (
			PM_Taxlots.TaxlotOpenQty <> 0
			OR Datediff(d, PM_Taxlots.AUECModifiedDate, @InputDate) <= 0
			)
	
	UNION
	
	SELECT DISTINCT 
	    LeadCurrency + ' CURNCY' AS BloombergSymbol
		,'' AS CUSIPSymbol
		,'' AS SEDOLSymbol
		,LeadCurrency AS TickerSymbol
		,'' AS ISINSymbol
		,LeadCurrency + ' CURNCY' AS NirvanaTicker
		,LeadCurrency As TradeCurrency
	FROM #TempSMTABLE TempSM
	WHERE TempSM.AssetID IN ( 
			5,11 )
	
	UNION
	
	SELECT DISTINCT 
	    VsCurrency + ' CURNCY' AS BloombergSymbol
		,'' AS CUSIPSymbol
		,'' AS SEDOLSymbol
		,VsCurrency AS TickerSymbol
		,'' AS ISINSymbol
		,VsCurrency + ' CURNCY' AS NirvanaTicker
		,VsCurrency As TradeCurrency 
	FROM #TempSMTABLE TempSM
	WHERE TempSM.AssetID IN ( 
			5,11 )
	
	UNION
	
	SELECT DISTINCT 
	    CLocal.CurrencySymbol + ' CURNCY' AS BloombergSymbol
		,'' AS CUSIPSymbol
		,'' AS SEDOLSymbol
		,CLocal.CurrencySymbol AS TickerSymbol
		,'' AS ISINSymbol
		,CLocal.CurrencySymbol AS NirvanaTicker
		,CLocal.CurrencySymbol AS TradeCurrency
	FROM PM_CompanyFundCashCurrencyValue CFCC
	INNER JOIN T_Currency CLocal ON CFCC.LocalCurrencyID = CLocal.CurrencyID
	INNER JOIN @Fund Funds ON Funds.FundId = CFCC.FundID
	-- Convert BBG Symbol in Last word in camel Case
	update #TempOpenUniqueSymbol
	set BloombergSymbol =LTRIM(SUBSTRING(BloombergSymbol, 1, LEN(BloombergSymbol) - CHARINDEX(' ', REVERSE(BloombergSymbol))+1))+
UPPER(LEFT(RTRIM(SUBSTRING(BloombergSymbol, LEN(BloombergSymbol) - CHARINDEX(' ', REVERSE(BloombergSymbol))+2,LEN(BloombergSymbol))),1))+LOWER(SUBSTRING(RTRIM(SUBSTRING(BloombergSymbol, LEN(BloombergSymbol) - CHARINDEX(' ', REVERSE(BloombergSymbol))+2,LEN(BloombergSymbol))),2,LEN(RTRIM(SUBSTRING(BloombergSymbol, LEN(BloombergSymbol) - CHARINDEX(' ', REVERSE(BloombergSymbol))+2,LEN(BloombergSymbol))))))

where BloombergSymbol!=''
and (BloombergSymbol like '%equity' OR BloombergSymbol like '%index' OR BloombergSymbol like '%INDEX' OR BloombergSymbol like '%EQUITY') 
Select Distinct 
Replace(BloombergSymbol, ' GR ', ' GY ') As [Security],
'' As DVD_SH_LAST, 
'' As DVD_CRNCY,
'' As DVD_EX_DT,
'' As DVD_PAY_DT,
'' As PX_LAST,
'' As FIXED_CLOSING_PRICE_NY,
'' As GICS_SECTOR_NAME,
'' As GICS_SUB_INDUSTRY_NAME,
'' As LAST_DPS_GROSS,
'' As MID,
'' As SECURITY_TYP2,
'' As ID_CUSIP,
'' As ID_ISIN,
'' As ID_SEDOL1
From #TempOpenUniqueSymbol
Where (BloombergSymbol <> '' And BloombergSymbol IS Not Null)

Drop Table #TempSMTABLE,#TempTaxlotPK, #TempOpenUniqueSymbol