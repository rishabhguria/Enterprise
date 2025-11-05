CREATE VIEW [dbo].[V_SubAccountCashValue]      
AS      
SELECT DISTINCT       
TOP (100) PERCENT Journal.TaxLotID,       
Journal.FundID,       
Journal.SubAccountID,      
Journal.Symbol,       
Journal.PBDesc,       
Journal.TransactionDate AS PayOutDate,       
Journal.TransactionID AS CashID,       
Journal.DR - Journal.CR AS CashValue,       
CurrencyConversionRate.ConversionRate AS FXRate,       
0 AS IsAutomatic,       
Journal.TransactionDate AS AccrueDate,       
Journal.TransactionDate AS TraderDate,       
SubAccounts.TransactionTypeId,       
AccType.TransactionType,       
CurrencyConversionRate.ConversionRateID,      
Journal.CurrencyID       
FROM       
dbo.T_Journal AS Journal     
INNER JOIN dbo.T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID     
INNER JOIN dbo.T_TransactionType AS AccType ON AccType.TransactionTypeId = SubAccounts.TransactionTypeId       
INNER JOIN dbo.T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID     
LEFT OUTER JOIN dbo.T_CurrencyStandardPairs AS CurrencyStandardPairs     
ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID       
AND CurrencyStandardPairs.ToCurrencyID = (SELECT TOP (1) BaseCurrencyID FROM  dbo.T_Company)     
LEFT OUTER JOIN  dbo.T_CurrencyConversionRate AS CurrencyConversionRate     
ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID     
AND CurrencyConversionRate.Date = Journal.TransactionDate      
WHERE ((Journal.TaxLotID IS NULL) OR (Journal.TaxLotID = ''))     
AND (AccType.TransactionType = 'Cash')      