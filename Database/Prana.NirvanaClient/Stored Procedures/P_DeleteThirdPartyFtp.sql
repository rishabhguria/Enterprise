/**  
Modified By:  Ankit Gupta
Description: [Admin] - FTP in use is deletable. CHMW-1402
**/  
CREATE Procedure [dbo].[P_DeleteThirdPartyFtp]
(
	@FtpId int
)
AS
BEGIN
	Declare @InUse int
	SELECT @InUse =  Count(*) FROM T_ThirdPartyBatch WHERE FtpId = @FtpId
	if @InUse = 0
	BEGIN
		Delete FROM T_ThirdPartyFtp WHERE FtpId = @FtpId	
	END
	SELECT -@InUse
END
