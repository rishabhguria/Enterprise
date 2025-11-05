
/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyBatch]    Script Date: 05/03/2013 11:12:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_GetThirdPartyBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_GetThirdPartyBatch]
GO

/****** Object:  StoredProcedure [dbo].[P_GetThirdPartyBatch]    Script Date: 05/03/2013 11:12:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- exec P_GetThirdPartyBatch 
-- =============================================
CREATE PROCEDURE [dbo].[P_GetThirdPartyBatch](@batchId varchar(50) = '-1')
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


if @batchId <> '-1'

SELECT DISTINCT 
                      dbo.T_ThirdPartyBatch.ThirdPartyBatchId, dbo.T_ThirdPartyBatch.Description, dbo.T_ThirdPartyType.ThirdPartyTypeName, dbo.T_ThirdParty.ThirdPartyName, 
                      dbo.T_ThirdPartyFileFormat.FileFormatName, dbo.T_ThirdPartyFtp.FtpName, dbo.T_ThirdPartyGnuPG.GnuPGName, dbo.T_ThirdPartyBatch.IsLevel2Data, 
                      dbo.T_ThirdPartyBatch.Active, dbo.T_ThirdPartyBatch.ThirdPartyTypeId, dbo.T_ThirdPartyBatch.ThirdPartyId, dbo.T_ThirdPartyBatch.ThirdPartyFormatId, 
                      dbo.T_ThirdPartyBatch.FtpId, dbo.T_ThirdPartyBatch.GnuPGId, dbo.T_ThirdPartyBatch.ThirdPartyCompanyId, T_ThirdPartyEmailData.EmailName AS EmailDataName, 
                      T_ThirdPartyEmailLog.EmailName AS EmailLogName, '' AS [View], '' AS [Create], '' AS Send, dbo.T_ThirdPartyBatch.EmailDataId, 
                      dbo.T_ThirdPartyBatch.EmailLogId
FROM         dbo.T_ThirdPartyBatch INNER JOIN
                      dbo.T_ThirdPartyType ON dbo.T_ThirdPartyBatch.ThirdPartyTypeId = dbo.T_ThirdPartyType.ThirdPartyTypeID LEFT OUTER JOIN
                      dbo.T_ThirdPartyEmail AS T_ThirdPartyEmailLog ON dbo.T_ThirdPartyBatch.EmailLogId = T_ThirdPartyEmailLog.EmailId LEFT OUTER JOIN
                      dbo.T_ThirdPartyEmail AS T_ThirdPartyEmailData ON dbo.T_ThirdPartyBatch.EmailDataId = T_ThirdPartyEmailData.EmailId LEFT OUTER JOIN
                      dbo.T_ThirdParty ON dbo.T_ThirdPartyBatch.ThirdPartyId = dbo.T_ThirdParty.ThirdPartyID LEFT OUTER JOIN
                      dbo.T_ThirdPartyFileFormat ON dbo.T_ThirdPartyBatch.ThirdPartyFormatId = dbo.T_ThirdPartyFileFormat.FileFormatId LEFT OUTER JOIN
                      dbo.T_ThirdPartyGnuPG ON dbo.T_ThirdPartyBatch.GnuPGId = dbo.T_ThirdPartyGnuPG.GnuPGId LEFT OUTER JOIN
                      dbo.T_ThirdPartyFtp ON dbo.T_ThirdPartyBatch.FtpId = dbo.T_ThirdPartyFtp.FtpId
where ThirdPartyBatchId = @batchId

else

SELECT DISTINCT 
                      dbo.T_ThirdPartyBatch.ThirdPartyBatchId, dbo.T_ThirdPartyBatch.Description, dbo.T_ThirdPartyType.ThirdPartyTypeName, dbo.T_ThirdParty.ThirdPartyName, 
                      dbo.T_ThirdPartyFileFormat.FileFormatName, dbo.T_ThirdPartyFtp.FtpName, dbo.T_ThirdPartyGnuPG.GnuPGName, dbo.T_ThirdPartyBatch.IsLevel2Data, 
                      dbo.T_ThirdPartyBatch.Active, dbo.T_ThirdPartyBatch.ThirdPartyTypeId, dbo.T_ThirdPartyBatch.ThirdPartyId, dbo.T_ThirdPartyBatch.ThirdPartyFormatId, 
                      dbo.T_ThirdPartyBatch.FtpId, dbo.T_ThirdPartyBatch.GnuPGId, dbo.T_ThirdPartyBatch.ThirdPartyCompanyId, T_ThirdPartyEmailData.EmailName AS EmailDataName, 
                      T_ThirdPartyEmailLog.EmailName AS EmailLogName, '' AS [View], '' AS [Create], '' AS Send, dbo.T_ThirdPartyBatch.EmailDataId, 
                      dbo.T_ThirdPartyBatch.EmailLogId
FROM         dbo.T_ThirdPartyBatch INNER JOIN
                      dbo.T_ThirdPartyType ON dbo.T_ThirdPartyBatch.ThirdPartyTypeId = dbo.T_ThirdPartyType.ThirdPartyTypeID LEFT OUTER JOIN
                      dbo.T_ThirdPartyEmail AS T_ThirdPartyEmailLog ON dbo.T_ThirdPartyBatch.EmailLogId = T_ThirdPartyEmailLog.EmailId LEFT OUTER JOIN
                      dbo.T_ThirdPartyEmail AS T_ThirdPartyEmailData ON dbo.T_ThirdPartyBatch.EmailDataId = T_ThirdPartyEmailData.EmailId LEFT OUTER JOIN
                      dbo.T_ThirdParty ON dbo.T_ThirdPartyBatch.ThirdPartyId = dbo.T_ThirdParty.ThirdPartyID LEFT OUTER JOIN
                      dbo.T_ThirdPartyFileFormat ON dbo.T_ThirdPartyBatch.ThirdPartyFormatId = dbo.T_ThirdPartyFileFormat.FileFormatId LEFT OUTER JOIN
                      dbo.T_ThirdPartyGnuPG ON dbo.T_ThirdPartyBatch.GnuPGId = dbo.T_ThirdPartyGnuPG.GnuPGId LEFT OUTER JOIN
                      dbo.T_ThirdPartyFtp ON dbo.T_ThirdPartyBatch.FtpId = dbo.T_ThirdPartyFtp.FtpId
END

