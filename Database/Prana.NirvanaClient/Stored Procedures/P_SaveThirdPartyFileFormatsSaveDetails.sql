/*
Name : <P_SaveThirdPartyFileFormatsSaveDetails>
Created by : <Kanupriya>
Date : <10/17/2006>
Purpose : <To save the  flatfile save details of a third party.>

*/

CREATE PROCEDURE [dbo].[P_SaveThirdPartyFileFormatsSaveDetails]
	(
	@companyThirdPartyID int ,
	@companyIdentifier varchar(15),
	@namingConvention varchar(100),
	@saveGeneratedFileIn varchar(100),
	@result int
	)
	
AS
	declare @total int
	set @total = 0
	
	SELECT     @total = Count(*)
	FROM         T_CompanyThirdPartyFlatFileSaveDetails
	WHERE     (CompanyThirdPartyID = @companyThirdPartyID) 

if(@total > 0)
begin
UPDATE    T_CompanyThirdPartyFlatFileSaveDetails
SET              SaveGeneratedFileIn = @saveGeneratedFileIn,
				 NamingConvention = @namingConvention,
				 CompanyIdentifier = @companyIdentifier
				WHERE     (CompanyThirdPartyID = @companyThirdPartyID)

set @result = -1
end

else
begin 
INSERT INTO T_CompanyThirdPartyFlatFileSaveDetails
                      (CompanyThirdPartyID, SaveGeneratedFileIn, NamingConvention, CompanyIdentifier)
VALUES     (@companyThirdPartyID,@saveGeneratedFileIn,@namingConvention,@companyIdentifier)
set @result = scope_identity() 
end

select @result

