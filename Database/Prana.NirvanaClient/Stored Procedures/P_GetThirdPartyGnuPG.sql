


CREATE PROCEDURE [dbo].[P_GetThirdPartyGnuPG](@GnuPGId int)
as
begin

if @GnuPGId = -1
	select * from T_ThirdPartyGnuPG
else
	select * from T_ThirdPartyGnuPG where GnuPGId = @GnuPGId
end

