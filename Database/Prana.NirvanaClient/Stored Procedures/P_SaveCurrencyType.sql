
/****** Object:  Stored Procedure dbo.P_SaveCurrencyType    Script Date: 12/07/2005 9:00:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCurrencyType

	(
		@currencyTypeID int,
		@currencyType varchar(50),
		@result	int
	)
AS

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_CurrencyType
Where CurrencyTypeID = @currencyTypeID

if(@total > 0)
begin	
	select @count = count(*)
		from T_Currencytype
		Where CurrencyType = @currencyType AND CurrencyTypeID <> @currencyTypeID
		if(@count = 0)
		begin
			UPDATE T_CurrencyType 
			Set CurrencyType = @currencyType
			Where CurrencyTypeID = @currencyTypeID
			Set @result = @currencyTypeID
	end
	else
	begin
		Set @result = -1
	end
end
else
begin
select @count = count(*)
	from T_CurrencyType 
	Where CurrencyType = @currencyType
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT INTO T_CurrencyType(CurrencyType)
		Values (@currencyType) 
		Set @result = scope_identity()
	end
end

select @result

