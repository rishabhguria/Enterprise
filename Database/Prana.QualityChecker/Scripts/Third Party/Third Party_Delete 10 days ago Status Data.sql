Declare @errormsg varchar(max)
set @errormsg = ''

Declare @recordCount Int
Set @recordCount = 0

Declare @numDays Int
Set @numDays = 10

Declare @dbName VARCHAR(MAX)
Select @dbName = DB_NAME()

Set @recordCount = (Select Count(*) From T_PBWiseTaxlotState TPB With (NOLOCK) Inner Join V_Taxlots VT With (NOLOCK) On VT.TaxlotID = TPB.TaxlotID Where DateDiff(Day,VT.AUECLocalDate,getdate()) > @numDays)

Delete TPB From T_PBWiseTaxlotState TPB
Where TPB.TaxlotID Not In
(
Select TaxlotID From V_TaxLots
)

If (@recordCount > 0 and @dbName<>'SSPV1.7.1')
Begin
	Delete TPB From T_PBWiseTaxlotState TPB Inner Join V_Taxlots VT On VT.TaxlotID = TPB.TaxlotID Where DateDiff(Day,VT.AUECLocalDate,getdate()) > @numDays  

	Set @errormsg = Convert(varchar(10),@numDays) + ' days ago data deleted from Third Party'

	Select @errormsg AS [Message], @recordCount As [Record Count]
End
Select @errormsg As ErrorMsg
