


/****** Object:  Stored Procedure dbo.P_SaveCountryFlag    Script Date: 02/02/2006 11:00:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCountryFlag
	(
		@countryFlagID				int, 
		@countryFlagName				varchar(50), 
		@CountryFlagImage				image 
	)
AS

Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CountryFlag
Where CountryFlagID = @countryFlagID
if(@total > 0) 
begin
	--Update
	Update T_CountryFlag
	Set CountryFlagName = @countryFlagName, 
		CountryFlagImage = @countryFlagImage
		
	Where CountryFlagID = @countryFlagID
	Set @result = @countryFlagID
end
else
begin
	--Insert
	Insert into T_CountryFlag(CountryFlagName, CountryFlagImage)
	Values(@CountryFlagName, @CountryFlagImage)
                    
   Set @countryFlagID = scope_identity()
end
	Select @countryFlagID
	


