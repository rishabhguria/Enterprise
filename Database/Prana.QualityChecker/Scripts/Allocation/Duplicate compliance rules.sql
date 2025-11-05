---Description :  To check duplicate mark price on the same same day


Declare @errormsg varchar(max)
set @errormsg=''

--Check for Duplicate Symbol, remove if duplicate symbol          
select count(*) DuplicateCount,RuleName,ruleid  into  #DuplicateRules  from T_CA_RulesUserDefined group by RuleName,ruleid
having count(*)>1


IF EXISTS(Select * from #DuplicateRules)
Begin

set @errormsg='Duplicate compliance rules.'
Select * from #DuplicateRules 
END



select @errormsg as ErrorMsg

Drop Table #DuplicateRules