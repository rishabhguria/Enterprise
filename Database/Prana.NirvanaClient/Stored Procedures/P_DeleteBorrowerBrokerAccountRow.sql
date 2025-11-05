CREATE PROCEDURE [dbo].[P_DeleteBorrowerBrokerAccountRow]
	@param int
AS
Delete From T_ShortLocateBrokerAccountMapping where BrokerID = @param
RETURN 0
