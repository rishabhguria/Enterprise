----------------------------------------------------  
--Modified By : Bharat Kumar Jangir  
--Modification Date : 07/10/2013  
--Description : Adding columns in Cash Divident  
----------------------------------------------------  
  
CREATE PROC [dbo].[P_GetCashDividendsValue]          
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
EXEC ('Select Div.DivPKId, Div.FundID, Div.TaxlotId, Div.Symbol, Div.Dividend, Div.PayoutDate, Div.ExDate, Div.CurrencyID,       
 taxlots.CorpActionId, taxlots.ParentRow_Pk, V_corpactiondata.DivRate, Div.RecordDate, Div.DeclarationDate, Div.Description,
 V_SecMasterData.CompanyName
 from T_TaxlotCashDividends Div Left outer join PM_CorpActiontaxlots taxlots on Div.DivPKId = taxlots.FKId      
 Left outer join V_corpactiondata on V_corpactiondata.CorpActionId=taxlots.CorpActionId   
 Left outer join V_SecMasterData ON V_SecMasterData.TickerSymbol = Div.Symbol    
 where datediff(d,Div.' + @dateType + ',''' + @fromDate + ''')<=0 and datediff(d,Div.' + @dateType + ',''' + @toDate + ''')>=0  
 and Div.Symbol like  
 CASE ''' + @symbol + '''  
  WHEN '''' THEN ''%''  
  ELSE ''' + @symbol + '''  
 END')
  
 END  
ELSE  
 BEGIN
EXEC ('Select Div.DivPKId, Div.FundID, Div.TaxlotId, Div.Symbol, Div.Dividend, Div.PayoutDate, Div.ExDate, Div.CurrencyID,       
 taxlots.CorpActionId, taxlots.ParentRow_Pk, V_corpactiondata.DivRate, Div.RecordDate, Div.DeclarationDate, Div.Description,  
 V_SecMasterData.CompanyName
 from T_TaxlotCashDividends Div Left outer join PM_CorpActiontaxlots taxlots on Div.DivPKId = taxlots.FKId      
 Left outer join V_corpactiondata on V_corpactiondata.CorpActionId=taxlots.CorpActionId   
 Left outer join V_SecMasterData ON V_SecMasterData.TickerSymbol = Div.Symbol        
 where datediff(d,Div.' + @dateType + ',''' + @fromDate + ''')<=0 and datediff(d,Div.' + @dateType + ',''' + @toDate + ''')>=0  
 and Div.FundID in (' + @commaSeparatedFundIds + ')  
 and Div.Symbol like  
 CASE ''' + @symbol + '''  
  WHEN '''' THEN ''%''  
  ELSE ''' + @symbol + '''  
 END')
  
 END
