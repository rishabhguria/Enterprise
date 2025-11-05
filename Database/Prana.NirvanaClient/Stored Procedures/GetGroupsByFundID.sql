

Create Procedure GetGroupsByFundID
(
@FundID varchar (20)
) 
as
select GroupID from T_FundsGroups where FundID=@FundID


