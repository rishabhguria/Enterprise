
--Name : P_GetAllAssetsWithUndefinedValue
--Date : 12-16-2008
--Execultion : exec P_GetAllAssetsWithUndefinedValue   
--Purpose : To get all assets along with the undefined value so as to accomodate those asset values whose asset is
--			not defined.
  
--Modified Date: 23-Sep-2015                                                                                          
--Modified By : Pooja Porwal                                                                                                                                                             
--Description: Added a Equity Swap as a saperate AssetType     
--Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-9557   
--exec P_GetAllAssetsWithUndefinedValue     
  
CREATE PROCEDURE dbo.P_GetAllAssetsWithUndefinedValue
AS  
  
  
  
Declare @EquitySwapId Int    
select  @EquitySwapId=MAX(AssetId)+1 from T_Asset     
   
 select * from   
 ( SELECT AssetID, AssetName, Comment From T_Asset     
  UNION ALL  
  SELECT @EquitySwapId, 'EquitySwap', ''   
 UNION ALL
  SELECT -2147483648, 'Undefined', '' )  Asset   
order BY Asset.AssetName   
  
  