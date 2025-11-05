-----------------------------------------------------
--Created By: Bharat raturi
--Date: 03/04/2014
--Purpose: Get the Decryption methods for file setting
--usage P_GetDecryptionForFileSetting
-------------------------------------------------------
CREATE procedure [dbo].[P_GetDecryptionForFileSetting]
as
select GnuPGId, GnuPGName from T_ThirdPartyGnuPG where Command=3
