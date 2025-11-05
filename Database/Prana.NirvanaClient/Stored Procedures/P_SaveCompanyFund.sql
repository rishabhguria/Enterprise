-- =============================================  
-- Modified by: Faisal Shah
-- Date: 26 Dec 2014
-- Purpose: Add Local Currency while saving Funds 
-- =============================================
CREATE  PROCEDURE [dbo].[P_SaveCompanyFund]
(
		@companyFundID int,
		@fundName varchar(50),
		@fundShortName varchar(50),
		@companyID int,
		@fundTypeID int
		
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyFunds
Where CompanyFundID = @companyFundID
declare @BaseCurrencyID INT

SET @BaseCurrencyID = isnull((SELECT BaseCurrencyID FROM T_Company where CompanyID = @companyID),1)
if(@total > 0)
begin	
	--Update CompanyFunds
	Update T_CompanyFunds 
	Set FundName = @fundName, 
		FundShortName = @fundShortName,
		CompanyID = @companyID,
		FundTypeID = @fundTypeID,
		LocalCurrency = @BaseCurrencyID
		
	Where CompanyFundID = @companyFundID
	
	Set @result = @companyFundID
end
else
--Insert CompanyFunds
begin
	INSERT T_CompanyFunds(FundName, FundShortName, CompanyID, FundTypeID,LocalCurrency)
	Values(@fundName, @fundShortName, @companyID, @fundTypeID,@BaseCurrencyID)
	
	Set @result = scope_identity()
		--	end
end
select @result

