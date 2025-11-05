-- =============================================
-- Author:		<harshkumar>
-- Create date: <03/11/2006>
-- Description:	<Inserts sharedTradingAccount IDs corresponding to Baskets>
-- =============================================
CREATE PROCEDURE P_BTSaveSharedTradingAccounts
(
@basketID varchar(50),
@tradingAccountID int
)	
AS
insert 
T_BTSharedTradingAccounts(BasketID, AccountID)
values(@basketID,@tradingAccountID)
