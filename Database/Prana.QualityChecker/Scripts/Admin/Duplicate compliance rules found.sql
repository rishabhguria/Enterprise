Declare @ToDate Datetime
Declare @ErrorMsg Varchar(Max)

set @ToDate=''
Set @ErrorMsg=''

select RuleName, PackageName, count(*) as NumberOfRecords into #temp from T_CA_RulesUserDefined group by RuleName,PackageName having count(*) >1


If Exists (Select * from #temp)
Begin
Select * from #temp
Set @ErrorMsg = @ErrorMsg + 'Duplicate compliance rules found in T_CA_RulesUserDefined table.'
End 

Select @ErrorMsg as ErrorMsg

Drop Table #temp