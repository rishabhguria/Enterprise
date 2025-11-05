CREATE proc P_BTGetBenchmarkByFilterTypeID
(
@filterTypeID int
)
AS
select BenchmarkID, BenchmarkName from T_BTBenchMarks
where FilterTypeID = @filterTypeID