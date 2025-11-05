Create PROCEDURE [dbo].[P_GetFundWiseFieldActivites]
(
   @StartDate datetime,
   @EndDate datetime
)
AS
select FieldID,FieldName,FundID,DateValue,ActivityBase from T_FundWiseFieldActivites 
where DateValue between @StartDate and @EndDate
order by DateValue, FundID