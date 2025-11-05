CREATE PROCEDURE [dbo].[PMGetAllSymbolsVolatility]       
(                                          
  @FromDate DateTime,                                    
  @ToDate DateTime,                                    
  @Type int, -- 0 for Same Date,1 for Week , 2 for Month                                          
  @ErrorMessage varchar(500) output,                                          
  @ErrorNumber int output                                          
 )                                          
AS                                          
                                    
DECLARE @FirstDateofMonth varchar(50)                                    
DECLARE @LastDateofMonth varchar(50)                                    
                                    
If(@Type=0) -- Daily view                          
Begin                                    
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                     
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                     
end                                    
Else If(@Type=1) -- Weekly view                          
Begin                                    
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                     
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@ToDate,101)                                     
End                                    
Else If(@Type=2) -- Monthly view                          
Begin                                   
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)                          
 Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                                               
End                                    
                                          
SET @ErrorMessage = 'Success'                                          
SET @ErrorNumber = 0                                          
                                          
BEGIN TRY      
  
CREATE TABLE #PM_Taxlots  
(  
GroupID varchar(50),  
Symbol varchar(100)  
)  
  
Insert into #PM_Taxlots (GroupID,Symbol)  
Select GroupID,Symbol  
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                             
 (                                                                                                   
    Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,@LastDateofMonth) >=0                                                                                                                                      
    group by taxlotid                                                   
  )   
  
CREATE TABLE #T_AUEC  
(  
AuecID int,  
ExchangeIdentifier varchar(100)  
)  
  
Insert into #T_AUEC( AuecID, ExchangeIdentifier)  
Select AuecID,ExchangeIdentifier  
from T_AUEC  
    
CREATE TABLE #SecMasterData    
(    
TickerSymbol varchar(100),    
UnderlyingSymbol varchar(100),    
AUECID int,    
AssetID int,  
BloombergSymbol  varchar(100)  
)         
    
Insert into #SecMasterData    
Select     
TickerSymbol,    
UnderlyingSymbol,    
AUECID,    
AssetID,  
BloombergSymbol    
from V_SecMasterData     
                      
 CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesALL                                                                   
   (                                                                                      
    Symbol varchar(200),           
    DayVolatilityID int,                                                                
    Date_Associated datetime ,                                                              
    AUECID int,                                                                      
    AUECIdentifier varchar(200),            
 AssetID int,    
 BloombergSymbol nvarchar(200)                              
   )                                
                                                                               
 INSERT INTO                                                           
  #TempPositionsAndAllocatedTradesALL                                                                                    
   (                                                      
    Symbol,                                                                                    
    Date_Associated,                                                                      
    AUECID,                                                                      
    AUECIdentifier,    
    AssetID,    
 BloombergSymbol                                                                                                   
   )                                                                 
 SELECT DISTINCT                                                                                     
  T_Group.Symbol AS Symbol,                                
  MAX(T_Group.AUECLocalDate) AS Date_Associated,                                
  MAX(T_Group.AUECID) as AUECID,                                
  MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,    
  MAX(T_Group.AssetID) as AssetID,    
  MAX(SM.BloombergSymbol) as BloombergSymbol  
                                          
 FROM                                                                       
  [dbo].T_Group LEFT JOIN #T_AUEC AUEC ON T_Group.AUECID = AUEC.AUECID     
  Left Join #SecMasterData SM ON  T_Group.Symbol = SM.TickerSymbol                                                             
  WHERE  (DateDiff(day,T_Group.AUECLocaldate,@LastDateofMonth) >= 0 And T_Group.StateID=1)     
  or (DateDiff(day, T_Group.AUECLocaldate, @FirstDateofMonth) <= 0 AND     
  datediff(day,T_Group.AUECLocaldate , @LastDateofMonth) >= 0)            
  GROUP BY Symbol           
                                                                                                                                                                                                                                                              
                                                                                                                                                                                                                     
-- collect open trade symbols                                            
INSERT INTO                                                           
  #TempPositionsAndAllocatedTradesALL                                             
  (                                                      
    Symbol,                                                                                    
    Date_Associated,                                                                      
    AUECID,                                                                      
    AUECIdentifier,    
    AssetID,    
    BloombergSymbol                                                                                         
  )                                                 
Select Distinct                 
 PT.Symbol,                                            
 MAX(G.AUECLocalDate) AS Date_Associated,                                                                      
 MAX(G.AUECID) as AUEDID,                                                                      
 MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,    
 MAX(G.AssetID) as AssetID,    
MAX(SM.BloombergSymbol) as BloombergSymbol                                                
From #PM_Taxlots PT                     
Inner Join T_Group G on G.GroupID = PT.GroupID     
LEFT JOIN #SecMasterData SM on  PT.Symbol = SM.TickerSymbol                                               
LEFT JOIN #T_AUEC AUEC ON G.AUECID = AUEC.AUECID                                                                                      
GROUP BY PT.Symbol                                                      
                                              
                                              
-- In case of options, we also need to fetch the underlying symbol mark as these would be utilized in no of places                                     
 --getting underlying symbols                                       
INSERT INTO #TempPositionsAndAllocatedTradesALL                                                   
(                                                      
    Symbol,                                                                                    
    Date_Associated,                                                                      
    AUECID,                                                                      
    AUECIdentifier,  
 BloombergSymbol                                                                                       
 )                                                 
