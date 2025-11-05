


/****** Object:  Stored Procedure dbo.P_DeleteCompanyCounterPartyByID    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCounterPartyByID
(
	@companyID int,
	@companyCounterPartyID int
	
)
AS

Delete T_CompanyCounterParties
Where CounterPartyID = @companyCounterPartyID AND CompanyID = @companyID



