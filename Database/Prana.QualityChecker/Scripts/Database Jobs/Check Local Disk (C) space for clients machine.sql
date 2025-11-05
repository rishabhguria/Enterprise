/*

Author:  Sachin Mishra
Created Date 1/30/2018

Purpose: TO detect the server space and server IP if less than 100gb

*/

Declare @Date datetime
Declare @FromDate datetime
Declare @ToDate datetime
Declare @errormsg varchar(max)

Set @errormsg=''
Set @FromDate=''
Set @ToDate=''


DECLARE @ip_address       varchar(15)
DECLARE @tcp_port         int 
DECLARE @connectionstring nvarchar(max) 
DECLARE @parm_definition  nvarchar(max)
DECLARE @command          nvarchar(max)

SET @connectionstring = N'Server=tcp:' + @@SERVERNAME + ';Trusted_Connection=yes;'
SET @parm_definition  = N'@ip_address_OUT varchar(15) OUTPUT
                       , @tcp_port_OUT   int         OUTPUT';

SET @command          = N'SELECT  @ip_address_OUT = a.local_net_address,
                                 @tcp_port_OUT   = a.local_tcp_port
                         FROM OPENROWSET(''SQLNCLI''
                                , ''' + @connectionstring + '''
                                , ''SELECT local_net_address
                                         , local_tcp_port
                                    FROM sys.dm_exec_connections
                                    WHERE session_id = @@spid
                                  '') as a'

EXEC SP_executeSQL @command
                , @parm_definition
                , @ip_address_OUT = @ip_address OUTPUT
                , @tcp_port_OUT   = @tcp_port OUTPUT;




-- space less than 100GB then send e-mail alert
create table #temp
(
drive varchar(10),
MBFree bigint
)

insert into #temp
EXEC master..xp_fixeddrives

if exists(select * from #temp where drive ='C' and MBFree<100000) -- space less than 100GB then send e-mail alert
begin 
set @errormsg='Disk space is less than 100GB on this server, Please clean some files.'
end

IF  ( @errormsg <> '')
BEGIN
SELECT @errormsg as [Message], @ip_address as ServerIP
END

select @errormsg as errormsg
drop table #temp