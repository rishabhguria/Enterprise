-- =============================================
-- Author:		<Harsh kumar>
-- Create date: <25Mar2009>
-- Description:	<Saves BaseTimeZone and BaseTimeZone Clearance Time>
-- =============================================
CREATE PROCEDURE P_SavePMClearanceBaseSettings 
	-- Add the parameters for the stored procedure here
	@baseTimeZone varchar(200),
@baseClearanceTime datetime
AS
BEGIN
delete from T_PMBaseTimeSettings
insert into T_PMBaseTimeSettings 
values
(
 @baseTimeZone,
@baseClearanceTime
)
END
