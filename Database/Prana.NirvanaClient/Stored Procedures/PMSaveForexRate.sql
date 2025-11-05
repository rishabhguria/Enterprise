

/****************************************************************************                  
Name :   PMSaveForexRate                 
Date Created: 03-Jan-2008                   
Purpose:  Save Forex Rate DateWise.     
Module: MarkPriceAndForexConversion/PM             
Author: Sandeep Singh      
    
Execution Statement     
[PMSaveForexRate]      
'<NewDataSet>    
  <Table1>    
    <FromCurrencyID>1</FromCurrencyID>    
    <ToCurrencyID>2</ToCurrencyID>    
    <ConversionType>1</ConversionType>    
    <Date>1/8/2008 12:00:00 AM</Date>    
    <ConversionFactor>7.8031</ConversionFactor>    
    <Symbol>HKD A0-FX</Symbol>    
  </Table1>    
  <Table1>    
    <FromCurrencyID>1</FromCurrencyID>    
    <ToCurrencyID>3</ToCurrencyID>    
    <ConversionType>1</ConversionType>    
    <Date>1/8/2008 12:00:00 AM</Date>    
    <ConversionFactor>109.25</ConversionFactor>    
    <Symbol>JPY A0-FX</Symbol>    
  </Table1>    
  <Table1>    
    <FromCurrencyID>1</FromCurrencyID>    
    <ToCurrencyID>4</ToCurrencyID>    
    <ConversionType>1</ConversionType>    
    <Date>1/8/2008 12:00:00 AM</Date>    
    <ConversionFactor>1.976</ConversionFactor>    
    <Symbol>GBP A0-FX</Symbol>    
  </Table1>    
  <Table1>    
    <FromCurrencyID>1</FromCurrencyID>    
    <ToCurrencyID>5</ToCurrencyID>    
    <ConversionType>1</ConversionType>    
    <Date>1/8/2008 12:00:00 AM</Date>    
    <ConversionFactor>3.6725</ConversionFactor>    
    <Symbol>AED A0-FX</Symbol>    
  </Table1>    
</NewDataSet>','',0    
                
****************************************************************************/                  
 CREATE Proc [dbo].[PMSaveForexRate]                  
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
                  
Declare @count int       
Set @count=0      
Declare @count1 int       
Set @count1=0     
                  
DECLARE @handle int                     
exec sp_xml_preparedocument @handle OUTPUT,@Xml                     
                  
  CREATE TABLE #TempForexRate                                                                         
  (                                                                         
    FromCurrencyID int                  
   ,ToCurrencyID int                     
   ,Date datetime              
   ,ConversionType bit                 
   ,ConversionFactor float         
   ,Symbol varchar(50)                 
   )                                                                        
                                                                        
INSERT INTO #TempForexRate                  
 (                                                                        
    FromCurrencyID                  
   ,ToCurrencyID                    
   ,Date               
   ,ConversionType               
   ,ConversionFactor        
   ,Symbol                   
                    
 )                                                                        
SELECT                                                                         
  FromCurrencyID                  
 ,ToCurrencyID                    
 ,Date               
 ,ConversionType               
 ,ConversionFactor                 
 ,Symbol               
    FROM OPENXML(@handle, '//Table1', 2)                                                                           
 WITH                                                                         
 (                                                   
  FromCurrencyID int                  
 ,ToCurrencyID int                     
 ,Date datetime              
 ,ConversionType bit                 
 ,ConversionFactor float                  
 ,Symbol varchar(50)        
 )                  
                
DELETE  T_CurrencyConversion                  
  WHERE                   
