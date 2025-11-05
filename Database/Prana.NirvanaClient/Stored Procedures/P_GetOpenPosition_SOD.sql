/*
exec [P_GetOpenPosition_SOD] @thirdPartyID=86,@companyFundIDs=N'1257',
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

CREATE Procedure [dbo].[P_GetOpenPosition_SOD]                        
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
CF.FundName As AccountName, 
CTPM.FundAccntNo,             
PT.Symbol,         
Case    
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'Long'                      
	Else 'Short'                      
End as PositionIndicator,  
PT.TaxlotOpenQty As OpenPositions,
SM.BloombergSymbol,
Curr.CurrencySymbol As LocalCurrency,
SM.Multiplier As AssetMultiplier, 
SM.ISINSymbol,
SM.CUSIPSymbol,
SM.CompanyName As SecurityDescription, 
Case 
	When G.IsSwapped  = 1
	Then 'EquitySwap'
	Else A.AssetName 
End As AssetClass,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost, 
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier

Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = PT.FundID
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
INNER JOIN T_Group G ON G.GroupID = PT.GroupID
Inner join T_Exchange Ex ON Ex.ExchangeID = G.ExchangeID
Where PT.TaxlotOpenQty > 0


Select               
Temp.AccountName As AccountName,              
Temp.Symbol, 
Max(LocalCurrency) As CurrencySymbol,
Temp.FundAccntNo,           
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions, 
Sum(Temp.TotalCost) As TotalCost,
AssetClass As AssetClass,
Max(ISINSymbol) As ISINSymbol,
Max(CUSIPSymbol) As CUSIPSymbol

Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.AccountName,Temp.Symbol,Temp.FundAccntNo,Temp.AssetClass

Select * from #TempTable   
Order By AccountName,Symbol,FundAccntNo,AssetClass

Drop Table #TempOpenPositionsTable, #TempTable
Drop Table #TempTaxlotPK