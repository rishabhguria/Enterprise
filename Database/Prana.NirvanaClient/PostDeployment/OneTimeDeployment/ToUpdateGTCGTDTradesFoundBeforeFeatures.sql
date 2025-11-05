
Declare @ScriptAlreadyRun int
set @ScriptAlreadyRun=0

select @ScriptAlreadyRun=count(*) from T_DataScripts where scriptname = 'ToUpdateGTCGTDTradesFoundBeforeFeatures.sql'
select @ScriptAlreadyRun
if(@ScriptAlreadyRun = 0)
Begin
	Declare @count int
	set @count=0
	declare @Date datetime

	select top 1 @count=1  ,@Date= date  from T_dbversion where ( version like  '%2.12.%' or version like  '%2.14.%' ) order by revision ,date asc
	Select @count  ,@Date

	if(@count = 1)
	begin
	update  sub set timeinforce =0  from T_Sub  sub where  DATEDIFF(dd,InsertionTime,@Date)>0
    update  sub set TimeInForceID =0 from T_Fills sub where   DATEDIFF(dd,InsertionTime,@Date)>0
    update  sub set timeinforce =0  from T_tradedorders sub  where DATEDIFF(dd,InsertionTime,@Date)>0
	end
	else 
	begin 
  	update  sub set timeinforce =0  from T_Sub  sub where  DATEDIFF(dd,InsertionTime,GETDATE())>0
    update  sub set TimeInForceID =0 from T_Fills sub where   DATEDIFF(dd,InsertionTime,getdate())>0
    update  sub set timeinforce =0  from T_tradedorders sub  where DATEDIFF(dd,InsertionTime,GETDATE())>0
	end 
End

