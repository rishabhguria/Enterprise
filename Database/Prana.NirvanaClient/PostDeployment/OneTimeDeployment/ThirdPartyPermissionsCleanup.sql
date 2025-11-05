/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*
	ScriptType: General
	Description: Cleanup of invalid third party permissions and preferences
*/

--------------------------------------------------------------------------------------
*/
DELETE
FROM T_ThirdPartyPermittedFunds
WHERE ThirdPartyID IN (
		SELECT CTP.CompanyThirdPartyID
		FROM T_CompanyThirdParty CTP
		WHERE CTP.ThirdPartyID NOT IN (
				SELECT TP.ThirdPartyID
				FROM T_ThirdParty TP
				WHERE TP.ThirdPartyTypeID = 1
				)
		)

DELETE
FROM T_CompanyThirdPartyCVIdentifier
WHERE CompanyThirdPartyID_FK IN (
		SELECT CTP.CompanyThirdPartyID
		FROM T_CompanyThirdParty CTP
		WHERE CTP.ThirdPartyID NOT IN (
				SELECT TP.ThirdPartyID
				FROM T_ThirdParty TP
				WHERE TP.ThirdPartyTypeID = 1
				)
		)

DELETE
FROM T_CompanyThirdPartyMappingDetails
WHERE CompanyThirdPartyID_FK IN (
		SELECT CTP.CompanyThirdPartyID
		FROM T_CompanyThirdParty CTP
		WHERE CTP.ThirdPartyID NOT IN (
				SELECT TP.ThirdPartyID
				FROM T_ThirdParty TP
				WHERE TP.ThirdPartyTypeID = 1
				)
		)

DELETE
FROM T_CompanyThirdPartyFlatFileSaveDetails
WHERE CompanyThirdPartyID IN (
		SELECT CTP.CompanyThirdPartyID
		FROM T_CompanyThirdParty CTP
		WHERE CTP.ThirdPartyID NOT IN (
				SELECT TP.ThirdPartyID
				FROM T_ThirdParty TP
				WHERE TP.ThirdPartyTypeID = 1
				)
		)

DELETE
FROM T_CompanyThirdParty
WHERE ThirdPartyID NOT IN (
		SELECT TP.ThirdPartyID
		FROM T_ThirdParty TP
		WHERE TP.ThirdPartyTypeID = 1
		)