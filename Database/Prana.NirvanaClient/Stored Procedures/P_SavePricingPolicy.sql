
Create PROCEDURE [dbo].[P_SavePricingPolicy]
(@xmlDoc ntext, @deletedIds varchar(500))
as 
declare @result int 
declare @handle int
set @result = 0
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc
IF len(@deletedIds) > 0
BEGIN
CREATE TABLE #TEMPIDSFORDELETION( pricingid int)

INSERT INTO #TEMPIDSFORDELETION (pricingid)
    SELECT Items
    FROM [dbo].[Split] (@deletedIds, ',') 
END

create table #TempPricingPolicy
(
	Id int,
	IsActive bit,
	PolicyName varchar(200),
	SPName varchar(200),
	IsFileAvailable bit,
	FilePath varchar(200),
	FolderPath varchar(200),
	IsModified bit
)
insert into #TempPricingPolicy
(Id,IsActive,PolicyName,SPName,IsFileAvailable,FilePath,FolderPath,IsModified)
SELECT Id,IsActive,PolicyName,SPName,IsFileAvailable,FilePath,FolderPath,IsModified
from openxml(@handle,'/dsPricing/dtPricing',2) 
with
(
	Id int,
	IsActive bit,
	PolicyName varchar(200),
	SPName varchar(200),
	IsFileAvailable bit,
	FilePath varchar(200),
	FolderPath varchar(200),
	IsModified bit
)
begin try


if len(@deletedIds) > 0
begin
set @result= (select count(*) from T_PricingPolicy where Id in (select pricingid from #TEMPIDSFORDELETION)and IsActive = 1)
IF(@result = 0)
begin 
delete  from T_PricingPolicy where Id in (select pricingid from #TEMPIDSFORDELETION)and IsActive = 0
end
END

insert into T_PricingPolicy(IsActive,PolicyName,SPName,IsFileAvailable,FilePath,FolderPath)
select  IsActive,PolicyName,SPName,IsFileAvailable,FilePath,FolderPath from #TempPricingPolicy where IsModified = 0


update T_PricingPolicy
set 
	IsActive = #TempPricingPolicy.IsActive,
	PolicyName = #TempPricingPolicy.PolicyName,
	SPName = #TempPricingPolicy.SPName,
	IsFileAvailable = #TempPricingPolicy.IsFileAvailable,
	FilePath = #TempPricingPolicy.FilePath,
	FolderPath = #TempPricingPolicy.FolderPath 
from T_PricingPolicy
inner join #TempPricingPolicy 
on T_PricingPolicy.Id = #TempPricingPolicy.Id 
where #TempPricingPolicy.IsModified = 1

exec sp_xml_removedocument @handle 
end try
begin catch
set @result=-2
end catch


SELECT @result
