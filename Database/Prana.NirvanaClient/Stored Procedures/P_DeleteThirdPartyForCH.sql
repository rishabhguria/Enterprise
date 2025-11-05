
/*        
Modified By: Ankit Gupta on 10 Oct, 2014
Description: If user deletes a third party, do not remove it's entry from DB, instead, mark it as inactive.
*/    
    
CREATE PROCEDURE [dbo].[P_DeleteThirdPartyForCH] 
	(
		@thirdPartyID int	
	)
AS
Declare @total int
Declare @thirdPartyName varchar(100)
Declare @msg varchar(250)
         
-- To check if thirdparty exist in batch setup
       Select  @thirdPartyName = isnull(ShortName,'') From T_ThirdParty Where ThirdPartyID = @thirdPartyID
		Select @total =  Count(*) From T_CompanyThirdParty Where ThirdPartyID = @thirdPartyID        
        if ( @total > 0)
		begin
        Set @msg = @thirdPartyName + ' is associated with Company' + '. Delete association first.'
        end

        -- To check if thirdparty exist in batch setup
        if ( @total = 0)
		begin
		Select @total =  Count(*) From T_ImportFileSettings Where ThirdPartyID = @thirdPartyID
        
        if ( @total > 0)
        Set @msg = @thirdPartyName + ' is associated with Batch' + '. Delete association first.'
        end
        
        -- To check if thirdparty exist in T_CompanyFunds
        if ( @total = 0)
		begin
        Select @total = Count(*) From T_CompanyFunds Where CompanyThirdPartyID = @thirdPartyID
        if ( @total > 0)
        Set @msg = @thirdPartyName + ' is associated with company funds. Delete association first.'
        end
		
        --Delete ThirdParty.	
		if ( @total = 0)
		begin
		--Delete T_ThirdParty
		--Where ThirdPartyID = @thirdPartyID
		UPDATE T_ThirdParty
		SET isActive=0
		WHERE ThirdPartyID=@thirdPartyID;
		end
		
Select @msg

