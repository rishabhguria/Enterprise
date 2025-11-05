/****** Object:  StoredProcedure [dbo].[P_SaveCompanyStrategy]    Script Date: 08/05/2014 18:59:47 ******/
-----------------------------------------------------------------
--Modified BY: Faisal Shah
--Date: 05-Aug-2014
--Purpose: Message for Duplicate Strategies with InActive State
-----------------------------------------------------------------

/****** Object:  Stored Procedure dbo.P_SaveCompanyStrategy    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_SaveCompanyStrategy]
(
		@companyStrategyID int,
		@strategyName varchar(50),
		@strategyShortName varchar(50),
		@companyID int
		
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyStrategy
Where CompanyStrategyID = @companyStrategyID
IF (SELECT COUNT(*) FROM T_CompanyStrategy WHERE StrategyName = @strategyName and CompanyID = @companyID and IsActive = 0) > 0 
BEGIN
SET @result = -3
END
ELSE IF (SELECT COUNT(*) FROM T_CompanyStrategy WHERE StrategyShortName = @strategyShortName and CompanyID = @companyID  and IsActive = 0) > 0
BEGIN
SET @result = -4
END
ELSE IF(@total > 0)
begin	
	Update T_CompanyStrategy 
	Set StrategyName = @strategyName, 
		StrategyShortName = @strategyShortName,
		CompanyID = @companyID
		
	Where CompanyStrategyID = @companyStrategyID
	
	Set @result = @companyStrategyID
end
else
begin
SET IDENTITY_INSERT T_CompanyStrategy ON
	INSERT T_CompanyStrategy(companyStrategyID, StrategyName, StrategyShortName, CompanyID)
	Values(@companyStrategyID, @strategyName, @strategyShortName, @companyID)
	
	Set @result = scope_identity()
SET IDENTITY_INSERT T_CompanyStrategy OFF
end
select @result

