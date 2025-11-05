CREATE PROCEDURE [dbo].[P_SaveShortLocateParemetersPreference]
	@param1 varchar(30) ,
	@param2 varchar(30)
AS
UPDATE  
T_PranaKeyValuePreferences  
SET T_PranaKeyValuePreferences.PreferenceValue=(
CASE
WHEN @param1='Company Level'
	THEN 0
ELSE 1
END 
) 
where T_PranaKeyValuePreferences.PreferenceKey='IsShowmasterFundOnShortLocate'

UPDATE  
T_PranaKeyValuePreferences  
SET T_PranaKeyValuePreferences.PreferenceValue=(
CASE
WHEN @param2='False'
	THEN 0
ELSE 1
END 
) 
where T_PranaKeyValuePreferences.PreferenceKey='IsImportOverrideOnShortLocate'

RETURN 0