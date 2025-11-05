


/****** Object:  Stored Procedure dbo.P_GetCompanyCounterParties    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyCounterParties
	(
		@companyID int		
	)
AS
SELECT     T_CompanyCounterParties.CompanyID, T_CounterParty.CounterPartyID, T_CounterParty.FullName, 
			T_CounterParty.ShortName, T_CounterParty.Address, T_CounterParty.Phone, T_CounterParty.Fax, 
			T_CounterParty.ContactName1, T_CounterParty.Title1, T_CounterParty.EMail1, T_CounterParty.ContactName2,
			T_CounterParty.Title2, T_CounterParty.EMail2, T_CounterParty.CounterPartyTypeID, T_CounterParty.IsOTDorEMS
FROM         T_CounterParty INNER JOIN
                      T_CompanyCounterParties ON T_CounterParty.CounterPartyID = T_CompanyCounterParties.CounterPartyID
WHERE     T_CompanyCounterParties.CompanyID = @companyID
	
	
	
	
	/*SELECT     CPV.CompanyID, CPV.CounterPartyID, CP.FullName, CP.ShortName, CP.Address, 
				CP.Phone, CP.Fax, CP.ContactName1, CP.Title1, CP.Email1, CP.ContactName2, CP.Title2, CP.Email2, 
				CP.Acronym, CP.BaseCurrency, CP.IsElectronicTrading, CP.FIXVersionID, CP.FIXCapabilities, 
				CP.CounterPartyTypeID, CP.OutgoingCompanyID, CP.IncomingCompanyID, CP.Comment
	FROM         T_CompanyCounterParties CPV, T_CounterParty CP
	Where CPV.CompanyID = @companyID AND CPV.CounterPartyID = CP.CounterPartyID */
	
	
/*SELECT     T_CompanyCounterPartyVenues.CompanyID, 
                      T_CounterParty.CounterPartyID, T_CounterParty.FullName, T_CounterParty.ShortName, T_CounterParty.Address, T_CounterParty.Phone, 
                      T_CounterParty.Fax, T_CounterParty.ContactName1, T_CounterParty.Title1, T_CounterParty.EMail1, T_CounterParty.ContactName2, 
                      T_CounterParty.Title2, T_CounterParty.EMail2, T_CounterParty.Acronym, T_CounterParty.BaseCurrency, T_CounterParty.IsElectronicTrading, 
                      T_CounterParty.FIXVersionID, T_CounterParty.FIXCapabilities, T_CounterParty.CounterPartyTypeID, T_CounterParty.OutgoingCompanyID, 
                      T_CounterParty.IncomingCompanyID, T_CounterParty.Comment
FROM         T_CounterParty INNER JOIN
                      T_CounterPartyVenue ON T_CounterParty.CounterPartyID = T_CounterPartyVenue.CounterPartyID INNER JOIN
                      T_CompanyCounterPartyVenues ON T_CounterPartyVenue.CounterPartyVenueID = T_CompanyCounterPartyVenues.CounterPartyVenueID 
                      AND T_CompanyCounterPartyVenues.CompanyID = @companyID */
                      
                      



