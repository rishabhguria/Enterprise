CREATE PROCEDURE [dbo].[P_GetCashCommissionPreferences]
	
AS
	SELECT AssetClass, IsChecked
	FROM T_CashPreferencesforCommission

