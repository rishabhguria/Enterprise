-- Author	: Gaurav
-- Date		: 07 dec 12
-- Description: Picks up the max id from T_Group. This id is further used to generate the new distinct ids greater than this.
CREATE procedure [dbo].[P_GetMaxGeneratedNumberForGroup]
as
select max(cast(groupid as numeric ) )from T_Group


Print 'Finished Executing P_GetMaxGeneratedNumberForGroup'