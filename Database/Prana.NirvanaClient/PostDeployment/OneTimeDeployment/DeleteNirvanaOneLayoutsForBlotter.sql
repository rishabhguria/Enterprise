BEGIN TRY

    -- Delete from each table
    DELETE FROM T_Samsara_CompanyUserLayouts where ViewName = 'Blotter';
    PRINT 'Deleted from T_Samsara_CompanyUserLayouts.';
    
END TRY
BEGIN CATCH
    -- Log error message
    PRINT 'An error occurred:';
    PRINT ERROR_MESSAGE();
END CATCH;