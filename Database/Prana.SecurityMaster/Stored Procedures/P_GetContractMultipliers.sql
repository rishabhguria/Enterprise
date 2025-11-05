
CREATE PROCEDURE [dbo].[P_GetContractMultipliers]

AS

BEGIN TRY  
BEGIN TRAN   
  
 DECLARE @UDAString  VARCHAR(MAX)  
 DECLARE @TempTableStr NVARCHAR(MAX)  
 SET  @UDAString = ''  
  
 SELECT @UDAString = @UDAString + ',' + ' COALESCE(DynamicUDA.value(''(/DynamicUDAs/' + Tag  + ')[1]'', ''varchar(100)''), ''' + ISNULL(REPLACE(DefaultValue,'''',''''''),'Undefined') + ''') As [' + Tag +']'  
 FROM T_UDA_DynamicUDA  
  
 SET  @TempTableStr = 'SELECT   
        Symbol,  
        Exchange,   
        Multiplier,  
        PSSymbol,   
        UnderlyingSymbol,  
        CutOffTime,  
        ProxyRoot,  
        UDAAssetClassID,  
        UDASecurityTypeID,  
        UDASectorID,  
        UDASubSectorID,  
        UDACountryID,  
        IsCurrencyFuture '  
        + @UDAString + ' ,  
        BBGYellowKey,
        BBGRoot 
        FROM T_FutureMultipliers'  
 EXEC(@TempTableStr)  
  
COMMIT   
  
END TRY  
  
BEGIN CATCH  
 --print('Error occured rolling back transaction')  
 ROLLBACK  
    DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
  
    SELECT @ErrorMessage = ERROR_MESSAGE(),  
           @ErrorSeverity = ERROR_SEVERITY(),  
           @ErrorState = ERROR_STATE();  
   
  
    -- Use RAISERROR inside the CATCH block to return   
    -- error information about the original error that   
    -- caused execution to jump to the CATCH block.  
    RAISERROR (@ErrorMessage, -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState -- State.  
               );  
END CATCH

