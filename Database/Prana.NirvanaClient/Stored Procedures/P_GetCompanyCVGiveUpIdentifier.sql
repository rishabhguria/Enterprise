
CREATE PROCEDURE dbo.P_GetCompanyCVGiveUpIdentifier
(
   @companyCounterPartyVenueID int=0
)
AS	

SELECT CompanyCounterPartyVenueID,
 CompanyCVenueGiveupIdentifierID as CompanyCVGiveupIdentifierID,
 GiveUpIdentifier FROM T_CompanyCVGiveUpIdentifier 
WHERE CompanyCounterPartyVenueID=@companyCounterPartyVenueID 


