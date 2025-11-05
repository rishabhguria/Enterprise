/********************************************  
  
AUTHOR : RAHUL GUPTA     CREATED ON : 2012-03-28  

MODIFIED BY : RAHUL GUPTA	
MODIFIED DATE : 2013-02-07 
DESCRIPTION : 
Checking T_Country table has identity column or not and if it exists then setting IDENTITY_INSERT ON  
********************************************/   
  
CREATE PROCEDURE P_SaveSMCountryDetails(    
@countryID int,    
@Name varchar(50)    
)    
AS    
    
DECLARE @Identity bit
SET @Identity = 0
DECLARE @Total int    
SET @Total = (SELECT COUNT(*) FROM T_Country WHERE CountryID = @countryID)    
    
IF(@Total > 0)    
BEGIN    
UPDATE T_Country     
SET CountryName = @Name WHERE CountryID = @countryID    
END    
ELSE    
BEGIN 
  SELECT @Identity = dbo.F_IsIdentityExists('T_Country')
  IF(@Identity = 1)
  SET IDENTITY_INSERT T_Country ON     
INSERT INTO T_Country(CountryID, CountryName) values (@countryID,@Name)    
END    