DATEADD(day, DATEDIFF(day, 0, T_CurrencyConversion.Date), 0) in (select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempForexRate.Date, 113)), 0) from   #TempForexRate)           
--and T_CurrencyConversion.FromCurrencyID in (select #TempForexRate.FromCurrencyID From #TempForexRate)              
--and T_CurrencyConversion.ToCurrencyID in (select #TempForexRate.ToCurrencyID From #TempForexRate )                
--  AND                  
--  PM_DayMarkPrice.Symbol = #TempMarkPrices.Symbol   )                
Declare     
@fromCurrencyID  int,    
@toCurrencyID int,    
@date Datetime,        
@conversionType bit,    
@convertionFactor float,    
@symbol varchar(100)    
    
DECLARE CurrencyConversion_Cursor CURSOR FAST_FORWARD FOR                                
Select      
  FromCurrencyID                  
 ,ToCurrencyID                    
 ,Date               
 ,ConversionType               
 ,ConversionFactor                 
 ,Symbol     
From  #TempForexRate    
    
Open CurrencyConversion_Cursor;                              
                              
FETCH NEXT FROM CurrencyConversion_Cursor INTO       
@fromCurrencyID ,    
@toCurrencyID ,    
@date,         
@conversionType ,    
@convertionFactor ,    
@symbol  ;    
    
WHILE @@fetch_status = 0                                
  BEGIN     
-- insert the same values what we get from UI        
Set @count1= (Select count(*) from T_CurrencyConversion where FromCurrencyID=@fromCurrencyID and ToCurrencyID=@toCurrencyID    
And DATEADD(day, DATEDIFF(day, 0, T_CurrencyConversion.Date), 0) in     
(select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, @date, 113)), 0)) )    
if(@count1=0)    
begin               
 INSERT INTO                 
 T_CurrencyConversion                  
   (                  
    Date,                  
    FromCurrencyID,                  
    ToCurrencyID,                  
    ConversionType,                  
    ConversionFactor,      
    Symbol                      
    )                  
                  
  SELECT     
 @date,                  
 @fromCurrencyID ,    
 @toCurrencyID ,        
 @conversionType ,    
 @convertionFactor ,        
 Case When  @symbol='' then null                 
 Else @symbol End     
End    
Else    
Begin     
Update T_CurrencyConversion    
Set     
ConversionFactor =@convertionFactor,    
Symbol=Case When  @symbol='' then null                 
         Else @symbol End      
Where FromCurrencyID= @fromCurrencyID and ToCurrencyID= @toCurrencyID    
And DATEADD(day, DATEDIFF(day, 0, T_CurrencyConversion.Date), 0) in     
(select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, @date, 113)), 0))    
End    
     
-- check whether the reciprocal values lies or not    
-- if not then insert same for oposite currency ID    
Set @count= (Select count(*) from T_CurrencyConversion where FromCurrencyID=@toCurrencyID   and ToCurrencyID=@fromCurrencyID    
And DATEADD(day, DATEDIFF(day, 0, T_CurrencyConversion.Date), 0) in     
(select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, @date, 113)), 0)) )      
if(@count=0)      
begin      
INSERT INTO                 
 T_CurrencyConversion                  
   (                  
    Date,                  
    FromCurrencyID,                  
    ToCurrencyID,                  
    ConversionType,                  
    ConversionFactor,      
    Symbol                      
    )                  
                  
  SELECT                   
    @date,         
    @toCurrencyID,                
    @fromCurrencyID,                                 
    @conversionType,     
 Case When  @convertionFactor > 0 then Round(1/@convertionFactor,8)                 
 Else @convertionFactor End,         
    null     
end     
    
    
FETCH NEXT FROM CurrencyConversion_Cursor INTO                                 
    
@fromCurrencyID ,    
@toCurrencyID ,    
@date ,        
@conversionType ,    
@convertionFactor ,    
@symbol  ;    
    
END                                
CLOSE CurrencyConversion_Cursor;                                
DEALLOCATE CurrencyConversion_Cursor;             
                  
DROP TABLE #TempForexRate                 
                  
EXEC sp_xml_removedocument @handle                  
                  
COMMIT TRANSACTION TRAN1                  
                  
END TRY                  
BEGIN CATCH                   
 SET @ErrorMessage = ERROR_MESSAGE();                  
print @errormessage                  
 SET @ErrorNumber = Error_number();          
 ROLLBACK TRANSACTION TRAN1                   
END CATCH;
