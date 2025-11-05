CREATE procedure [dbo].[P_GetPricingPolicyDetail]
as
select
Id,
IsActive,
PolicyName,
SPName,
IsFileAvailable,
FolderPath,
FilePath
from T_PricingPolicy 

