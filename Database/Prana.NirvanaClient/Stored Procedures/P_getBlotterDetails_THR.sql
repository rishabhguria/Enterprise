
/*****************************************************************                                
                                
P_getBlotterDetails_THR '2014/12/17','2014/12/17' ,'1199,1211,1212,1213,1214,1215,1268,1269,1270,1271,1272,1273','1,2,3,4,5,6,7,8,9',8                
              
*****************************************************************/                                
                     
CREATE proc P_getBlotterDetails_THR                        
(                                
 @StartDate datetime,                                
 @EndDate datetime ,                            
 @Fund varchar(max),                                                  
 @Asset varchar(max),                            
 @companyID int                                  
)                                
                                
as                                 
begin                
                              
Declare @T_FundIDs Table                                                            
(                                                            
 FundId int                                                            
)                                                            
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                                                   
                                                  
Declare @T_AssetIDs Table                                                                
(                                                                
 AssetId int                                                                
)                                                                
Insert Into @T_AssetIDs Select * From dbo.Split(@Asset, ',')                                                      
                                                               
 Select                                                           
 T_Asset.*                                                       
 InTo #T_Asset                                                             
 From T_Asset INNER JOIN @T_AssetIDs AID ON T_Asset.AssetID = AID.AssetId                                                    
                                                  
CREATE TABLE #T_CompanyFunds                                                            
(                                                            
 CompanyFundID int,                                                            
 FundName varchar(50),                                                            
 FundShortName varchar(50),                                                            
 CompanyID int,                                                            
 FundTypeID int,                                                    
 UIOrder int NULL                                                           
)                                                   
                                                         
Insert Into #T_CompanyFunds                                                            
Select                                         
 CompanyFundID,                                                            
 FundName,                                                            
 FundShortName,                                                            
 CompanyID,                                                            
 FundTypeID,                                                    
 UIOrder                                                              
 From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                  
 Where T_CompanyFunds.IsActive=1
              
select               
 convert(varchar(10),TradeDate,120) as Date,              
 SUM(              
  case              
   When (Fills_PK is null)              
   Then 0              
   else 1              
  End              
 ) as FillsCounter              
 into #DaysFills              
from T_BlotterHistory                
where               
(datediff(d,TradeDate,@StartDate)<=0 and datediff(d,TradeDate,@EndDate)>=0 )                           
group by convert(varchar(10),TradeDate,120)      
              
              
select               
 T_BlotterHistory.ParentCLOrderID,              
 LastFillPK,              
 T_BlotterHistory.TradeDate,              
 T_BlotterHistory.CumQty              
 into #LastFills              
From              
 (              
  select               
   ParentCLOrderID              
   ,convert(varchar(10),TradeDate,120) as TradeDate              
   ,MAX(Fills_PK) as LastFillPK               
                 
  from T_BlotterHistory              
  where (datediff(d,T_BlotterHistory.TradeDate,@StartDate)<=0 and datediff(d,T_BlotterHistory.TradeDate,@EndDate)>=0 )       
  and IsSummary = 1                          
  group by convert(varchar(10),TradeDate,120),ParentCLOrderID              
                
 ) TempTable              
inner join  T_BlotterHistory on T_BlotterHistory.ParentCLOrderID = TempTable.ParentCLOrderID and T_BlotterHistory.Fills_PK = TempTable.LastFillPK              
inner join  #DaysFills on datediff(day,#DaysFills.Date,T_BlotterHistory.Tradedate)=0              
where #DaysFills.FillsCounter > 0              
       
    
insert into #LastFills              
select               
 T_BlotterHistory.ParentCLOrderID,              
 Fills_PK,              
 T_BlotterHistory.TradeDate,              
 T_BlotterHistory.CumQty              
              
From              
 (              
  select               
   ParentCLOrderID              
   ,convert(varchar(10),TradeDate,120) as TradeDate              
   ,MAX(TradeDate) as LastTradeTime              
                 
  from T_BlotterHistory              
  where (datediff(d,T_BlotterHistory.TradeDate,@StartDate)<=0 and datediff(d,T_BlotterHistory.TradeDate,@EndDate)>=0 )                          
  group by convert(varchar(10),TradeDate,120),ParentCLOrderID              
                
 ) TempTable              
