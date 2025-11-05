IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_UserWidgetConfigDetails]'))      
BEGIN                
Drop table T_UserWidgetConfigDetails      
END          

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_OpenfinPageInfo]'))      
BEGIN                
Drop table T_OpenfinPageInfo      
END     

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_OpenfinWorkspaceInfo]'))      
BEGIN                
Drop table T_OpenfinWorkspaceInfo      
END     

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_Samsara_CompanyUserLayouts]'))      
--BEGIN                
--Drop table T_Samsara_CompanyUserLayouts      
--END  