/****** Object:  Stored Procedure dbo.P_GetCompanyCVAUECs    Script Date: 04/20/2006 2:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyCVAUECs
	(
		@companyID	int,
		@companyCounterPartyCVID int	 
	)
AS
	
	/* Select distinct CVA.CVAUECID, CA.CompanyID, CA.AUECID, AssetID, UnderLyingID, AUECExchangeID
	From T_CompanyCounterPartyVenues CCPV inner join T_CVAUEC CVA 
	on CCPV.CounterPartyVenueID = CVA.CounterPartyVenueID inner join T_CompanyAUEC CA
	on CA.AUECID = CVA.AUECID inner join T_AUEC A
	on A.AUECID = CVA.AUECID
	Where CA.CompanyID = @companyID AND CVA.CounterPartyVenueID = @companyCounterPartyCVID */
	
	Declare @CounterPartyVenueID int
	
	Select @CounterPartyVenueID = CounterPartyVenueID From T_CompanyCounterPartyVenues Where 
	CompanyCounterPartyCVID = @companyCounterPartyCVID
	
	Select distinct CVA.CVAUECID, @CounterPartyVenueID, CA.AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName
	From T_CompanyCounterPartyVenues CCPV, T_CVAUEC CVA 
	inner join T_CompanyAUEC CA
	on CA.AUECID = CVA.AUECID inner join T_AUEC A
	on A.AUECID = CVA.AUECID
	Where CA.CompanyID = @companyID AND CVA.CounterPartyVenueID = @CounterPartyVenueID
	
	/* Select distinct CVA.CVAUECID, CA.CompanyID, CA.AUECID, AssetID, UnderLyingID
	From T_CompanyCounterPartyVenues CCPV, T_CVAUEC CVA 
	inner join T_CompanyAUEC CA
	on CA.AUECID = CVA.AUECID inner join T_AUEC A
	on A.AUECID = CVA.AUECID
	Where CA.CompanyID = @companyID AND CVA.CounterPartyVenueID = @CounterPartyVenueID */