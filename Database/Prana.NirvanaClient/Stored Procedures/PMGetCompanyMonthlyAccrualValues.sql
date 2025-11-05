CREATE PROCEDURE [dbo].[PMGetCompanyMonthlyAccrualValues]

	@CompanyId int=0 ,
	@Year int=0
	
AS

--Declare @count int
--set @count=0

--Select @count=count(*) from PM_CompanyMonthlyAccruals
--Where CompanyId=@CompanyId And [Year]=@Year

--if @count<12
--begin
--Select CompanyID, [Month],AccrualValue,[Year] from PM_CompanyMonthlyAccrualsTemp
--end
--else

BEGIN
	
Select CompanyID,MonthId, [Month],AccrualValue,[Year] from PM_CompanyMonthlyAccruals
Where CompanyId=@CompanyId And [Year]=@Year order By MonthId
	
END