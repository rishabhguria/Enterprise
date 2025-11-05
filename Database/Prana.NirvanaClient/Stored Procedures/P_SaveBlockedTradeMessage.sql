CREATE PROCEDURE [dbo].[P_SaveBlockedTradeMessage] (@pranaMessage varchar(max))
AS

  INSERT INTO T_BlockedTradeHistory
    VALUES (GETDATE(), @pranaMessage)