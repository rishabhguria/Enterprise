CREATE PROCEDURE dbo.P_GetCompanyCVCMTAIdentifier
(
   @companyCounterPartyVenueID int=0
)
AS	

SELECT CompanyCounterPartyVenueID,
 CompanyCVenueCMTAIdentifierID as CompanyCVCMTAIdentifierID,
 CMTAIdentifier FROM T_CompanyCVCMTAIdentifier 
WHERE CompanyCounterPartyVenueID=@companyCounterPartyVenueID 

