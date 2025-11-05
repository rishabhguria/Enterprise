/***************************************************************                                                                
Author : Pooja Porwal                 
Creation Date: 6 Aug 2015                                                                     
Description : Get Best Execution Report data from middleware and Veda via linked server      
http://jira.nirvanasolutions.com:8080/browse/PRANA-6602                
                                  
Usage:                                                              
                  
*******************************************************************/                                                                
                  
Create Procedure [dbo].[P_MW_GetExecutionReport]                  
(                                                                
@StartDate datetime,                  
@EndDate datetime,                  
@Funds Varchar(max), 
@SearchString Varchar(5000),                  
@SearchBy Varchar(100)                  
)                     
As                  
                
                
select * into #Fund from dbo.Split(@Funds, ',')                  
Select * InTo #Symbol From dbo.split(@SearchString , ',')                  
                
                
select RunDate                
,CONVERT(VARCHAR(10),TradeDate,101) as TradeDate           
,Symbol                
,Fund                
,MasterFund                
,Quantity                
,AvgPrice                
,Side                
,SideMultiplier                
,Open_CloseTag                
,PutOrCall                
,CounterParty                
,MW.Asset                
,MW.UDASector                
,MW.UDACountry                
,MW.UDASecurityType                
,MW.UDAAssetClass                
,MW.UDASubSector                
,MW.TradeCurrency         
,MW.BloomBergSymbol        
,MW.IDCOSymbol        
,MW.ISINSymbol        
,MW.OSISymbol        
,MW.SedolSymbol        
,MW.UnderlyingSymbol        
,MW.SecurityName               
                
 into #BestExecution                
                
From T_MW_Transactions MW      
Inner join T_CompanyFunds CF ON CF.FundName =MW.Fund      
INNER JOIN #Fund F  ON F.Items=CF.CompanyFundID       
where                 
datediff(d,@StartDate,rundate) >=0                 
and                 
datediff(d,rundate,@EndDate)>=0 
AND Open_CloseTag = 'o'    
OR Open_CloseTag = 'c'   
 
                
                
Alter Table #BestExecution                
Add Date datetime                
 ,OpenPrice Float                
 ,LowPrice Float                
 ,HighPrice Float                
 ,ClosePrice Float                
 ,VWAPPrice Float              
 ,SBPVsOpenPrice Float              
 ,SBPVsLowPrice Float              
 ,SBPVsHighPrice Float              
 ,SBPVsClosePrice Float              
 ,SBPVsVwapPrice float              
 ,SVVsOpenPrice Float              
 ,SVVsLowPrice Float              
 ,SVVsHighPrice Float              
 ,SVVsClosePrice float              
 ,SVVsVwapPrice Float               
                
    
update #BestExecution                
Set          
 Date=Modified.Date                 
 ,OpenPrice = Modified.[Open]    
 ,LowPrice = Modified.[Low]    
 ,HighPrice = Modified.[High]    
 ,ClosePrice = Modified.[Close]    
 ,VWAPPrice = Modified.[Vwap]    
from #BestExecution BEM    
inner join    
(    
SELECT     
  DBar.Date                 
 ,ISnull(Dbar.[Open],0) as [OPEN]               
 ,ISnull(DBar.Low,0) as [LOW]               
 ,ISnull(DBar.High,0) as [HIGH]              
 ,ISnull(DBar.[Close],0) as [CLOSE]    
 ,ISnull(V.Vwap,0) as [VWAP]    
 ,DBar.Symbol    
                
 FROM  Historical.Historicals.dbo.DailyBars DBar     
 Inner join #BestExecution BE on BE.Symbol=DBar.Symbol and BE.TradeDate=DBar.Date            
 left outer JOIN  Historical.Historicals.dbo.VWAPS V         
 ON BE.Symbol = V.Symbol and BE.TradeDate=V.Date    
)Modified      
on BEM.Symbol = Modified.Symbol and BEM.TradeDate=Modified.Date    
    
          
update #BestExecution                
Set          
 OpenPrice = ISnull(OpenPrice,0)          
 ,LowPrice = ISnull(LowPrice,0)               
 ,HighPrice = ISnull(HighPrice,0)               
 ,ClosePrice = ISnull(ClosePrice,0)             
 ,VWAPPrice = ISnull(VWAPPrice,0)             
          
          
              
