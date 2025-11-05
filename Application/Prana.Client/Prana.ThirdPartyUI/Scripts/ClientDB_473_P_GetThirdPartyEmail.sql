/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyEmail]    Script Date: 05/03/2013 11:03:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_GetThirdPartyEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_GetThirdPartyEmail]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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
