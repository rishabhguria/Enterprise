create proc PMGetPositionIDForTaxLotID
(
@taxlotID int
)
As
Select PositionId from pm_netpositions where ApplicationTaxLotID = @taxlotID
