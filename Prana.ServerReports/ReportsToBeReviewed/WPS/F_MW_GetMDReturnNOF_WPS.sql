-- =============================================                
-- Author:  Pooja Porwal             
-- Create date: <5/6/2015>                
-- Description: <returns the Modified Dietz Return given date range for the entity, note this is for non-linked returns that are less than a month. For linked returns, use F_getLinkedMDReturn>                
--    2/3/2011-- expanded to include calculating returns for multi-entities as a table               
--    Sample:  select dbo.F_MW_GetMDReturnNOF('7/1/2010','7/2/2010', 'Symbol', 'SPY')                  
--    select dbo.F_MW_GetMDReturnNOF('7/1/2010','7/2/2010', 'Aggregate', '')                 
--    select dbo.F_MW_GetMDReturnNOF('1/1/2011','1/31/2011', 'Fund', 'UBS Series,May 09 Series')                 
--    select dbo.F_MW_GetMDReturnNOF('1/1/2011','1/31/2011', 'Fund', 'May 09 Series')                 
--    select dbo.F_MW_GetMDReturnNOF('1/1/2011','1/31/2011', 'Symbol', 'SPY,C')                 
-- =============================================                
Create Procedure [dbo].[F_MW_GetMDReturnNOF_WPS]                 
(                
 @fromDate datetime,                  
 @toDate datetime,                
 @funds varchar(MAX),              
 @groupfield varchar(50),                
 @entity varchar(MAX)                
)                
AS                
BEGIN                
                
declare @result float                
declare @test float                
create table #T_Funds (entity varchar(max))                
insert into #T_Funds select * from dbo.split(@funds, ',')              
              
create table #T_entity (entity varchar(max))                  
insert into #T_entity select * from dbo.split(@entity, ',')                
              
--total days in period                  
declare @D float                
set @D = DATEDIFF(d,@fromdate, @toDate) + 1                  
              
--days remaining in period                  
declare @daysRemain float               
set @daysRemain = @D                
                
declare @LastBusinessDateOfFromdate datetime                
set @LastBusinessDateOfFromdate = dbo.AdjustBusinessDays(@fromDate, -1, 1)                
                
declare @NextBusinessDateIfNotBusinessDay datetime                
if dbo.IsBusinessDay(@fromdate,1) = 0                
 begin                 
 select @NextBusinessDateIfNotBusinessDay = dbo.AdjustBusinessDays(@fromDate, 1, 1)                
 end                 
else                
 begin                
 select @NextBusinessDateIfNotBusinessDay = @fromDate                
 end                
                
declare @BaseCurrencyID int                
select @BaseCurrencyID = BaseCurrencyID from T_Company                
                
create table #InvestorCash              
(fundname varchar(max),masterfund varchar(max),payoutdate datetime,TransactionType varchar(max),TransactionTypeID int, MasterCategoryName varchar(max),  SubCategoryName varchar(max), Name varchar(max),cashvalue float)          
                
insert into #InvestorCash                
SELECT Funds.FundName,MFunds.MasterFundName as MasterFund,Journal.TransactionDate AS PayOutDate,TransactionType,SubAccounts. TransactionTypeID,       
MasterCategoryName,SubCategoryName,Name, case when Journal.CurrencyID = @BaseCurrencyID             
then Journal.CR- Journal.DR else isnull(CurrencyConversionRate.ConversionRate,1)*(Journal.CR- Journal.DR) end AS CashValue                
FROM T_Journal AS Journal                 
INNER JOIN T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID                 
INNER JOIN T_SubCategory SC on SubAccounts.SubCategoryID = SC.SubCategoryID                
INNER JOIN T_MasterCategory MC on MC.MasterCategoryID=SC.MasterCategoryID                
INNER JOIN T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID                 
INNER JOIN T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID                 
INNER JOIN T_CompanyMasterFundSubAccountAssociation AS MFMappingFunds ON MFMappingFunds.CompanyFundID = Funds.CompanyFundID
INNER JOIN T_CompanyMasterFunds AS MFunds ON MFunds.CompanyMasterFundID = MFMappingFunds.CompanyMasterFundID
LEFT OUTER JOIN T_CurrencyStandardPairs AS CurrencyStandardPairs ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID AND CurrencyStandardPairs.ToCurrencyID = @BaseCurrencyID                 
LEFT OUTER JOIN T_CurrencyConversionRate AS CurrencyConversionRate ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID AND CurrencyConversionRate.Date = Journal.TransactionDate                 
WHERE TransactionType = 'Investor Cash Transactions' or MasterCategoryName in ('Expenses' , 'Income')                
and SubAccounts. TransactionTypeID <> 3                
              
