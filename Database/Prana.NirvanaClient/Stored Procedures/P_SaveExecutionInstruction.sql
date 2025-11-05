
/****** Object:  Stored Procedure dbo.P_SaveExecutionInstruction    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveExecutionInstruction
(
		@executionInstructionsID int,	
		@executionInstructions varchar(50),
		@executionInstructionsTagValue varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_ExecutionInstructions
Where ExecutionInstructionsID = @executionInstructionsID

if(@total > 0)
begin	
	select @count = count(*)
		from T_ExecutionInstructions
		Where (ExecutionInstructions = @executionInstructions OR ExecutionInstructionsTagValue = @executionInstructionsTagValue) AND ExecutionInstructionsID <> @executionInstructionsID
		if(@count = 0)
		begin
			Update T_ExecutionInstructions
			Set ExecutionInstructions = @executionInstructions,
				ExecutionInstructionsTagValue = @executionInstructionsTagValue
			Where ExecutionInstructionsID = @executionInstructionsID
			Set @result = @executionInstructionsID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_ExecutionInstructions 
	Where (ExecutionInstructions = @executionInstructions OR ExecutionInstructionsTagValue = @executionInstructionsTagValue)
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
	INSERT T_ExecutionInstructions(ExecutionInstructions, ExecutionInstructionsTagValue)
	Values(@executionInstructions, @executionInstructionsTagValue)
        Set @result = scope_identity()
	end
end
select @result
