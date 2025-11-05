/*********************************************      
Create Date: 08-October-2015      
Created By: Pankaj Sharma      
Decsription:  Get Currency Pairs filters in the report    
exec P_GetCurrencyPairs  
*********************************************/    
  
CREATE Proc P_GetCurrencyPairs  
As    
Select     
FromCurr.CurrencySymbol As FromCurrency,    
ToCurr.CurrencySymbol As ToCurrency,    
FromCurr.CurrencySymbol + '-' + ToCurr.CurrencySymbol As CurrencyPair,    
SP.FromCurrencyID,    
SP.ToCurrencyID,    
SP.CurrencyPairID     
From T_CurrencyStandardPairs SP    
Inner Join T_Currency FromCurr On FromCurr.CurrencyID = SP.FromCurrencyID    
Inner Join T_Currency ToCurr On ToCurr.CurrencyID = SP.ToCurrencyID    
Order by CurrencyPair