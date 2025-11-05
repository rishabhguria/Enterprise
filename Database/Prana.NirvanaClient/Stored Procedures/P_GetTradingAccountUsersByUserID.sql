


-- =============================================
-- Author:		<Ashish Poddar>
-- Create date: <12 Sept, 2006>
-- Description:	<To Get all users attached with the given trading account id>
-- =============================================
CREATE PROCEDURE [dbo].[P_GetTradingAccountUsersByUserID] 
	-- Add the parameters for the stored procedure here
	@userID int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	SELECT  CTA.TradingAccountID, CTA.CompanyUserID,CU.ShortName , CAUEC.AUECID
From T_CompanyUserTradingAccounts CTA  WITH (NOLOCK)
join T_CompanyUser CU WITH (NOLOCK) on CU.UserID = CTA.CompanyUserID  
join T_CompanyUserAUEC as CUAUEC WITH (NOLOCK) on CTA.CompanyUserID = CUAUEC.CompanyUserID  
join T_CompanyAUEC as CAUEC WITH (NOLOCK) on CUAUEC.CompanyAUECID = CAUEC.CompanyAUECID  
where CTA.TradingAccountID in
(Select CTA.TradingAccountID From T_CompanyUserTradingAccounts CTA  WITH (NOLOCK)
 
 where CTA.CompanyUserID = @userID)

order by CTA.CompanyUserID
END
