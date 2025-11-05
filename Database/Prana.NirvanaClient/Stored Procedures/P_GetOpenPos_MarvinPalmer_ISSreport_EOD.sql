
/*
exec [P_GetOpenPos_MarvinPalmer_IISreport_EOD] @thirdPartyID=86,@companyFundIDs=N'1257',
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

Create Procedure [dbo].[P_GetOpenPos_MarvinPalmer_ISSreport_EOD]                        
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
--Set @InputDate = '07-22-2017'

--Declare @CompanyFundIDs varchar(max) 
--Set @companyFundIDs = '1257'   

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund                                                                                                    
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',') 



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
    
Select
Case 
	When G.IsSwapped  = 1
	Then 'EquitySwap'
	Else A.AssetName 
End As AssetClass, 
Case                      
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'Long'                      
	Else 'Short'                      
End As LongShort,
PT.Symbol,
PT.TaxlotOpenQty As Quantity,
SM.Multiplier,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
CF.FundName,
SM.SEDOLSymbol AS SEDOL,
SM.ISINSymbol AS ISIN,
SM.CUSIPSymbol AS CUSIP,
SM.CompanyName AS SecurityDescription,
SM.CountryName AS CountryName,
CP.ShortName AS CounterParty,
SM.AssetName AS UDAAssetClass

Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
INNER JOIN T_Currency AS TC ON TC.CurrencyID = PT.SettlCurrency
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
Inner Join T_Group G ON G.GroupID = PT.GroupID
INNER JOIN T_CounterParty CP With (NoLock) ON CP.CounterPartyID = G.CounterPartyID 
Where PT.TaxlotOpenQty > 0


Select               
Temp.AssetClass,    
Temp.LongShort AS PositionIndicator,    
Temp.Symbol,
Sum(Temp.Quantity * Temp.SideMultiplier) As OpenPositions,
Max(Temp.Multiplier) As Multiplier,
Temp.FundName As AccountName,
Max(Temp.SEDOL) AS SEDOL,
Max(Temp.ISIN) AS ISIN,
Max(Temp.CUSIP) AS CUSIP,
Max(Temp.SecurityDescription) AS SecurityDescription,
Max(Temp.CountryName) AS CountryName,
Max(Temp.CounterParty) AS CounterParty,
Max(Temp.UDAAssetClass) AS UDAAssetClass
Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.FundName,Temp.Symbol, Temp.LongShort,Temp.AssetClass
      
Select * from #TempTable 
Order By AccountName,Symbol

Drop Table #TempOpenPositionsTable, #TempTable
Drop Table #TempTaxlotPK
