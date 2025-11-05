/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyGnuPG]    Script Date: 05/05/2013 19:57:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_GetThirdPartyGnuPG]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_GetThirdPartyGnuPG]
GO

/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyGnuPG]    Script Date: 05/05/2013 19:57:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[P_GetThirdPartyGnuPG](@GnuPGId int)
as
begin

if @GnuPGId = -1
	select * from T_ThirdPartyGnuPG
else
	select * from T_ThirdPartyGnuPG where GnuPGId = @GnuPGId
end

GO

