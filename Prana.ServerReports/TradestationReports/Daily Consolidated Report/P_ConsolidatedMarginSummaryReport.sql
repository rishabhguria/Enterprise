/* 

Author: Sandeep Singh
Date: 06-APRIL - 2015
DESC: Customized report for Trade Station to show Long and Short Market Value and NAV. 
It also show Maintenance Excess which we pick from Margin Balance report fundwise and Rebate Rate which we pick from Rebate Report          
P_ConsolidatedMarginSummaryReport '03-30-2015','1239',1          
*/          
          
ALTER Procedure P_ConsolidatedMarginSummaryReport          
(          
@EndDate DateTime,          
@Funds Varchar(5000),          
@paramNAVbyMWorPM Int    
)          
As           
          
--Declare @EndDate DateTime          
--Set @EndDate = '03-31-2015'          
--          
--Declare @Funds Varchar(5000)          
--Set @Funds = '1239'          
--          
--Declare @paramNAVbyMWorPM Int          
--Set @paramNAVbyMWorPM = 1     
    
Select * Into #Funds                                          
from dbo.Split(@Funds, ',')          
          
Create Table #FundwiseNAV          
(          
Fund Varchar(200),          
NAV Float          
)            
If (@paramNAVbyMWorPM = 1)                      
	 BEGIN    
		Insert Into #FundwiseNAV                  
		Select   
			Fund,   
			SUM(ISNULL(EndingMarketValueBase,0)) As FundNAV     
			From T_MW_GenericPNL                   
			Where Open_CLoseTag = 'O' And DATEDIFF(d,Rundate,@EndDate)=0     
			And Fund In           
			(Select F.FundName from #Funds          
			Inner Join T_CompanyFunds F on F.CompanyFundID = #Funds.Items)                     
		Group By Fund                     
	 END                      
Else                      
          
Begin           
	Insert Into #FundwiseNAV                   
	Select CF.FundName, Sum(ISNULL(NAV.NAVValue,0)) As FundNAV From PM_NAVValue NAV                        
	Inner Join T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID                      
	Where datediff(d,@EndDate,Date)=0 and CF.CompanyFundID In (Select * from #Funds)                      
	Group By Date,CF.FundName                       
End               
                       
SELECT                           
                          
TradeDate,                         
Symbol,                        
CUSIPSymbol ,                         
OSISymbol,                         
UnderlyingSymbol ,                        
PNL.Fund,                         
Asset,                          
TradeCurrency,                         
Side,            
SecurityName,                         
MasterFund,                        
UnitCostLocal,                         
EndingFXRate,                         
EndingPriceLocal,                         
CASE              
 WHEN (Asset = 'CASH')                             
 Then EndingMarketValueBase              
 Else BeginningQuantity              
END AS BeginningQuantity,                             
Multiplier,                
CASE              
 WHEN (asset = 'CASH')                             
 Then 1               
 Else SideMultiplier              
END AS SideMultiplier,                              
                         
Case          
 When Side = 'Long'          
 Then EndingMarketValueBase          
 Else 0           
End As LongMarketValue,          
Case          
 When Side = 'Short'          
 Then EndingMarketValueBase          
 Else 0           
End As ShortMarketValue,        
        
EndingMarketValueBase                       
                 
Into #TempPNL                 
                        
FROM T_MW_GenericPNL  PNL            
inner join T_CompanyFunds F on F.FundName = PNL.Fund           
Where Open_CloseTag ='O' And Asset <> 'Cash'            
And DateDiff(d,Rundate,@EndDate) = 0 And F.CompanyFundID In (Select * from #Funds)            
        
----Get MAINTENANCE_EXCESS in a temp table fundwise        
SELECT           
MAINTENANCE_EXCESS,        
CF.FundName        
Into #TempMaintence_Excess          
        
FROM T_MARGIN_BALANCE          
inner join T_RebateReportAccountMapping CFM On CFM.ClientAccountNumber=T_MARGIN_BALANCE.ACCOUNT_ID           
Inner Join T_CompanyFunds CF On CF.CompanyFundID = CFM.PranaFundID            
Where DateDiff(Day,CALCULATION_DATE,@EndDate)=0 And MAINTENANCE_EXCESS <> '*'           
Order by CF.FundName        
      
      
Select       
Fund,     
MasterFund,      
Sum(LongMarketValue) As LongMarketValue,      
Sum(ShortMarketValue) As ShortMarketValue      
InTo #TempFundWiseMarketValueAndNAV      
From #TempPNL      
Group By MasterFund,Fund      
      
Alter Table #TempFundWiseMarketValueAndNAV          
Add MAINTENANCE_EXCESS Float          
          
Update #TempFundWiseMarketValueAndNAV          
Set MAINTENANCE_EXCESS = 0          
          
Update #TempFundWiseMarketValueAndNAV          
Set MAINTENANCE_EXCESS = IsNull(ME.MAINTENANCE_EXCESS,0)          
From #TempFundWiseMarketValueAndNAV          
Inner Join #TempMaintence_Excess ME On ME.FundName = #TempFundWiseMarketValueAndNAV.Fund          
          
--Select Sum(EndingMarketValueBase) From #TempPNL          
--Where Fund = 'Market-Neutral' --And asset <> 'CASH' --In ('Equity','EquityOption')          
          
----Select * from #FundwiseNAV #FundwiseNAV          
Select       
MVAndNAV.Fund,      
MVAndNAV.MasterFund,      
MVAndNAV.LongMarketValue,      
MVAndNAV.ShortMarketValue,      
MVAndNAV.MAINTENANCE_EXCESS,      
#FundwiseNAV.NAV As FundwiseNAV       
From #TempFundWiseMarketValueAndNAV MVAndNAV      
Inner Join #FundwiseNAV On #FundwiseNAV.Fund = MVAndNAV.Fund            
Order By MVAndNAV.Fund          
          
Drop Table #TempPNL,#Funds,#FundwiseNAV,#TempMaintence_Excess,#TempFundWiseMarketValueAndNAV 