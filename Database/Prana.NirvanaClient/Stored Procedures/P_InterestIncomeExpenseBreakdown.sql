/*********************************************              
Create Date: 09-October-2015              
Created By: Pankaj Sharma              
Decsription:  We are giving breakdown of the value available in PSR              
Execution Statement:                                     
            
exec P_InterestIncomeExpenseBreakdown      
@StartDate='2015-07-01 00:00:00:000',            
@EndDate='2015-07-31 00:00:00:000',            
@fund=N'1254,1252,1253,1239,1213,1251,1214,1255,1238,1250,1247,1248,1249,1240,1241,1256',            
@Currency=N'3,6,7,1,4,2,16,14,8',            
@SubCategory=N'Interest Expense,Interest Income',            
@SubAccount=N'12,11'            
*************************************************/              
      
CREATE Procedure [dbo].[P_InterestIncomeExpenseBreakdown]      
(              
@StartDate datetime,                        
@EndDate datetime,              
@fund varchar(max),            
@Currency varchar(max),            
@SubCategory varchar(max),            
@SubAccount varchar(max)            
)                        
As                      
                        
--Declare @StartDate datetime                      
--Declare @EndDate datetime                        
--Declare @fund varchar(max)                      
--Set @StartDate = '03/1/2015'                       
--Set @EndDate = '03/31/2015'                       
--Set @fund = 'Maple Rock MF: BMO,Maple Rock MF: GS,Maple Rock MF: GS Custody,Maple Rock MF: UBS,Maple Rock OS: BMO,Maple Rock OS: GS,Maple Rock OS: UBS,Maple Rock US: BMO,Maple Rock US: GS,Maple Rock US: UBS'              
                      
Select * Into #Funds                                                            
from dbo.Split(@fund, ',')             
            
Select * Into #TempCurrency              
from dbo.Split(@Currency, ',')                       
            
Select * Into #TempSubCategory            
from dbo.Split(@SubCategory, ',')                       
            
Select * Into #TempSubAccount            
from dbo.Split(@SubAccount, ',')                       
            
              
Create table #FinalData      
(                        
 SubCategoryID int,                        
 SubCategoryName varchar(max),                        
 Name varchar(max),                        
 TransactionTypeID int,                        
 FundID int,              
 FundName varchar(50),      
 Symbol varchar(max),              
 SubAccountID int,                        
 CurrencyID int,              
 CurrencyName nvarchar(100),              
 TransactionDate datetime,                        
 CurrentDR float,      
 CurrentCR float      
)      
      
              
insert into #FinalData              
SELECT                         
a. subcategoryID,                         
a.SubCategoryName ,                        
b.Name,            
b.TransactionTypeID,                        
c.FundID,              
CF.FundName,              
c.Symbol,      
c.SubAccountID,                        
c.CurrencyID,                       
Cur.CurrencySymbol,               
c.TransactionDate,                        
c.DR,      
c.CR      
from                         
T_SubCategory a                        
inner join T_SubAccounts b on b.acronym<>'cash'               
        and               
        a.SubCategoryID=b.SubCategoryID            
        and               
        (a.SubCategoryName IN (Select items from #TempSubCategory))            
        and             
        (b.SubAccountID IN (Select items from #TempSubAccount))            
inner join T_journal c on  b.SubaccountID=c.SubaccountID                 
        and               
        ((DATEDIFF(d,@StartDate,c.TransactionDate)>=0) AND DATEDIFF(d,c.TransactionDate,@EndDate)>=0)                   
        and               
        b.Acronym <> 'TAX'               
inner join  #Funds TempFunds on c.FundID =  TempFunds.items              
inner join T_CompanyFunds CF on CF.CompanyFundID = c.FundID              
inner join T_Currency Cur on    Cur.CurrencyID = c.CurrencyID           
INNER JOIN #TempCurrency TC on TC.items = Cur.CurrencyID                
                        
Where               
--MasterCategory in EQUITY, INCOME, EXPENSE and transactionType is not "Daily Calculation"            
--or            
--transactionType is Daily Calculation and transactionSource is  Trading Ticket and CAStockDividend            
 (MasterCategoryID in ('3','4','5') and TransactionTypeID<>3)                        
 or               
 (TransactionTypeID=3 and (TransactionSource in (2,6)))         
              
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
Exec P_GetAllFXConversionRatesForGivenDateRange @StartDate,@EndDate              
              
 UPDATE @FXConversionRates                                                                                                   
 Set RateValue = 1.0/RateValue                                                                                                                           
 Where RateValue <> 0 and ConversionMethod = 1                                                                                                                                                        
                                                                                                             
UPDATE @FXConversionRates                                                                                       
 Set RateValue = 0                                                                                
 Where RateValue is Null                              
------------------------------------------------------------------------------------------------------------                           
Alter table #FinalData      
add Rate float      
      
UPDATE #FinalData      
SET Rate = RateValue      
from @FXConversionRates      
WHERE ToCurrencyID = '1' and Date=TransactionDate and CurrencyID = FromCurrencyID                        
      
UPDATE #FinalData      
Set Rate = 1      
where CurrencyName='USD'      
      
UPDATE #FinalData      
Set Symbol=''      
where Symbol is NULL      
      
    
Select TransactionDate, FundName, CurrencyName, Name,CurrentCR-CurrentDR as Amount,Rate as [FX Rate],(CurrentCR-CurrentDR)*(Rate) as [Base Amount]      
,Symbol        
from #FinalData order by TransactionDate,CurrencyName      
                        
Drop table #Funds,#FinalData,#TempSubAccount,#TempCurrency,#TempSubCategory