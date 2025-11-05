/* =============================================          
 Author:  Sumit kakra          
 Create date: 10 July 2008
 Description: Insert Data
 Usage : Might Need to change values every time
		EXEC [InsertBulkCVAUECCompliance] 12,0,''
-- =============================================          
*/  
  
CREATE Procedure [dbo].[InsertBulkCVAUECCompliance] (@StartID bigint,@LastID bigint, @SkipIDs varchar(MAX))
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
			-- Insert Into Compliance Table
			Insert Into T_CVAUECCompliance (CVAUECID, 
											FollowCompliance, 
											ShortSellConfirmation, 
											IdentifierID,
											ForeignID)
			Values (@NextID, 0, 1, 1, '')

		   -- Insert Into OrderTypesTable
			Insert Into T_CVAUECOrderTypes (CVAUECID, OrderTypesID) Values (@NextID, 1)
			Insert Into T_CVAUECOrderTypes (CVAUECID, OrderTypesID) Values (@NextID, 2)

		   -- Insert Into OrderSides
		   -- Sides for Options and Futures are different from sides for other classes
			If EXISTS (Select * from T_CVAUEC,T_AUEC 
						Where T_CVAUEC.AUECID = T_AUEC.AUECID 
							  And T_AUEC.AssetID in (2,3) 
							  And CVAUECID = @NextID)
			Begin -- Sides for Options and Futures
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 10)
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 9)
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 12)
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 11)
			End
			Else -- Sides for other Asset classes
			Begin
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 1)
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 10)
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 2)
				Insert Into T_CVAUECSide (CVAUECID, SideID) Values (@NextID, 5)
			End



		   -- Insert Into T_CVAUECExecutionInstructions
			Insert Into T_CVAUECExecutionInstructions (CVAUECID, ExecutionInstructionsID) Values (@NextID, 17)
			Insert Into T_CVAUECExecutionInstructions (CVAUECID, ExecutionInstructionsID) Values (@NextID, 13)
			Insert Into T_CVAUECExecutionInstructions (CVAUECID, ExecutionInstructionsID) Values (@NextID, 15)
			Insert Into T_CVAUECExecutionInstructions (CVAUECID, ExecutionInstructionsID) Values (@NextID, 16)

		   -- Insert Into T_CVAUECHandlingInstructions
			Insert Into T_CVAUECHandlingInstructions (CVAUECID, HandlingInstructionsID) Values (@NextID, 3)
			Insert Into T_CVAUECHandlingInstructions (CVAUECID, HandlingInstructionsID) Values (@NextID, 1)
			Insert Into T_CVAUECHandlingInstructions (CVAUECID, HandlingInstructionsID) Values (@NextID, 2)

		   -- Insert Into T_CVAUECTimeInForce
			Insert Into T_CVAUECTimeInForce (CVAUECID, TimeInForceID) Values (@NextID, 1)
			Insert Into T_CVAUECTimeInForce (CVAUECID, TimeInForceID) Values (@NextID, 5)
			Insert Into T_CVAUECTimeInForce (CVAUECID, TimeInForceID) Values (@NextID, 7)
			Insert Into T_CVAUECTimeInForce (CVAUECID, TimeInForceID) Values (@NextID, 2)

		End
		Set @NextID = @NextID + 1   
	End
End


