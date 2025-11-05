/*        
--,G2 Investment Partners QP LP        
P_MW_GetWinnerLoserONPnL '05-21-2014',10,'Symbol','Winners','G2 Investment Partners LP,G2 Investment Partners QP LP',1,'MTD'        
    
*/        
CREATE Procedure P_MW_GetWinnerLoserONPnL        
(        
 @EndDate DateTime,                                                                                                                                                             
 @TopX int,                                                                                                                                                                
 @TopBy char(50),-- Symbol or Underlying Symbol                                                
 @OrderBy char(20),--Winners or Losers                                              
 @Funds varchar(max),        
 @paramNAVbyMWorPM Int,        
 @DateRangeType Varchar(200)           
)        
As        
        
--Declare @EndDate DateTime        
--Set @EndDate = '08-29-2014'        
--        
--Declare @TopX int        
--Set @TopX = 10        
--        
--Declare @TopBy char(50)        
--Set @TopBy = 'Symbol'        
--        
--Declare @OrderBy Varchar(100)        
--Set @OrderBy = 'Winners'         
--        
--         
--Declare @paramNAVbyMWorPM Bit        
--Set @paramNAVbyMWorPM = 1        
--        
--Declare @Funds Varchar(Max)        
--Set @Funds = ('G2 Investment Partners LP,G2 Investment Partners QP LP')        
--        
--Declare @DateRangeType Varchar(200)        
--Set @DateRangeType = 'DLY' --'MTD'--'QTD'        
--       
declare  @StartDate datetime,@MTDFromdate datetime,@QTDFromdate datetime,@YTDFromdate datetime           
           
Select @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)          
Select @QTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,5)          
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)          
        
Select @StartDate =         
Case        
When @DateRangeType = 'DLY'        
Then @EndDate        
When @DateRangeType = 'MTD'        
Then @MTDFromdate        
When @DateRangeType = 'QTD'        
Then @QTDFromdate      
When @DateRangeType = 'YTD'        
Then @YTDFromdate        
Else @EndDate      
End         
        
Select * Into #Funds                                                  
from dbo.Split(@Funds, ',')         
        
--Create Table #NAVFundAndDatewise          
--(          
-- Date DateTime,          
-- NAVValue Float,          
-- FundName Varchar(200)          
--)          
          
--If (@paramNAVbyMWorPM = 1)            
-- BEGIN            
--  Insert Into #NAVFundAndDatewise          
--   Select          
--   RunDate As Date,           
--   SUM(ISNULL(EndingMarketValueBase,0)) As NAVValue,          
--   Fund as FundName          
--   From T_MW_GenericPNL         
--   Where Open_CLoseTag <> 'C' And DATEDIFF(d,Rundate,@EndDate)=0 And Fund In (Select * from #Funds)          
--   Group By Fund,Rundate            
-- END            
--Else            
-- Begin          
--  Insert Into #NAVFundAndDatewise             
--   Select           
--   NAV.Date,          
--   Sum(ISNULL(NAV.NAVValue,0)) As NAVValue,          
--   CF.FundName as FundName          
--   from PM_NAVValue NAV              
--   inner JOIN T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID            
--   Where datediff(d,@EndDate,Date)=0 and CF.FundName In (Select * from #Funds)            
--   Group By FundName,Date             
-- End     
Declare @GlobalNAV Float    
If (@paramNAVbyMWorPM = 1)            
 BEGIN            
   Select @GlobalNAV = SUM(ISNULL(EndingMarketValueBase,0)) From T_MW_GenericPNL         
   Where Open_CLoseTag = 'O' And DATEDIFF(d,Rundate,@EndDate)=0 And Fund In (Select * from #Funds)          
   Group By Rundate            
 END            
Else            
 Begin          
    Select @GlobalNAV = Sum(ISNULL(NAV.NAVValue,0)) From PM_NAVValue NAV              
 Inner Join T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID            
   Where datediff(d,@EndDate,Date)=0 and CF.FundName In (Select * from #Funds)            
   Group By Date             
 End         
        
----Select * from #NAVFundAndDatewise        
Create Table #TempOutput        
(        
Fund Varchar(200),        
Symbol Varchar(200),        
UnderlyingSymbol Varchar(200),        
SecurityName Varchar(500),        
TotalPNL Float,        
BeginningQuantity Float,        
EndingMarketValueBase Float,        
PercEquityClose Float        
)        
        
--Open Positions      
Insert InTo #TempOutput        
Select         
Fund,        
Symbol,        
UnderlyingSymbol,        
SecurityName,        
0 As TotalPNL,   
CASE        
WHEN (Asset = 'CASH')                       
Then EndingMarketValueLocal  ----* SideMultiplier      
Else BeginningQuantity * SideMultiplier        
END AS BeginningQuantity,          
  
EndingMarketValueBase,        
Case        
When @GlobalNAV <> 0        
Then (EndingMarketValueBase / @GlobalNAV) * 100    
Else 0        
End As PercEquityClose        
        
From T_MW_genericPNL PNL                  
--LEFT OUTER JOIN #NAVFundAndDatewise P on PNL.Fund = P.FundName and Datediff(Day,P.Date,@EndDate)=0                        
Where Datediff(Day , Rundate , @EndDate) = 0  And Fund In (Select * from #Funds) and (Open_CloseTag ='O') -- asset <> 'CASH'        
        
-- Closed Positions      
Insert InTo #TempOutput        
Select         
Fund,        
Symbol,        
UnderlyingSymbol,        
SecurityName,        
(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) As TotalPNL,        
0 BeginningQuantity,        
0 EndingMarketValueBase,        
0 PercEquityClose        
        
From T_MW_genericPNL PNL                  
--LEFT OUTER JOIN #NAVFundAndDatewise P on PNL.Fund = P.FundName and Datediff(Day,P.Date,@EndDate)=0            
Where Datediff (Day , @Startdate , Rundate) >= 0 and Datediff(Day , Rundate , @EndDate) >= 0 and Open_CloseTag <> 'Accruals' and Fund In (Select * from #Funds)        
           
----Select * from #TempOutput        
----Order by TotalPNL         
        
If @OrderBy = 'Winners'                         
Begin                                                
 Set @OrderBy = ' DESC'                                                
End                                                
Else                                        
Begin                            
 Set @OrderBy = ' ASC'                                                
End         
        
Exec ('Select TOP(' + @TopX + ')                                                   
  MIN(Symbol) As Symbol                                                
 ,MIN(UnderlyingSymbol) As UnderlyingSymbol                                                
 ,MIN(SecurityName) As SecurityName        
 ,Sum(BeginningQuantity) As OpenPositions        
 ,SUM(PercEquityClose) As PercEquityClose         
 ,SUM(EndingMarketValueBase) As EndingMarketValueBase                                              
 ,SUM(TotalPNL) As TotalPNL                                                
from #TempOutput                                                
Group By ' + @TopBy + '                                                
Order By TotalPNL ' + @OrderBy)        
        
Drop Table #TempOutput,#Funds----,#NAVFundAndDatewise    
        
        
        