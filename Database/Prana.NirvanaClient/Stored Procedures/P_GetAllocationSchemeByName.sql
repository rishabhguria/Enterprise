  
--P_GetAllocationSchemeByName 'Test Allocation Scheme'  
  
CREATE Procedure [dbo].[P_GetAllocationSchemeByName]  
(   
@allocationSchemeName varchar(100)  
)  
As  


Select 
AllocationSchemeID,
Date,
AllocationSchemeName,
AllocationScheme,
IsPrefVisible,
CreationSource
From T_AllocationScheme
Where AllocationSchemeName = @allocationSchemeName  
