CREATE PROCEDURE [dbo].[P_UpdateRebalPreferencesForAllAccounts]
	@rebalPreferenceKey varchar(200),
	@rebalPreferenceValueXml nText
AS
BEGIN

   DECLARE @handle int                          
   exec sp_xml_preparedocument @handle OUTPUT,@rebalPreferenceValueXml

    CREATE TABLE #TempTablePreferences                                                                               
  (                                                                               
    AccountId int,      
    PreferenceValue NVARCHAR(MAX)                    
   )        
      
   INSERT INTO #TempTablePreferences                     
   (                                                                              
      AccountId                        
     ,PreferenceValue                                                       
   ) 
   SELECT                                                                               
	AccountId                        
   ,PreferenceValue                                      
    FROM OPENXML(@handle, '/DSPreferenceMapping/TABPreferenceMapping', 2)                                                                                 
   WITH                                                                               
   (                                                         
   AccountId int,      
   PreferenceValue NVARCHAR(MAX)               
   ) 

   DELETE FROM T_RebalPreferences 
   WHERE AccountId in (SELECT AccountId FROM #TempTablePreferences) AND PreferenceKey = @rebalPreferenceKey

   INSERT into T_RebalPreferences (PreferenceKey,AccountId,PreferenceValue) SELECT @rebalPreferenceKey,AccountId,PreferenceValue FROM #TempTablePreferences

   EXEC sp_xml_removedocument @handle
END
