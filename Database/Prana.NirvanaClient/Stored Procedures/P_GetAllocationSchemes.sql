CREATE Procedure [dbo].[P_GetAllocationSchemes]                  
As                  
Select                   
AllocationSchemeID,              
AllocationSchemeName,
Date,
IsprefVisible,
CreationSource            
from T_AllocationScheme
order by Date desc
