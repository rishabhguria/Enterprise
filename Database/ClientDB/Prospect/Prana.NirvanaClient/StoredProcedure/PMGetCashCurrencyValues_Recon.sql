CREATE PROCEDURE [dbo].[PMGetCashCurrencyValues_Recon_New]                                                                                                 
(                                                                                                                                                                      
 @StartDate datetime,                       
 @EndDate datetime,                                      
 @AssetIds VARCHAR(MAX),                                      
 @FundIds VARCHAR(MAX)                                                                                                 
 --@symbol varchar(50)                                                                                                              
)                                                                                                                                                                      
As                                                
Begin                          
                        
declare @Local_StartDate  datetime                                   
declare @Local_EndDate datetime                                                  
declare @Local_AssetIds VARCHAR(MAX)                                                  
declare @Local_FundIds VARCHAR(MAX)                          
set @Local_StartDate = @StartDate                        
set @Local_EndDate = @EndDate                        
--set @Local_AssetIds = @AssetIds                        
set @Local_FundIds = @FundIds                        
--                        
-- Create Table #AssetClass (AssetID int)                                                                  
-- if (@Local_AssetIds is NULL or @Local_AssetIds = '')                                                                  
-- Insert into #AssetClass                                                                  
-- Select AssetID from T_Asset                                                                  
-- else                                                                   
-- Insert into #AssetClass                                                                  
-- Select Items as AssetID from dbo.Split(@Local_AssetIds,',')                                                                  
                                                                  
 Create Table #Funds (FundID int)                                                                  
 if (@Local_FundIds is NULL or @Local_FundIds = '')                                                                  
 Insert into #Funds                                                                  
 Select CompanyFundID as FundID from T_CompanyFunds                                                                  
 else                                                                  
 Insert into #Funds                                                                  
 Select Items as FundID from dbo.Split(@Local_FundIds,',')                                                                  
                   
                                    
--Declare @FromAUECDatesTable Table                                                    
--(                                                    
--AUECID int,                                                    
--CurrentAUECDate DateTime                                                    
--)                                          
--                                     
--Declare @ToAUECDatesTable Table                                                    
--(                                                    
--AUECID int,                                                    
--CurrentAUECDate DateTime                                                    
--)                                                                                                                                        
--                                                     
--Insert Into @ToAUECDatesTable           
--Select * From dbo.GetAllAUECDatesFromString(@Local_ToAllAUECDatesString)                                
--                                      
--Insert Into @FromAUECDatesTable                                              
--Select * From dbo.GetAllAUECDatesFromString(@Local_FromAllAUECDatesString)                                                
                                            
-- variable to hold  From DATE                                                                    
--Declare @FROMDate datetime                                                     
--SELECT @FROMDate = (SELECT TOP 1 CurrentAUECDate FROM @FromAUECDatesTable)                        
--                                            
---- variable to hold To DATE                                                                                  
--Declare @ToDate datetime                                                                                                                                                  
--SELECT @ToDate = (SELECT TOP 1 CurrentAUECDate FROM @ToAUECDatesTable)                                            
                        
select                         
CF.FundName as AccountName,                         
CompanyFundCashCurrencyValue.Date as TradeDate,                         
CurrencyLocal.CurrencySymbol as Symbol,                         
CompanyFundCashCurrencyValue.CashValueBase as CashValueBase,                         
--CurrencyBase.CurrencySymbol as BaseCurrency,                         
CompanyFundCashCurrencyValue.CashValueLocal as CashValueLocal                        
from PM_CompanyFundCashCurrencyValue CompanyFundCashCurrencyValue               
inner join #Funds                
on #Funds.FundId = CompanyFundCashCurrencyValue.FundID                       
inner join T_CompanyFunds CF                         
on CF.CompanyFundId = CompanyFundCashCurrencyValue.FundID                        
left join T_Currency CurrencyLocal                         
on CurrencyLocal.CurrencyId = CompanyFundCashCurrencyValue.LocalCurrencyID                        
left join T_Currency CurrencyBase                         
on CurrencyBase.CurrencyId = CompanyFundCashCurrencyValue.BaseCurrencyID                        
where datediff(dd, CompanyFundCashCurrencyValue.Date, @StartDate)>=0                        
and datediff(dd, CompanyFundCashCurrencyValue.Date, @EndDate)<=0                        
order by CompanyFundCashCurrencyValue.Date                        
                        
--drop table #Funds,#AssetClass                        
                        
End 