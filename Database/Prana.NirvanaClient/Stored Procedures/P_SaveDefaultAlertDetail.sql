

CREATE PROCEDURE dbo.P_SaveDefaultAlertDetail
(
		@alertTypeID int,
		@refreshRateCalculation int,
		@companyID int
			
	)
AS 
declare @result int
declare @total int

	set @total = 0
	select @total = count(*)
	from T_RMDefaultAlerts 
	Where CompanyID = @companyID
		
if(@total > 0)

begin	
	Update T_RMDefaultAlerts 
	Set 
		AlertTypeID = @alertTypeID, 
		RefreshRateCalculation = @refreshRateCalculation, 
		CompanyID = @companyID
			
	Where CompanyID = @companyID  
				
	Set @result = @total
	
	end
else
begin

INSERT into T_RMDefaultAlerts (  AlertTypeID , 
		RefreshRateCalculation,CompanyID)
		Values(@alertTypeID , @refreshRateCalculation ,
		@companyID) 
		   
		   Set @result = scope_identity()
end
select @result

