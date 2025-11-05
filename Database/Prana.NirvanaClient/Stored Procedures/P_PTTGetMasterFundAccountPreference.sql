CREATE PROCEDURE [dbo].[P_PTTGetMasterFundAccountPreference]

AS
	Select [T_PTTMasterFundPreference].[MasterFundId],
	[T_PTTMasterFundPreference].[UseProrataPreference], 
	[T_PTTMasterFundPreference].[PreferenceType]
	From T_PTTMasterFundPreference

	Select [T_PTTAccountPercentagePreference].AccountId,
	[T_PTTAccountPercentagePreference].PercentInMasterFund,
	[T_PTTAccountPercentagePreference].AccountFactor,
	[T_PTTAccountPercentagePreference].PreferenceType
	From T_PTTAccountPercentagePreference
RETURN 0
