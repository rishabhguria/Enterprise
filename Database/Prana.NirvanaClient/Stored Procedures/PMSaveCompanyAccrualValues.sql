
CREATE PROCEDURE [dbo].[PMSaveCompanyAccrualValues] (
		
		@CompanyId int,
		@MonthId int, 
		@Month Varchar(20),
		@AccrualValue Decimal(18,2),
		@Year int
	)
AS
	Declare @result int
	set @result = 1

Declare @Count int
set @Count=0

Select @Count=Count(*) from PM_CompanyMonthlyAccruals Where CompanyID=@CompanyId
And MonthId=@MonthId And [Month]=@Month And [Year]=@Year

if @Count>0
begin
Update PM_CompanyMonthlyAccruals set AccrualValue=@AccrualValue Where CompanyID=@CompanyId
And MonthId=@MonthId And [Month]=@Month And [Year]=@Year

end
Else
Begin
Insert INTO  PM_CompanyMonthlyAccruals (CompanyID,MonthId,[Month],AccrualValue,[Year]) 
             Values(@CompanyId,@MonthId,@Month,@AccrualValue,@Year)
--Select @result = scope_identity()
End
	
	
select @result






