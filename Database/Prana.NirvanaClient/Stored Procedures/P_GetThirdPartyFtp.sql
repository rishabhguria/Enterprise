
CREATE PROCEDURE [dbo].[P_GetThirdPartyFtp](@ftpId int)
as
begin
if @ftpId = -1
	select * from T_ThirdPartyFtp
else
	select * from T_ThirdPartyFtp where FtpId = @ftpId
end
