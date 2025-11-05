/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*ScriptType: General
	Description: This OTD script will be used to update default value for IsSettlementCurrencyBase in T_UserTTGeneralPreferences & T_CompanyTTGeneralPreferences tables for existing records
	Created By: Shubham Awasthi
	Dated: 14 August 2017
*/

--------------------------------------------------------------------------------------
*/
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_UserTTGeneralPreferences')
BEGIN 
Update T_UserTTGeneralPreferences
set IsSettlementCurrencyBase=0 where IsSettlementCurrencyBase is Null

Update T_CompanyTTGeneralPreferences
set IsSettlementCurrencyBase=0 where IsSettlementCurrencyBase is Null
End
