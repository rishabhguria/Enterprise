


/****** Object:  Stored Procedure dbo.P_SaveContractListingType    Script Date: 08/12/2005 6:00:22 PM ******/
CREATE PROCEDURE dbo.P_SaveContractListingType

	(
		@contractListingTypeID int,
		@contractListingType varchar(50),
		@result	int
	)
AS

if @contractListingTypeID > 0
begin

	UPDATE T_ContractListingType 
	Set ContractListingType = @contractListingType

	Where ContractListingTypeID = @contractListingTypeID

	Set @result = @contractListingTypeID
end
else
begin

declare @total int
	set @total = 0
	
	select @total = count(*)
	from T_ContractListingType 
	Where ContractListingTypeID = @contractListingTypeID	
	
	if(@total > 0)
	begin
		Set @result = -1
	end

INSERT INTO T_ContractListingType(ContractListingType)
Values (@contractListingType) 

Set @result = scope_identity()

end

select @result



