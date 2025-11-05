Create PROCEDURE [dbo].[P_SaveShortLocateDetails]
	@Xml ntext,
	@IsOverRideAllowed BIT 
AS
	BEGIN TRAN TRAN2  
    BEGIN TRY 
    DECLARE @handle int                                                                  
	EXEC sp_xml_preparedocument @handle OUTPUT, @Xml    
    
    CREATE TABLE #TempShortLocateDetails (  
    [BorrowerId] VARCHAR(20) NOT NULL, 
    [ClientMasterfund]  VARCHAR(max) NOT NULL,
    [BrokerId] VARCHAR(20) NOT NULL, 
	[Ticker] VARCHAR(20) NOT NULL, 
	[BorrowSharesAvailable]  INT NOT NULL, 
	[BorrowRate]  float NOT NULL, 
	[BorrowedShare]  INT NOT NULL, 
	[BorrowedRate]  float NOT NULL, 
	[SODBorrowshareAvailable]  INT NOT NULL,
	[SODBorrowRate]  float NOT NULL, 
	[StatusSource] VARCHAR(20) NOT NULL, 
	[SLImportDate] DATE NULL
     )  
  
      INSERT INTO #TempShortLocateDetails 
	  ( [BorrowerId],
       [ClientMasterfund], 
        [BrokerId] ,  
       [Ticker] ,
		[BorrowSharesAvailable]  ,
       [BorrowRate]  ,
       [BorrowedShare] ,   
       [BorrowedRate]  , 
       [SODBorrowShareAvailable],
       [SODBorrowRate],  
		[StatusSource] )
		select *
        FROM OPENXML(@handle, '//ShortLocateDetails', 2)  
         WITH  
        (  
            [BorrowerId] VARCHAR(20), 
		    [ClientMasterfund] VARCHAR(max), 
		    [Broker] VARCHAR(20) , 
			[Ticker] VARCHAR(20) , 
			[BorrowSharesAvailable]  INT, 
			[BorrowRate]  float , 
			[BorrowedShare]  INT , 
			[BorrowedRate]  float , 
			[SODBorrowshareAvailable]  INT ,
			[SODBorrowRate]  float , 
			[StatusSource] VARCHAR(20)
	 )
  
  	update #TempShortLocateDetails 
	set SLImportDate=CONVERT(date, getdate())

	DECLARE @ShowMasterFund INT
	SELECT @ShowMasterFund=CONVERT(INT, PreferenceValue)        
	FROM T_PranaKeyValuePreferences        
	WHERE PreferenceKey = 'IsShowmasterFundOnShortLocate'

	  if(@IsOverRideAllowed=1)
	  BEGIN
	  if(@ShowMasterFund=1)
	  BEGIN
	  DELETE FROM T_ShortLocateDetails 
	  WHERE  ClientMasterfundId = (select TOP 1 CMF.CompanyMasterFundID from #TempShortLocateDetails 
	  INNER JOIN T_CompanyMasterFunds CMF ON #TempShortLocateDetails.ClientMasterfund=CMF.MasterFundName) 
	  and  BrokerId = (SELECT TOP 1 TC.ThirdPartyID from #TempShortLocateDetails tempShortLocateDetails  
		INNER JOIN T_ThirdParty TC ON TempShortLocateDetails.BrokerId=TC.ShortName ) AND SLImportDate=CONVERT(date, getdate())
	  END
	  else
	  BEGIN
	  DELETE FROM T_ShortLocateDetails WHERE BrokerId = (SELECT TOP 1 TC.ThirdPartyID from #TempShortLocateDetails tempShortLocateDetails  
		INNER JOIN T_ThirdParty TC ON TempShortLocateDetails.BrokerId=TC.ShortName ) AND SLImportDate=CONVERT(date, getdate())
	  END
	  END

     INSERT INTO [T_ShortLocateDetails]  
        SELECT  
          tempShortLocateDetails.[BorrowerId],  
          ISNULL(CMF.CompanyMasterFundID,0) As ClientMasterfundId,  
          TC.ThirdPartyID AS BrokerId,  
          tempShortLocateDetails.[Ticker],  
          tempShortLocateDetails.[BorrowSharesAvailable],  
          tempShortLocateDetails.[BorrowRate],  
          tempShortLocateDetails.[BorrowedShare],  
          tempShortLocateDetails.[BorrowedRate],  
		  tempShortLocateDetails.[SODBorrowshareAvailable],
		  ISNULL(tempShortLocateDetails.[SODBorrowRate],0) AS SODBorrowRate,
		  tempShortLocateDetails.[StatusSource],
		  tempShortLocateDetails.[SLImportDate]
  
        FROM #TempShortLocateDetails tempShortLocateDetails  
		INNER JOIN T_ThirdParty TC ON TempShortLocateDetails.BrokerId=TC.ShortName 
		Left outer JOIN T_CompanyMasterFunds CMF ON tempShortLocateDetails.ClientMasterfund=CMF.MasterFundName
      DROP TABLE #TempShortLocateDetails  
      EXEC sp_xml_removedocument @handle  

    COMMIT TRANSACTION TRAN2 
  END TRY                                                                                                                                                          
  BEGIN CATCH                                                     
	ROLLBACK TRANSACTION TRAN2                            
  END CATCH;
RETURN 0
