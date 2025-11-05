Declare @Date datetime 
Declare @FromDate datetime 
Declare @ToDate datetime 
Declare @errormsg varchar(max) 



set @FromDate='' 
set @ToDate='' 
set @errormsg='' 


select  distinct g.Symbol,s.CompanyName,s.AssetName INTO #tempSymbolDisc from T_Group g
Inner Join T_Level2allocation l
on g.Groupid= l.Groupid
Inner join v_secmasterdata s
on s.tickersymbol= g.Symbol
where l.Level2id=0 order by symbol

IF EXISTS(Select * from #tempSymbolDisc ) 
Begin 
SELECT* from #tempSymbolDisc
Set @errormsg='Traded Symbol with Unallocated Strategy!' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempSymbolDisc
