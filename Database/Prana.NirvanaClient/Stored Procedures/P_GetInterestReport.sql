
/***********************************  
Author: Ankit 
Date: April 26, 2013  
Desc: Get Interest Report For Lazard
P_GetInterestReport '2013-4-5','2013-4-6',1195          
***********************************/          
          
CREATE Procedure [dbo].[P_GetInterestReport]           
(            
@StartDate datetime,            
@EndDate datetime,  
@AccountID Varchar(max)  
--,     @PranaFundID varchar(max)          
)            
AS            
          
--Declare @Fund Table                                                                              
--(                                                                              
--FundID int                                                                          
--)              
--Insert into @Fund                            
--Select Cast(Items as int) from dbo.Split(@PranaFundID,',')         
  
Declare @Account Table                                                                              
(                                                                              
AccountID int                                                                          
)              
Insert into @Account                            
Select Cast(Items as int) from dbo.Split(@AccountID,',')                
            
Select           
I.Account ,          
I.Date ,          
I.LongShort ,          
I.Balance ,          
I.InterestRate ,          
I.DailyInterest ,          
--PranaFundID as FundID  ,        
'USD' as Currency,        
'Margin' as AccountType        
from           
[T_InterestReport] I          
--Left Join [T_StockLoanAccountMapping] SL on I.Account =SL.StockLoan_Account          
Where DateDiff(d,date,@StartDate)<=0            
And DateDiff(d,Date,@EndDate)>=0            
And I.Account in (select AccountID from @Account)   
--And SL.PranaFundID in (select FundID from @Fund) 

