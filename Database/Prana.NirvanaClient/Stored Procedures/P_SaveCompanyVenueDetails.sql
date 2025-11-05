


/****** Object:  Stored Procedure dbo.P_SaveCompanyVenueDetails    Script Date: 04/01/2006 9:25:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyVenueDetails
(
		@companyVenueID int,
		@fullName varchar(50),
		@shortName varchar(50),
		@venueTypeID int,
		@timeZone varchar(50),
		
		@preMarketTime int,
		@preMarketStartTime datetime,
		@preMarketEndTime datetime,
		@regularMarketTime int,
		@regularMarketStartTime datetime,
		@lunchTime int,
		@lunchStartTime datetime,
		@lunchEndTime datetime,
		@regularMarketEndTime datetime,
		@postMarketTime int,
		@postMarketStartTime datetime,
		@postMarketEndTime datetime,
		
		@companyID int,
		@result int 
	)
AS 
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyVenue 
Where CompanyID = @companyID

if(@total > 0)
begin	
	
	--Update T_CompanyVenue
	Update T_CompanyVenue 
	Set FullName = @fullName, 
		ShortName = @shortName, 
		VenueTypeID = @venueTypeID,
		TimeZone = @timeZone,
		
		PreMarketTime = @preMarketTime, 
		PreMarketStartTime = @preMarketStartTime, 
		PreMarketEndTime = @preMarketEndTime,
		RegularMarketTime = @regularMarketTime,
		RegularMarketStartTime = @regularMarketStartTime, 
		LunchTime = @lunchTime, 
		LunchStartTime = @lunchStartTime,
		LunchEndTime = @lunchEndTime,
		RegularMarketEndTime = @regularMarketEndTime, 
		PostMarketTime = @postMarketTime, 
		PostMarketStartTime = @postMarketStartTime,
		PostMarketEndTime = @postMarketEndTime,
		
		CompanyID = @companyID
			
	Where CompanyID = @companyID 
				
		
	Set @result = @companyVenueID 
	
end
else
--Insert T_CompanyVenue
begin
	INSERT T_CompanyVenue(FullName, ShortName, VenueTypeID, TimeZone, PreMarketTime, PreMarketStartTime, 
			 PreMarketEndTime, RegularMarketTime, RegularMarketStartTime, LunchTime, LunchStartTime, LunchEndTime, 
			 RegularMarketEndTime, PostMarketTime, PostMarketStartTime, PostMarketEndTime, CompanyID)
	Values(@fullName, @shortName, @venueTypeID, @timeZone, @preMarketTime, @preMarketStartTime, 
			 @preMarketEndTime, @regularMarketTime, @regularMarketStartTime, @lunchTime, @lunchStartTime, 
			 @lunchEndTime, @regularMarketEndTime, @postMarketTime, @postMarketStartTime, @postMarketEndTime, 
			 @companyID)  
					
				Set @result = scope_identity()
end
select @result


