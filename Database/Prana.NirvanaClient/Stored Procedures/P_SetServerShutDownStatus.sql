CREATE procedure P_SetServerShutDownStatus
as

if(select count(*)  from T_ServerShutDownLogs)=0
insert into T_ServerShutDownLogs(NormalShutDown) values(1)

else
update  T_ServerShutDownLogs 
set NormalShutDown =1

