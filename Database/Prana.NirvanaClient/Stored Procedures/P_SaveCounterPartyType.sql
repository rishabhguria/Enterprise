
/****** Object:  Stored Procedure dbo.P_SaveCounterPartyType    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveCounterPartyType
(
		@counterPartyTypeID int,
		@counterPartyType varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_CounterPartyType
Where CounterPartyTypeID = @counterPartyTypeID

if(@total > 0)
begin	
	select @count = count(*)
		from T_CounterPartyType
		Where CounterPartyType = @counterPartyType AND CounterPartyTypeID <> @counterPartyTypeID
		if(@count = 0)
		begin
			Update T_CounterPartyType 
			Set CounterPartyType = @counterPartyType
			Where CounterPartyTypeID = @counterPartyTypeID
			Set @result = @counterPartyTypeID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_CounterPartyType 
	Where CounterPartyType = @counterPartyType
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_CounterPartyType(CounterPartyType)
		Values(@counterPartyType)
		Set @result = scope_identity()
	end
end
select @result
