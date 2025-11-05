-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE P_GetSwapParameters 
(
@AUECDateString varchar(MAX)
)	

AS
BEGIN
Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                      
Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@AUECDateString)                                                      

select
SwapParams.GroupID,
SwapParams.NotionalValue,
SwapParams.BenchMarkRate,
SwapParams.Differential,
SwapParams.OrigCostBasis,
SwapParams.DayCount,
SwapParams.SwapDescription,
SwapParams.FirstResetDate,
SwapParams.OrigTransDate,
SwapParams.ResetFrequency,
SwapParams.ClosingPrice,
SwapParams.ClosingDate,
SwapParams.TransDate

from T_Group as G right outer join T_SwapParameters as SwapParams on G.GroupID = SwapParams.GroupID
	INNER JOIN @AUECDatesTable As AUECDates on AUECDates.AUECID = G.AUECID

END
