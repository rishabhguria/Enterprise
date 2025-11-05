
CREATE PROCEDURE [dbo].[P_GetPricingPolicy]
AS

	
	Select
	Id,       
    IsActive,
	PolicyName,
	SPName,
	IsFileAvailable,
	FilePath,
	FolderPath            
	from t_PricingPolicy    



