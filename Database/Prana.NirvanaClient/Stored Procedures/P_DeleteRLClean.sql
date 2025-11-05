CREATE PROCEDURE dbo.P_DeleteRLClean
(
	@Dummy int
)
AS
	
	DECLARE @RLID int
	
		
	
DECLARE c CURSOR
FOR SELECT     RLID 
    FROM         T_RoutingLogic
    WHERE     (Name = '')

OPEN c
FETCH c INTO @RLID

WHILE (@@FETCH_STATUS=0) 
	BEGIN	
		EXEC P_DeleteRLogic @RLID		

		FETCH c INTO @RLID
	END
	
CLOSE c
DEALLOCATE c

SELECT 1
