
-- =============================================
-- Author:		<harshkumar	>
-- Create date: <03/11/2006>
-- Description:	<Gets shares Trading Accounts for a given basket>
-- =============================================
CREATE PROCEDURE [dbo].[P_BTGetSharedTradingAccounts]
(
@basketID varchar(50)
)
AS
select AccountID from T_BTSharedTradingAccounts 
where BasketID = @basketID
