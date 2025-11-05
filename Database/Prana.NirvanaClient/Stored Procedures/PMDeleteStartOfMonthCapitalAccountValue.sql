CREATE Procedure [dbo].[PMDeleteStartOfMonthCapitalAccountValue]
(
	@deletionDate datetime
)
AS

DELETE FROM PM_StartOfMonthCapitalAccount WHERE Date=@deletionDate

