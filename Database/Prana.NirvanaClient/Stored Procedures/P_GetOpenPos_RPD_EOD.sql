

CREATE Procedure [dbo].[P_GetOpenPos_RPD_EOD]                        
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
    
--declare                                  
--@ThirdPartyID int,                                            
--@CompanyFundIDs varchar(max),                                                                                                                                                                          
--@InputDate datetime,                                                                                                                                                                      
--@CompanyID int,                                                                                                                                      
--@AUECIDs varchar(max),                                                                            
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
--@FileFormatID int                                  


--set @thirdPartyID=94
--set @companyFundIDs=N'1393,1383,1391,1390,1396,1392,1397,1394,1395'
--set @inputDate='2024-03-28 02:13:18'
--set @companyID=7
--set @auecIDs=N'1,15,11,62,73,12'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=17

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund                                                                                                    
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',') 

Create Table #Fund                                                           
(                
FundID int,
FundName Varchar(100),
LocalCurrency Int,
MasterFundName Varchar(100)                      
)  

Insert into #Fund 
Select 
CF.CompanyFundID,
CF.FundName,
CF.LocalCurrency, 
MF.MasterFundName
From T_CompanyFunds CF WITH(NOLOCK)
Inner Join @Fund F On F.FundID = CF.CompanyFundID
Inner Join T_CompanyMasterFundSubAccountAssociation MFA WITH(NOLOCK) On MFA.CompanyFundID = CF.CompanyFundID 
Inner Join T_CompanyMasterFunds MF WITH(NOLOCK) On MF.CompanyMasterFundID = MFA.CompanyMasterFundID

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
CF.MasterFundName As MasterFund,              
CF.FundName As AccountName,          
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
SM.CompanyName As SecurityDescription, 
SM.ISINSymbol,
SM.SEDOLSymbol,
SM.OSISymbol,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
SM.CUSIPSymbol,
A.AssetName

Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join #Fund CF With (NoLock) On CF.FundID = PT.FundID
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
Inner Join T_Asset A On A.AssetID = SM.AssetID
INNER JOIN T_Group G ON G.GroupID = PT.GroupID	
Where PT.TaxlotOpenQty > 0


Select    
Temp.MasterFund As MasterFundName,              
Temp.Symbol,              
Temp.PositionIndicator As PositionIndicator,   
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions, 
CONVERT(VARCHAR(10), @InputDate, 101) As TradeDate,
Max(BloombergSymbol) As BloombergSymbol, 
Max(LocalCurrency) As LocalCurrency, 
Max(Temp.AssetMultiplier) As AssetMultiplier,      
Max(SecurityDescription) As SecurityDescription, 
Max(ISINSymbol) As ISINSymbol,
Max(SEDOLSymbol) As SEDOLSymbol,
Max(CUSIPSymbol) As CUSIPSymbol,
Max(Temp.AssetName) AS Asset
Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.MasterFund,Temp.Symbol,Temp.PositionIndicator    
      
--Select * from #TempTable 
--Order By MasterFundName,Symbol,PositionIndicator  

Select 
T.MasterFundName,
Symbol,
PositionIndicator,
Round(OpenPositions,0) As OpenPositions,
Convert(varchar(10), @InputDate,101) As TradeDate, 
BloombergSymbol,
LocalCurrency,
AssetMultiplier,
SecurityDescription,
ISINSymbol,
SEDOLSymbol,
CUSIPSymbol,
Asset,
2 As CustomOrder
InTo #Temp_FinalTable
From #TempTable T


Insert into #Temp_FinalTable        
select   
 
CF.MasterFundName As MasterFundName,                             
(CurrencyLocal.CurrencySymbol+' Currnecy') As Symbol,
'' AS PositionIndicator,                                 
Cash.CashValueLocal as OpenPositions,
Convert(varchar, @InputDate,101) As TradeDate, 
'' AS BloombergSymbol,
CurrencyLocal.CurrencySymbol As LocalCurrency,
'' AS AssetMultiplier,
CurrencyLocal.CurrencySymbol As SecurityDescription, 
'' As ISINSymbol,
'' As SEDOLSymbol,
'' As CUSIPSymbol,
'Cash' As Asset,
1 As CustomOrder                           
From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                     
Inner join #Fund CF On CF.FundId = Cash.FundID                      
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                
Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID 
Where                               
DateDiff(Day, Cash.Date, @inputDate) = 0

Select *
From #Temp_FinalTable
Order By CustomOrder, MasterFundName,Symbol 

Drop Table #TempOpenPositionsTable, #TempTable,#Temp_FinalTable,#Fund 
Drop Table #TempTaxlotPK