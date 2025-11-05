/*  
Modified By: Sandeep Singh  
Date: Dec 16, 2013  
Desc: Added Level2ID. PM_CorpActiontaxlots contains all taxlots  and T_CashTransactions contains data group by Fund Strategy  
So, #Temp_PM_CorpActiontaxlots is used to fetch data on the basis of Corp ActionID and FKId  



Modified By : Surendra bisht
Date: Dec 23, 2014  
Desc: We will only Pickup Data from T_CashTransactions Table.

----------------------------------------------------      
--Modified By : Nishant Kumar Jain            
--Modification Date :  2015-04-21           
--Description : Adding columns in TransactionSource, ModifyDate, EntryDate 
----------------------------------------------------- 
  
*/  
  
CREATE PROC [dbo].[P_GetCashTransactions]                  
(                  
 @symbol varchar(50),              
 @commaSeparatedFundIds varchar(max),            
 @dateType varchar(50),                
 @fromDate datetime,                  
 @toDate datetime                  
)                  
As       
      
  
          
IF @commaSeparatedFundIds='-1'          
 BEGIN        
EXEC ('Select Div.CashTransactionId, Div.FundID,Div.Level2Id,Div.TaxlotId, Div.Symbol, Div.Amount, Div.PayoutDate, Div.ExDate, Div.CurrencyID, Div.RecordDate, Div.DeclarationDate, Div.Description,        
Div.ActivityTypeId, Div.FXRate, Div.FXConversionMethodOperator, Div.OtherCurrencyID, Div.TransactionSource, Div.ModifyDate, Div.EntryDate  ,Div.UserID  as UserId     
 from T_CashTransactions Div           
 where datediff(d,Div.' + @dateType + ',''' + @fromDate + ''')<=0 and datediff(d,Div.' + @dateType + ',''' + @toDate + ''')>=0          
 and Div.Symbol like          
 CASE ''' + @symbol + '''          
  WHEN '''' THEN ''%''          
  ELSE ''' + @symbol + '''          
 END')        
          
 END          
ELSE          
 BEGIN        
EXEC ('Select Div.CashTransactionId, Div.FundID,Div.Level2Id,Div.TaxlotId, Div.Symbol, Div.Amount, Div.PayoutDate, Div.ExDate, Div.CurrencyID, Div.RecordDate, Div.DeclarationDate, Div.Description,        
Div.ActivityTypeId, Div.FXRate, Div.FXConversionMethodOperator, Div.OtherCurrencyID, Div.TransactionSource, Div.ModifyDate, Div.EntryDate     ,Div.UserID as UserId
 from T_CashTransactions Div                          
 where datediff(d,Div.' + @dateType + ',''' + @fromDate + ''')<=0 and datediff(d,Div.' + @dateType + ',''' + @toDate + ''')>=0          
 and Div.FundID in (' + @commaSeparatedFundIds + ')          
 and Div.Symbol like          
 CASE ''' + @symbol + '''          
  WHEN '''' THEN ''%''          
  ELSE ''' + @symbol + '''          
 END')        
          
      
     
END   
  

