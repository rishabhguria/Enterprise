GO 
  print 'Started Executing: ClientDB_P_MW_GetTradingNontradingType.sql'
GO 
                      
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[P_MW_GetTradingNontradingType]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
DROP PROCEDURE [dbo].[P_MW_GetTradingNontradingType]
END
GO

Create procedure P_MW_GetTradingNontradingType  
AS  
select   
1 as ID,  
'Trading' As [DisplayName]  
Union   
select   
2 As ID,  
'Non trading' As [DisplayName]  

  GO 
  print 'Done Executing: ClientDB_P_MW_GetTradingNontradingType.sql'
 GO 
