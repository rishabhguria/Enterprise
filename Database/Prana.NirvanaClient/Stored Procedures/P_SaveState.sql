/**********************************************

AUTHOR : RAHUL GUPTA       CREATED ON : 2012-03-28

************************************************/


CREATE PROCEDURE P_SaveState  
(@stateID int,   
@countryID int,  
@Name varchar(50)  
)  
  
AS  
  
DECLARE @Total int  
SET @Total = (SELECT COUNT(*) FROM T_State WHERE StateID = @stateID and CountryID = @countryID)  
  
IF(@Total > 0)  
BEGIN  
UPDATE T_State   
SET State = @Name WHERE StateID = @stateID and CountryID = @countryID  
END  
ELSE  
BEGIN   
INSERT INTO T_State(State, CountryID) values (@Name, @countryID)  
SET @StateID = SCOPE_IDENTITY()  
END  
SELECT @StateID  
  