/*
Created by Sandeep as on 31-july to update the SwapParameters from Expiration UI
When Swap Exipres or ExpireandRollovers
*/

CREATE Procedure UpdateSwapParametersFromExpiration 
(
@taxLotID varchar(50),
@benchMarkRate float,
@differential float,
@dayCount int
)
as
Update T_SwapParameters 
Set BenchMarkRate=@benchMarkRate ,
Differential=@differential,
DayCount=@dayCount 
where GroupID in(Select GroupID from V_Taxlots where TaxLotID=@taxLotID)