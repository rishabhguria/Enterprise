/*********************************************        
Create Date: 13-October-2015        
Created By: Pankaj Sharma        
Decsription:  Get Currency for filters in the report      
    
exec P_GetCurrenciesFilter  
    
*********************************************/                        
        
CREATE PROCEDURE [dbo].[P_GetCurrenciesFilter]  
        
As        
select  distinct CurrencySymbol,CurrencyID  From T_currency where CurrencyID in(select distinct CurrencyID  From T_journal) Order By CurrencySymbol