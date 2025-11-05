
/****** Object:  Stored Procedure dbo.P_SaveHandlingInstruction    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveHandlingInstruction
(
		@handlingInstructionsID int,	
		@handlingInstructions varchar(50),
		@handlingInstructionsTagValue varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_HandlingInstructions
Where HandlingInstructionsID = @handlingInstructionsID

if(@total > 0)
begin	
	select @count = count(*)
		from T_HandlingInstructions
		Where (HandlingInstructions = @handlingInstructions OR HandlingInstructionsTagValue = @handlingInstructionsTagValue) AND HandlingInstructionsID <> @handlingInstructionsID
		if(@count = 0)
		begin
			Update T_HandlingInstructions
			Set HandlingInstructions = @handlingInstructions,
				HandlingInstructionsTagValue = @handlingInstructionsTagValue
			Where HandlingInstructionsID = @handlingInstructionsID
			Set @result = @handlingInstructionsID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_HandlingInstructions 
	Where (HandlingInstructions = @handlingInstructions OR HandlingInstructionsTagValue = @handlingInstructionsTagValue)
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_HandlingInstructions(HandlingInstructions, HandlingInstructionsTagValue)
		Values(@handlingInstructions, @handlingInstructionsTagValue)
	        
			Set @result = scope_identity()
	end
end
select @result
