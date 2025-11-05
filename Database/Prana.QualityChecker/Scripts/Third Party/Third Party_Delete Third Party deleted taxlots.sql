Declare @errormsg varchar(max)
set @errormsg = ''

Declare @recordCount Int
Set @recordCount = 0

Set @recordCount = (Select Count(*) From T_DeletedTaxlots With (NOLOCK) 
Where TaxlotState = 1)

If (@recordCount > 0)
Begin
	Delete T_DeletedTaxlots Where TaxlotState = 1

	Set @errormsg = Convert(varchar(10),@recordCount) + ' Third Party Deleted Taxlots deleted'

	Select @errormsg AS [Message], @recordCount As [Record Count]
End
Select @errormsg As ErrorMsg
