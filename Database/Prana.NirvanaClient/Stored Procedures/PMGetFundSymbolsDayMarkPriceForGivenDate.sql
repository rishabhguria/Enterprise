-----------------------------------------------------------------

--modified BY: Omshiv
--Date: 15/05/14
--Purpose: Get Fund-Symbols DayMarkPrice For GivenDate  (funds-symbol markprice Changes for CH)

----------------------------------------------------------------

create PROCEDURE [dbo].[PMGetFundSymbolsDayMarkPriceForGivenDate]                                              
 (                                                                                
  @Date DateTime,                                                                                                                       
  @ErrorMessage varchar(500) output,                                                                                
  @ErrorNumber int output                                                                                
 )                                                                                
AS    
--DECLARE @FirstDateofMonth varchar(50)                                                                          
--DECLARE @LastDateofMonth varchar(50)                      
--                                                          
--Set @FirstDateofMonth=CONVERT(VARCHAR(25),@Date,101)                                                                           
--Set @LastDateofMonth=CONVERT(VARCHAR(25),@Date,101)                                                                           
                                                                                
SET @ErrorMessage = 'Success'                                         
SET @ErrorNumber = 0                               
                                                                         
BEGIN TRY        
  
CREATE TABLE #SecMasterData  
(  
TickerSymbol varchar(100),  
LeadCurrencyID int,  
VsCurrencyID int,  
UnderlyingSymbol varchar(100),  
AUECID int,  
AssetID int  
)       
  
Insert into #SecMasterData  
Select   
TickerSymbol,  
LeadCurrencyID,  
VsCurrencyID,  
UnderlyingSymbol,  
AUECID,  
AssetID  
from V_SecMasterData   
                                                       
                           
 CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesALL                                                             
   (                                                                                
    Symbol varchar(200),                                                                                
    DayMarkPriceID int,                                                          
    Date_Associated datetime ,                                                        
    AUECID int,                                                                
    AUECIdentifier varchar(200),      
    ForwardPoints float,  
    AssetID int,  
    LeadCurrencyID int,  
    VsCurrencyID int ,
FundID int                                           
   )                          
       


                                                              
-- INSERT INTO                                                     
--  #TempPositionsAndAllocatedTradesALL                                                                              
--   (                                                
--    Symbol,                                                                          
--    Date_Associated,                                                                
--    AUECID,                                                                
--    AUECIdentifier,  
--    AssetID,  
--    LeadCurrencyID,  
--    VsCurrencyID                                                                                 
--   )                                                           
-- SELECT DISTINCT                                                                               
--  T_Group.Symbol AS Symbol,                          
--  MAX(T_Group.AUECLocalDate) AS Date_Associated,                          
--  MAX(T_Group.AUECID) as AUEDID,                          
--  MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,  
--  MAX(T_Group.AssetID) as AssetID,  
--  MAX(SM.LeadCurrencyID) as LeadCurrencyID,  
--  MAX(SM.VsCurrencyID) as VsCurrencyID                              
-- FROM                                                                 
--  [dbo].T_Group 
--  LEFT JOIN T_AUEC AUEC ON T_Group.AUECID = AUEC.AUECID    
--  Left Join #SecMasterData SM ON  T_Group.Symbol = SM.TickerSymbol                                                  
----AssetID 5 is for fx And T_Group.StateID=1(Unallocated Trades), 2(Allocated Trades)      
--  WHERE   DateDiff(day,T_Group.AUECLocaldate,@Date) >= 0  And T_Group.StateID=1                                                                             
--  GROUP BY Symbol  
--                                                           
--                                                             
--UNION                                                            
-- Select DISTINCT                                                                               
--  PMD.Symbol AS Symbol,                 
--  MAX(PMD.Date) AS Date_Associated,                                                                
--  0 as AUECID,                                                                
--  'Indices-Indices' as AUECIdentifier,  
--  0 as AssetID,  
--  0 as LeadCurrencyID,  
--  0 as VsCurrencyID                                                   
--  From PM_DayMarkPrice PMD                                                            
--  WHERE DATEDIFF(d,PMD.Date,@Date) >= 0 and Substring(PMD.Symbol,1,1) = '$'                          
--  GROUP BY PMD.Symbol                                       
                                      
                                      
-- collect open trade symbols                                      
INSERT INTO                                                     
  #TempPositionsAndAllocatedTradesALL                                       
  (                                                
    Symbol,                                                                              
    Date_Associated,                                                                
    AUECID,                                                                
    AUECIdentifier,  
    AssetID,  
    LeadCurrencyID,  
    VsCurrencyID,
      FundID                                                                            
  )                                           
