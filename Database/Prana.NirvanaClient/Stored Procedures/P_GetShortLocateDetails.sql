CREATE PROCEDURE [dbo].[P_GetShortLocateDetails]
	@ClientMasterFund VARCHAR(50) 
AS
	DECLARE @ShowMasterFund INT
	SELECT @ShowMasterFund=CONVERT(INT, PreferenceValue)        
	FROM T_PranaKeyValuePreferences        
	WHERE PreferenceKey = 'IsShowmasterFundOnShortLocate'

	 if(@ShowMasterFund=1)
	  BEGIN
        SELECT  
		  ShortLocateDetails.[NirvanaLocateID],
          ShortLocateDetails.[BorrowerId],  
          CMF.[MasterFundName] AS ClientMasterfund,
          TC.ShortName AS [Broker],
          ShortLocateDetails.[Ticker],  
          ShortLocateDetails.[BorrowSharesAvailable],  
          ShortLocateDetails.[BorrowRate],  
          ShortLocateDetails.[BorrowedShare],  
          ShortLocateDetails.[BorrowedRate],  
		  ShortLocateDetails.[SODBorrowshareAvailable],
		  ShortLocateDetails.[SODBorrowRate],
		  ShortLocateDetails.[StatusSource]
  
        FROM T_ShortLocateDetails ShortLocateDetails
		INNER JOIN T_ThirdParty TC ON ShortLocateDetails.BrokerId=TC.ThirdPartyID 
		left JOIN T_CompanyMasterFunds CMF ON ShortLocateDetails.ClientMasterfundId=CMF.CompanyMasterFundID 
		where ShortLocateDetails.[SLImportDate]=CONVERT(date, getdate()) and  ShortLocateDetails.ClientMasterfundId!=0
		END
	else
	 BEGIN
        SELECT  
		  ShortLocateDetails.[NirvanaLocateID],
          ShortLocateDetails.[BorrowerId],  
          CMF.[MasterFundName] AS ClientMasterfund,
          TC.ShortName AS [Broker],
          ShortLocateDetails.[Ticker],  
          ShortLocateDetails.[BorrowSharesAvailable],  
          ShortLocateDetails.[BorrowRate],  
          ShortLocateDetails.[BorrowedShare],  
          ShortLocateDetails.[BorrowedRate],  
		  ShortLocateDetails.[SODBorrowshareAvailable],
		  ShortLocateDetails.[SODBorrowRate],
		  ShortLocateDetails.[StatusSource]
  
        FROM T_ShortLocateDetails ShortLocateDetails
		INNER JOIN T_ThirdParty TC ON ShortLocateDetails.BrokerId=TC.ThirdPartyID 
		left JOIN T_CompanyMasterFunds CMF ON ShortLocateDetails.ClientMasterfundId=CMF.CompanyMasterFundID
		where ShortLocateDetails.[SLImportDate]=CONVERT(date, getdate()) and ShortLocateDetails.ClientMasterfundId=0
		END
RETURN 0
