-----------------------------------------------------  
--Created By: Bharat raturi  
--Date: 25/3/2014  
--Purpose: Get the details of the batch for the setup  
  
--Modified By: Bharat raturi  
--Date: 29/3/2014  
--Purpose: Get the details of the batch for the setup 

--Modified By: Narendra Jangir 
--Date: 2014-04-17
--Purpose: Get all the batches if @thirdPartyID=-1

--Modified By: Bharat raturi  
--Date: 8/may/2014  
--Purpose: Get the details of the batch for the setup where 'isActive' is true

--usage: P_GetAllBatchDetails 1
-----------------------------------------------------  
CREATE procedure [dbo].[P_GetAllBatchDetails]  
@thirdPartyID int  
as 
BEGIN
IF @thirdPartyID=-1
	select  
	BatchSchedulerID,  
	t1.FormatName,  
	FundID,  
	ThirdPartyTypeShortName,  
	EnablePriceTolerance,    
	PriceTolerance,   
	ScheduleTypeID,   
	ExecutionTime,  
	AutoExecution,   
	CronExpression,   
	t1.ThirdPartyID  
	from T_BatchSchedulers t1 inner JOIN  
	T_ImportFileSettings t5 on t1.BatchSchedulerID=t5.ImportFileSettingID  
	inner JOIN    
	(select ThirdPartyID, ThirdPartyName, t3.ThirdPartyTypeShortName   
	from T_ThirdParty t2  
	inner JOIN T_ThirdPartyType t3 on t2.ThirdPartyTypeID=t3.ThirdPartyTypeID) t4   
	ON t1.ThirdPartyID=t4.ThirdPartyID  where t5.IsActive=1 
ELSE
	select  
	BatchSchedulerID,  
	t1.FormatName,  
	FundID,  
	ThirdPartyTypeShortName,  
	EnablePriceTolerance,    
	PriceTolerance,   
	ScheduleTypeID,   
	ExecutionTime,  
	AutoExecution,   
	CronExpression,   
	t1.ThirdPartyID  
	from T_BatchSchedulers t1 inner JOIN  
	T_ImportFileSettings t5 on t1.BatchSchedulerID=t5.ImportFileSettingID  
	inner JOIN    
	(select ThirdPartyID, ThirdPartyName, t3.ThirdPartyTypeShortName   
	from T_ThirdParty t2  
	inner JOIN T_ThirdPartyType t3 on t2.ThirdPartyTypeID=t3.ThirdPartyTypeID) t4   
	ON t1.ThirdPartyID=t4.ThirdPartyID   
	where t1.ThirdPartyID=@thirdPartyID and t5.IsActive=1
END
 
