CREATE Procedure [dbo].[PMDeleteUserDefinedMTDPnLValue]
(
	@deletionDate datetime
)
AS

DELETE FROM PM_UserDefinedMTDPnL WHERE Date=@deletionDate

