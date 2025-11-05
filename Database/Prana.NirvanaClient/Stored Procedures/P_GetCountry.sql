create PROCEDURE [dbo].[P_GetCountry]
(
		@countryID int,
		@countryName varchar(50)
	)
AS
if(@countryID<>-1)
begin
	Select CountryID, CountryName
	From T_Country
	Where CountryID = @countryID
end
else
begin
    Select CountryID, CountryName
	From T_Country
	Where CountryName = @countryName
end

