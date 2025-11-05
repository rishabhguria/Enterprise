

-- =============================================
-- Author:		Ashish Poddar
-- Create date: 22 Feb, 2007
-- Description:	To Update UserID and Status in trading instructions.
-- =============================================
Create PROCEDURE [dbo].[P_UpdateTradingInstruction]
	-- Add the parameters for the stored procedure here
	@ClOrderID varchar(50),
	@UserID int,
	@Status int
AS
set nocount on
DECLARE @exists int

SELECT @exists =  
	COUNT(*) 
FROM 
	T_TradingInstructions
where 
	ClOrderID = @ClOrderID

if(@exists = 1)

BEGIN
	
Update  
	T_TradingInstructions

Set 
	CompanyUserID = @UserID,
	IsAccepted = @Status

where ClOrderID = @ClOrderID
END


