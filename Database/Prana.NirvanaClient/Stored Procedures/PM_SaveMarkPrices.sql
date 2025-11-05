  
/****************************************************************************                                      
Name :   PM_SaveMarkPrices                                      
Date Created: 22-Jun-2007                                       
Purpose:  Save Mark prices for day in database from Mark Price UI and Import Data.                                      
Module: MarkPriceAndForexConversion/PM                                  
Usage: exec PM_SaveMarkPrices                                   
 '<NewDataSet>
  <MarkPriceImport>
    <Symbol>MSFT</Symbol>
    <Date>05/17/2012</Date>
    <MarkPrice>10</MarkPrice>
    <ForwardPoints>0</ForwardPoints>
    <MarkPriceImportType>P</MarkPriceImportType>
  </MarkPriceImport>
  <MarkPriceImport>
    <Symbol>EUR-JPY 20160423</Symbol>
    <Date>05/17/2012</Date>
    <MarkPrice>12</MarkPrice>
    <ForwardPoints>0</ForwardPoints>
    <MarkPriceImportType>P</MarkPriceImportType>
  </MarkPriceImport>
  <MarkPriceImport>
    <Symbol>INR-USD SPOTMAN</Symbol>
    <Date>05/17/2012</Date>
    <MarkPrice>13</MarkPrice>
    <ForwardPoints>0</ForwardPoints>
    <MarkPriceImportType>P</MarkPriceImportType>
  </MarkPriceImport>
  <MarkPriceImport>
    <Symbol>EUR-JPY 20120619</Symbol>
    <Date>05/17/2012</Date>
    <MarkPrice>14</MarkPrice>
    <ForwardPoints>0</ForwardPoints>
    <MarkPriceImportType>P</MarkPriceImportType>
  </MarkPriceImport>
  <MarkPriceImport>
    <Symbol>INR-JPY 20120619</Symbol>
    <Date>05/17/2012</Date>
    <MarkPrice>15</MarkPrice>
    <ForwardPoints>0</ForwardPoints>
    <MarkPriceImportType>P</MarkPriceImportType>
  </MarkPriceImport>
</NewDataSet>', ' ', 0                                  
Author: Abhishek Mehta                          
Modified by Sandeep and Ashish as on 07-Jan-2009                          
 ****************************************************************************/                                      
CREATE Proc [dbo].[PM_SaveMarkPrices]                                      
(                     
   @Xml nText        
 , @ErrorMessage varchar(500) output                                      
 , @ErrorNumber int output            
)                                      
AS  
SET @ErrorMessage = 'Success'                                      
SET @ErrorNumber = 0                                      
BEGIN TRAN TRAN1                                       
                               
BEGIN TRY                            
                           
 DECLARE @handle int                                         
 exec sp_xml_preparedocument @handle OUTPUT,@Xml                                         
                                       
 CREATE TABLE #TempMarkPrices                                               
 (                                                                                             
  Symbol varchar(100)                    
  ,Date datetime                                    
  ,MarkPrice float                    
  ,MarkPriceImportType Varchar(5)  
,ForwardPoints float                                      
 )                                                   
                                                                                            
 INSERT INTO #TempMarkPrices                                      
 (                                                                                 
    Symbol                                    
   ,Date                                    
   ,MarkPrice                    
   ,MarkPriceImportType,ForwardPoints                           
  )                                                                                            
 SELECT                                                                                            
    Symbol                                    
   ,Date                                    
   ,MarkPrice                     
   ,MarkPriceImportType,ForwardPoints                                    
                                     
 FROM OPENXML(@handle, '//MarkPriceImport', 2)                                                                                               
  WITH                                                                                             
  (                                                                       
   Symbol varchar(100)                                         
  ,Date datetime                                    
  ,MarkPrice float                     
  ,MarkPriceImportType Varchar(5) ,  
  ForwardPoints float                                         
  ) 

