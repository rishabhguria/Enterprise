CREATE Procedure [dbo].[PMGetDailyCreditLimitValues]
AS
SELECT FundID,LongDebitLimit,ShortCreditLimit,LongDebitBalance,ShortCreditBalance FROM PM_DailyCreditLimit