Select Distinct                                      
 PT.Symbol,                                      
 MAX(G.AUECLocalDate) AS Date_Associated,                                                                
 MAX(G.AUECID) as AUEDID,                                                                
 MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,  
 MAX(G.AssetID) as AssetID,  
 MAX(SM.LeadCurrencyID) as LeadCurrencyID,  
 MAX(SM.VsCurrencyID) as VsCurrencyID  ,  
 max(PT.FundID) as FundID                                        
From PM_Taxlots PT                                      
Inner Join T_Group G on G.GroupID = PT.GroupID   
LEFT JOIN #SecMasterData SM on  PT.Symbol = SM.TickerSymbol                                            
LEFT JOIN T_AUEC AUEC ON G.AUECID = AUEC.AUECID                                      
Where TaxLotOpenQty<>0                                                                                                   
   and Taxlot_PK in                                                                                             
   (                                                                                                 
    Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                             
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,@Date) >=0                  
    group by taxlotid                                                 
  )                                      
GROUP BY PT.Symbol,PT.FundID                                                
                                        
-- In case of options, we also need to fetch the underlying symbol mark as these would be utilized in no of places                               
--- Default NASD-Equity hard coded because V_SecMasterdata is not fetching any info for underlying symbols as these might not be traded.                                        
-- Since AUECIdentifier is just used on Valuation UI filtering so it shouldn't create problems                                        
                                        
