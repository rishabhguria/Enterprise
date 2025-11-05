/*************************************************                
Exec P_MW_13FReport '11/10/2014','1213,1214,1233,1234'                
************************************************/                
CREATE proc [dbo].[P_MW_13FReport]                                            
(                                            
 @EndDate datetime,           
 @Funds varchar(max)                                        
)                  
AS           
          
Select * Into #Funds                                              
from dbo.Split(@Funds, ',')          
          
                                                   
SELECT                 
      
  MAX(SecurityName) as SecurityName          
 ,Symbol  as UDASecuritytype          
 ,UDAAssetClass as TitleOfClass             
 ,MAX(CusipSymbol) as CusipSymbol                
 ,ROUND(SUM(endingmarketvaluebase)/1000,0) as MarketValue                
 ,  
SUM(  
CASE   
WHEN Open_CloseTag='O'  
THEN BeginningQuantity  
ELSE 0  
END) as Quantity          
 ,'SOLE' as InvestmentDiscretion  
 ,MAX(Asset) AS Asset         
                          
          
 FROM T_MW_genericPNL           
 inner join T_Companyfunds on T_Companyfunds.FundName = T_MW_genericPNL.Fund          
 inner join #Funds on #Funds.items = T_Companyfunds.CompanyFundID          
               
 WHERE datediff(d,@EndDate,rundate)=0  AND Side<>'Short'            
 GROUP BY Symbol,UDAAssetClass                
 ORDER BY Symbol          
          
          
Drop Table #Funds