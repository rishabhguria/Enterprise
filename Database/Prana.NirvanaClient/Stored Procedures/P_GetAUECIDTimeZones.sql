-- =============================================
-- Author:		<Harsh Kumar>
-- Create date: <23/08/2007>
-- Description:	<Get AUECIDs and corresponding TimeZone strings>
-- =============================================
CREATE PROCEDURE P_GetAUECIDTimeZones

AS
BEGIN
select AUECID,TimeZone from T_AUEC
END
