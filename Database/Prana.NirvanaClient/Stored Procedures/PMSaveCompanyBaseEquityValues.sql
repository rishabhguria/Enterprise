
CREATE PROCEDURE [dbo].[PMSaveCompanyBaseEquityValues]
 (
		@CompanyId int,
		@CompanyBaseEquityDate DateTime,		
		@BaseEquityValue Decimal(18,2)
	)
AS
	Declare @result int
	set @result = 1

Delete from PM_CompanyBaseEquityValue

Begin
Insert INTO  PM_CompanyBaseEquityValue (CompanyID,BaseEquityValue,BaseEquityDate) 
             Values(@CompanyId,@BaseEquityValue,@CompanyBaseEquityDate)
End
	
select @result