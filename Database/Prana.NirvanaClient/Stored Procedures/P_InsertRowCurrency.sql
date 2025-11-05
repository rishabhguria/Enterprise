
/****** Object:  Stored Procedure dbo.P_InsertRowCurrency    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_InsertRowCurrency
	(
		@currencyID int,
		@currencyName varchar(50),
		@currencySymbol varchar(50)
	)
AS

--Declare @DeletedID int
--set @DeletedID = 1
declare @total int
if @currencyID > 0
begin
select @total = count(*)
	from T_Currency  
	Where CurrencySymbol = @currencySymbol AND CurrencyID <> @currencyID
	
	if(@total > 0)
	begin
		Set @currencyID = -1
	end
	else
	begin
		UPDATE T_Currency 
		Set CurrencyName = @currencyName, CurrencySymbol = @currencySymbol
		Where CurrencyID = @currencyID
	end
end
else
begin
	set @total = 0
	select @total = count(*)
	from T_Currency 
	Where CurrencySymbol = @currencySymbol
	
	if(@total > 0)
	begin
		Set @currencyID = -1
	end
	else
	begin
		INSERT INTO T_Currency(CurrencyName
		--,DeletedID
		,CurrencySymbol)
		Values (@currencyName, 
		--@DeletedID, 
		@currencySymbol)
		Set @currencyID = scope_identity()
	end
end
select @currencyID
