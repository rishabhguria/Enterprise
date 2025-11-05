CREATE PROCEDURE [dbo].[P_SavePTTDetails] (  
  @AllocationPrefID int
, @Symbol nvarchar(50) 
, @Target DECIMAL (32, 19)  
, @Type int  
, @AddOrSet int  
, @MasterFundOrAccount nvarchar(50)  
, @CombinedAccountsTotalValue bit  
, @Price DECIMAL (32, 19)  
, @Xml ntext  
, @IsRoundLot BIT 
, @SelectedFundIds NVARCHAR(MAX)
)  
AS  
  -- check if @AllocationPrefID already exists if not then add new record  

  -- Changed PTTDetails.PercentageChange to PercentageType as the column name has been renamed on 26-Oct-17
  IF NOT EXISTS (SELECT TOP 1 [T_PTTDefinition].[PTTId] FROM [T_PTTDefinition] WHERE [T_PTTDefinition].[PTTId] = @AllocationPrefID)  
  BEGIN
  BEGIN TRAN TRAN1  
  BEGIN TRY 
    INSERT INTO [dbo].[T_PTTDefinition] ([PTTId]  
    , [Symbol]  
    , [Target]  
    , [Type]  
    , [Add_Set]  
    , [MasterFundOrAccount]  
    , [CombinedAccountsTotalValue]  
    , [Price]  
	, [IsRoundLot]
    , [SelectedFundIds]
	)
      VALUES (
		@AllocationPrefID, 
		@Symbol, 
		@Target, 
		@Type, 
		@AddOrSet, 
		@MasterFundOrAccount, 
		@CombinedAccountsTotalValue, 
		@Price, 
		@IsRoundLot,
        @SelectedFundIds)  
   
   COMMIT TRANSACTION TRAN1  
   END TRY
   BEGIN CATCH
   ROLLBACK TRANSACTION TRAN1
   END CATCH
   END
   
    BEGIN TRAN TRAN2  
    BEGIN TRY 
    DECLARE @handle int                                                                  
	EXEC sp_xml_preparedocument @handle OUTPUT,  
      @Xml  
    
      CREATE TABLE #TempPTTDetails (  
        [PTTId] int NOT NULL,  
        [AccountId] int NOT NULL,  
        [StartingPosition] DECIMAL (38, 19) NOT NULL,  
        [StartingValue] DECIMAL (38, 19) NOT NULL,  
        [AccountNAV] DECIMAL (38, 19) NOT NULL,  
        [StartingPercentage] DECIMAL (38, 19) NOT NULL,  
        [PercentageType] DECIMAL (38, 19) NOT NULL,  
        [TradeQuantity] DECIMAL (38, 19) NOT NULL,  
        [EndingPercentage] DECIMAL (38, 19) NOT NULL,  
        [EndingPosition] DECIMAL (38, 19) NOT NULL,  
        [EndingValue] DECIMAL (38, 19) NOT NULL,  
        [PercentageAllocation] DECIMAL (38, 19) NOT NULL,  
		[OrderSideID] VARCHAR(10) NULL 
      )  
  
      INSERT INTO #TempPTTDetails ([PTTId],  
      AccountId,  
      StartingPosition,  
      [StartingValue],  
      AccountNAV,  
      [StartingPercentage],  
      [PercentageType],  
      TradeQuantity,  
      [EndingPercentage],  
      [EndingPosition],  
      [EndingValue],  
      [PercentageAllocation],
	  [OrderSideID] )  
        SELECT  
          PTTId,  
          AccountId,  
          StartingPosition,  
		  StartingValue,  
		  AccountNAV,  
          StartingPercentage,  
          PercentageType,  
          TradeQuantity,  
          EndingPercentage,  
          EndingPosition,  
          EndingValue,  
          PercentageAllocation,
		  OrderSideID 
        FROM OPENXML(@handle, '//PTTDetails', 2)  
        WITH  
        (  
        PTTId int,  
        AccountId int,  
        StartingPosition DECIMAL (38, 19),  
		StartingValue DECIMAL (38, 19),  
		AccountNAV DECIMAL (38, 19),  
        StartingPercentage DECIMAL (38, 19),  
        PercentageType DECIMAL (38, 19),  
        TradeQuantity DECIMAL (38, 19),  
        EndingPercentage DECIMAL (38, 19),  
        EndingPosition DECIMAL (38, 19),  
        EndingValue DECIMAL (38, 19),  
        PercentageAllocation DECIMAL (38, 19),
		OrderSideID VARCHAR(10) 
        )
  
      INSERT INTO [T_PTTDetails]  
        SELECT  
          tempPTTDetails.[PTTId],  
          tempPTTDetails.AccountId,  
          tempPTTDetails.[StartingPosition],  
          tempPTTDetails.[StartingValue],  
		  tempPTTDetails.AccountNAV,  
          tempPTTDetails.[StartingPercentage],  
          tempPTTDetails.[PercentageType],  
          tempPTTDetails.TradeQuantity,  
          tempPTTDetails.[EndingPercentage],  
          tempPTTDetails.[EndingPosition],  
		  tempPTTDetails.[EndingValue],  
          tempPTTDetails.[PercentageAllocation],
		  tempPTTDetails.[OrderSideID] 
        FROM #TempPTTDetails tempPTTDetails   
      DROP TABLE #TempPTTDetails  
      EXEC sp_xml_removedocument @handle;
	  
	  BEGIN  --IF THERE IS MORE THAN ONE SIDE GENERATED IN PTT DETAIL THEN ITS A TRADE BREAK IN PTTDEFINATION TABLE (FOR SAMSARA PST)
		DECLARE @OrderSideCount INT;

		-- This will count the distinct orderside id based on pttId
		SELECT @OrderSideCount = COUNT(DISTINCT OrderSideID) 
		FROM T_PTTDetails 
		WHERE PTTId = @AllocationPrefID; 
		 
		UPDATE T_PTTDefinition 
		SET IsTradeBreak = CASE 
							 WHEN @OrderSideCount > 1 THEN 1 
							 ELSE 0 
						   END 
		WHERE PTTId = @AllocationPrefID; 
	  END

    COMMIT TRANSACTION TRAN2 
  END TRY                                                                                                                                                          
  BEGIN CATCH                                                     
	ROLLBACK TRANSACTION TRAN2                            
  END CATCH;
 
 BEGIN TRAN TRAN3
    BEGIN TRY 
        INSERT INTO T_PTTAllocationMapping SELECT @AllocationPrefID, @AllocationPrefID
    COMMIT TRANSACTION TRAN3
  END TRY                                                                                                                                                          
  BEGIN CATCH                                                     
	ROLLBACK TRANSACTION TRAN3                           
  END CATCH;
 Return 0

  