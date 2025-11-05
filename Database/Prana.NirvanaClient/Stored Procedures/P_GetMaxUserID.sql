-----------------------------------------------------------------
--Created BY: Bhavana
--Date: 23-may-14
--Purpose: Get max user id
-----------------------------------------------------------------
Create procedure [dbo].[P_GetMaxUserID]
as
select isnull(max(UserID),0) from T_CompanyUser
