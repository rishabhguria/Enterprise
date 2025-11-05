
CREATE PROCEDURE [dbo].[P_GetCompanyFundAssociation] (  
 @fundID int
)  
AS   
 declare @result int  
DECLARE @FundIds VARCHAR(MAX) 
Declare @count int  
Set @count=0
 
  declare  @msg varchar(250)
  set @msg = ''
             --------------------------- Start: Check Dependent data -----------------------------------  

            -- To check if fund is associated with thirdparty
		    Select @count = Count(*) From T_ImportFileSettings Where FundID=@fundID
            if ( @count > 0)
		    begin
			set @msg = 'Fund is associated with Batch. Delete association first.'
            end

            Select @count = Count(*) From T_CompanyUserFunds Where CompanyFundID=@fundID
            if ( @count > 0)
		    begin
			set @msg = 'Fund is permitted with user. Delete association first.'
            end

            -- To check if fund is associated with release
            if ( @count = 0)
		    begin
			Select @count = Count(*) From T_CompanyFunds Where FundReleaseID in (select CompanyReleaseID from T_CompanyReleaseDetails)
            and CompanyFundID=@fundID
			if ( @count > 0)
			set @msg = 'Fund is associated with release. Delete association first.'
			end

			-- To check if fund is associated with Fund group
            if ( @count = 0)
		    begin
			Select @count = Count(*) From T_GroupFundMapping Where FundID=@fundID
			if ( @count > 0)
			set @msg = 'Fund is associated with fund group. Delete association first.'
			end

			-- To check if fund is associated with pricing
            if ( @count = 0)
		    begin
			Select @count = Count(*) From T_CompanyPricingMaster Where CompanyFundID=@fundID
			if ( @count > 0)
			set @msg = 'Fund is associated with pricing. Delete association first.'
			end

			-- To check if fund is associated with Navlock 
            if ( @count = 0)
		    begin
			Select @count = Count(*) From PM_Taxlots Where FundID=@fundID
			if ( @count > 0)
			set @msg = 'Some Taxlots are allocated to this fund, Hence cannot delete the fund.'
			end

			-- To check if fund is associated with SM Batch
			if(@count = 0)
			begin
			SELECT @FundIds = COALESCE(@FundIds + ',', '') + FundID FROM T_SMBatchSetup
			Select @count = Count(*) from dbo.split(@FundIds, ',') Where Items=@fundID
			if ( @count > 0)  
			set @msg = 'Fund is associated with SM Batch. Delete association first.'  
			end 

 select @msg