select * into #tempT_MW_GenericPNL  from T_MW_GenericPNL               
where Rundate between @fromdate and @todate and fund in (select * from #T_Funds)              
              
create table #FinalData(GroupingLevel varchar(max),PNL  float,BMVsecurities float,BMVcash float,BMVcashflow float)              
              
--for Masterfund         
if @groupfield = 'MasterFund' 
 Begin         
insert into #FinalData(GroupingLevel)        
select distinct entity as GroupingLevel from #T_entity        
---------PNL        
Update #FinalData Set PNL = isnull(TempPNL1.PNL,0)        
From  #FinalData FData         
inner join        
(        
select Masterfund,sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) as pnl from #tempT_MW_GenericPNL         
  WHERE asset<>'cash'         
  group by Masterfund        
) TempPNL1        
on TempPNL1.Masterfund = FData.GroupingLevel        
        
Update #FinalData Set PNL = isnull(PNL + isnull(TempPNL2.PNLsupp,0),0)        
From  #FinalData FData         
inner join        
(        
select I.Masterfund,isnull(sum(CashValue),0) as PNLsupp from #InvestorCash I         
  where payoutdate between @fromdate and @todate and MasterCategoryName<>'Equity'           
  group by I.Masterfund        
)TempPNL2        
on TempPNL2.Masterfund = FData.GroupingLevel        
        
--------- BMVsecurities column        
Update #FinalData Set BMVsecurities = isnull(TempBMVsecurities.BMVsecurities,0)        
From  #FinalData FData         
inner join        
(        
  select Masterfund,sum(beginningmarketvaluebase) as BMVsecurities from #tempT_MW_GenericPNL         
  where asset<>'cash' and DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay        
  group by Masterfund          
) TempBMVsecurities        
on TempBMVsecurities.Masterfund = FData.GroupingLevel        
        
--------- BMVCash column        
update #FinalData Set BMVcash = isnull(TempBMVcash.BMVcash,0)        
From #FinalData FData        
inner join        
(        
select Masterfund,sum(beginningmarketvaluebase) as BMVcash from #tempT_MW_GenericPNL         
  where asset='cash' AND rundate = @NextBusinessDateIfNotBusinessDay        
  group by Masterfund          
)TempBMVcash        
on TempBMVcash.Masterfund = FData.GroupingLevel        
        
---------cashflow columns        
update #FinalData Set BMVcashflow = isnull(TempBMVcashflow.BMVcashflow,0)        
From #FinalData FData         
inner join        
(        
select Masterfund,sum(((DATEDIFF(d,PayOutDate, @todate) + 1)/@D)*Cashvalue) as BMVcashflow from #InvestorCash         
  where payoutdate between @fromdate and @todate        
  group by Masterfund        
)TempBMVcashflow        
on TempBMVcashflow.Masterfund = FData.GroupingLevel        
        
select GroupingLevel ,PNL ,BMVsecurities,BMVcash,BMVcashflow,isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0) as sum,         
case         
 when (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0)) <= -1         
 then -.999         
 else (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0))        
end as result         
from #FinalData        
        
END        
        
              
              
--for fund               
if @groupfield = 'Fund'                
 Begin               
insert into #FinalData(GroupingLevel)              
select distinct entity as GroupingLevel from #T_entity              
---------PNL              
Update #FinalData Set PNL = isnull(TempPNL1.PNL,0)              
From  #FinalData FData               
inner join              
(              
select fund,sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) as pnl,fund as fundname from #tempT_MW_GenericPNL               
  WHERE asset<>'cash'               
  group by fund              
) TempPNL1              
on TempPNL1.Fund = FData.GroupingLevel              
              
Update #FinalData Set PNL = isnull(PNL + isnull(TempPNL2.PNLsupp,0),0)              
From  #FinalData FData               
inner join              
(              
select I.fundname as fund,isnull(sum(CashValue),0) as PNLsupp from #InvestorCash I               
  where payoutdate between @fromdate and @todate and MasterCategoryName<>'Equity'                 
  group by I.fundname              
)TempPNL2              
on TempPNL2.Fund = FData.GroupingLevel              
              
--------- BMVsecurities column              
Update #FinalData Set BMVsecurities = isnull(TempBMVsecurities.BMVsecurities,0)              
From  #FinalData FData               
inner join              
(              
  select Fund,sum(beginningmarketvaluebase) as BMVsecurities from #tempT_MW_GenericPNL               
  where asset<>'cash' and DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay              
  group by fund                
) TempBMVsecurities              
on TempBMVsecurities.Fund = FData.GroupingLevel              
              