inner join  T_BlotterHistory on T_BlotterHistory.ParentCLOrderID = TempTable.ParentCLOrderID and T_BlotterHistory.TradeDate = TempTable.LastTradeTime              
inner join  #DaysFills on datediff(day,#DaysFills.Date,T_BlotterHistory.Tradedate)=0              
where #DaysFills.FillsCounter = 0              
  
  
 SELECT  DISTINCT
 SM.TickerSymbol,      
 COALESCE(NonHistoryData.CompanyName, OptionData.ContractName, FutureData.ContractName, FxData.LongName, FxForwardData.LongName, FixedIncomeData.BondDescription) AS CompanyName,      
 SM.CUSIPSymbol, 
 SM.SEDOLSymbol,    
 SM.ISINSymbol,       
 COALESCE(NonHistoryData.Multiplier, OptionData.Multiplier, FutureData.Multiplier, FxData.Multiplier, FxForwardData.Multiplier, FixedIncomeData.Multiplier) AS Multiplier,     
 SM.AssetId
 INTO #SMDATA 
FROM T_BlotterHistory   
INNER JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable AS SM      
 ON T_BlotterHistory.Symbol = SM.TickerSymbol AND SM.AssetID!=7
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData AS NonHistoryData      
 ON NonHistoryData.Symbol_PK = SM.Symbol_PK 
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMOptionData AS OptionData      
 ON OptionData.Symbol_PK = SM.Symbol_PK    
LEFT OUTER JOIN  [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable UnderlyingSM      
 ON SM.UnderlyingSymbol = UnderlyingSM.TickerSymbol
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData AS UnderlyingSMData      
 ON UnderlyingSMData.Symbol_PK = UnderlyingSM.Symbol_PK   
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFutureData AS FutureData      
 ON FutureData.Symbol_PK = SM.Symbol_PK   
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxData AS FxData      
 ON FxData.Symbol_PK = SM.Symbol_PK     
LEFT OUTER JOIN T_Currency AS CurrencyFxData1      
 ON CurrencyFxData1.CurrencyID = FxData.LeadCurrencyID 
LEFT OUTER JOIN T_Currency AS CurrencyFxData2      
 ON CurrencyFxData2.CurrencyID = FxData.VsCurrencyID   
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData AS FxForwardData      
 ON FxForwarddata.Symbol_PK = SM.Symbol_PK      
LEFT OUTER JOIN T_Currency AS CurrencyFxForwardData1      
 ON CurrencyFxForwardData1.CurrencyID = FxForwardData.LeadCurrencyID   
LEFT OUTER JOIN T_Currency AS CurrencyFxForwardData2      
 ON CurrencyFxForwardData2.CurrencyID = FxForwardData.VsCurrencyID  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFixedIncomeData AS FixedIncomeData      
 ON FixedIncomeData.Symbol_PK = SM.Symbol_PK      
           
Select                                         
 BH.Side                                
,BH.ParentCLOrderID                                
,BH.TradeDate as 'TradeDate'                                
,BH.Symbol as 'Symbol'                                
,#SMDATA.CompanyName as 'Symbol Description'                                        
,#SMDATA.CUSIPSymbol as 'Cusip'                                        
,#SMDATA.SEDOLSymbol as 'Sedol'                                        
,#SMDATA.ISINSymbol as 'Isin'                                        
,#SMDATA.Multiplier as 'Multiplier'                                        
,BH.LastShares 'LastShare'                                
,BH.AveragePrice 'LastPrice'                                
,IsNull(BH.AveragePrice * BH.LastShares * IsNUll(#SMDATA.Multiplier,0) * dbo.GetSideMultiplier(BH.OrderSidetagValue),0) as  'principalAmountLocal'                                        
,BH.Broker AS 'Broker'                                
,BH.users  AS 'User'                                
,BH.Description as 'Description'                                
,BH.CumQty as CumQty               
,#LastFills.CumQty as  LastCumQty                             
from T_BlotterHistory BH                              
INNER JOIN #SMDATA on #SMDATA.TickerSymbol = BH.Symbol                                      
INNER JOIN T_Company C ON C.companyID = @companyID                                                                            
INNER JOIN #T_Asset A ON #SMDATA.AssetID = A.AssetID                                              
inner join #LastFills  on #LastFills.ParentCLOrderID = BH.ParentCLOrderID              
Where BH.CumQty > 0 and         
(datediff(d,@StartDate,BH.TradeDate)>=0 and datediff(d,BH.TradeDate,@EndDate)>=0 )               
order by  BH.Symbol        
                            
Drop table  #T_CompanyFunds,#T_Asset,#LastFills,#DaysFills ,#SMDATA                                   
      
END 

