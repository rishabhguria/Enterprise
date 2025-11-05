
-- =============================================
-- Author:		<harshkumar>
-- Create date: <03/11/2006>
-- Description:	<Deletes shared prefs corresponding to a basket >
-- =============================================
CREATE PROCEDURE [dbo].[P_BTDeleteSharedTradingAccounts] 
(
@basketID varchar(50)
)
AS
delete from T_BTSharedTradingAccounts where BasketID=@basketID
