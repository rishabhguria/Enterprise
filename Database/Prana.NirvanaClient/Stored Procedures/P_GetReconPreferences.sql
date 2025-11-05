
/****** Object:  StoredProcedure [dbo].[P_GetReconPreferences]    Script Date: 10/21/2015 17:04:58 ******/
CREATE PROCEDURE [dbo].[P_GetReconPreferences]  
AS  
BEGIN  
 SELECT ReconPreferenceId,
ClientID,
ReconTypeID,
TemplateName,
TemplateKey,
IsShowCAGeneratedTrades 
FROM T_ReconPreferences
END