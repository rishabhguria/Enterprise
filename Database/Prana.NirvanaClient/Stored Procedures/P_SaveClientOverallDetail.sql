CREATE PROCEDURE dbo.P_SaveClientOverallDetail
(
		@companyClientRMID int,
		@clientID int,
		@clientExposureLimit int,
		@companyID int,
		@result int
			
	)
AS 

if( @companyClientRMID > 0)

begin	
	Update T_RMCompanyClientOverall 
	Set  
		ClientExposureLimit = @clientExposureLimit					
	Where CompanyClientRMID= @companyClientRMID

Set @result = -1  
end 
	else
	/*begin
	
	declare @total int
	set @total = 0
	select @total = count(*)
	from T_RMCompanyClientOverall 
	Where  CompanyClientRMID= @companyClientRMID
if(@total > 0)
begin	
	
	Set @result = -1  
end
else*/
	begin
	
	INSERT into T_RMCompanyClientOverall ( ClientID, ClientExposureLimit, CompanyID)
		Values( @clientID, @clientExposureLimit, @companyID)  
			
		Set @result = scope_identity()
	end
	--end
select @result
