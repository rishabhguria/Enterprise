                  
                  
/*************************************************                                
Author : Ankit                                  
Creation Date : 21st February , 2013                                  
Description : Script for Change in Equity part of Portfolio Summary Middleware Report                  
                  
----Ashish 20121217: We can remove all TRA type transactions like Commission, FX Gain etc. As these are included already in the Realized Gain/Loss Section                  
                  
---- Ashish 20121217: Remove all accounts except Income and expenses from the journal entry list                  
  
Modified By: Sandeep  
Date: 03 SEPT 2015  
DESC:   
http://jira.nirvanasolutions.com:8080/browse/MAPLEROCK-225  
PSR PNL Issue         
---- Only month start date, not business adjusted day    
---- becuase for bonds, accruals calculated on weekends also             
                  
Execution Statement:                               
  P_MW_ChangeInEquity_PSR '2014-2-5' ,'BP Master Fund:002465110' , '2014-1-1'                             
*************************************************/                   
                  
CREATE Procedure [dbo].[P_MW_ProfitLoss_PSR]                  
(                  
@EndDate datetime,                  
@fund varchar(max),                  
@ITD datetime                  
)                  
As                
                  
--Declare @EndDate datetime                  
--Declare @fund varchar(max)                
--Declare @ITD datetime                
--Set @EndDate = '01-08-2015'                 
--Set @fund = 'Maple Rock MF: BMO,Maple Rock MF: GS,Maple Rock MF: GS Custody,Maple Rock MF: UBS,Maple Rock OS: BMO,Maple Rock OS: GS,Maple Rock OS: UBS,Maple Rock US: BMO,Maple Rock US: GS,Maple Rock US: UBS'                
--Set @ITD='2012-12-31 00:00:00:000'                
                
Select * Into #Funds                                                      
from dbo.Split(@fund, ',')                  
                  
Select @ITD= dateadd(day,1,@ITD)                          
                   
                  
declare @MTDFromdate datetime,@QTDFromdate datetime,@YTDFromdate datetime                   
         
---- Only month start date, not business adjusted day    
---- becuase for bonds, accruals calculated on weekends also             
Select @MTDFromdate= DATEADD(mm, DATEDIFF(m,0,@EndDate),0)--dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                  
Select @QTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,5)                  
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)                  
                  
select @EndDate= case When datediff(d,@EndDate,@ITD)>=0 then @ITD Else @EndDate end                    
select @MTDFromdate= case When datediff(d,@MTDFromdate,@ITD)>=0 then @ITD Else @MTDFromdate end                      
select @QTDFromdate= case When datediff(d,@QTDFromdate,@ITD)>=0 then @ITD Else @QTDFromdate end                      
select @YTDFromdate= case When datediff(d,@YTDFromdate,@ITD)>=0 then @ITD Else @YTDFromdate end                      
                  
                  
                  
--print @QTDFromdate                  
                  
DECLARE @j table                  
(                  
 SubCategoryID int,                  
 SubCategoryName varchar(max),                  
 Name varchar(max),                  
 TransactionTypeID int,                  
 FundID int,                  
 SubAccountID int,                  
 CurrencyID int,                  
 TransactionDate datetime,                  
 CurrentDR float,                  
 CurrentCR float                  
)                  
                  
--Need to add master category as well in the below table                  
insert into @j                  
SELECT                   
a. subcategoryID,                   
a.SubCategoryName ,                  
b.Name,                  
b.TransactionTypeID,                  
c.FundID,                  
c.SubAccountID,              
c.CurrencyID,                  
c.TransactionDate,                  
c.DR,                  
c.CR                  
from                   
T_SubCategory a                  
inner join T_SubAccounts b on b.acronym<>'cash'                    
  and a.SubCategoryID=b.SubCategoryID                  
                    
