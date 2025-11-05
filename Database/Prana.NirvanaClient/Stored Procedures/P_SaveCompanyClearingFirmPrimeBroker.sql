


/****** Object:  Stored Procedure dbo.P_SaveCompanyClearingFirmPrimeBroker    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyClearingFirmPrimeBroker
(
		@companyClearingFirmsPrimeBrokersID int,
		@clearingFirmsPrimeBrokersName varchar(50),
		@clearingFirmsPrimeBrokersShortName varchar(50),
		@companyID int
		
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyClearingFirmsPrimeBrokers
Where CompanyClearingFirmsPrimeBrokersID = @companyClearingFirmsPrimeBrokersID

if(@total > 0)
begin	
	--Update CompanyClearingFirmsPrimeBrokers
	Update T_CompanyClearingFirmsPrimeBrokers 
	Set ClearingFirmsPrimeBrokersName = @clearingFirmsPrimeBrokersName, 
		ClearingFirmsPrimeBrokersShortName = @clearingFirmsPrimeBrokersShortName,
		CompanyID = @companyID
		
	Where CompanyClearingFirmsPrimeBrokersID = @companyClearingFirmsPrimeBrokersID
	
	Set @result = @companyClearingFirmsPrimeBrokersID
end
else
--Insert SymbolMapping
begin
	INSERT T_CompanyClearingFirmsPrimeBrokers(ClearingFirmsPrimeBrokersName, ClearingFirmsPrimeBrokersShortName, CompanyID)
	Values(@clearingFirmsPrimeBrokersName, @clearingFirmsPrimeBrokersShortName, @companyID)
	
	Set @result = scope_identity()
		--	end
end
select @result




