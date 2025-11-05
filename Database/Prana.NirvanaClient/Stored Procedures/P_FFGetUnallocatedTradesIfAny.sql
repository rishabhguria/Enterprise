


/*
Name :<P_FFGetUnallocatedTradesIfAny>
Created By : <Kanupriya>
Date :<12/05/2006>
Purpose: <To check whether any unallocated trades exists or not.>

*/

CREATE PROCEDURE [dbo].[P_FFGetUnallocatedTradesIfAny]
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
	
declare @result int

 if exists(
	select * from T_Fills 
	where
	T_Fills.OrderID not in (select ClOrderID from T_EntityOrder)
)
begin
set @result = 1
end
else
begin 
set @result =0
end

select @result


