-----------------------------------------------------
--Created By: Bharat raturi
--Date: 29/3/2014
--Purpose: Get the Email recipients for file setting
-------------------------------------------------------
CREATE procedure [dbo].[P_GetEmailForFileSetting]
@type int
as
IF @type=0
select EmailId,EmailName from T_ThirdPartyEmail where MailType=0;
else 
SELECT EmailId,EmailName from T_ThirdPartyEmail where MailType=1;
