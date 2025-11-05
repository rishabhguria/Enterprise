
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_CA_RuleUserPermissions' AND TABLE_NAME='T_CA_RuleLevelUserPermissionTemp')
BEGIN

	-- Add permission to new table T_CA_RuleUserPermissions based on permission defined in T_CA_RuleLevelUserPermission table
	CREATE TABLE #RuleUserPermissions
	(
	RuleID	VARCHAR(100),
	PopUp	BIT,
	UserID	INT,
	OverridePermission	INT
	)

	SELECT DISTINCT UserId INTO #UserTable FROM T_CA_RuleLevelUserPermissionTemp

	INSERT INTO	#RuleUserPermissions
	SELECT	R.RuleId,
			1,
			UT.UserId,
			CASE U.UserId WHEN UT.UserId THEN 1 ELSE 2 END 
	FROM T_CA_RulesUserDefined R
	CROSS JOIN #UserTable UT
	INNER JOIN T_CA_RuleLevelUserPermissionTemp U
	ON R.RuleId = U.RuleId
	WHERE R.PackageName = 'PreTrade' 

	DROP TABLE #UserTable

	INSERT INTO	#RuleUserPermissions
	SELECT	R.RuleId,
			1,
			U.UserId,
			1
	FROM T_CA_RulesUserDefined R
	CROSS JOIN T_CA_RuleLevelUserPermissionTemp U
	WHERE R.PackageName = 'PreTrade' AND R.RuleId NOT IN (SELECT RuleId FROM T_CA_RuleLevelUserPermissionTemp WHERE RuleId <> '')

	INSERT INTO T_CA_RuleUserPermissions
	SELECT * FROM #RuleUserPermissions
	DROP TABLE #RuleUserPermissions	
			
	-- Delete table T_CA_RuleLevelUserPermission as it is no longer used
	DROP TABLE T_CA_RuleLevelUserPermissionTemp		
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_CA_OtherCompliancePermission')
BEGIN
	UPDATE	T_CA_OtherCompliancePermission
	SET		DefaultRuleOverrideType = CASE IsOverridePermission WHEN 1 THEN 1 ELSE 2 END
	WHERE	DefaultRuleOverrideType IS NULL

	UPDATE	T_CA_OtherCompliancePermission
	SET		DefaultPrePopUp = 1
	WHERE	DefaultPrePopUp IS NULL

	UPDATE	T_CA_OtherCompliancePermission
	SET		DefaultPostPopUp = 1
	WHERE	DefaultPostPopUp IS NULL

END 
	
