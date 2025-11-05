CREATE proc P_IsGroupClosed
(
@groupID varchar(20)
)
as
declare @count int
select @count=count(TaxLotID) from 
(select TaxLotID from 
         dbo.T_Level2Allocation AS L2 INNER JOIN
                      dbo.T_FundAllocation AS L1 ON L2.Level1AllocationID = L1.AllocationId INNER JOIN
                      dbo.T_Group AS G ON G.GroupID = L1.GroupID

where G.GroupID = @groupID )as AllocationGroup,PM_Taxlotclosing
where PM_Taxlotclosing.PositionalTaxLotID=AllocationGroup.TaxLotID
or PM_Taxlotclosing.ClosingTaxLotID=AllocationGroup.TaxLotID

select  @count