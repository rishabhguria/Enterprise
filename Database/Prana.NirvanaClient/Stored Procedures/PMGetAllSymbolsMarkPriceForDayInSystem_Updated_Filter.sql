-----------------------------------------------------------------  
  
--modified BY: Omshiv  
--Date: 15/05/14  
--Purpose: get only open Allocated symbols  (funds symbol)  
  
--Created BY: Bharat Raturi  
--Date: 11/04/14  
--Purpose: Get the mark prices fund-symbol wise and apply filters  
--Usage: PMGetAllSymbolsMarkPriceForDayInSystem_Updated_Filter '<dsFund><dtFund><FundID>1182</FundID></dtFund><dtFund><FundID>1183</FundID></dtFund><dtFund><FundID>1182</FundID></dtFund></dsFund>', '04/01/2014','04/10/2014',3,false,'hi',2,2  
  
--modified BY: Bhavana  
--Date: 6/06/14  
--Purpose: To get data for custom dates.  
-----------------------------------------------------------------  
CREATE PROCEDURE [dbo].[PMGetAllSymbolsMarkPriceForDayInSystem_Updated_Filter]      
(  
  @fundIDs varchar(max),                                                                                      
  @Date DateTime,  
  @endDate DateTime,                                                                                
  @Type int, -- 0 for Same Date,1 for Week , 2 for Month, 3 for daterange             
  @isFxFXForwardData bit,                                                                                     
  @ErrorMessage varchar(500) output,                                                                                      
  @ErrorNumber int output,  
  @filter int                                                                                      
 )                                                                                      
AS   
         
DECLARE @FirstDateofMonth varchar(50)                                                                                
DECLARE @LastDateofMonth varchar(50)                            
                                                                
If(@Type=0) -- Daily view                                                                      
Begin                                                                                
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@Date,101)                                                                                 
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@Date,101)                                                                                 
end                                                                                
Else If(@Type=1) -- Weekly view                                                                      
Begin                                                                                
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@Date,101)               
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@Date,101)                                                          
End                                                                                
Else If(@Type=2) -- Monthly view                                                                      
Begin                                                                               
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@Date)-1),@Date),101)                                                                      
 Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@Date))),DATEADD(mm,1,@Date)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                                                
 If(@LastDateofMonth >= GETDATE())
 Begin
 Set @LastDateofMonth = GETDATE()
 End
End                                                        
Else If(@Type=3) -- Month range                                                                      
Begin                                                                               
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@Date,101)                                                                      
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@endDate,101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--    
--select @FirstDateofMonth, @LastDateofMonth, @Type                                               
End                               
        
declare @filterType varchar(10)   
set @filterType='all'  
IF(@filter=1)  
begin  
set @filterType='missing'  
end  
else IF (@filter=2)  
begin  
set @filterType='manual'  
end   
                 
SET @ErrorMessage = 'Success'                                               
SET @ErrorNumber = 0                                     
                                                                               
BEGIN TRY                                                                        
  
CREATE TABLE #PM_Taxlots  
(  
GroupID varchar(50),  
Symbol varchar(200),  
fundID int  
)  
  
declare @handle int  
exec sp_xml_preparedocument @handle output, @FundIDs  
  
create table #FundIDs  
(fundID int)  
  
insert INTO #FundIDs(fundID)  
  
select fundID from openXML(@handle,'dsFund/dtFund',2)  
with  
(FundID int)  
  
Insert into #PM_Taxlots (GroupID,Symbol,fundID)  
Select GroupID,Symbol, FundID  
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                             
 (                                                                                                   
    Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,@LastDateofMonth) >=0                                                                                                                                      
    group by taxlotid                                                   
  )   
and FundID in (select fundID from #FundIDs)  
  
  
  
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
LeadCurrencyID int,    
VsCurrencyID int,    
UnderlyingSymbol varchar(100),    
AUECID int,    
AssetID int,  
BloombergSymbol  varchar(100),
Symbol_PK varchar(100),
ISINSymbol  varchar(100),
CUSIPSymbol  varchar(100),
Currency nvarchar(100),
ExpirationDate datetime
)         
    
Insert into #SecMasterData    
Select     
TickerSymbol,    
LeadCurrencyID,    
VsCurrencyID,    
UnderlyingSymbol,    
AUECID,    
AssetID,  
BloombergSymbol,
Symbol_PK,
ISINSymbol,
CUSIPSymbol,
C.CurrencySymbol as Currency,
ExpirationDate   

