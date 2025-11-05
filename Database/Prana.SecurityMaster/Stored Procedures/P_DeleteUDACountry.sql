CREATE Procedure [dbo].[P_DeleteUDACountry]
(
 @CountryID int
)

as

UPDATE T_SMSymbolLookUpTable
SET UDACountryID = -2147483648
WHERE UDACountryID = @CountryID
 
DELETE FROM T_UDACountry 
WHERE CountryID = @CountryID
