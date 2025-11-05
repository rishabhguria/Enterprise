  
--exec PMGetFundPositionsForDate getutcdate() '2007-11-02 02:10:00'                                     
                               
/****************************************************************************                                                                                  
Name :   [PMGetTotalOpenAllocatedDataForBaskets]                                                                                 
Date Created: 13-sp-2007                                                                                   
Purpose:                                          
Author: Bhupesh Bareja                                                                                  
Parameters:                               
Execution StateMent:   
declare @AllAUECDatesString VARCHAR(MAX)  
Set @AllAUECDatesString =  '0^11/26/2008 12:00:00 AM~1^11/26/2008 12:00:00 AM~11^11/26/2008 12:00:00 AM~12^11/26/2008 12:00:00 AM~15^11/26/2008 12:00:00 AM~16^11/26/2008 12:00:00 AM~17^11/26/2008 12:00:00 AM~18^11/26/2008 12:00:00 AM~19^11/26/2008 12:00:0
0 AM~20^11/26/2008 12:00:00 AM~21^11/26/2008 12:00:00 AM~22^11/26/2008 12:00:00 AM~23^11/26/2008 12:00:00 AM~24^11/26/2008 12:00:00 AM~26^11/26/2008 12:00:00 AM~27^11/26/2008 12:00:00 AM~28^11/26/2008 12:00:00 AM~29^11/26/2008 12:00:00 AM~30^11/26/2008 12
:00:00 AM~31^11/26/2008 12:00:00 AM~32^11/26/2008 12:00:00 AM~33^11/26/2008 12:00:00 AM~34^11/26/2008 12:00:00 AM~36^11/26/2008 12:00:00 AM~37^11/26/2008 12:00:00 AM~38^11/26/2008 12:00:00 AM~39^11/26/2008 12:00:00 AM~43^11/26/2008 12:00:00 AM~44^11/26/20
08 12:00:00 AM~45^11/26/2008 12:00:00 AM~47^11/26/2008 12:00:00 AM~48^11/26/2008 12:00:00 AM~49^11/26/2008 12:00:00 AM~53^11/26/2008 12:00:00 AM~54^11/26/2008 12:00:00 AM~55^11/26/2008 12:00:00 AM~56^11/26/2008 12:00:00 AM~57^11/26/2008 12:00:00 AM~58^11/
26/2008 12:00:00 AM~59^11/26/2008 12:00:00 AM~60^11/26/2008 12:00:00 AM~61^11/26/2008 12:00:00 AM~62^11/26/2008 12:00:00 AM~63^11/26/2008 12:00:00 AM~64^11/26/2008 12:00:00 AM~65^11/26/2008 12:00:00 AM~66^11/26/2008 12:00:00 AM~67^11/26/2008 12:00:00 AM~6
8^11/26/2008 12:00:00 AM~69^11/26/2008 12:00:00 AM~70^11/26/2008 12:00:00 AM~71^11/26/2008 12:00:00 AM~72^11/26/2008 12:00:00 AM~73^11/26/2008 12:00:00 AM~74^11/26/2008 12:00:00 AM~75^11/26/2008 12:00:00 AM~76^11/26/2008 12:00:00 AM~77^11/26/2008 12:00:00
 AM~78^11/26/2008 12:00:00 AM~79^11/26/2008 12:00:00 AM~80^11/26/2008 12:00:00 AM~81^11/26/2008 12:00:00 AM~82^11/26/2008 12:00:00 AM~83^11/26/2008 12:00:00 AM~86^11/26/2008 12:00:00 AM~87^11/26/2008 12:00:00 AM~'  
exec  PMGetFundOpenPositionsForDateBase @AllAUECDatesString                                                           
EXEC [PMGetTotalOpenAllocatedDataForBaskets] @AllAUECDatesString,5  , '', 0                                      
Date Modified: <02-Nov-2007>                                                                                   
Description:     <DescriptionOfChange>                                                                                   
Modified By:     <Sandeep>    
Date Modified: <13-May-2008>                                                                                   
Description:     <DescriptionOfChange>                                                                                   
Modified By:     <Sandeep>                                                              
****************************************************************************/                                
                                                                                             
CREATE PROCEDURE [dbo].[PMGetTotalOpenAllocatedDataForBaskets] (                                                                                  
  @AllAUECDatesString VARCHAR(MAX),                    
  @CompanyID int,   --this parameter is not in use in this sp but afterwards will be used                              
  @ErrorMessage varchar(500) output,                                        
  @ErrorNumber int output                                                         
 )                                                                                  
                                                            
AS                                             
                                        
SET @ErrorMessage = 'Success'                                          
SET @ErrorNumber = 0                                
CREATE TABLE #Temp                               
(                              
TaxLotID varchar(50),                              
CreationDate datetime,                              
SideID varchar(10),                               
Symbol varchar(50),                          
OpenQuantity float,                           
AveragePrice float ,                          
FundID varchar(20),                                                                                                       
AssetID int,                            
UnderLyingID int,                            
ExchangeID int,                            
CurrencyID int,                            
AUECID int,                                                                                                               
TotalCommissionandFees float ,     
Multiplier float  ,                                                                                                                                    
SettlementDate datetime NULL,                                                              
VsCurrencyID int ,                                                    
TradedCurrencyID int ,                                                     
ExpirationDate datetime NULL ,                      
Description varchar(500),                    
Level2ID int  ,            
NotionalValue float,            
BenchMarkRate float ,             
Differential float,            
OrigCostBasis float,            
DayCount int,            
SwapDescription varchar(100),            
FirstResetDate datetime,            
OrigTransDate datetime,            
IsSwapped int ,     
AUECLocalDate datetime,                  
GroupID varchar(50),            
PositionTag int,  
FXRate float,  
FXConversionMethodOperator char(5)                      
)                              
INSERT INTO #Temp                              
exec  PMGetFundOpenPositionsForDateBase_New @AllAUECDatesString                                
                               
 SELECT -- the commented fields are previously used, now they are replaced                              
 FundID  --FundID                                   
 ,Symbol   --Symbol                                       
 ,OpenQuantity --Quantity                                          
 ,AUECID --AUECID                               
 ,CASE T_Side.SideTagValue                                                                                                              
   WHEN '1' Then 0                                                          
   WHEN '2' Then 1                                                                                                             
   WHEN '5' Then 1    --Sell Short                                                                                                            
   WHEN 'A' Then 0     --Buy To Open                                                                                                            
   WHEN 'B' Then 0    --Buy To Close                                                       
   WHEN 'C' Then 1    --Sell To Open                                                               
   WHEN 'D' Then 1   --Sell To Close                                                                                     
  END As IsPosition --PositionType                                           
 ,AssetID   --assetID                                       
 ,UnderLyingID  --UnderLyingID                                    
 ,AveragePrice --AveragePrice                               
 ,TaxLotID --TaxLotID                                   
 ,T_Side.Side As Side --Side                               
                               
 FROM #Temp                              
 INNER JOIN T_Side On T_Side.SideTagValue = #TEMP.SideID                               
                               
 DROP TABLE #Temp                         
  