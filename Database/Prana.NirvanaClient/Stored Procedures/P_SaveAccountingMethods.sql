Create    procedure P_SaveAccountingMethods        
(        
@accMethodData varbinary(max)      
)        
as        
 
if((select count(*) from T_AccountingMethods) > 0 )     
begin 
update T_AccountingMethods
set 
AccountingMethodData = @accMethodData
where ID is not null     
end
else
begin
insert into T_AccountingMethods(AccountingMethodData)         
values(@accMethodData) 
end

-- P_AccountingMethods 'abhilash katiayr'
-- select * from T_AccountingMethods
        
      
        
        
        