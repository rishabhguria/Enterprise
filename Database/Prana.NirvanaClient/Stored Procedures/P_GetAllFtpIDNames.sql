-----------------------------------------------------
--Created By: Bharat raturi
--Date: 29/3/2014
--Purpose: Get the FTP ID-FTP Name for file setting
-------------------------------------------------------

Create Procedure [dbo].[P_GetAllFtpIDNames]
as
select FtpId, FtpName from T_ThirdPartyFtp
