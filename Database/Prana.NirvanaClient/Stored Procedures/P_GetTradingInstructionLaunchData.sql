

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_GetTradingInstructionLaunchData]
	-- Add the parameters for the stored procedure here
(
	@UserID int

)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		TI.ClOrderID ,
		TI.Symbol ,
		TI.Quantity	,
		TI.Instructions ,
		TI.ClientOrderID ,
		TI.CompanyUserID ,
		TI.TradingAccountID ,
		TI.SideTagValue,
		TI.IsAccepted,
		TI.MsgType
from T_TradingInstructions as TI

join T_CompanyUserTradingAccounts as CUTA on  CUTA.TradingAccountID = TI.TradingAccountID
where CUTA.CompanyUserID  = @UserID -- orders corresponding to the UserID given
and TI.IsAccepted = 0 -- Select only the onces which have not been accepted or rejected yet.

END


