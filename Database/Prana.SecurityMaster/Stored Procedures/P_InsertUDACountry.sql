/*******************************************************************************
created by : omshiv
Date - 11 Jun 2014
Desc- increase the column length of UDA  

********************************************************************************/
create Procedure [dbo].[P_InsertUDACountry] 
(
@CountryName varchar(100),
@CountryID  int 
)
AS
 
DELETE FROM T_UDACountry where CountryID = @CountryID


INSERT INTO T_UDACountry(CountryName ,CountryID)
VALUES(@CountryName,@CountryID)

