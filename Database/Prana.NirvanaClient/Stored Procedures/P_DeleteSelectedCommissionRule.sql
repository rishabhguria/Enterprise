/****** Object:  Stored Procedure dbo.P_DeleteCommissionRule   Script Date: 2/22/2006 12:35:23 PM ******/
CREATE PROCEDURE dbo.P_DeleteSelectedCommissionRule 
	(
		
		@FundId int,
		@CVId int,
		@AUECId int
	)

AS
Declare @Count int
Set @Count=0
Declare @result int
Set @result=0

Declare @AUECID1 int
Set @AUECID1=0

Select @AUECID1=AUECID from T_CVAUEC Where CVAUECID=@AUECId

Select @Count=Count(*) from T_ThirdPartyFFRunFundDetails
Where ExchangeID=@AUECID1 and CompanyFundID=@FundId And  CompanyCVID=@CVId


If @Count>0
Begin
set @result=-1
End

Else

Begin
Delete from T_CompanyThirdPartyCVCommissionRules Where CompanyFundID=@FundId And CompanyCounterPartyCVID=@CVId
And CVAUECID=@AUECID
set @result=1
End

Select @result


