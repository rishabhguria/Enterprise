/* =============================================          
 Author:  Sumit kakra          
 Create date: 10 July 2008
 Description: Insert Data
 Usage : Need to change values every time
-- =============================================          
*/  
  
Create Procedure [dbo].[InsertCompliance] (@StartID bigint,@LastID bigint, @SkipIDs varchar(MAX))
AS  
Begin  
    Declare @NextID bigint

	Declare @SkipIDsTable Table(SkipID varchar(max))                    
	Insert Into @SkipIDsTable Select * From dbo.Split(@SkipIDs,',')                    

    set @NextID = @StartID  
  
    Declare @Loop bigint  
    set @Loop = 0  
  
    while @NextID <= @LastID  
    Begin
		if NOT EXISTS(Select Convert(bigint,SkipID) 
						from @SkipIDsTable 
						where Convert(bigint,SkipID) = @NextID) 
		   And EXISTS (Select CVAUECID 
						from T_CVAUEC
						where CVAUECID = @NextID)
		Begin
			Insert Into T_CVAUECCompliance (CVAUECID, 
											FollowCompliance, 
											ShortSellConfirmation, 
											IdentifierID,
											ForeignID)
			Values (@NextID, 
					0, 
					1, 
					1,
					'')
		End
		Set @NextID = @NextID + 1   
	End
End


