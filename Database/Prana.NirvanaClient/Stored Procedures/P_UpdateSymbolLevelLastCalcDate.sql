create proc [dbo].[P_UpdateSymbolLevelLastCalcDate](@Xml nText)

as
begin
	DECLARE @handle int
	EXEC sp_xml_preparedocument @handle OUTPUT,@Xml
	create table #tempLastCalcDateUpdate
	(
		AccountID int,
		SubAcID int,
		CurrencyId int,
		TransactionDate DateTime
	)

	INSERT INTO #tempLastCalcDateUpdate(AccountID, SubAcID, CurrencyId, TransactionDate)
	SELECT
		AccountID, 
		SubAcID, 
		CurrencyId, 
		TransactionDate                                               
	FROM OPENXML(@handle, '/NewDataSet/LastCalcDate',2)
	WITH
	(   
		AccountID int,
		SubAcID int,
		CurrencyID int,
		TransactionDate DateTime
	)
	--select * from #tempLastCalcDateUpdate
	EXEC sp_xml_removedocument @handle

	Update symbolLevel set symbolLevel.LastCalcDate=temp.TransactionDate from #tempLastCalcDateUpdate temp inner join 
	[T_IntermediateSymbolLevelLastCalcDate] symbolLevel on symbolLevel.FundId=temp.AccountID and symbolLevel.SubAcID=temp.SubAcID
	and symbolLevel.CurrencyID=temp.CurrencyId where datediff(d,symbolLevel.LastCalcDate,temp.TransactionDate)<=0
end