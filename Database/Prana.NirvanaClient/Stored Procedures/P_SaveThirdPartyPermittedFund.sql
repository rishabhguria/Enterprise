/*
Name :<P_SaveThirdPartyPermittedFund>
Created By : <Kanupriya>
Date :<10/11/2006>
Purpose:< To save the fund permitted to a thirdParty.>

*/


CREATE PROCEDURE dbo.P_SaveThirdPartyPermittedFund
	
	(
	@thirdPartyID int ,
	@companyFundID int
	)
	
AS

Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_ThirdPartyPermittedFunds
Where (ThirdPartyID = @thirdPartyID) AND (CopanyFundID = @companyFundID)

if(@total > 0)
begin	

	Set @result = @thirdPartyID
end
else

begin
	INSERT INTO T_ThirdPartyPermittedFunds
                        (ThirdPartyID, CopanyFundID)
  VALUES     (@thirdPartyID, @companyFundID)
	
	Set @result = scope_identity()

end
select @result

