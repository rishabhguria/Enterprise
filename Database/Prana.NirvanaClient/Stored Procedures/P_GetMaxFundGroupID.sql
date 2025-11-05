-----------------------------------------------------------------
--Created BY: Bhavana
--Date: 23-may-14
--Purpose: Get max fund group id
-----------------------------------------------------------------
Create procedure [dbo].[P_GetMaxFundGroupID]
as
select isnull(max(FundGroupID),0) from T_FundGroups
