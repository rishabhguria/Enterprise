

      
/*************************************    
Author: Ankit    
Date: 03-10-2014    
Description: To get Dividend report on the basis of Ex date or Payout Date
Execution Method:    
P_GetDividendReports_Proc '2014/11/01','2014/11/11','1213,1214,1233,1234,1235,1236,1237',0
    
**************************************/    


create  Procedure [dbo].[P_GetDividendReports]    
(    
 @Startdate datetime,    
 @EndDate datetime,
 @Funds varchar(max), 
 @IsPayoutDate Bit   
)    
As    
    
/*    
Declare @Startdate datetime    
Declare @EndDate datetime    
    

SET @Startdate = '2012-10-10'    
SET @EndDate = '2013-10-10' 
SET @Funds   = '1195'
SET @IsPayoutDate = 0
 
*/    
    
SET NOCOUNT ON    
    
Select * Into #Funds                                  
from dbo.Split(@Funds, ',')    
    
declare @MinDividentExDate datetime

set @MinDividentExDate = (
SELECT MIN(ExDate) FROM T_CashTransactions
where 
DATEDIFF(d,@Startdate,PayoutDate)>=0 
AND
DATEDIFF(d,PayoutDate,@EndDate)>=0
) 

Set 
@MinDividentExDate = 
Case When (datediff(d,@MinDividentExDate,@Startdate)>0)
Then @MinDividentExDate
Else @Startdate
End

--Select @MinDividentExDate
    
Create Table #FXConversionRates                                                                                     
(                                                                                                                                                                                                              
 FromCurrencyID int,                                                                                                                                                                                    
 ToCurrencyID int,                                                                                        
 RateValue float,                                                                                                        
 ConversionMethod int,                                                                        
 Date DateTime,                                                               
 eSignalSymbol varchar(max)                                                                  
)     
    
    
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @MinDividentExDate,@EndDate                                                                          
                                                                             
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

CASE 
When @IsPayoutDate = 1
Then   
	Case     
		when ((TCD.Amount>=0 and (TCD.Description is NULL or TCD.Description ='')) or TCD.Description='DIV RECEIVED' ) and     
		  (datediff(d,@Startdate,PayoutDate)>=0 and  datediff(d,PayoutDate,@EndDate)>=0 )    
		then 'Dividends Received'    
		when ((TCD.Amount<0 and (TCD.Description is NULL or TCD.Description ='')) or TCD.Description='DIV CHARGED') and     
		  (datediff(d,@Startdate,PayoutDate)>=0 and  datediff(d,PayoutDate,@EndDate)>=0 )    
		then 'Dividends Charged' 
		Else TCD.Description    
	End 
Else
	Case       
		when ((TCD.Amount>=0 and (TCD.Description is NULL or TCD.Description ='')) or TCD.Description='DIV RECEIVED' ) and       
		  (datediff(d,@Startdate,ExDate)>=0 and  datediff(d,Exdate,@EndDate)>=0 )      
		then 'Dividends Received'      
		when ((TCD.Amount<0 and (TCD.Description is NULL or TCD.Description ='')) or TCD.Description='DIV CHARGED') and       
		  (datediff(d,@Startdate,ExDate)>=0 and  datediff(d,Exdate,@EndDate)>=0 )      
		then 'Dividends Charged'      
		when (TCD.Description='TAX WITHHELD') and (datediff(d,@Startdate,PayoutDate)>=0 and  datediff(d,PayoutDate,@EndDate)>=0 )      
		then  'Tax Withheld'    
	Else TCD.Description 
	End
END as Activity,    
    
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
on 
(
FXDayRatesForExDate.FromCurrencyID = TCD.CurrencyID 
And FXDayRatesForExDate.ToCurrencyID = @BaseCurrencyID    
And datediff(d,FXDayRatesForExDate.Date,TCD.ExDate)=0 
)    
    
Where    
datediff(d,@Startdate,
CASE 
When @IsPayoutDate = 1
Then PayoutDate   
Else ExDate
End ) >=0

And 
datediff(d,
CASE 
When @IsPayoutDate = 1
Then PayoutDate   
Else ExDate
End ,@EndDate)>=0

and (FundID in (Select * from #Funds ))    

    
Update #Dividend set NetAmountBase= (NetAmountLocal * FXRate)    
    
Select * from #Dividend     
    
drop table #Dividend,#FXConversionRates,#Funds    

