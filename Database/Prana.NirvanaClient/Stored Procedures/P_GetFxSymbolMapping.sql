
CREATE procedure [dbo].[P_GetFxSymbolMapping]
as
Select   
      StanPair.eSignalSymbol AS eSignalSymbol,
      Currency1.CurrencySymbol+'-'+Currency2.CurrencySymbol as TickerSymbol,
      0 AS ConversionMethod

   From  T_CurrencyStandardPairs StanPair        
   join T_Currency as Currency1 on Currency1.CurrencyID=StanPair.FromCurrencyID  
   join T_Currency as  Currency2 on Currency2.CurrencyID=StanPair.ToCurrencyID                   


UNION        
        
  Select  StanPair.eSignalSymbol AS eSignalSymbol,
      Currency2.CurrencySymbol +'-'+ Currency1.CurrencySymbol as TickerSymbol,
      1 AS ConversionMethod
  
 From  T_CurrencyStandardPairs StanPair        
   join T_Currency as Currency1 on Currency1.CurrencyID=StanPair.FromCurrencyID  
   join T_Currency as  Currency2 on Currency2.CurrencyID=StanPair.ToCurrencyID              


