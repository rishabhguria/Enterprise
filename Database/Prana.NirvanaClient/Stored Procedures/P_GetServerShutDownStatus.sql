CREATE procedure P_GetServerShutDownStatus
as
select NormalShutDown from T_ServerShutDownLogs
update  T_ServerShutDownLogs 
set NormalShutDown =0