CREATE PROCEDURE [dbo].[P_GetRebalPreferences]

AS
BEGIN
	SELECT   AccountId,PreferenceKey, PreferenceValue
    FROM     T_RebalPreferences
END
