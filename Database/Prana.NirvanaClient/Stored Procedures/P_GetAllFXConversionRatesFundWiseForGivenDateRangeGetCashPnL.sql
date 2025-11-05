/********************************************************                           
 Author:  Kashish Goyal                          
 Create date: Mar 14,2016                            
 Description: Returns fund wise Forex rate with standard pairs and reverse of standard pairs for the date range given(for P_GetCashPnL)   
 Usage:                             
 Exec [P_GetAllFXConversionRatesFundWiseForGivenDateRangeGetCashPnL] '11-01-2015','03-20-2015' 

********************************************************/                             
CREATE Procedure [dbo].[P_GetAllFXConversionRatesFundWiseForGivenDateRangeGetCashPnL]           
(          
@startingDate datetime,           
@endingDate datetime,
@CurrencyIDs Varchar(200) = NULL,
@fundIDs VARCHAR(max)
)          
AS 

--Declare @startingDate datetime,           
--@endingDate datetime,
--@CurrencyIDs Varchar(200) = NULL,
--@fundIDs VARCHAR(max)

--Set @startingDate =   '09-24-2018'         
--Set @endingDate = '05-19-2022'
--Set @CurrencyIDs = '1,4,8,13'
--Set @fundIDs = '1279'
                           
BEGIN                            
CREATE TABLE #CurrencyIDs (CurrencyID varchar(200))   

CREATE TABLE #Funds (FundID INT)

		INSERT INTO #Funds
		SELECT Items AS FundID
		FROM dbo.Split(@FundIDs, ',') 
		
		--INSERT INTO #Funds
		--Select 0 As FundId 

IF (@CurrencyIDs IS NOT NULL)
BEGIN
 INSERT INTO #CurrencyIDs
	SELECT Items AS CurrencyID
	FROM dbo.Split(@CurrencyIDs, ',')
END                    
 -- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.                          
                         
  Select StanPair.FromCurrencyID AS FromCurrencyID,                            
      StanPair.ToCurrencyID AS ToCurrencyID,                            
      Isnull(CCR.ConversionRate,0) AS RateValue,                             
      0 AS ConversionMethod,                             
      CCR.Date AS Date,                            
      StanPair.eSignalSymbol AS eSignalSymbol,
	  CCR.FundID AS FundID                           
   From T_CurrencyConversionRate AS CCR                            
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK  
   Inner Join #Funds F On F.FundID = CCR.FundID                     
   WHERE DateDiff(d,@startingDate,CCR.Date) >=0           
  AND DateDiff(d,CCR.Date,@endingDate) >=0                
  AND (@CurrencyIDs IS NULL OR (@CurrencyIDs IS NOT NULL AND (FromCurrencyID IN (Select CurrencyID from #CurrencyIDs) OR ToCurrencyID  IN (Select CurrencyID from #CurrencyIDs))))
                          
  UNION                          
                          
  Select StanPair.ToCurrencyID AS FromCurrencyID,                            
      StanPair.FromCurrencyID AS ToCurrencyID,                            
      Isnull(CCR.ConversionRate,0) AS RateValue,                             
      1 AS ConversionMethod,                             
      CCR.Date AS Date,                            
      StanPair.eSignalSymbol AS eSignalSymbol,
	  CCR.FundID AS FundID                             
   from T_CurrencyConversionRate AS CCR                            
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK  
   Inner Join #Funds F On F.FundID = CCR.FundID                         
   WHERE DateDiff(d,@startingDate,CCR.Date) >=0               
  AND DateDiff(d,CCR.Date,@endingDate) >=0                   
  AND (@CurrencyIDs IS NULL OR (@CurrencyIDs IS NOT NULL AND (FromCurrencyID IN (Select CurrencyID from #CurrencyIDs) OR ToCurrencyID  IN (Select CurrencyID from #CurrencyIDs))))                        


  Drop Table #Funds,#CurrencyIDs
END