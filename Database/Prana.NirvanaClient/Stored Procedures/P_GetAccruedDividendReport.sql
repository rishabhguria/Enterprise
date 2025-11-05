
CREATE Procedure [dbo].[P_GetAccruedDividendReport]        
(      
 @EndDate datetime,        
 @Funds varchar(max)        
)        
As        
        
/*        
Declare @Startdate datetime        
Declare @EndDate datetime        
        
set @Startdate = '2012-9-1'        
Set @EndDate = '2013-10-1'        
*/        
        
SET NOCOUNT ON        
        
Select * Into #Funds                                      
from dbo.Split(@Funds, ',')        
        
        
Create Table #FXConversionRates                                                                                         
(                                                                                                                                                                                                                  
 FromCurrencyID int,                                                                                                                                                                                        
 ToCurrencyID int,                                                                                            
 RateValue float,                                                                                                            
 ConversionMethod int,                                                                            
 Date DateTime,                                                                   
 eSignalSymbol varchar(max)                                                                      
)         
        
        
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @EndDate,@EndDate                                                                              
                                                                                 
 Update #FXConversionRates                                                                                                                                                      
 Set RateValue = 1.0/RateValue                                                                                                                 
 Where RateValue <> 0 and ConversionMethod = 1                                                                            
        
      
--Select * from #FXConversionRates      
      
Declare @BaseCurrencyID int                                                                          
Set @BaseCurrencyID=(select top 1 BaseCurrencyID from T_Company where companyId <> -1)        
        
        
Create table #Dividend        
(        
 ExDate datetime,        
 PayoutDate datetime,        
 Symbol varchar(max), 
 BloombergSymbol Varchar(max),
 ISINSymbol Varchar(max),
 SEDOLSymbol Varchar(max),
 CUSIPSymbol Varchar(max),
 ReutersSymbol Varchar(max),
 UnderLyingSymbol Varchar(max),       
 FundID int,        
 Fund varchar(max),        
 Asset Varchar(max),        
 CurrencyID int,        
 TradeCurrency varchar(max),        
 UDAAssetClass varchar(max),        
 UDASecurityType varchar(max),        
 UDASector varchar(max),         
 UDASubSector varchar(max),        
 UDACountry varchar(max),        
 CompanyName varchar(max),        
 NetAmountLocal float,        
 Description Varchar(max),        
 Activity Varchar(max),        
 FXRate float,        
 NetAmountBase float         
)        
        
Insert into #Dividend        
        
select         
TCD.Exdate,         
TCD.PayoutDate,         
TCD.Symbol,  
BloombergSymbol,
ISINSymbol,
SEDOLSymbol,
CUSIPSymbol,
ReutersSymbol,
UnderLyingSymbol,     
TCD.FundID,         
F.FundName,        
A.AssetName,        
TCD.CurrencyID ,        
C.CurrencySymbol,  
ISNULL(SM.AssetName,'Undefined') As UDAAssetClass ,
ISNULL(SM.SecurityTypeName,'Undefined') As UDASecurityType ,        
ISNULL(SM.SectorName,'Undefined') As UDASector,        
ISNULL(SM.SubsectorName,'Undefined') As UDASubSector,        
ISNULL(SM.CountryName,'Undefined') As UDACountry ,         
ISNULL(SM.CompanyName,'Undefined') As CompanyName,
TCD.Amount,         
ISNULL(TCD.Description,'') as Description,         
Case         
when ((TCD.Amount>=0 and (TCD.Description is NULL or TCD.Description ='')) or TCD.Description='DIV RECEIVED' )   
then 'Dividends Received'      
when ((TCD.Amount<0 and (TCD.Description is NULL or TCD.Description ='')) or TCD.Description='DIV CHARGED')  
then 'Dividends Charged'         
Else   TCD.Description      
End as Activity,        
        
CASE        
WHEN (TCD.CurrencyID <>@BaseCurrencyID)        
THEN FXDayRatesForExDate.RateValue         
ELSE 1        
END as FXRate,        
0        
        
        
from T_CashTransactions TCD
inner JOIN T_ActivityType on (T_ActivityType.ActivityTypeId = TCD.ActivityTypeId and ActivitySource = 2)
inner join T_CompanyFunds F on TCD.FundID=F.CompanyFundID        
inner join T_Currency C on TCD.CurrencyID =C.CurrencyID        
Left outer join V_SecMasterData SM on TCD.Symbol = SM.TickerSymbol        
Left outer join T_Asset A on SM.AssetID = A.AssetID        
        
Left outer join #FXConversionRates FXDayRatesForExDate                                                                                                                                                             
on (FXDayRatesForExDate.FromCurrencyID = TCD.CurrencyID   
And FXDayRatesForExDate.ToCurrencyID = @BaseCurrencyID        
And DateDiff(d,@EndDate,FXDayRatesForExDate.Date)=0 )                  
        
where        
(  
DATEDIFF(d,ExDate,@EndDate)>=0  
AND   
DATEDIFF(D,@EndDate,PayoutDate)>0   
and        
FundID in (Select * from #Funds)   
)        
        
        
Update #Dividend set NetAmountBase= (NetAmountLocal * FXRate)        
        
Select * from #Dividend         
        
drop table #Dividend,#FXConversionRates,#Funds 

