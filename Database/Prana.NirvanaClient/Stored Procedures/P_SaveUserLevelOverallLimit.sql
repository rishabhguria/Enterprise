CREATE PROCEDURE dbo.P_SaveUserLevelOverallLimit
(
		@rMCompanyUserID int,
		@companyUserID int,
		@userExposureLimit int,
		@maximumPNLLoss int,
		@maximumSizePerOrder int,
		@maximumSizePerBasket int,
		@companyID int,
		@result int
			
)
AS 

if (@rMCompanyUserID > 0)
begin

UPDATE    T_RMCompanyUsersOverall
SET              UserExposureLimit = @userExposureLimit, MaximumPNLLoss = @maximumPNLLoss, MaximumSizePerOrder = @maximumSizePerOrder, 
                      MaximumSizePerBasket = @maximumSizePerBasket
WHERE     (RMCompanyUserID = @rMCompanyUserID)
	
		Set @result = -1  
	end 
	else
	/*begin
	
	declare @total int
	set @total = 0
	select @total = count(*)
	from T_RMCompanyUsersOverall  
	Where RMCompanyUserID = @rMCompanyUserID
	
	


		if(@total > 0)
begin	
	
	
	Set @result = -1  
end
else */
	begin
		INSERT into T_RMCompanyUsersOverall ( CompanyUserID, UserExposureLimit, MaximumPNLLoss, MaximumSizePerOrder, MaximumSizePerBasket, CompanyID)
		Values(  @companyUserID,@userExposureLimit, @maximumPNLLoss, @maximumSizePerOrder, @maximumSizePerBasket,@companyID)  
			
		Set @result = scope_identity()
	end
	--end
select @result



	
	
