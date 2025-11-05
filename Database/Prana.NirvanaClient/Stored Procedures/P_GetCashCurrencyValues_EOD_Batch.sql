                 
Create PROCEDURE [dbo].[P_GetCashCurrencyValues_EOD_Batch]                                                                                                                     
(                                                                                                                                                                                          
 @CompanyFundIDs VARCHAR(max)                    
 ,@InputDate DATETIME                    
     
)                                                                                                                                                                                          
As             
            
SET NOCOUNT On             
                                                                   
Begin                                              
           
--declare @lastworkingday datetime                   
--SELECT @lastworkingday=CONVERT(VARCHAR(10),dbo.AdjustBusinessDays(GetDate(),-1,1), 110)                    
----select @lastworkingday                  
--                  
--set @InputDate=                  
--case when @InputDate =''                  
--then @lastworkingday                                         
--else @InputDate                  
--end              
                 
              
--SET NOCOUNT On           
                                           
If (@InputDate = '')    
Begin    
 Set @InputDate = GetDate()    
End    
        
-- Update input date to last business date           
--SELECT @InputDate = dbo.AdjustBusinessDays(@InputDate,-1,1)      
    
----Select @InputDate                                 
                                
DECLARE @Fund TABLE                             
(                            
  FundID INT                            
)                            
    
If (@CompanyFundIDs Is NULL Or @CompanyFundIDs = '')                                                                
 Insert InTo @Fund                                                                
  Select                        
  CompanyFundID as FundID                         
  From T_CompanyFunds  
  Where IsActive=1                                                                
Else                                                                
 INSERT INTO @Fund                                                                
  SELECT Cast(Items AS INT)                            
  FROM dbo.Split(@companyFundIDs, ',')                                                                                    
                                                         
                                            
Select                                             
 CF.FundName as FundName,                                             
 LEFT(CONVERT(VARCHAR, CompanyFundCashCurrencyValue.Date, 101), 10) as TradeDate,                                             
 CurrencyLocal.CurrencySymbol as Symbol,                
CompanyFundCashCurrencyValue.CashValueLocal as CashValueLocal ,                                           
CompanyFundCashCurrencyValue.CashValueBase as CashValueBase                                             
                                            
 From PM_CompanyFundCashCurrencyValue CompanyFundCashCurrencyValue                                   
 Inner Join @Fund F on F.FundId = CompanyFundCashCurrencyValue.FundID                                           
 Inner Join T_CompanyFunds CF On CF.CompanyFundId = CompanyFundCashCurrencyValue.FundID                                            
 Left Join T_Currency CurrencyLocal On CurrencyLocal.CurrencyId = CompanyFundCashCurrencyValue.LocalCurrencyID                                            
 Left Join T_Currency CurrencyBase On CurrencyBase.CurrencyId = CompanyFundCashCurrencyValue.BaseCurrencyID                                            
 Where Datediff(Day, CompanyFundCashCurrencyValue.Date, @InputDate) = 0                                            
Order by CompanyFundCashCurrencyValue.Date                                            
                                            
End 