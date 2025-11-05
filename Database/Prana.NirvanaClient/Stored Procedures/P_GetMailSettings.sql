


/****** Object:  Stored Procedure dbo.P_GetMailSettings    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetMailSettings
AS
	SELECT     MailSettingID, [From], [To], CarbonCopy, BlankCarbonCopy, Subject, Body, SMTPServer
	FROM         T_MailSetting