from V_SecMasterData
Inner Join T_Currency C on C.CurrencyID = V_SecMasterData.CurrencyID      
                
                                                                               
 CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesALLFinal                                                                                      
   (                                                                                      
    Symbol varchar(200),                                  
    ApplicationMarkPrice numeric(18,4),                                                                                  
    FinalMarkPrice numeric(18,4),                                                                                  
    DayMarkPriceID int,                                                     
    Date1 DateTime,                                                                      
    AUECID int,                                                                      
    AUECIdentifier varchar(200),            
    ForwardPoints numeric(18,4),    
    AssetID int,    
    LeadCurrencyID int,    
    VsCurrencyID int,  
    BloombergSymbol nvarchar(200),  
    fundID int, 
    Symbol_PK nvarchar(100),    
    ISINSymbol nvarchar(200),
    CUSIPSymbol nvarchar(200),
    Currency nvarchar(100),
    ExpirationDate datetime                                                   
   )                      
                                                                      
 INSERT INTO [dbo].#TempPositionsAndAllocatedTradesALLFinal                                                                                      
   (                                                                                      
    Symbol,                                                                                
    Date1,                                                      
    AUECID,                                                                      
    AUECIdentifier,    
    AssetID,    
    LeadCurrencyID,    
    VsCurrencyID,  
    BloombergSymbol,  
    fundID,
    Symbol_PK,
	ISINSymbol,
	CUSIPSymbol,
    Currency,
    ExpirationDate                                                                                  
   )                                                                                      
 SELECT distinct                                             
 PT.Symbol,                                             
 MAX(G.AUECLocalDate) AS Date_Associated,                                      
 MAX(G.AUECID) as AUEDID,                                             
 MAX(AUEC.ExchangeIdentifier) as AUECIdentifier,   
 MAX(G.AssetID) as AssetID,        
 MAX(SM.LeadCurrencyID) as LeadCurrencyID,        
 MAX(SM.VsCurrencyID) as VsCurrencyID ,  
 MAX(SM.BloombergSymbol) as BloombergSymbol,  
 max(PT.FundID) as FundID,
 max(SM.Symbol_PK) as Symbol_PK,
 MAX(SM.ISINSymbol) as ISINSymbol,
 MAX(SM.CUSIPSymbol) as CUSIPSymbol,
 MAX(SM.Currency) as Currency,
 MAX(SM.ExpirationDate) as ExpirationDate
                                                                                
	From #PM_Taxlots PT                                            
	Inner Join T_Group G on G.GroupID = PT.GroupID    
	LEFT JOIN #SecMasterData SM on  PT.Symbol = SM.TickerSymbol                                               
	LEFT JOIN #T_AUEC AUEC ON G.AUECID = AUEC.AUECID     
	GROUP BY PT.Symbol,PT.fundID
 
  
--We are just taking safe side by taking 35 days              
Select Top 35 AllDates.Items into #TempDates                          
  from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items desc             
                                       
Select * into #TempPositionsAndAllocatedTradesALLFinal1   
From #TempDates as TEMP, #TempPositionsAndAllocatedTradesALLFinal as TPATAF                        

--Select Symbol, max(Date) as Date --into #TempSymbolMaxDate                  
--from PM_DayMarkPrice                   
--where FinalMarkPrice <> 0 and datediff(d,@date,Date) <= 0 and FundID<>0  --modified by omshiv   for add fundid check              
--Group By Symbol order by Symbol
                        
--Select Symbol, max(Date) as Date into #TempSymbolMaxDate                  
--from PM_DayMarkPrice                   
--where FinalMarkPrice <> 0 and datediff(d,@date,Date) <= 0 and FundID<>0  --modified by omshiv   for add fundid check              
--Group By Symbol order by Symbol                  
   

 --select * from #TempPositionsAndAllocatedTradesALLFinal1     
            
if ((@Type=0 or @Type = 2 or @Type=3) and @filterType='all')                
 Begin                
 Select distinct     
 TPATAF.Symbol,     
 TPATAF.Items as Date,     
 IsNull(PMDMP.FinalMarkPrice,0) AS FinalMarkPrice,     
 TPATAF.AUECID,     
 TPATAF.AUECIdentifier,                          
 CASE                                   
  WHEN datediff(d,PMDMP.date,TPATAF.Items) = 0                                     
  THEN 0                                                
  ELSE 1                                                
 END AS MarkPriceIndicator,    
 IsNull(PMDMP.ForwardPoints,0) AS ForwardPoints,    
 TPATAF.AssetID,    
 TPATAF.LeadCurrencyID,    
 TPATAF.VsCurrencyID,  
 TPATAF.BloombergSymbol,  
 TPATAF.fundID,  
 PMDMP.SourceID,
 TPATAF.Symbol_PK,       
 f.FundName,
 TPATAF.ISINSymbol,
 TPATAF.CUSIPSymbol,
 TPATAF.Currency,
 TPATAF.ExpirationDate
              
 From #TempPositionsAndAllocatedTradesALLFinal1 as TPATAF  
 inner JOIN (SELECT CompanyFundID,FundName from T_CompanyFunds Where T_CompanyFunds.IsActive=1)AS f   
 on f.CompanyFundID=TPATAF.fundID                        
 LEFT OUTER JOIN PM_DayMarkPRice PMDMP ON TPATAF.Symbol = PMDMP.Symbol and TPATAF.FundID = PMDMP.FundID and DateDiff(d,TPATAF.Items,PMDMP.Date) = 0                
 Order By TPATAF.Symbol, Date desc              
    End     
  
