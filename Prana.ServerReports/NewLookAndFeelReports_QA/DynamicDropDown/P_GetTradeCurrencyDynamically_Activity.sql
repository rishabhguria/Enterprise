/*      
Author: Ankit Misra         
Date:   16 Jan. 2015        
Desc:   Return only the TradeCurrency that were held by the client.       
Exec:   P_GetTradeCurrencyDynamically_Activity      
*/      
      
      
CREATE Procedure P_GetTradeCurrencyDynamically_Activity          
As          
Begin      
 Create Table #TempTradeCurrency      
 (      
  CurrencySymbol Varchar(100)      
 )         
-------------------------------------------------------------------------------      
-- FILL TRADECURRENCY WHICH ARE PRESENT IN T_MW_Transactions TABLE      
-------------------------------------------------------------------------------      
 Insert Into #TempTradeCurrency        
  Select Distinct      
  CurrencySymbol      
  from T_Currency      
  Inner Join T_MW_Transactions Transactions On CurrencySymbol = TradeCurrency        
  Order by CurrencySymbol      
----------------------------------------------------------------------      
-- IF NO DATA IN T_MW_Transactions FILL ALL TRADECURRENCY      
----------------------------------------------------------------------      
      
 If(Select Count(*) from #TempTradeCurrency) = 0      
 Begin      
  Insert Into #TempTradeCurrency       
  Select       
  CurrencySymbol      
  From T_Currency      
 End      
       
 Select * from #TempTradeCurrency      
 Drop Table #TempTradeCurrency      
End