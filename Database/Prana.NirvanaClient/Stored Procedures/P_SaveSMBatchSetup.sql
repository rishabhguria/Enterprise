--Modified BY: Bhavana    
--Date: 15-july-14    
--Purpose: filter clause field added in T_SMBatchSetup    
-----------------------------------------------------------------    
-----------------------------------------------------------------    
--Created BY: Bharat Raturi    
--Date: 15-may-14    
--Purpose: Get the details of the SM Batch saved in the DB    
-----------------------------------------------------------------    
    
CREATE procedure [dbo].[P_SaveSMBatchSetup]    
(    
@SMbatchSetupID int,    
@systemLevelName varchar(100),    
@userDefinedName varchar(100),    
@fundIDs varchar(max),    
@fields varchar(max),    
@isHistoric bit,    
@noOfHistoricDaysRequired int,        
@runTimeType int,    
@cronExpression varchar(100),    
@filterClause varchar(max),  
@indices varchar(max),
@batchType int
)    
as      
declare @idCount int      
set @idCount=0      
select @idCount=COUNT(*) from T_SMBatchSetup where SMBatchID=@SMbatchSetupID      
begin      
IF (@idCount>0)      
begin      
update T_SMBatchSetup      
SET SystemLevelName=@systemLevelName,      
CronExpression=@cronExpression,      
RunTimeTypeID=@runTimeType,      
FundID=@fundIDs,      
IsHistoricDataRequired=@isHistoric,    
DaysOfHistoricData=@noOfHistoricDaysRequired,    
UserDefinedName=@userDefinedName,      
Fields=@fields,    
FilterClause=@filterClause,  
Indices=@indices   ,
BatchType=@batchType
where SMBatchID=@SMbatchSetupID      
set @idCount=@SMbatchSetupID      
end      
else      
begin       
insert INTO T_SMBatchSetup      
(SystemLevelName,CronExpression,RunTimeTypeID,FundID,IsHistoricDataRequired,DaysOfHistoricData,UserDefinedName,Fields,FilterClause,Indices,BatchType)      
VALUES(@systemLevelName, @cronExpression, @runTimeType, @fundIDs,@isHistoric,@noOfHistoricDaysRequired,@userDefinedName,@fields,@filterClause,@indices,@batchType)      
set @idCount=scope_identity()      
end      
      
end      
      
select @idCount      
