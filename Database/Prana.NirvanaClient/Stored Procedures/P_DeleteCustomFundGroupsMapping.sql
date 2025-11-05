CREATE PROCEDURE [dbo].[P_DeleteCustomFundGroupsMapping]
          @customGroupId int
AS
BEGIN
	Delete From T_FundGroups Where FundGroupID = @customGroupId
    Delete From T_GroupFundMapping Where FundGroupID = @customGroupId
END

