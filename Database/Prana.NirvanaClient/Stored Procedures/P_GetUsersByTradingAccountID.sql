
-- =============================================
-- Author:		<Ashish Poddar>
-- Create date: <12 Sept, 2006>
-- Description:	<To Get all users attached with the given trading account id>
-- =============================================
CREATE PROCEDURE [dbo].[P_GetUsersByTradingAccountID] 
	-- Add the parameters for the stored procedure here
	@tradingAccountID int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  CTA.CompanyUserID,CU.ShortName
From T_CompanyUserTradingAccounts CTA
join T_CompanyUser CU on CU.UserID = CTA.CompanyUserID

where CTA.TradingAccountID = @tradingAccountID
END

