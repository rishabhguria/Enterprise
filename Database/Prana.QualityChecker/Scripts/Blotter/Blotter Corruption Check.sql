Declare @errormsg varchar(max) 

set @errormsg='' 

Select  *,Cast('Blank Stage Order ID' as Varchar(100)) as Errmsg into #tempSub from T_sub where stagedorderid not in(select Clorderid from T_sub ) and stagedorderid<>''
--select * from #tempSub
Insert into #tempSub
Select  *,'origclorderid is -2147483648' from T_sub where origclorderid not in(select Clorderid from T_sub ) and origclorderid<>'-2147483648'

IF EXISTS(Select * from #tempSub ) 
Begin 
SELECT* from #tempSub
Set @errormsg='Corruption in Blotter Data' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempSub