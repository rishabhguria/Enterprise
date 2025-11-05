/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*
	ScriptType: General
	Description: Use Prorata Pref as default while calculating using Master Fund Pref
	Created By: Shubham Awasthi
	Dated: 25 May 2017
*/

--------------------------------------------------------------------------------------
*/
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_PTTMasterFundPreference'	) AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ' T_PTTAccountPercentagePreference')
	BEGIN
	   Update T_PTTMasterFundPreference
       SET UseProrataPreference=1
       FROM
        (Select Sum(pttAcc.PercentInMasterFund) As TotalPercentage,mfAcc.CompanyMasterFundID FROM T_PTTAccountPercentagePreference pttAcc
        INNER JOIN T_CompanyMasterFundSubAccountAssociation mfAcc on pttAcc.AccountId=mfAcc.CompanyFundID 
        INNER JOIN T_PTTMasterFundPreference on mfAcc.CompanyMasterFundID=T_PTTMasterFundPreference.MasterFundId
        GROUP BY mfAcc.CompanyMasterFundID
        HAVING Sum(pttAcc.PercentInMasterFund)=0)
        As AccTotalPercentage
        WHERE MasterFundId=AccTotalPercentage.CompanyMasterFundID 
	END

	