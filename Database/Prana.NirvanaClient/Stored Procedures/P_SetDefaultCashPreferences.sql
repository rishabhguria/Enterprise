-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 2-july-14
--Purpose: Get the cash preferences from DB
--usage: P_SetDefaultCashPreferences
-----------------------------------------------------------------
CREATE procedure [dbo].[P_SetDefaultCashPreferences]
as
--declare @prefCount int
--select @prefCount= COUNT(*) from T_CashPreferences
--print @prefCount
--IF(@prefCount=1)
begin
if not exists( select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME='T_cashpreferences_Backup')
begin
Select top 1 * into T_cashpreferences_Backup from T_CashPreferences
end
select TOP 1 * into #tempPref from T_CashPreferences_Backup
delete from T_CashPreferences
--insert INTO T_CashPreferences SELECT TOP 1 * from #tempPref


--update T_CashPreferences SET FundID=-1, ID=-1
insert INTO T_CashPreferences
(ID, CashMgmtStartDate, MarginPercentage,CollateralFrequencyInterest,
IsCalculatePnL, IsCalulateDividend, IsCalculateBondAccural,IsCalculateCollateral,IsBreakRealizedPnlSubaccount,IsBreakTotalIntoTradingAndFxPnl, FundID)
SELECT CompanyFundID, CashMgmtStartDate, MarginPercentage,CollateralFrequencyInterest, IsCalculatePnL,
IsCalulateDividend, IsCalculateBondAccural,IsCalculateCollateral,IsBreakRealizedPnlSubaccount,IsBreakTotalIntoTradingAndFxPnl, CompanyFundID from T_CompanyFunds cross join #tempPref where CompanyFundID>0 AND T_CompanyFunds.IsActive=1 
DROP TABLE #temppref
--SELECT * FROM T_CashPreferences_backup
--SELECT * FROM T_CashPreferences
end