update #bestExecution              
set              
SBPVsOpenPrice = case               
     when ISnull(AvgPrice,0)=0               
     then 0               
     else 
		Case 
		When SideMultiplier=1
		THEN   ISnull((OpenPrice-AvgPrice)*10000/AvgPrice,0)  
		ELSE   ISnull(-(OpenPrice-AvgPrice)*10000/AvgPrice,0)          
		End  
	End   
          
 ,SBPVsLowPrice = case         
     when ISnull(AvgPrice,0)=0               
     then 0           
     Else 
		Case 
		When SideMultiplier=1
		THEN  ISnull((LowPrice-AvgPrice)*10000/AvgPrice,0) 
		ELSE  ISnull(-(LowPrice-AvgPrice)*10000/AvgPrice,0) 
		End  
     END  
              
 ,SBPVsHighPrice =case               
     when ISnull(AvgPrice,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN  ISnull((HighPrice-AvgPrice)*10000/AvgPrice,0)  
		ELSE  ISnull(-(HighPrice-AvgPrice)*10000/AvgPrice,0)
		END           
     End 
             
 ,SBPVsClosePrice =case               
     when ISnull(AvgPrice,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN ISnull((ClosePrice-AvgPrice)*10000/AvgPrice,0)   
		ELSE ISnull(-(ClosePrice-AvgPrice)*10000/AvgPrice,0) 
		End         
     End  
            
 ,SBPVsVwapPrice = case               
     when ISnull(AvgPrice,0)=0               
     then 0               
     Else Case 
		When SideMultiplier=1
		THEN ISnull((VwapPrice-AvgPrice)*10000/AvgPrice,0) 
		ELSE ISnull(-(VwapPrice-AvgPrice)*10000/AvgPrice,0)
		END            
     End  
            
 ,SVVsOpenPrice = case               
     when ISnull(Quantity,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN ISnull((OpenPrice-AvgPrice)*Quantity,0)  
		ELSE ISnull(-(OpenPrice-AvgPrice)*Quantity,0) 
		END           
     End   
           
 ,SVVsLowPrice = case               
     when ISnull(Quantity,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN ISnull((LowPrice-AvgPrice)*Quantity,0)
		ELSE ISnull(-(LowPrice-AvgPrice)*Quantity,0)
		END             
     End   
           
 ,SVVsHighPrice = case               
     when ISnull(Quantity,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN ISnull((HighPrice-AvgPrice)*Quantity,0)
		ELSE ISnull(-(HighPrice-AvgPrice)*Quantity,0)
		END              
     End    
          
 ,SVVsClosePrice = case               
     when ISnull(Quantity,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN ISnull((ClosePrice-AvgPrice)*Quantity,0) 
		ELSE ISnull(-(ClosePrice-AvgPrice)*Quantity,0) 
		END            
     End   
           
 ,SVVsVwapPrice =  case               
     when ISnull(Quantity,0)=0               
     then 0               
     Else 
		Case 
		When SideMultiplier=1
		THEN ISnull((VwapPrice-AvgPrice)*Quantity,0)  
		ELSE ISnull(-(VwapPrice-AvgPrice)*Quantity,0)
		END             
     End              
              
              
from #BestExecution              
                
IF (@SearchString <> '') BEGIN            
IF (@searchby = 'Symbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.Symbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'underlyingSymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.underlyingSymbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'BloombergSymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.BloombergSymbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'SedolSymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.SedolSymbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'OSISymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.OSISymbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'IDCOSymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.IDCOSymbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'ISINSymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.ISINSymbol            
ORDER BY symbol            
END ELSE IF (@searchby = 'CUSIPSymbol') BEGIN            
SELECT            
 *            
FROM #BestExecution            
INNER JOIN #Symbol            
 ON #Symbol.items = #BestExecution.CUSIPSymbol            
ORDER BY Symbol            
END            
END ELSE BEGIN            
SELECT            
 *            
FROM #BestExecution            
ORDER BY symbol            
END               
                
drop TABLE #BestExecution 