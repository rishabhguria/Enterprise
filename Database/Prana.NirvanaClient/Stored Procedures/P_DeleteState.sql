/********************************************

AUTHOR : RAHUL GUPTA    CREATED ON : 2012-03-28

*******************************************/ 

CREATE PROCEDURE P_DeleteState    
(  
@stateID int    
)    
    
AS    
    
DECLARE @Total int    
SET @Total = (SELECT COUNT(*) FROM T_State WHERE StateID = @stateID)    
    
IF(@Total > 0)    
DELETE T_State WHERE StateID = @stateID    
    
   