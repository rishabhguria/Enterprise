BEGIN TRY

    -- Delete from each table
    DELETE FROM T_Samsara_CompanyUserLayouts;
    PRINT 'Deleted from T_Samsara_CompanyUserLayouts.';
    DELETE FROM T_Samsara_OpenfinWorkspaceInfo;
    PRINT 'Deleted from T_Samsara_OpenfinWorkspaceInfo.';
    DELETE FROM T_Samsara_OpenfinPageInfo;
    PRINT 'Deleted from T_Samsara_OpenfinPageInfo.';
    DELETE FROM T_RTPNL_UserWidgetConfigDetails;
    PRINT 'Deleted from T_RTPNL_UserWidgetConfigDetails.';

END TRY
BEGIN CATCH
    -- Log error message
    PRINT 'An error occurred:';
    PRINT ERROR_MESSAGE();
END CATCH;