
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_SMSymbolLookUpTable')
BEGIN 
Update T_SMSymbolLookUpTable
set SharesOutstanding = 0 where SharesOutstanding is Null

End
