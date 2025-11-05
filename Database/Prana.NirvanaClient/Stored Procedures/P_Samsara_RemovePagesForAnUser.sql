/****************************************************************************
Name    : [P_Samsara_RemovePagesForAnUser]
Purpose : Deletes all relevant layout and configuration data for the specified user
          based on the provided module context.
Module  : SamsaraWorkspaceManagement
****************************************************************************/
CREATE PROCEDURE [dbo].[P_Samsara_RemovePagesForAnUser]
    @userId INT,
    @modules VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON; -- Prevents returning the count of rows affected

    -- Normalize the @modules input for case-insensitive comparison
    DECLARE @normalizedModules VARCHAR(MAX) = LOWER(@modules);

    BEGIN TRY
        -- Start a transaction to ensure atomicity (all operations succeed or all are rolled back)
        BEGIN TRANSACTION;

        ------------------------------------------------------------------------
        -- Conditional deletion based on module for T_Samsara_CompanyUserLayouts
        ------------------------------------------------------------------------
        IF @normalizedModules = 'rtpnl'
        BEGIN
            DELETE FROM T_Samsara_CompanyUserLayouts
            WHERE userId = @userId AND ModuleName = 'RTPNL';
        END
        ELSE IF @normalizedModules = 'blotter'
        BEGIN
            DELETE FROM T_Samsara_CompanyUserLayouts
            WHERE userId = @userId AND ModuleName = 'Blotter';
        END
        ELSE IF @normalizedModules = 'all'
        BEGIN
            -- Delete all layout data for the user
            DELETE FROM T_Samsara_CompanyUserLayouts
            WHERE userId = @userId;
        END

        ------------------------------------------------------------------------
        -- Delete from T_Samsara_OpenfinPageInfo (relevant for RTPNL and All)
        ------------------------------------------------------------------------
        IF @normalizedModules IN ('rtpnl', 'all')
        BEGIN
            DELETE FROM T_Samsara_OpenfinPageInfo
            WHERE userId = @userId;
        END
        
        ------------------------------------------------------------------------
        -- Delete from T_RTPNL_UserWidgetConfigDetails (relevant for RTPNL and All)
        ------------------------------------------------------------------------
        IF @normalizedModules IN ('rtpnl', 'all')
        BEGIN
            DELETE FROM T_RTPNL_UserWidgetConfigDetails
            WHERE userId = @userId;
        END

        ------------------------------------------------------------------------
        -- Delete from T_Samsara_OpenfinWorkspaceInfo (only for 'All')
        ------------------------------------------------------------------------
        IF @normalizedModules = 'all'
        BEGIN
            DELETE FROM T_Samsara_OpenfinWorkspaceInfo
            WHERE userId = @userId;
        END

        -- If all operations succeed, commit the transaction
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- If an error occurs, check if a transaction is open and roll it back
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Re-throw the error to the calling application (or log it)
        -- This ensures the caller knows something went wrong.
        THROW;

        -- Optionally, you can log the error details:
        -- INSERT INTO ErrorLog (ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, ErrorTime)
        -- VALUES (ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), GETDATE());
    END CATCH
END;