--INSERT INTO                                                                               
--   #TempPositionsAndAllocatedTradesALL                                                 (                                                
--    Symbol,                                                                              
--    Date_Associated,                                                                
--    AUECID,                                                                
--    AUECIdentifier                                                                                 
-- )                                           
--SELECT DISTINCT                                                                               
--  MAX(SecMaster.UnderlyingSymbol) AS Symbol,                                                
--  MAX(T_Group.AUECLocalDate) AS Date_Associated,                         
--  MAX(SecMaster.AUECID) as AUEDID,                                            
--  --MAX('NASD-Equity') as AUECIdentifier                                                            
--MAX(AUEC.ExchangeIdentifier) as AUECIdentifier                                                            
--
-- FROM                                                                 
--  [dbo].T_Group LEFT JOIN T_AUEC AUEC ON T_Group.AUECID = AUEC.AUECID                                        
--    LEFT JOIN #SecMasterData SecMaster on  T_Group.Symbol = SecMaster.TickerSymbol                                    
----Gaurav: applied handling for Asset class FutureOption and FXOption as well along with EquityOption
--    WHERE AUEC.AssetID in (2,4,10) and DateDiff(day,T_Group.AUECLocaldate,@Date) >= 0                                     
--    and SecMaster.UnderlyingSymbol not in (Select symbol from #TempPositionsAndAllocatedTradesALL)                                                                         
--    GROUP BY Symbol    
--

                                     
                                          
                                                                         
 CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesALLFinal                                                                                
   (                                                                                
    Symbol varchar(200),                            
    ApplicationMarkPrice numeric(18,4),                                                                            
    FinalMarkPrice numeric(18,4),                                                                            
    DayMarkPriceID int,                                               
    Date1 DateTime,                                                                
    AUECID int,                                                                
    AUECIdentifier varchar(200) ,    
    ForwardPoints float,  
    AssetID int,  
    LeadCurrencyID int,  
    VsCurrencyID int ,
FundID int                                                   
   )                                                                         
                                                                
 INSERT INTO                                                                                 
  [dbo].#TempPositionsAndAllocatedTradesALLFinal                                                                                
   (                                                                                
    Symbol,                                                                          
    Date1,                                                
    AUECID,                                                                
    AUECIdentifier,  
 AssetID,  
 LeadCurrencyID,  
 VsCurrencyID,
FundID                                                                                  
   )                                                                    
 SELECT                             
  distinct                                       
 SYMBOL,                                       
 MAX(Date_Associated),                                
 MAX(TPAATA.AUECID),                                       
 MAX(TPAATA.AUECIdentifier),  
 MAX(AssetID),  
 MAX(LeadCurrencyID),  
 MAX(VsCurrencyID),
MAX(FundID)                                                                  
 FROM                                                                                 
  #TempPositionsAndAllocatedTradesALL TPAATA                                       
  GROUP BY Symbol,FundID                                                      
      
      
Select Top 35 AllDates.Items into #TempDates                    
  from dbo.GetDateRange(@Date, @Date) as AllDates Order By AllDates.Items desc                                
                          
Select * into #TempPositionsAndAllocatedTradesALLFinal1                  
From #TempDates as TEMP, #TempPositionsAndAllocatedTradesALLFinal as TPATAF          
      
--- Code To Have Symbol wise Maximum and Minimum Date      
      
CREATE TABLE #TempSymbolMaxMinMarkPriceDate      
(      
 auecid int,  
 MaxDate datetime,  
 MinDate datetime      
)      
INSERT INTO 
#TempSymbolMaxMinMarkPriceDate      
Select distinct   
#TempPositionsAndAllocatedTradesALL.AUECID,   
max(PM_DayMarkPrice.Date) as MaxDate,  
min(PM_DayMarkPrice.Date) as MinDate      
from PM_DayMarkPrice   
inner join #TempPositionsAndAllocatedTradesALL   
on #TempPositionsAndAllocatedTradesALL.Symbol = PM_DayMarkPrice.Symbol      
where PM_DayMarkPrice.FinalMarkPrice > 0   
and datediff(d,@date,PM_DayMarkPrice.Date) <= 0      
Group By #TempPositionsAndAllocatedTradesALL.AUECID   
order by #TempPositionsAndAllocatedTradesALL.AUECID       
      
--- End Code To Have Symbol wise Maximum and Minimum Date      
      
--- Code To Have Symbol wise  Business Adjusted Date      
      
declare @currectauecid int,@maxdate datetime,@mindate datetime      
      
create table #BusinessAdjustedSymbolWiseMaxDate(Symbol varchar(50),MaxDate datetime)      
create table #AuecHolidays(Holidays datetime)      
      
while(Select Count(distinct auecid) From #TempSymbolMaxMinMarkPriceDate)> 0      
begin      
      
 Select top(1) @currectauecid=auecid,@maxdate=MaxDate,@mindate=MinDate  From #TempSymbolMaxMinMarkPriceDate      
      
 insert into #AuecHolidays  EXEC [P_GetAUECwiseHolidaysBetweenTwoDates] @mindate,@maxdate,@currectauecid--,null OUTPUT, null output      
      
 insert into #BusinessAdjustedSymbolWiseMaxDate(Symbol,MaxDate)      
 Select   
 PM_DayMarkPrice.Symbol,  
 max(PM_DayMarkPrice.Date) as Date       
 from PM_DayMarkPrice   
 inner join #TempPositionsAndAllocatedTradesALL   
 on #TempPositionsAndAllocatedTradesALL.Symbol = PM_DayMarkPrice.Symbol      
 where PM_DayMarkPrice.FinalMarkPrice > 0 and datediff(d,@Date,PM_DayMarkPrice.Date) <= 0       
 AND #TempPositionsAndAllocatedTradesALL.AUECID = @currectauecid       
 AND PM_DayMarkPrice.Date not in (select Holidays from #AuecHolidays)      
 Group By PM_DayMarkPrice.Symbol order by PM_DayMarkPrice.Symbol      
      
 delete from #TempSymbolMaxMinMarkPriceDate where auecid=@currectauecid      
 delete from #AuecHolidays      
      
end      
      
      
--- End Of Code To Have Symbol wise  Business Adjusted Date      
      
      
 Select   
 distinct TPATAF.Symbol,   
 IsNull(PMDMP1.Date,TPATAF.Items) as Date,   
 IsNull(PMDMP1.FinalMarkPrice,0) AS FinalMarkPrice,   
 TPATAF.AUECID,   
 TPATAF.AUECIdentifier,                    
  CASE                             
  WHEN datediff(d,PMDMP1.date,TPATAF.Items) = 0                               
  THEN 0                                          
  ELSE 1                                          
  END AS MarkPriceIndicator ,    
 IsNull(PMDMP1.ForwardPoints,0) as ForwardPoints,  
  TPATAF.AssetID,  
TPATAF.LeadCurrencyID,  
TPATAF.VsCurrencyID,  
TPATAF.FundID            
 From #TempPositionsAndAllocatedTradesALLFinal1 as TPATAF                  
 LEFT OUTER JOIN         
 (Select PMDMP.Symbol, PMDMP.FinalMarkPrice, PMDMP.Date, PMDMP.ForwardPoints from PM_DayMarkPRice PMDMP         
  INNER JOIN #BusinessAdjustedSymbolWiseMaxDate symbolDate ON DateDiff(d,symbolDate.MaxDate,PMDMP.Date) = 0 and symbolDate.Symbol = PMDMP.Symbol ) PMDMP1        
 ON TPATAF.Symbol = PMDMP1.Symbol             
 Order By TPATAF.Symbol, Date          
         
                  
DROP TABLE #TempPositionsAndAllocatedTradesALL,#TempPositionsAndAllocatedTradesALLFinal  
DROP TABLE #TempPositionsAndAllocatedTradesALLFinal1,#TempDates, #SecMasterData                                 
drop table #AuecHolidays,#TempSymbolMaxMinMarkPriceDate,#BusinessAdjustedSymbolWiseMaxDate      
                                  
END TRY                                     
BEGIN CATCH                                                                                 
 SET @ErrorMessage = ERROR_MESSAGE();                                                           
 SET @ErrorNumber = Error_number();                                                                                 
END CATCH;    
