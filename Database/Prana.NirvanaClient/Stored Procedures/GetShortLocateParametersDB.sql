CREATE PROCEDURE [dbo].[GetShortLocateParametersDB]
as    
DECLARE @ShowMasterFund INT
DECLARE @ImportOverride INT
CREATE TABLE #TempTable (ShowMasterFund INT,ImportOverride INT)

IF EXISTS (        
  SELECT PreferenceValue        
  FROM T_PranaKeyValuePreferences        
  WHERE PreferenceKey = 'IsShowmasterFundOnShortLocate'        
  )        
 SELECT @ShowMasterFund=CONVERT(INT, PreferenceValue)        
 FROM T_PranaKeyValuePreferences        
 WHERE PreferenceKey = 'IsShowmasterFundOnShortLocate'


IF EXISTS (        
  SELECT PreferenceValue        
  FROM T_PranaKeyValuePreferences        
  WHERE PreferenceKey = 'IsImportOverrideOnShortLocate'        
  )        
 SELECT @ImportOverride=CONVERT(INT, PreferenceValue)        
 FROM T_PranaKeyValuePreferences        
 WHERE PreferenceKey = 'IsImportOverrideOnShortLocate'

Insert into #TempTable values(@ShowMasterFund,@ImportOverride)
select * from #TempTable
