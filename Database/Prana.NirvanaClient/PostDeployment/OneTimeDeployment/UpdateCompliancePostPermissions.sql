IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_CA_RuleUserPermissions')
BEGIN

	-- Add default permission for post rules to new table T_CA_RuleUserPermissions 
	CREATE TABLE #RuleUserPermissions
	(
	RuleID	VARCHAR(100),
	PopUp	BIT,
	UserID	INT,
	OverridePermission	INT
	)

	SELECT DISTINCT UserId INTO #UserTable FROM T_CompanyUser

	INSERT INTO	#RuleUserPermissions
	SELECT	R.RuleId,
			1,
			U.UserId,
			NULL
	FROM T_CA_RulesUserDefined R
	CROSS JOIN #UserTable U
	WHERE R.PackageName = 'PostTrade'

	INSERT INTO T_CA_RuleUserPermissions
	SELECT * FROM #RuleUserPermissions
		
	DROP TABLE #UserTable
	DROP TABLE #RuleUserPermissions					
END