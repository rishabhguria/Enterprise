create procedure P_CleanUpFundfromSymbolLevel(@FundID int)
as begin
	DELETE from T_SymbolLevelAccrualsJournal where FundID = @FundID
	DELETE from T_SymbolLevelAccrualsSubAccountBalances where FundID = @FundID
	DELETE from T_IntermediateSymbolLevelAccrualAllActivity where FundID = @FundID
	DELETE from [T_IntermediateSymbolLevelLastCalcDate] where FundID = @FundID
end