inner join T_journal c on b.SubaccountID=c.SubaccountID                   
  and FundID in                   
   (SELECT CompanyFundID from t_companyfunds                   
   WHERE FundName in( Select * from #Funds))                  
  and ((DATEDIFF(d,@YTDFromdate,c.TransactionDate)>=0) AND DATEDIFF(d,c.TransactionDate,@EndDate)>=0)             
  and b.Acronym <> 'TAX'                 
                    
                  
Where (MasterCategoryID in ('3','4','5') and TransactionTypeID<>3)                  
or (TransactionTypeID=3 and (TransactionSource in (2,6)))                  
                  
                  
----SELECT * from @j                  
-------------------------------FX Convension-------------------------------------------------                  
DECLARE  @FXConversionRates Table                                         
(                                                                                                                                         
 FromCurrencyID int,                           
 ToCurrencyID int,                                                     
 RateValue float,                                   
 ConversionMethod int,                                                                                           
 Date DateTime,                  
 eSignalSymbol varchar(max)                  
                                                                   
)                    
                  
Insert into @FXConversionRates                          
Exec P_GetAllFXConversionRatesForGivenDateRange @YTDFromdate,@EndDate                                                                                                                                                  
--  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@MinTradeDate,@EndDate) as A                                                                                                                                                 
                                                                          
 UPDATE @FXConversionRates                                                                                             
 Set RateValue = 1.0/RateValue                                                                                                                     
 Where RateValue <> 0 and ConversionMethod = 1                                                                                                                                                  
                                                                                                       
UPDATE @FXConversionRates                                                                                                                                               
 Set RateValue = 0                                                                          
 Where RateValue is Null                        
                  
--SELECT * from @FXConversionRates WHERE conversionMethod ='1'order by date                  
------------------------------------------------------------------------------------------------------------                  
                  
                  
------------------------------------------------------------------------------------------------------------                  
                  
Select *                   
into #JournalPNL                  
from t_journal                  
where                  
datediff(d,@YTDFromdate,TransactionDate)>=0                  
and                  
datediff(d,TransactionDate,@EndDate)>=0       
and                                        
FundID in         
  (SELECT CompanyFundID from t_companyfunds                  
   WHERE FundName in( Select * from #Funds))                  
and                  
        
(        
 (        
  subAccountID IN (17) And                            
  (        
   (CharIndex('Spot',PBDesc) <> 0 and taxlotid='')                     
   or                     
   (transactionsource In (7) and  CharIndex('Spot',symbol) <> 0 )                    
  )         
 )        
        
Or        
 (        
  subAccountID IN (17,135) And  taxlotid <> ''                     
   And transactionsource In (9) --And PBDESC <> 'Cash to Unrealized FXRate PNL Entry'                
  )        
)                    
                 
--and                  
--subAccountID = 17                  
--and                                    
--FundID in                         
--   (SELECT CompanyFundID from t_companyfunds                         
--   WHERE FundName in( Select * from #Funds))                        
--and                        
--((CharIndex('Spot',PBDesc) <> 0 and taxlotid='')                 
--or                 
--transactionsource=7 and  CharIndex('Spot',symbol) <> 0                 
--)                  
--PBDesc <> 'Spot'                  
                  
Alter table #JournalPNL                  
add Rate float                  
                  
Alter table #JournalPNL                  
add PNL float                  
                  
UPdate #JournalPNL                  
Set                  
Rate = RateValue                  
from @FXConversionRates                   
WHERE                   
ToCurrencyID = '1'                   
and DateDiff(Day,Date,TransactionDate) = 0                   
and CurrencyID = FromCurrencyID                  
                  
Update #JournalPNL                  
Set                   
PNL = (Rate-FXRATE)*(DR-CR)                  
Where                   
(FXRATE is not NULL and FXRATE <> 0)                
                
                 
------------------------------------------------------------------------------------------------------------                  
                  
UPDATE @j                   
SET CurrentDR = CurrentDR * RateValue,                   
CurrentCR = CurrentCR * RateValue                   
from @FXConversionRates                   
WHERE ToCurrencyID = '1' and Date=TransactionDate and CurrencyID = FromCurrencyID                  
                  
UPDATE @j SET CurrentDR=-CurrentDR                   
                  
--SELECT * from @j                  
                  
DECLARE @EquityItems table                  
(                  
 Items varchar(max),                  
 Daily float,                  
 Monthly float,                  
 Quarterly float,                  
 Yearly float                  
)                  
                  
--Ashish [20130131] Currently this will have an impact on Englander as all subCatergories                   
--will show up whether they contain data or not                  
INSERT into @EquityItems(Items)                   
SELECT subCategoryName  from T_SubCategory Sub                  
inner join T_MasterCategory MC on Sub.MasterCategoryID = MC.MasterCategoryID                  
--Removing Asset and Liabilities.                  
WHERE Sub.SubCategoryName <> 'Cash'                   
and MC.MasterCategoryID not in (1,2)                   
                  
Insert into @EquityItems (Items)  values ('Realized Unrealized Gain/(Loss) on Securities')                  
Insert into @EquityItems (Items)  values ('Unrealized Gain On Cash')                  
Insert into @EquityItems (Items)  values ('Unrealized Gain On Cash Transaction')                  
--create expense subCatergories to group Income Expense accounts.                
Insert into @EquityItems (Items)  values ('Interest Income/Expense')                  
Insert into @EquityItems (Items)  values ('Gross Dividend Income/Expense')                  
Insert into @EquityItems (Items)  values ('Misc Income/Expense')                  
                  
                  
---------------------------------------------------------------------------------------------------------------                  
                  
Select                   
Rundate,                  
Asset,                  
Open_CloseTag,                  
sum(TotalRealizedPNLOnCost)+Sum(ChangeInUnrealizedPNL) as PNL1,                  
sum(UnrealizedTotalGainOnCostD2_Base) as UnrealizedTotalGainOnCostD2_Base,                  
sum(Dividend) as Dividend                  
into #PNL                   
from T_MW_GenericPNL                   
WHERE                   
datediff(d,@YTDFromdate,runDate)>=0                   
and datediff(d,runDate,@EndDate)>=0                   
and Fund in (Select * from #Funds)                  
group by Rundate,Asset,Open_CloseTag                  
                  
                  
------------------------------------------------Daily----------------------------------------------------------                  
UPDATE @EquityItems                   
SET Daily =(SELECT sum( CurrentDR+CurrentCR) from @j                   
             WHERE datediff(d,TransactionDate,@EndDate)=0 and SubCategoryName=Items)                  
                  
--Get Realized + Unreazlied gain loss from Generic Pnl table                  
UPDATE @EquityItems                   
SET Daily = isnull((SELECT sum(PNL1) from #PNL                  
                  WHERE datediff(d,rundate,@EndDate)=0 and asset <> 'cash'),0)                  
               WHERE Items = 'Realized Unrealized Gain/(Loss) on Securities'                  
                  
-- Get unrealized gain on cash from Generic PNL table                  
UPDATE @EquityItems                   
SET Daily = isnull((SELECT sum(UnrealizedTotalGainOnCostD2_Base) from #PNL                  
                  WHERE datediff(d,rundate,@EndDate)=0 and asset = 'cash'),0)                  
                   WHERE Items = 'Unrealized Gain On Cash'                  
                  
--Group Interest Income and expesens into one heading                  
UPDATE @EquityItems                   
SET Daily = (SELECT sum(CurrentDR + CurrentCR)                   
    from @j WHERE datediff(d,TransactionDate,@EndDate)=0 and                   
            (SubCategoryName='Interest Expense' or SubCategoryName='Interest Income'))                  
    WHERE Items = 'Interest Income/Expense'                  
                  
--Group Interest Income and Expenses into one heading                  
UPDATE @EquityItems                   
SET Daily = (SELECT sum(CurrentDR + CurrentCR)                   
   from @j WHERE datediff(d,TransactionDate,@EndDate)=0 and                   
            (SubCategoryName='Misc Expense' or SubCategoryName='Misc Income'))                  
       WHERE Items = 'Misc Income/Expense'                  
                  
--Get dividend from Generic PNL Table                  
UPDATE @EquityItems                   
SET Daily = (SELECT sum(Dividend)                   
    from #PNL WHERE datediff(d,rundate,@EndDate)=0 and Open_CloseTag = 'D')                  
       WHERE Items = 'Gross Dividend Income/Expense'                  
                  
--Get Unrealized Gain on Cash Transactions                  
UPDATE @EquityItems                   
SET Daily = (SELECT sum(PNL)                   
    from #JournalPNL WHERE datediff(d,TransactionDate,@EndDate)=0)                  
    WHERE Items = 'Unrealized Gain On Cash Transaction'                  
                
                 
-------------------------------------------------Monthly-------------------------------------------------------                  
                  
UPDATE @EquityItems                   
SET Monthly = (SELECT sum( CurrentDR+CurrentCR) from @j                   
             WHERE datediff(d,@MTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0                   
             and SubCategoryName=Items)                  
                  
UPDATE @EquityItems                   
SET Monthly = isnull((SELECT sum(PNL1) from #PNL                  
       WHERE datediff(d,@MTDFromdate,rundate)>=0 and datediff(d,rundate,@EndDate)>=0 and asset <> 'cash'),0)                  
              WHERE Items='Realized Unrealized Gain/(Loss) on Securities'                  
                  
UPDATE @EquityItems                   
SET Monthly = isnull((SELECT sum(UnrealizedTotalGainOnCostD2_Base) from #PNL                  
              WHERE datediff(d,@MTDFromdate,rundate)>=0 and datediff(d,rundate,@EndDate)>=0 and asset = 'cash'),0)                  
              WHERE Items='Unrealized Gain On Cash'                  
                  
                  
UPDATE @EquityItems                   
SET Monthly = (SELECT sum(CurrentDR + CurrentCR)                 
      from @j WHERE (datediff(d,@MTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0) and                   
     (SubCategoryName='Interest Expense' or SubCategoryName='Interest Income'))                  
      WHERE Items='Interest Income/Expense'                  
                  
UPDATE @EquityItems                   
SET Monthly = (SELECT sum(CurrentDR + CurrentCR)                   
      from @j WHERE (datediff(d,@MTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0) and                   
     (SubCategoryName='Misc Expense' or SubCategoryName='Misc Income'))                  
      WHERE Items='Misc Income/Expense'                  
                  
UPDATE @EquityItems                   
SET Monthly = (SELECT sum(Dividend)                   
    from #PNL WHERE datediff(d,@MTDFromdate,rundate)>=0 and datediff(d,rundate,@EndDate)>=0 and Open_CloseTag = 'D')                  
       WHERE Items = 'Gross Dividend Income/Expense'                  
                  
--Get Unrealized Gain on Cash Transactions                  
UPDATE @EquityItems                   
SET Monthly = (SELECT sum(PNL)                   
    from #JournalPNL WHERE (datediff(d,@MTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0))                  
    WHERE Items = 'Unrealized Gain On Cash Transaction'                  
                  
-------------------------------------------------Quarterly----------------------------------------------------                  
                  
                  
UPDATE @EquityItems                   
SET Quarterly = (SELECT sum( CurrentDR+CurrentCR) from @j                   
                 WHERE datediff(d,@QTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0                  
                 and SubCategoryName=Items)                  
                  
UPDATE @EquityItems                   
SET Quarterly = isnull((SELECT sum(PNL1) from #PNL                  
                WHERE (datediff(d,@QTDFromdate,rundate)>=0 and datediff(d,rundate,@EndDate)>=0) and asset <> 'cash'),0)                  
                WHERE Items='Realized Unrealized Gain/(Loss) on Securities'                  
                  
UPDATE @EquityItems                   
SET Quarterly = isnull((SELECT sum(UnrealizedTotalGainOnCostD2_Base) from #PNL                  
                WHERE (datediff(d,@QTDFromdate,rundate)>=0 and datediff(d,rundate,@EndDate)>=0) and asset = 'cash'),0)                  
                WHERE Items='Unrealized Gain On Cash'                  
                  
                  
UPDATE @EquityItems                   
SET Quarterly = (SELECT sum(CurrentDR + CurrentCR)                   
        from @j WHERE (datediff(d,@QTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0) and                   
    (SubCategoryName='Interest Expense' or SubCategoryName='Interest Income'))                  
        WHERE Items='Interest Income/Expense'                  
                  
UPDATE @EquityItems                   
SET Quarterly = (SELECT sum(CurrentDR + CurrentCR)                   
     from @j WHERE (datediff(d,@QTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0) and                   
    (SubCategoryName='Misc Expense' or SubCategoryName='Misc Income'))                  
        WHERE Items='Misc Income/Expense'                  
                  
                  
UPDATE @EquityItems                   
SET Quarterly = (SELECT sum(Dividend)                   
    from #PNL WHERE (datediff(d,@QTDFromdate,rundate)>=0 and datediff(d,rundate,@EndDate)>=0) and Open_CloseTag = 'D')                  
       WHERE Items = 'Gross Dividend Income/Expense'                  
                  
--Get Unrealized Gain on Cash Transactions                  
UPDATE @EquityItems                   
SET Quarterly = (SELECT sum(PNL)                   
    from #JournalPNL WHERE (datediff(d,@QTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0))                
    WHERE Items = 'Unrealized Gain On Cash Transaction'                  
                  
------------------------------------------------Yearly----------------------------------------------------------                  
UPDATE @EquityItems                   
SET Yearly = (SELECT sum( CurrentDR+CurrentCR) from @j                   
             WHERE datediff(d,@YTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0                  
             and SubCategoryName=Items)                  
                  
                  
UPDATE @EquityItems                   
SET Yearly = isnull((SELECT sum(PNL1) from #PNL                  
             WHERE datediff(d,@YTDFromdate,runDate)>=0 and datediff(d,runDate,@EndDate)>=0  and asset <> 'cash'),0)                  
             WHERE Items='Realized Unrealized Gain/(Loss) on Securities'                  
                  
UPDATE @EquityItems                   
SET Yearly=                   
isnull((SELECT sum(UnrealizedTotalGainOnCostD2_Base) from #PNL                  
                  WHERE datediff(d,@YTDFromdate,runDate)>=0 and datediff(d,runDate,@EndDate)>=0 and asset = 'cash'),0)                  
                   WHERE Items='Unrealized Gain On Cash'                  
                  
UPDATE @EquityItems                   
SET Yearly = (SELECT sum(CurrentDR + CurrentCR)                   
     from @j WHERE (datediff(d,@YTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0) and                   
     (SubCategoryName='Interest Expense' or SubCategoryName='Interest Income'))                  
     WHERE Items='Interest Income/Expense'                  
                  
UPDATE @EquityItems                   
SET Yearly = (SELECT sum(CurrentDR + CurrentCR)                   
        from @j WHERE (datediff(d,@YTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0) and                   
    (SubCategoryName='Misc Expense' or SubCategoryName='Misc Income'))                  
        WHERE Items='Misc Income/Expense'                  
                  
UPDATE @EquityItems                   
SET Yearly = (SELECT sum(Dividend)                   
    from #PNL WHERE datediff(d,@YTDFromdate,runDate)>=0 and datediff(d,runDate,@EndDate)>=0 and Open_CloseTag = 'D')                  
       WHERE Items = 'Gross Dividend Income/Expense'                  
                  
--Get Unrealized Gain on Cash Transactions                  
UPDATE @EquityItems                   
SET Yearly = (SELECT sum(PNL)                   
    from #JournalPNL WHERE (datediff(d,@YTDFromdate,TransactionDate)>=0 and datediff(d,TransactionDate,@EndDate)>=0))                  
    WHERE Items = 'Unrealized Gain On Cash Transaction'                 
                
                 
-----------------------------------------------------------------------------------------------------                  
                  
DELETE FROM @EquityItems WHERE items in ('Interest Income','Interest Expense','Misc Income','Misc Expense','Cash Contribution')                  
                  
-----------------------------------------------------------------------------------------------------                  
                  
SELECT                   items,                   
isnull(Daily,0) as 'Daily',                   
isnull(monthly,0)as 'Monthly',                   
isnull(Quarterly,0)as'Quarterly',                   
isnull(yearly,0) as 'Yearly'                   
from @EquityItems                   
order by Items                  
                  
Drop table #Funds ,#PNL,#JournalPNL 