-----------------------------------------------------------------  
--modified BY: Omshiv  
--Date: 1/11/14  
--Purpose: Get Fund Wise workflow status 
-----------------------------------------------------------------  
CREATE procedure [dbo].[P_GetFundWorkflowData]
(
@StartDate datetime,
@fundIDs varchar(max),
@isSearchByFileExecutionDate bit
   
)
as

declare @handle int  
exec sp_xml_preparedocument @handle output, @FundIDs  
  
create table #FundIDs  
(fundID int)  
  
insert INTO #FundIDs(fundID)  
  
select fundID from openXML(@handle,'dsFund/dtFund',2)  
with  
(FundID int)

CREATE TABLE #Temp                                          
 (                                                                                           
  CompanyFundID int                 
  ,FormatName Varchar(200)
  ,FormatType int                                
  ,WorkflowID int                  
 )                                                 
                                                                                          
 INSERT INTO #Temp                                
 (                                                                               
   CompanyFundID                  
  ,FormatName
  ,FormatType                                
  ,WorkflowID                  
  ) 
                                                                                      
SELECT
F.CompanyFundID,
FS.FormatName,
FormatType,
COALESCE(WF.WorkflowID,0) as WorkflowID
FROM T_CompanyFunds as F  
inner JOIN T_ImportFileSettings as I ON I.FundID = F.CompanyFundID
inner JOIN T_BatchSchedulers as FS ON FS.BatchSchedulerID = I.ImportFileSettingID
cross JOIN T_Workflows as WF
WHERE F.IsActive = 1 AND I.IsActive=1 and WF.WorkflowID in (1,2,3,9) and I.FormatType=0
and F.CompanyFundID in (select fundID from #FundIDs)
--ORDER BY  F.CompanyFundID,I.FormatType,I.FormatName

Union 
SELECT
F.CompanyFundID,
FS.FormatName,
FormatType,
COALESCE(WF.WorkflowID,0) as WorkflowID
FROM T_CompanyFunds as F  
inner JOIN T_ImportFileSettings as I ON I.FundID = F.CompanyFundID
inner JOIN T_BatchSchedulers as FS ON FS.BatchSchedulerID = I.ImportFileSettingID
cross JOIN T_Workflows as WF
WHERE F.IsActive = 1 AND I.IsActive=1 and WF.WorkflowID in (4) and I.FormatType=1
and F.CompanyFundID in (select fundID from #FundIDs)

-- ORDER BY  CompanyFundID,FormatType,FormatName



SELECT DISTINCT
F.CompanyFundID,
COALESCE(W.StateID,7) as StatusID,
COALESCE(W.TaskID,0) as WorkflowID,
COALESCE(Comments,'') as  Comments,
COALESCE(W.Date,'1800-01-01') as  Date,
COALESCE(W.TaskRunTime,'1800-01-01') as  TaskRunTime,
COALESCE(W.ContextValue,'') as  ContextValue
into #TempWorkflowData
FROM T_CompanyFunds as F  
left join T_FundWorkflowStats as W ON W.FundId = F.CompanyFundID
WHERE F.IsActive = 1 AND DATEDIFF(d,Date,@StartDate)=0
order by StatusID,WorkflowID

select 
T.CompanyFundID                  
,T.FormatName
,T.WorkflowID,
COALESCE(W.StatusID,7) as StatusID,
COALESCE(Comments,'') as  Comments,
COALESCE(W.Date,@StartDate) as  Date,
COALESCE(W.TaskRunTime,@StartDate) as  TaskRunTime
 from #Temp T
left JOIN #TempWorkflowData W ON 
T.CompanyFundID = W.CompanyFundID and T.WorkflowID = W.WorkflowID and T.FormatName = W.ContextValue
ORDER BY  T.CompanyFundID,T.FormatName,T.WorkflowID

--select * from #Temp

drop TABLE #Temp,#TempWorkflowData
exec sp_xml_removedocument @handle



