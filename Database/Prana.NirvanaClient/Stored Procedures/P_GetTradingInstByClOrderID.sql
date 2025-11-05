

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_GetTradingInstByClOrderID]
	-- Add the parameters for the stored procedure here
@ClOrderID varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
ClOrderID,            

Quantity  ,           
Symbol     ,          
SideTagValue    ,

TradingAccountID     ,
CompanyUserID        ,
Instructions                 ,

ClientOrderID        ,
IsAccepted     ,
MsgType     ,
OnBehalfOfCompID 

from T_TradingInstructions
where
ClOrderID = @ClOrderID

END