SELECT DISTINCT                                                                                     
  MAX(SecMaster.UnderlyingSymbol) AS Symbol,                                                      
  MAX(T_Group.AUECLocalDate) AS Date_Associated,                                              
  MAX(SecMasterWithUnderlying.AUECID) as AUECID,                                                
  MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,  
  max(SecMasterWithUnderlying.BloombergSymbol) as BloombergSymbol  
                                                                     
 FROM                                                                       
  [dbo].T_Group LEFT JOIN #SecMasterData SecMaster on  T_Group.Symbol = SecMaster.TickerSymbol    
left join V_SecMasterData_WithUnderlying SecMasterWithUnderlying on  SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol  
LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID    
  --Gaurav: applied handling for Asset class FutureOption=4 and FXOption=10 as well along with EquityOption=2  
    WHERE T_Group.CumQty<>0 and T_Group.AssetID in (2,3,4,10) and DateDiff(day,T_Group.AUECLocaldate,@LastDateofMonth) >= 0 and T_Group.StateID=1                                            
or (DateDiff(day, T_Group.AUECLocaldate, @FirstDateofMonth) <= 0 AND     
  datediff(day,T_Group.AUECLocaldate , @LastDateofMonth) >= 0 and T_Group.StateID=1)    
and SecMaster.UnderlyingSymbol not in (Select symbol from #TempPositionsAndAllocatedTradesALL)                                                                               
    GROUP BY Symbol  
  
  
-- getting open underlying symbols  
INSERT INTO #TempPositionsAndAllocatedTradesALL                                                   
(                                                      
    Symbol,                                                                                    
    Date_Associated,                                                                      
    AUECID,                                                                      
    AUECIdentifier,  
 BloombergSymbol                                                                                       
 )                                                 
SELECT DISTINCT                                                                                     
  MAX(SecMaster.UnderlyingSymbol) AS Symbol,                                                      
  MAX(T_Group.AUECLocalDate) AS Date_Associated,                                              
  MAX(SecMasterWithUnderlying.AUECID) as AUECID,                                                
  MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,  
  MAX(SecMasterWithUnderlying.BloombergSymbol)  as BloombergSymbol                                                                   
  
 FROM                                                                       
  T_Group inner join #pm_taxlots on #pm_taxlots.groupid=T_Group.groupid left join #SecMasterData SecMaster on  #pm_taxlots.Symbol = SecMaster.TickerSymbol    
left join V_SecMasterData_WithUnderlying SecMasterWithUnderlying on  SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol  
LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID   
  --Gaurav: applied handling for Asset class FutureOption=4 and FXOption=10 as well along with EquityOption=2  
    WHERE T_Group.AssetID in (2,3,4,10)                                     
 and SecMaster.UnderlyingSymbol not in (Select symbol from #TempPositionsAndAllocatedTradesALL)                                                                               
    GROUP BY #pm_taxlots.Symbol                                       
                                                                                                                            
 CREATE TABLE [dbo].#TempVolatilityDataFinal                                                                                      
   (                                                                                      
    Symbol varchar(200),                                                                                                                 
    FinalVolatility numeric(18,4),                                                                                  
   DayVolatilityID int,                                                     
    Date1 DateTime,                                                                      
    AUECID int,                                                                      
    AUECIdentifier varchar(200),            
 AssetID int,    
 BloombergSymbol nvarchar(200)                                                         
   )                      
                                                                      
 INSERT INTO [dbo].#TempVolatilityDataFinal                                                                                      
   (                                                                                      
    Symbol,                                                                                
    Date1,                                                      
    AUECID,                                                                      
    AUECIdentifier,    
 AssetID,    
 BloombergSymbol                                                                                        
   )                                                                                      
 SELECT distinct                                             
 SYMBOL,                                             
 MAX(Date_Associated),                                      
 MAX(TPAATA.AUECID),                                             
 MAX(TPAATA.AUECIdentifier),    
 MAX(AssetID),    
 MAX(TPAATA.BloombergSymbol)  
                                                                                
 FROM                                                                                       
  #TempPositionsAndAllocatedTradesALL TPAATA                                             
  GROUP BY Symbol                                                                                                       
                                     
 DECLARE @Dates varchar(2000)                                      
 SET @Dates = ''                                      
 SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                                      
     FROM (select Top 35 AllDates.Items                                         
  from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items desc) MarkDate                                      
   SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)               
    
 exec ('select *                                        
  from (Select TPATAF.Symbol, Date, IsNull(PMDB.Volatility,0) AS FBV, TPATAF.AUECID, TPATAF.AUECIdentifier,TPATAF.BloombergSymbol FROM #TempVolatilityDataFinal TPATAF     
  LEFT OUTER JOIN PM_DailyVolatility PMDB                                      
  ON PMDB.Symbol = TPATAF.Symbol                                    
  ) AS DB                        
  PIVOT (MAX(FBV) FOR Date IN (' + @Dates + ')) AS pvt;')              
                                                  
END TRY                                          
BEGIN CATCH                                           
 SET @ErrorMessage = ERROR_MESSAGE();                     
 SET @ErrorNumber = Error_number();                                           
END CATCH;     
  
