

CREATE  PROCEDURE dbo.P_GetCompanyClientIdentifiers 
(@CompanyClientID int)

AS

select  CompanyClientIdentifierID as PrimaryKey, T_Identifier.IdentifierID,T_Identifier.IdentifierName as Identifier,Identifier as ClientIdentifierNAme
 from T_CompanyClientIdentifier 
join T_Identifier on
T_Identifier.IdentifierID=T_CompanyClientIdentifier.IdentifierID

 where CompanyClientID=@CompanyClientID
