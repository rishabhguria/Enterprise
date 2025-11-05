

CREATE PROCEDURE P_SaveSMPreferences
(
@useCutOffTime bit
)

	
AS
BEGIN
	
delete from T_SMGlobalPreferences
Insert into T_SMGlobalPreferences(useCutOffTime)
values(@useCutOffTime)

END
