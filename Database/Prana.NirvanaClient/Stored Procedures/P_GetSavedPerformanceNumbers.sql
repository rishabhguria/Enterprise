Create PROCEDURE [dbo].[P_GetSavedPerformanceNumbers]
(
   @FundIDs varchar(max),
   @StartDate datetime,
   @EndDate datetime
)
AS
select Date,FundID,PNL,AddRed,OpeningValue from T_SavedPerformanceNumbers 
where date between @StartDate and @EndDate
order by date, FundID
RETURN 0
