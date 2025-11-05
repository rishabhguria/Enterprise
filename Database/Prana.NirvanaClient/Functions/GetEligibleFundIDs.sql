CREATE FUNCTION dbo.GetEligibleFundIDs(@FundIDs varchar(max), @date datetime)
RETURNS varchar(max) 
AS 
BEGIN
DECLARE @Funds VARCHAR(max)
--declare @FundsTab table(FundID varchar(max)) 
--declare @RetFunds table(FundID varchar(max))
declare @FundsTab table(FundID int)
declare @RetFunds table(FundID int)
Insert into @FundsTab                                                          
Select Items as FundID from dbo.Split(@FundIDs,',')
insert INTO @RetFunds
select f.FundID from @FundsTab f inner join T_LastCalcDateRevaluation r on f.FundID=r.FundID
and DATEDIFF(d, @date, r.LastCalcDate)<=0 

SELECT @Funds = COALESCE(@Funds + ',', '') + cast(FundID as varchar(max)) from @RetFunds
RETURN @Funds;
END;
