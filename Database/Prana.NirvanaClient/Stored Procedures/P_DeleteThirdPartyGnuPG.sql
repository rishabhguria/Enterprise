
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE Procedure [dbo].[P_DeleteThirdPartyGnuPG]
(
	@GnuPGId int
)

AS
begin

Declare @InUse int

Select @InUse = Count(*) from T_ThirdPartyBatch Where GnuPGId = @GnuPGId

if @InUse = 0
begin
	Delete from T_ThirdPartyGnuPG where GnuPGId = @GnuPGId	
end

select -@InUse
end

