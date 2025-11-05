-- Check if T_RTPNL_UserWidgetConfigDetails exists
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_RTPNL_UserWidgetConfigDetails]'))  
BEGIN
    -- Check if WidgetKey column exists in T_RTPNL_UserWidgetConfigDetails and truncate if it does
    IF EXISTS (SELECT 1
               FROM INFORMATION_SCHEMA.COLUMNS
               WHERE TABLE_NAME = 'T_RTPNL_UserWidgetConfigDetails'
                 AND COLUMN_NAME = 'WidgetKey')
    BEGIN
        TRUNCATE TABLE [dbo].[T_RTPNL_UserWidgetConfigDetails];
        TRUNCATE TABLE [dbo].[T_Samsara_OpenfinWorkspaceInfo];
        TRUNCATE TABLE [dbo].[T_Samsara_OpenfinPageInfo];
    END
END

-- Check if T_RTPNL_UserWidgetConfigDetails_Deleted exists
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_RTPNL_UserWidgetConfigDetails_Deleted]'))  
BEGIN
    -- Check if WidgetKey column exists in T_RTPNL_UserWidgetConfigDetails_Deleted and truncate if it does
    IF EXISTS (SELECT 1
               FROM INFORMATION_SCHEMA.COLUMNS
               WHERE TABLE_NAME = 'T_RTPNL_UserWidgetConfigDetails_Deleted'
                 AND COLUMN_NAME = 'WidgetKey')
    BEGIN
        TRUNCATE TABLE [dbo].[T_RTPNL_UserWidgetConfigDetails_Deleted];
    END
END

-- Check if T_Samsara_CompanyUserLayouts exists
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_Samsara_CompanyUserLayouts]'))  
BEGIN
    -- Check if FileName column exists in T_Samsara_CompanyUserLayouts and truncate if it does
    IF EXISTS (SELECT 1
               FROM INFORMATION_SCHEMA.COLUMNS
               WHERE TABLE_NAME = 'T_Samsara_CompanyUserLayouts'
                 AND COLUMN_NAME = 'FileName')
    BEGIN
        TRUNCATE TABLE [dbo].[T_Samsara_CompanyUserLayouts];
        TRUNCATE TABLE [dbo].[T_Samsara_OpenfinWorkspaceInfo];
        TRUNCATE TABLE [dbo].[T_Samsara_OpenfinPageInfo];
    END
END

