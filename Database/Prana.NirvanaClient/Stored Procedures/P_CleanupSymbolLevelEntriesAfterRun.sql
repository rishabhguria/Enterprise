CREATE PROCEDURE [dbo].[P_CleanupSymbolLevelEntriesAfterRun]
	@FundId int 
	
AS
	Declare @SymbolWiseRevaluationDate DateTime
	Select @SymbolWiseRevaluationDate = SymbolWiseRevaluationDate from T_CashPreferences where FundID=@FundId
	Delete from T_SymbolLevelAccrualsJournal where FundID=@FundId and DATEDIFF(d,TransactionDate,@SymbolWiseRevaluationDate)>=0
	Delete from T_SymbolLevelAccrualsSubAccountBalances where FundID=@FundId and DATEDIFF(d,TransactionDate,@SymbolWiseRevaluationDate)>0

