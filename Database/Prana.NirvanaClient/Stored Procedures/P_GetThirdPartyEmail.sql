

CREATE PROCEDURE [dbo].[P_GetThirdPartyEmail](@EmailId int, @MailType int)
as
begin
if @EmailId = -1 and @MailType = -1
	select * from T_ThirdPartyEmail 
else if @EmailId = -1 and @MailType <> -1
	Select * from T_ThirdPartyEmail where MailType = @MailType
else if @EmailId <> -1 and @MailType <> -1
	select * from T_ThirdPartyEmail where EmailId = @EmailId and MailType = @MailType
else if @EmailId <> -1 and @MailType = -1
	select * from T_ThirdPartyEmail where EmailId = @EmailId
end