--------- BMVCash column              
update #FinalData Set BMVcash = isnull(TempBMVcash.BMVcash,0)              
From #FinalData FData              
inner join              
(              
select Fund,sum(beginningmarketvaluebase) as BMVcash from #tempT_MW_GenericPNL               
  where asset='cash' AND rundate = @NextBusinessDateIfNotBusinessDay              
  group by fund                
)TempBMVcash              
on TempBMVcash.Fund = FData.GroupingLevel              
              
---------cashflow columns              
update #FinalData Set BMVcashflow = isnull(TempBMVcashflow.BMVcashflow,0)              
From #FinalData FData               
inner join              
(              
select Fundname as Fund,sum(((DATEDIFF(d,PayOutDate, @todate) + 1)/@D)*Cashvalue) as BMVcashflow from #InvestorCash               
  where payoutdate between @fromdate and @todate              
  group by fundname              
)TempBMVcashflow              
on TempBMVcashflow.Fund = FData.GroupingLevel              
              
select GroupingLevel ,PNL ,BMVsecurities,BMVcash,BMVcashflow,isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0) as sum,               
case               
 when (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0)) <= -1               
 then -.999               
 else (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0))              
end as result               
from #FinalData              
              
END              
              
--for symbol              
if @groupfield = 'Symbol'                
 Begin               
insert into #FinalData(GroupingLevel)              
select distinct entity as GroupingLevel from #T_entity              
              
---------PNL              
Update #FinalData Set PNL = isnull(TempPNL1.PNL,0)              
From  #FinalData FData               
inner join              
(              
select symbol,sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) as pnl from #tempT_MW_GenericPNL               
  group by symbol              
) TempPNL1              
on TempPNL1.symbol = FData.GroupingLevel              
              
--------- BMVsecurities column              
Update #FinalData Set BMVsecurities = isnull(TempBMVsecurities.BMVsecurities,0)              
From  #FinalData FData               
inner join              
(              
  select symbol,sum(beginningmarketvaluebase) as BMVsecurities from #tempT_MW_GenericPNL               
  where DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay                
  group by symbol                
) TempBMVsecurities              
on TempBMVsecurities.symbol = FData.GroupingLevel              
              
--------- BMVCash column              
update #FinalData Set BMVcash = 0              
              
---------cashflow columns              
update #FinalData Set BMVcashflow = isnull(TempBMVcashflow1.BMVcashflow,0)              
From #FinalData FData               
inner join              
(              
select symbol,ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 1)/@D)*(netamountbase-dividend)),0) as BMVcashflow from T_MW_Transactions               
  where rundate between @fromdate and @toDate and open_closetag <> 'C'              
group by symbol              
)TempBMVcashflow1              
on TempBMVcashflow1.symbol = FData.GroupingLevel              
              
update #FinalData Set BMVcashflow = isnull(FData.BMVcashflow +isnull(TempBMVcashflow2.BMVcashflow,0),0)              
From #FinalData FData               
inner join              
(              
select symbol,ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 0)/@D)*(netamountbase-dividend)),0) as BMVcashflow from T_MW_Transactions               
  where rundate between @fromdate and @toDate and open_closetag = 'C'                
  group by symbol              
)TempBMVcashflow2              
on TempBMVcashflow2.symbol = FData.GroupingLevel              
              
              
select GroupingLevel ,PNL ,BMVsecurities,BMVcash,BMVcashflow,isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0) as sum,               
case               
 when (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0)) <= -1               
 then -.999               
 else (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0))              
end as result               
from #FinalData              
              
END               
              
              
--for underlying symbol              
if @groupfield = 'UnderlyingSymbol'                
 Begin               
insert into #FinalData(GroupingLevel)              
select distinct entity as GroupingLevel from #T_entity              
              
---------PNL              
Update #FinalData Set PNL = isnull(TempPNL1.PNL,0)              
From  #FinalData FData               
inner join              
(              
  select UnderlyingSymbol,sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) as pnl from #tempT_MW_GenericPNL               
  group by UnderlyingSymbol              
) TempPNL1              
on TempPNL1.UnderlyingSymbol = FData.GroupingLevel              
              
--------- BMVsecurities column              
Update #FinalData Set BMVsecurities = isnull(TempBMVsecurities.BMVsecurities,0)              
From  #FinalData FData           
inner join              
(              
  select UnderlyingSymbol,sum(beginningmarketvaluebase) as BMVsecurities from #tempT_MW_GenericPNL               
  where DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay              
  group by UnderlyingSymbol              
) TempBMVsecurities              
on TempBMVsecurities.UnderlyingSymbol = FData.GroupingLevel              
              
