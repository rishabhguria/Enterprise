/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*
	ScriptType: General
	Description: Insert Default Permission to include Commission in Cash Revaluation for all Asset Types
	Created By: Kashish Goyal
	Dated: 01 Dec 2017
*/

--------------------------------------------------------------------------------------
*/
TRUNCATE TABLE T_CashPreferencesforCommission

INSERT INTO T_CashPreferencesforCommission (AssetClass,IsChecked)
SELECT 'Equity',1
UNION ALL
SELECT 'EquityOption',1
UNION ALL
SELECT 'Future',1
UNION ALL
SELECT 'FutureOption',1
UNION ALL
SELECT 'Fx',1
UNION ALL
SELECT 'EquitySwap',1
UNION ALL
SELECT 'FixedIncome',1
UNION ALL
SELECT 'PrivateEquity',1
UNION ALL
SELECT 'FxForward',1
UNION ALL
SELECT 'ConvertibleBond',1
UNION ALL
SELECT 'CreditDefaultSwap',1


