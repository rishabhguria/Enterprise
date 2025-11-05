CREATE PROCEDURE [dbo].[P_SaveWorkAreaPreferences] (  
 @Xml XML  
 , @ErrorMessage VARCHAR(500) OUTPUT  
 , @ErrorNumber INT OUTPUT  
 )  
AS  
SET @ErrorNumber = 0  
SET @ErrorMessage = 'Success'  
BEGIN TRY  
 BEGIN TRAN TRAN1  
 DECLARE @handle INT  
 EXEC sp_xml_preparedocument @handle OUTPUT  
  , @Xml  
 CREATE TABLE #XmlItem (  
  CounterParty INT  
  , Venue INT  
  , TIF INT  
  , TradingAccount INT  
  , IsUseWorkAreaPreferences BIT  
  , CommaSeparatedFunds VARCHAR(MAX)  
  , CalculationStrategy INT  
  , IsIncludeExcludeAllowed BIT  
  )  
 INSERT INTO #XmlItem (  
  CounterParty  
  , Venue  
  , TIF  
  , TradingAccount  
  , IsUseWorkAreaPreferences  
  , CommaSeparatedFunds  
  , CalculationStrategy  
  , IsIncludeExcludeAllowed  
  )  
 SELECT CounterParty  
  , Venue  
  , TIF  
  , TradingAccount  
  , IsUseWorkAreaPreferences  
  , CommaSeparatedFunds  
  , CalculationStrategy  
  , IsIncludeExcludeAllowed  
 FROM OPENXML(@handle, '//WorkAreaPreferences', 2) WITH (  
   CounterParty INT  
   , Venue INT  
   , TIF INT  
   , TradingAccount INT  
   , IsUseWorkAreaPreferences BIT  
   , CommaSeparatedFunds VARCHAR(MAX)  
   , CalculationStrategy INT  
   , IsIncludeExcludeAllowed BIT  
   )  
 --delete existing work area preferences      
 DELETE  
 FROM T_WorkAreaPreferences  
 INSERT INTO T_WorkAreaPreferences (  
  CounterPartyID  
  , VenueID  
  , TIFID  
  , TradingAccountID  
  , IsUseWorkAreaPreferences  
  , CommaSeparatedFunds  
  , CalculationStrategy  
  , IsIncludeExcludeAllowed 
  )  
 SELECT CounterParty  
  , Venue  
  , TIF  
  , TradingAccount  
  , IsUseWorkAreaPreferences  
  , CommaSeparatedFunds  
  , CalculationStrategy  
  , IsIncludeExcludeAllowed  
 FROM #XmlItem  
 EXEC sp_xml_removedocument @handle  
 COMMIT TRANSACTION TRAN1  
END TRY  
BEGIN CATCH  
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();  
 ROLLBACK TRANSACTION TRAN1  
END CATCH;  
