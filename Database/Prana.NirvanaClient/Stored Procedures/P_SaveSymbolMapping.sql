
/****** Object:  Stored Procedure dbo.P_SaveSymbolMapping    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveSymbolMapping
(
		@cvSymbolMappingID int,
		@cvAUECID int,
		@symbol varchar(50),
		@mappedSymbol varchar(50)
		
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CVSymbolMapping 
Where CVSymbolMappingID = @cvSymbolMappingID

if(@total > 0)
begin	
	--Update CVSymbolMapping
	Update T_CVSymbolMapping 
	Set CVAUECID = @cvAUECID, 
		Symbol = @symbol, 
		MappedSymbol = @mappedSymbol
		
	Where CVSymbolMappingID = @cvSymbolMappingID
	
	Set @result = @cvSymbolMappingID
end
else
--Insert CVSymbolMapping
begin
	INSERT T_CVSymbolMapping(CVAUECID, Symbol, MappedSymbol)
	Values(@cvAUECID, @symbol, @mappedSymbol)
	
	Set @result = scope_identity()
		--	end
end
select @result


