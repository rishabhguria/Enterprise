/*Date: 09 Nov 2021
Issue in Third party (PRANA-39444)
*/

Create procedure [dbo].[P_CheckDuplicateThirdPartyBatch]
(
@ThirdPartyBatchId int,
@ThirdPartyTypeId int,
@ThirdPartyCompanyId int,
@ThirdPartyId int,
@ThirdPartyFormatId int
)
AS

Declare @Result int

If (@ThirdPartyCompanyId=0 And @ThirdPartyTypeId In (3,4))
Begin
Set @ThirdPartyCompanyId = @ThirdPartyId
End

If (@ThirdPartyCompanyId=0 And @ThirdPartyTypeId In (1) )
Begin
Set @ThirdPartyCompanyId = (Select CompanyThirdPartyID from T_CompanyThirdParty
Where ThirdPartyID = @ThirdPartyId)
End

Delete From T_ThirdPartyBatch
Where ThirdPartyTypeId=0
AND ThirdPartyCompanyId=0 
AND ThirdPartyId=0
AND ThirdPartyFormatId=0

Select @Result=Count(*) 
FROM T_ThirdPartyBatch 
Where ThirdPartyBatchId<>@ThirdPartyBatchId
AND ThirdPartyTypeId=@ThirdPartyTypeId 
AND ThirdPartyCompanyId=@ThirdPartyCompanyId 
AND @ThirdPartyId=@ThirdPartyId 
AND ThirdPartyFormatId=@ThirdPartyFormatId

Select @Result