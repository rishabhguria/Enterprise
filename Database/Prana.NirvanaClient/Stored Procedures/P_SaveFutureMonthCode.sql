


/****** Object:  Stored Procedure dbo.P_SaveFutureMonthCode    Script Date: 09/12/2005 12:00:22 PM ******/
CREATE PROCEDURE dbo.P_SaveFutureMonthCode

	(
		@futureMonthCodeID int,
		@futureMonth varchar(50),
		@abbreviation varchar(10),
		@result	int
	)
AS

if @futureMonthCodeID > 0
begin

	UPDATE T_FutureMonthCode 
	Set FutureMonth = @futureMonth,
		Abbreviation = @abbreviation

	Where FutureMonthCodeID = @futureMonthCodeID

	Set @result = @futureMonthCodeID
end
else
begin

declare @total int
	set @total = 0
	
	select @total = count(*)
	from T_FutureMonthCode 
	Where FutureMonthCodeID = @futureMonthCodeID	
	
	if(@total > 0)
	begin
		Set @result = -1
	end

INSERT INTO T_FutureMonthCode(FutureMonth, Abbreviation)
Values (@futureMonth, @abbreviation) 

Set @result = scope_identity()

end

select @result



