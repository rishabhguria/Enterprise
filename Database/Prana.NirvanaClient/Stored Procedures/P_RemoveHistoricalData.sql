CREATE Procedure [dbo].[P_RemoveHistoricalData] (@hrs int)
As
Begin

    If (@hrs=0) 
    BEGIN
	CREATE TABLE #TempSets
	(
		[SetID] [int] IDENTITY(1,1) NOT NULL,
		SetCreatedOn datetime
	)
	Insert into #TempSets select distinct CreatedOn from T_PMDataDump order by CreatedOn DESC
	Delete  T_PMDataDump where CreatedOn <= (Select Top 1 SetCreatedOn from #TempSets where SetID >= 3)
	Drop  table #TempSets
    END
    If (@hrs>0) 
    BEGIN
        Delete T_PMDataDump where datediff(hh,CreatedOn,getdate()) > @hrs
    END
End

