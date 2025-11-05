/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*
	ScriptType: General
	Description: Add default PTT preference for funds and master funds
	Created By: Shubham Awasthi
	Dated: 19 APRIL 2017
*/

--------------------------------------------------------------------------------------
*/
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyFunds'	)
AND EXISTS(SELECT count(1) FROM [T_CompanyFunds]) AND NOT EXISTS(SELECT 1 FROM T_PTTAccountPercentagePreference)
	BEGIN
	Insert into T_PTTAccountPercentagePreference
	(AccountId)
	Select [CompanyFundID] from [T_CompanyFunds] 
	END

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyMasterFunds'	)
AND EXISTS(SELECT count(1) FROM [T_CompanyMasterFunds]) AND NOT EXISTS(SELECT 1 FROM T_PTTMasterFundPreference)
	BEGIN
	Insert into T_PTTMasterFundPreference
	(MasterFundId)
	Select [CompanyMasterFundID] from [T_CompanyMasterFunds] 
	END