--Check for Mark Price Save from livefeed or Import Module i.e. P from Import and L from livefeed
Declare @DataFromPBOrFeed Varchar(5)
Set @DataFromPBOrFeed = (Select Top 1 MarkPriceImportType From #TempMarkPrices) 

Create Table #FXConversionRates                                                                                                                                                                                                 
(                                                                                                                                                                    
 FromCurrencyID int,                                                                                                                                          
 ToCurrencyID int,                                                                                                                                                                                                        
 RateValue float,                                                                                                                                                                 
 ConversionMethod int,                                                                                                                                                                                                         
 Date DateTime,                                                                                
 eSignalSymbol varchar(max)                                      
) 

If (@DataFromPBOrFeed = 'P')
Begin    

	--Check for FX SPOT  and FX Forward
	Declare @StartDate DateTime
	Set @StartDate = (Select Min(Date) From #TempMarkPrices)

	Declare @EndDate DateTime
	Set @EndDate = (Select Max(Date) From #TempMarkPrices)

	Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @StartDate,@EndDate 

	Update #FXConversionRates                                                                                              
	 Set RateValue = 1.0/RateValue                                                                   
	 Where RateValue <> 0 and ConversionMethod = 1                                                                                                                                                  
	                                                                                             
	Update #FXConversionRates                                     
	 Set RateValue = 0                                                                                                                                                  
	 Where RateValue is Null
End

If (@DataFromPBOrFeed = 'P')
Begin

	Declare @FXNonZeroCheck int
	-- Check for non zero Forward Point record
	Set @FXNonZeroCheck = (Select Count(*) From #TempMarkPrices
	Inner Join V_SecMasterData SM On SM.TickerSymbol = #TempMarkPrices.Symbol
	Where ((SM.AssetID = 5 Or SM.AssetID = 11) And #TempMarkPrices.ForwardPoints <> 0))
	
	If ( @FXNonZeroCheck > 0)
		Begin
			
			Update #TempMarkPrices
			Set MarkPrice = IsNull(FXDayRates.RateValue,0) + ForwardPoints
			From #TempMarkPrices
				Inner Join V_SecMasterData SM On SM.TickerSymbol = #TempMarkPrices.Symbol
				Left outer join #FXConversionRates FXDayRates
				on (FXDayRates.FromCurrencyID = SM.LeadCurrencyID And FXDayRates.ToCurrencyID = SM.VsCurrencyID
				And DateDiff(Day,FXDayRates.Date,#TempMarkPrices.Date) = 0)
				Where (SM.AssetID = 5 Or SM.AssetID = 11)		
		End 
	Else -- if Forward Point is Zero and Mark Price is non Zero
		Begin
	
			Update #TempMarkPrices
			Set ForwardPoints = MarkPrice - IsNull(FXDayRates.RateValue,0)
			From #TempMarkPrices
				Inner Join V_SecMasterData SM On SM.TickerSymbol = #TempMarkPrices.Symbol
				Left outer join #FXConversionRates FXDayRates
				on (FXDayRates.FromCurrencyID = SM.LeadCurrencyID And FXDayRates.ToCurrencyID = SM.VsCurrencyID
				And DateDiff(Day,FXDayRates.Date,#TempMarkPrices.Date) = 0)
				Where (SM.AssetID = 5 Or SM.AssetID = 11)

		End

End  
                                     
                            
--DateDiff(d,Convert(VARCHAR(10), #TempMarkPrices.Date, 110),PM_DayMarkPrice.Date)=0                    
DELETE PM_DayMarkPrice from PM_DayMarkPrice                            
inner join  #TempMarkPrices on DateDiff(d,#TempMarkPrices.Date,PM_DayMarkPrice.Date)=0                           
and #TempMarkPrices.Symbol = PM_DayMarkPrice.Symbol                 
                
INSERT INTO            
 PM_DayMarkPrice                                      
 (                                      
   ApplicationMarkPrice,                                      
   PrimeBrokerMarkPrice,                                      
   FinalMarkPrice,                                
   Symbol,                                      
   IsActive,                                      
   Date,  
ForwardPoints                                    
 )                                 
SELECT  Distinct                                     
 0,                    
 Case MarkPriceImportType                  
  When 'P' -- for Prime Broker Mark Price                       
  Then MarkPrice                     
  When 'L'  -- for Live feed Mark Price                      
  Then 0                    
 Else 0                    
 End,                    
 Temp.MarkPrice, -- Final Mark Price that is in Use                                     
 Temp.Symbol,                                      
 1,                                    
 Temp.Date ,  
  ForwardPoints                               
  FROM                                       
   #TempMarkPrices Temp            
--   Inner Join T_Group G On G.Symbol=Temp.Symbol             
            
Union            
            
SELECT  Distinct                                     
 0,                    
 Case MarkPriceImportType                    
  When 'P' -- for Prime Broker Mark Price                       
  Then MarkPrice                     
  When 'L'  -- for Live feed Mark Price                      
  Then 0                    
 Else 0                    
 End,                    
 Temp.MarkPrice, -- Final Mark Price that is in Use                                     
 Temp.Symbol,                                      
 1,                                    
 Temp.Date , ForwardPoints                                 
  FROM                                       
   #TempMarkPrices Temp            
   Where Substring(Temp.Symbol,1,1) = '$'                               
                                
 DROP TABLE #TempMarkPrices                           
                                  
 EXEC sp_xml_removedocument @handle                                      
                                       
COMMIT TRANSACTION TRAN1                                      
                                 
END TRY                                      
 BEGIN CATCH                                       
  SET @ErrorMessage = ERROR_MESSAGE();                                      
  print @errormessage                                      
  SET @ErrorNumber = Error_number();                                       
  ROLLBACK TRANSACTION TRAN1                                       
END CATCH;  