
CREATE procedure P_SaveFundDefaults        
(        
@DefaultID varchar(200),        
@defaultName varchar(500),        
     
@defaultAllocation varbinary(max)      
)        
as        
if((select count(*)from T_FundDefault where DefaultID=@DefaultID) =0)        
insert into T_FundDefault(DefaultID,DefaultName,DefaultAllocation)         
values(@DefaultID,@defaultName,@defaultAllocation)        
else        
update  T_FundDefault        
set         
DefaultName=@defaultName,        
DefaultAllocation=@defaultAllocation      
where DefaultID=@DefaultID 

