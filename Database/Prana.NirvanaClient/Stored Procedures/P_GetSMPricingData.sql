
-- [dbo].[P_GetSMPricingData] 'y','Bid,Ask','2014-08-20 00:00:00.000','2014-08-26 00:00:00.000'
CREATE Procedure [dbo].[P_GetSMPricingData]
(
@Symbol nvarchar(100),
@SecurityFields nvarchar(max),
@StartDate datetime,
@EndDate datetime
)
As

Create Table #Fields                                                                                                                     
(                                                                                                                    
Field nvarchar(50)                                                                                                                
) 
declare @condition varchar(max)
declare @query varchar(max)
set @condition = ''

Insert into #Fields                                                                  
Select Cast(Items as nvarchar) from dbo.Split(@SecurityFields,',') 

IF (@Symbol is not null and @Symbol<>'')
begin
set @condition = ' and Symbol like ''' + '%' + @Symbol + '%'''
end

IF (@SecurityFields is not null and @SecurityFields<>'')
begin
set @condition = @condition +' and PricingType in (select Field from #Fields)'
end

IF (@StartDate is not null and @StartDate<>'')
begin
set @condition = @condition + ' and Date >=''' +  CONVERT(VARCHAR(100),@StartDate, 101) +''''
end

IF (@EndDate is not null and @EndDate<>'')
begin
set @condition = @condition + ' and Date <=''' + CONVERT(VARCHAR(100),@EndDate, 101) +''''
end

set @query = 'SELECT * from V_SMPricingData where 1=1'  + @condition
exec (@query)

