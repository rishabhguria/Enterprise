CREATE PROCEDURE dbo.P_DeleteThirdPartyFileFormat
	(
		
		@thirdPartyId int	,
        @thirdPartyFileFormatId varchar(200) = ''	
	)
AS
	--Declare @result int
	--set @result = 1

if(@thirdPartyFileFormatId = '') 
	begin
	Delete From T_ThirdPartyFileFormat
           Where	ThirdPartyId=@thirdPartyId
   end
else
	begin
	
		exec ('Delete T_ThirdPartyFileFormat
		Where convert(varchar, FileFormatId) NOT IN(' + @thirdPartyFileFormatId + ') 
			 AND ThirdPartyId = ' + @thirdPartyId)
	end
	
--select @result	