else 

IF((@Type=0 or @Type=2 or @Type=3) and @filterType='missing')  
          Begin  

            
 Select distinct     
 TPATAF.Symbol,     
 TPATAF.Items as Date,     
 IsNull(PMDMP.FinalMarkPrice,0) AS FinalMarkPrice,     
 TPATAF.AUECID,     
 TPATAF.AUECIdentifier,                          
 CASE                                   
  WHEN datediff(d,PMDMP.date,TPATAF.Items) = 0                                     
  THEN 0                                                
  ELSE 1                                                
 END AS MarkPriceIndicator,    
 IsNull(PMDMP.ForwardPoints,0) AS ForwardPoints,    
 TPATAF.AssetID,    
 TPATAF.LeadCurrencyID,    
 TPATAF.VsCurrencyID,  
 TPATAF.BloombergSymbol,  
 TPATAF.fundID,  
 PMDMP.SourceID,
 TPATAF.Symbol_PK,     
 f.FundName,
 TPATAF.ISINSymbol,
 TPATAF.CUSIPSymbol,
 TPATAF.Currency,
 TPATAF.ExpirationDate
                  
 From #TempPositionsAndAllocatedTradesALLFinal1 as TPATAF  
 inner JOIN (SELECT CompanyFundID,FundName from T_CompanyFunds Where T_CompanyFunds.IsActive=1)AS f   
 on f.CompanyFundID=TPATAF.fundID                        
 LEFT OUTER JOIN PM_DayMarkPRice PMDMP ON TPATAF.Symbol = PMDMP.Symbol and TPATAF.FundID = PMDMP.FundID and DateDiff(d,TPATAF.Items,PMDMP.Date) = 0                
    where PMDMP.FinalMarkPrice=0 or  PMDMP.FinalMarkPrice is null
 Order By TPATAF.Symbol, Date desc              
    End  
else IF((@Type=0 or @Type=2 or @Type=3) and @filterType='manual')  
          Begin                
 Select distinct     
 TPATAF.Symbol,     
 TPATAF.Items as Date,     
 IsNull(PMDMP.FinalMarkPrice,0) AS FinalMarkPrice,     
 TPATAF.AUECID,     
 TPATAF.AUECIdentifier,                          
 CASE                                   
  WHEN datediff(d,PMDMP.date,TPATAF.Items) = 0                                     
  THEN 0                                                
  ELSE 1                                                
 END AS MarkPriceIndicator,    
 IsNull(PMDMP.ForwardPoints,0) AS ForwardPoints,    
 TPATAF.AssetID,    
 TPATAF.LeadCurrencyID,    
 TPATAF.VsCurrencyID,  
 TPATAF.BloombergSymbol,  
 TPATAF.fundID,  
 PMDMP.SourceID,
 TPATAF.Symbol_PK, 
 f.FundName,
 TPATAF.ISINSymbol,
 TPATAF.CUSIPSymbol,
 TPATAF.Currency,
 TPATAF.ExpirationDate                    
 From #TempPositionsAndAllocatedTradesALLFinal1 as TPATAF  
 inner JOIN (SELECT CompanyFundID,FundName from T_CompanyFunds Where T_CompanyFunds.IsActive=1)AS f   
 on f.CompanyFundID=TPATAF.fundID                        
 LEFT OUTER JOIN PM_DayMarkPRice PMDMP ON TPATAF.Symbol = PMDMP.Symbol and DateDiff(d,TPATAF.Items,PMDMP.Date) = 0                
    where PMDMP.SourceID=-2147483648  
 Order By TPATAF.Symbol, Date desc     
    End     
 


  
 DROP TABLE #PM_Taxlots, #T_AUEC, #SecMasterData    
 DROP TABLE #TempPositionsAndAllocatedTradesALLFinal,#TempPositionsAndAllocatedTradesALLFinal1,#TempDates                                                                                    
                           
exec sp_xml_removedocument @handle  
  
 END TRY                                           
 BEGIN CATCH                                                                                       
 SET @ErrorMessage = ERROR_MESSAGE();                                                                 
 SET @ErrorNumber = Error_number();                                                                                       
END CATCH;       

