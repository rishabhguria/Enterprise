CREATE PROCEDURE [dbo].[P_GetAllCustomFundGroupsMapping]

AS
BEGIN
	 
	SELECT  
		t.FundGroupID, t.FundId, 
		g.GroupName, 
		f.FundName
    FROM     T_GroupFundMapping t 
	JOIN T_FundGroups g ON t.FundGroupID = g.FundGroupID 
	JOIN T_CompanyFunds f ON f.CompanyFundID = t.FundID;
END

