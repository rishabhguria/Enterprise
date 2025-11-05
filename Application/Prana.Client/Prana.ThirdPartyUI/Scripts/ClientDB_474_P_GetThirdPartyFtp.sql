
/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyFtp]    Script Date: 05/03/2013 11:03:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_GetThirdPartyFtp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_GetThirdPartyFtp]
GO


/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyFtp]    Script Date: 05/03/2013 11:03:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[P_GetThirdPartyFtp](@ftpId int)
as
begin
if @ftpId = -1
	select * from T_ThirdPartyFtp
else
	select * from T_ThirdPartyFtp where FtpId = @ftpId
end
GO

