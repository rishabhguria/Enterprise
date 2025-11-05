


/****** Object:  Stored Procedure dbo.P_GetCompanyVenueDetails    Script Date: 01/04/2005 9:40:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyVenueDetails
	(
		@companyID	int	
	)
AS
	
	Select  CompanyVenueID, VenueTypeID, FullName, ShortName, TimeZone, PreMarketTime, PreMarketStartTime, 
			 PreMarketEndTime, RegularMarketTime, RegularMarketStartTime, LunchTime, LunchStartTime, LunchEndTime, 
			 RegularMarketEndTime, PostMarketTime, PostMarketStartTime, PostMarketEndTime, CompanyID
 
	From T_CompanyVenue
	Where CompanyID = @companyID


