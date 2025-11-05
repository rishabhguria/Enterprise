---------------------------------------------------  
--Created By: Bharat Raturi  
--Date: 03/04/2014  
--Purpose: Get all the GnuPG details for decryption  
--usage P_GetThirdPartyGnuPGForDecryption -1

--Modified By: Narendra Jangir 
--Date: 2014-04-12 
--Purpose: Added new column ExtensionToAdd, because sometimes encrypted dont have extension(e.g. .pgp), we need to add this extension manually before decryption
--usage P_GetThirdPartyGnuPGForDecryption -1    
---------------------------------------------------   
Create procedure P_GetThirdPartyGnuPGForDecryption(@GnuPGId int)  
as  
begin  
  
if @GnuPGId = -1  
 select GnuPGId, GnuPGName, HomeDirectory, Command, UseCmdBatch,  
 UseCmdYes, UseCmdArmor, VerboseLevel, Recipient, Originator, PassPhrase,  
PassPhraseDescriptor, [Timeout], [Enabled],[ExtensionToAdd] from T_ThirdPartyGnuPG where Command=3  
else  
 select GnuPGId, GnuPGName, HomeDirectory, Command, UseCmdBatch,  
 UseCmdYes, UseCmdArmor, VerboseLevel, Recipient, Originator, PassPhrase,  
PassPhraseDescriptor, [Timeout], [Enabled],[ExtensionToAdd]  
from T_ThirdPartyGnuPG where GnuPGId = @GnuPGId and Command=3  
end  