--------- BMVCash column              
update #FinalData Set BMVcash = 0              
              
---------cashflow columns              
update #FinalData Set BMVcashflow = isnull(TempBMVcashflow1.BMVcashflow,0)              
From #FinalData FData               
inner join              
(              
select UnderlyingSymbol,ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 1)/@D)*(netamountbase-dividend)),0) as BMVcashflow from T_MW_Transactions               
  where rundate between @fromdate and @toDate and open_closetag <> 'C'              
  group by UnderlyingSymbol              
)TempBMVcashflow1              
on TempBMVcashflow1.UnderlyingSymbol = FData.GroupingLevel              
              
update #FinalData Set BMVcashflow = isnull(FData.BMVcashflow +isnull(TempBMVcashflow2.BMVcashflow,0),0)              
From #FinalData FData               
inner join              
(              
select UnderlyingSymbol,ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 0)/@D)*(netamountbase-dividend)),0) as BMVcashflow from T_MW_Transactions               
  where rundate between @fromdate and @toDate and open_closetag = 'C'                
  group by UnderlyingSymbol              
)TempBMVcashflow2              
on TempBMVcashflow2.UnderlyingSymbol = FData.GroupingLevel              
              
select GroupingLevel ,PNL ,BMVsecurities,BMVcash,BMVcashflow,isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0) as sum,               
case               
 when (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0)) <= -1               
 then -.999               
 else (isnull(PNL,0) /NULLIF( abs(isnull(BMVsecurities,0)+ isnull(BMVcash,0)+isnull(BMVcashflow,0)),0))              
end as result               
from #FinalData              
              
END              
              
              
              
              
              
return              
end              
              
