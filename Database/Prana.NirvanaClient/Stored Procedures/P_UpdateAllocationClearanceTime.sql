





/****** Object:  Stored Procedure [P_UpdateClearanceTime]  7, '9/21/2006 5:00:00 PM' , '20'
	Script Date: 09/20/2006 4:00:23 PM ******/
CREATE PROCEDURE [dbo].[P_UpdateAllocationClearanceTime]	
(
	  @ClearanceTimeID int	
	, @ClearanceTime DATETIME	
	, @CompanyUserAUECID int
)

AS 

set nocount on
DECLARE @exists int

SELECT @exists =  
	COUNT(*) 
FROM 
	T_CompanyUserAUECClearanceTimeAllocation
where 
	CompanyUserAUECID = @CompanyUserAUECID

if(@exists = 1)
begin
	update 
		T_CompanyUserAUECClearanceTimeAllocation
	SET
		ClearanceTime = @ClearanceTime
	Where 
		CompanyUserAUECID = @CompanyUserAUECID
end
else
begin
	insert into 
		T_CompanyUserAUECClearanceTimeAllocation 
			(
			     CompanyUserAUECID
				, ClearanceTime)
		VALUES
			(
				@CompanyUserAUECID
				, @ClearanceTime)
end






