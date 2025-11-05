CREATE PROCEDURE dbo.P_GetAllThirdPartyTypes      
AS      
 SELECT     ThirdPartyTypeID, ThirdPartyTypeName      
 FROM         T_ThirdPartyType Where ThirdPartyTypeID <> 2 Order By ThirdPartyTypeID 