/*              
--------------symbol              
exec dbo.F_MW_GetMDReturnNOF_TESTing               
@fromDate = '1/1/2015',               
@toDate = '12/31/2016',              
@funds = 'T4E001012,T8A001009,T8A001017,T8A001025,T8A001033,T8A001058,T8A001066,T8A001074,T8A001090,T8A001108,T8A001116,T8A001124,T8A001132,T8A001157,T8A001165,T8A001173,T8A001181,T8A001199,T8A001207,T8A001215,T8A001223,T8A001231,T8A001249,T8A002007,T8A0
   
    
     
0        
          
            
2015,T8A002023,T8A002031,T8A002049,T8A002056,T8A002072,T8A002080,T8A002098,T8A002106,T8A002114,T8A002122,T8A002148,T8A002163,T8A002189,T8A002197,T8A002213,T8A002239,T8A002247' ,              
@groupfield = 'Symbol',              
@entity = '6461355L3,912810DW5,25476FDD7,886100QQ2,SBUX,167615EF6,92817SXJ2,889855US9,USD,MCD,ACN,BA,54423TAT9,49151EU68,LLY,SYX,97705LXD4,MON,AAPL,ECL,798781QX1,YUM,NEE,874441BC1,WGNR,TGT,ABBV,PLT,PG,AMZN,69661KCG3,495289PA2,VFC,DDC,SK3-DUB,LNT,ZTS,MSFT,
  
    
      
        
          
            
WMT,F,SCHW,658545FJ2,19668QGU3,686507FL3,ADP,366119XA8,NOVC,SINGF,MA,FM-TC,01882N118,AET,662835QQ3,61746SBR9,WSM,60636WMN2,NRT,WPSGX,HXL,663821VA1,7832436S2,CDK,561852EQ3,UTX,04048RKF2,MDT,PX,JNJ,UNM,93974CMK3,INTC,CYT,469485HH2,976834FQ9,5946142Z1,PCP,VR
  
    
      
        
          
            
SK,STT,KND,678514AM2,49118NDL6,ST,ANSS,AZO,684517QC2,CHK,ADBE,APH,GOOGL,AEP,EL,698148AJ2,SSE,207758LY7,969073MA0,GOOG,PAYX,CMCSA,FCX,736742VR5,468714DM9,812631FB0,GE,582188KK3,GILD,8987963J4,PCLN,165167BU0,SWMHF,010609AS4,190813MZ5,SK3-DUB SWAP,RL,COST,WU
  
    
      
        
          
            
,969073LU7'               
              
*/           
/*              
--------------UnderlyingSymbol              
exec dbo.F_MW_GetMDReturnNOF_TESTing               
@fromDate = '1/1/2015',               
@toDate = '12/31/2016',              
@funds = 'T4E001012,T8A001009,T8A001017,T8A001025,T8A001033,T8A001058,T8A001066,T8A001074,T8A001090,T8A001108,T8A001116,T8A001124,T8A001132,T8A001157,T8A001165,T8A001173,T8A001181,T8A001199,T8A001207,T8A001215,T8A001223,T8A001231,T8A001249,T8A002007,T8A0
0  
    
      
        
          
            
2015,T8A002023,T8A002031,T8A002049,T8A002056,T8A002072,T8A002080,T8A002098,T8A002106,T8A002114,T8A002122,T8A002148,T8A002163,T8A002189,T8A002197,T8A002213,T8A002239,T8A002247' ,              
@groupfield = 'UnderlyingSymbol',              
@entity = '49151EU68,8987963J4,F,798781QX1,60636WMN2,WGNR,912810DW5,CMCSA,ACN,YUM,165167BU0,ABBV,DDC,969073LU7,MON,969073MA0,662835QQ3,BA,698148AJ2,PLT,COST,NEE,561852EQ3,UTX,ST,ADBE,CHK,WPSGX,54423TAT9,UNM,889855US9,874441BC1,812631FB0,886100QQ2,93974CMK
  
    
      
        
          
            
3,CDK,ECL,NRT,JNJ,207758LY7,MCD,AMZN,PCP,GILD,190813MZ5,AET,976834FQ9,PG,WU,04048RKF2,01882N118,USD,25476FDD7,LNT,49118NDL6,CYT,SBUX,MDT,61746SBR9,69661KCG3,STT,468714DM9,WSM,686507FL3'              
              
*/              
/*              
--------------Fund              
exec dbo.F_MW_GetMDReturnNOF_TESTing               
@fromDate = '1/1/2015',               
@toDate = '12/31/2016',              
@funds = 'T4E001012,T8A001009,T8A001017,T8A001025,T8A001033,T8A001058,T8A001066,T8A001074,T8A001090,T8A001108,T8A001116,T8A001124,T8A001132,T8A001157,T8A001165,T8A001173,T8A001181,T8A001199,T8A001207,T8A001215,T8A001223,T8A001231,T8A001249,T8A002007,T8A00
  
    
      
        
          
            
2015,T8A002023,T8A002031,T8A002049,T8A002056,T8A002072,T8A002080,T8A002098,T8A002106,T8A002114,T8A002122,T8A002148,T8A002163,T8A002189,T8A002197,T8A002213,T8A002239,T8A002247' ,              
@groupfield = 'Fund',              
@entity = 'T4E001012,T8A001009,T8A001017,T8A001025,T8A001033,T8A001058,T8A001066,T8A001074,T8A001090,T8A001108,T8A001116,T8A001124,T8A001132,T8A001157,T8A001165,T8A001173,T8A001181,T8A001199,T8A001207,T8A001215,T8A001223,T8A001231,T8A001249,T8A002007,T8A0
  
    
      
        
                
02015,T8A002023,T8A002031,T8A002049,T8A002056,T8A002072,T8A002080,T8A002098,T8A002106,T8A002114,T8A002122,T8A002148,T8A002163,T8A002189,T8A002197,T8A002213,T8A002239,T8A002247'               
              
*/              
              
              
              
              
              
--exec dbo.F_MW_GetMDReturnNOF_TESTing @fromDate = '1/1/2015', @toDate = '12/31/2016',@funds = 'T4E001012,T8A001009,T8A001017,T8A001025,T8A001033,T8A001058,T8A001066,T8A001074,T8A001090,T8A001108,T8A001116,T8A001124,T8A001132,T8A001157,T8A001165,T8A001173
  
    
      
        
          
            
--,T8A001181,T8A001199,T8A001207,T8A001215,T8A001223,T8A001231,T8A001249,T8A002007,T8A002015,T8A002023,T8A002031,T8A002049,T8A002056,T8A002072,T8A002080,T8A002098,T8A002106,T8A002114,T8A002122,T8A002148,T8A002163,T8A002189,T8A002197,T8A002213,T8A002239,T8
  
    
      
        
          
--A0            
--02247' ,@groupfield = 'Fund',@entity = 'T4E001012,T8A001009,T8A001017,T8A001025,T8A001033,T8A001058,T8A001066,T8A001074,T8A001090,T8A001108,T8A001116,T8A001124,T8A001132,T8A001157,T8A001165,T8A001173,T8A001181,T8A001199,T8A001207,T8A001215,T8A001223,T8A
  
    
      
        
          
--1231,T8A001249,T8A002007,T8A002015,T8A002023,T8A002031,T8A002049,T8A002056,T8A002072,T8A002080,T8A002098,T8A002106,T8A002114,T8A002122,T8A002148,T8A002163,T8A002189,T8A002197,T8A002213,T8A002239,T8A002247' 