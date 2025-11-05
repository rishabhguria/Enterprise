


/****** Object:  Stored Procedure dbo.P_SaveClearingFirmPrimeBrokerNew    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveClearingFirmPrimeBrokerNew
(
		@clearingFirmsPrimeBrokersID int,
		@clearingFirmsPrimeBrokersName varchar(50),
		@clearingFirmsPrimeBrokersShortName varchar(50),
		@result	int
)
AS 

if(@clearingFirmsPrimeBrokersID > 0)
begin	
	Update T_ClearingFirmsPrimeBrokers 
	Set ClearingFirmsPrimeBrokersName = @clearingFirmsPrimeBrokersName,
        ClearingFirmsPrimeBrokersShortName = @clearingFirmsPrimeBrokersShortName	

	Where ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID
			
	
	Set @result = @clearingFirmsPrimeBrokersID
end
else
begin
	declare @total int
	set @total = 0
	select @total = count(*)
	from T_ClearingFirmsPrimeBrokers 
	Where ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID	
	
	if(@total > 0)
	begin
		Set @result = -1
	end
	else
	begin
	INSERT T_ClearingFirmsPrimeBrokers(ClearingFirmsPrimeBrokersName, ClearingFirmsPrimeBrokersShortName)
	Values(@clearingFirmsPrimeBrokersName, @clearingFirmsPrimeBrokersShortName)
        
        Set @result = scope_identity()
	end
end
select @result


