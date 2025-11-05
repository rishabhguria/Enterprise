/*
EXEC P_GetCashCurrencyValues_EOD 0,'13,1,2,11,3,4,12,8,9,10,5,6,7','05-25-2017',0,1,1,1,1,0
*/

CREATE PROCEDURE [dbo].[P_GetCashCurrencyValues_EOD]                                                                                                   
(                                                                                                                                                                        
  @ThirdPartyID INT  
 ,@CompanyFundIDs VARCHAR(max)  
 ,@InputDate DATETIME  
 ,@CompanyID INT  
 ,@AUECIDs VARCHAR(max)  
 ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                      
 ,@DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                      
 ,@FileFormatID INT  
 ,@IncludeSent Int = 0                                                                                                                 
)                                                                                                                                                                        
As                                                  
Begin                            
                          
DECLARE @Fund TABLE 
(
  FundID INT
)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')                                                                  
                                       
                          
Select                           
	CF.FundName as AccountName,                           
	CompanyFundCashCurrencyValue.Date as TradeDate,                           
	CurrencyLocal.CurrencySymbol as Symbol,                           
	CompanyFundCashCurrencyValue.CashValueBase as CashValueBase,                           
	CompanyFundCashCurrencyValue.CashValueLocal as CashValueLocal                          
	From PM_CompanyFundCashCurrencyValue CompanyFundCashCurrencyValue                 
	Inner Join @Fund F on F.FundId = CompanyFundCashCurrencyValue.FundID                         
	Inner Join T_CompanyFunds CF On CF.CompanyFundId = CompanyFundCashCurrencyValue.FundID                          
	Left Join T_Currency CurrencyLocal On CurrencyLocal.CurrencyId = CompanyFundCashCurrencyValue.LocalCurrencyID                          
	Left Join T_Currency CurrencyBase On CurrencyBase.CurrencyId = CompanyFundCashCurrencyValue.BaseCurrencyID                          
	Where Datediff(Day, CompanyFundCashCurrencyValue.Date, @InputDate) = 0                          
Order by CompanyFundCashCurrencyValue.Date                          
                          
End