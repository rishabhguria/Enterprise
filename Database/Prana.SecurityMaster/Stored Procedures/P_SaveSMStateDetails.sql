/********************************************  
  
AUTHOR : RAHUL GUPTA     CREATED ON : 2012-03-28  
  
MODIFIED BY : RAHUL GUPTA	
MODIFIED DATE : 2013-02-07 
DESCRIPTION : 
Checking T_State table has identity column or not and if it exists then setting IDENTITY_INSERT ON  
********************************************/   
  
CREATE PROCEDURE P_SaveSMStateDetails    
(@stateID int,       
@countryID int,      
@Name varchar(50)      
)      
      
AS      
      
DECLARE @Identity bit
SET @Identity = 0
DECLARE @Total int      
SET @Total = (SELECT COUNT(*) FROM T_State WHERE StateID = @stateID and CountryID = @countryID)      
      
IF(@Total > 0)      
BEGIN      
UPDATE T_State       
SET State = @Name WHERE StateID = @stateID and CountryID = @countryID      
END      
ELSE      
BEGIN 
  SELECT @Identity = dbo.F_IsIdentityExists('T_State')
  IF(@Identity = 1)
  SET IDENTITY_INSERT T_State ON        
INSERT INTO T_State(StateID, State, CountryID) values (@stateID, @Name, @countryID)        
END   

