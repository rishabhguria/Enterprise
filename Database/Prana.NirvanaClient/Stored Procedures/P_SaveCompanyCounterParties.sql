


/****** Object:  Stored Procedure dbo.P_SaveCompanyCounterParties    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyCounterParties
	(
		@companyID int,
		@counterPartyID int
	)
AS

	Insert T_CompanyCounterParties(companyID, CounterPartyID)
	Values(@companyID, @counterPartyID)
	
	


