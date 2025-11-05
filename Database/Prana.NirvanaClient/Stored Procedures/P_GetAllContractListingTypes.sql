


/****** Object:  Stored Procedure dbo.P_GetAllContractListingTypes    Script Date: 08/12/2005 6:15:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllContractListingTypes
AS
	Select ContractListingTypeID, ContractListingType
	From T_ContractListingType Order By ContractListingType
	


