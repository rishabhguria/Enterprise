/***************************************************

AUTHOR : RAHUL GUPTA     CREATED ON: 2012-03-28

***************************************************/


CREATE PROCEDURE P_SaveCountry  
(@countryID int,   
@Name varchar(50)  
)  
  
AS  
  
DECLARE @Total int  
SET @Total = (SELECT COUNT(*) FROM T_Country WHERE CountryID = @countryID)  
  
IF(@Total > 0)  
BEGIN  
UPDATE T_Country   
SET CountryName = @Name WHERE CountryID = @countryID  
END  
ELSE  
BEGIN   
INSERT INTO T_Country(CountryName) values (@Name)  
SET @CountryID = SCOPE_IDENTITY()  
END  
SELECT @CountryID  