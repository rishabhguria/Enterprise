        
/****************************************************************************                
Execution Statement :               
  delete from T_CommissionRuleAssets            
   select *  from   T_Commissionrules             
 select * from  T_CommissionRuleAssets            
 select * from  T_CommissionCriteria        
        
 Declare @ErrorMessage varchar(500)             
 Declare @ErrorNumber int             
  exec [P_SaveAndUpdateCommissionRules] '<?xml version="1.0"?>            
<ArrayOfCommissionRule xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">            
  <CommissionRule>            
    <RuleID>0430bae4-45d9-4ed5-a26d-a5315adb96b2</RuleID>            
    <AssetIdList>            
      <AssetCategory>1</AssetCategory>            
      <AssetCategory>2</AssetCategory>            
    </AssetIdList>            
    <RuleName>CommissionRule1</RuleName>            
    <RuleDescription>abcd</RuleDescription>            
    <ApplyRuleForTrade>0</ApplyRuleForTrade>            
    <RuleAppliedOn>0</RuleAppliedOn>            
    <CommissionRate>1</CommissionRate>            
    <MinCommission>0</MinCommission>            
    <IsClearingFeeApplied>false</IsClearingFeeApplied>            
    <ClearingFeeCalculationBasedOn>0</ClearingFeeCalculationBasedOn>            
    <ClearingFeeRate>0</ClearingFeeRate>            
    <MinClearingFee>0</MinClearingFee>            
    <IsCriteriaApplied>false</IsCriteriaApplied>            
    <IsModified>false</IsModified>            
  </CommissionRule>            
</ArrayOfCommissionRule>',@ErrorMessage,@ErrorNumber            
              
Date Created: Oct-15-2007            
Description: Insertion and Updation for Commission Rules             
Created By: Sandeep            
Date Modified : Oct-17-2007          
Description : Insertion           
Modified By : Abhishek          
  
  
Date Modified : 2012-04-09  
Description: Insertion  
Modified By : Rahul Gupta      Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1437  
****************************************************************************/              
CREATE Proc [dbo].[P_SaveAndUpdateCommissionRules]              
(              
   @Xml nText --XML input            
 , @ErrorMessage varchar(500) output              
 , @ErrorNumber int output              
)              
AS        
SET @ErrorMessage = 'Success'              
SET @ErrorNumber = 0              
BEGIN TRAN TRAN1               
              
BEGIN TRY              
            
DECLARE @handle int                 
exec sp_xml_preparedocument @handle OUTPUT,@Xml                 
            
CREATE TABLE #XmlItem            
(            
              
  RuleName varchar(100)            
, RuleDescription varchar(500)            
, ApplyRuleForTrade int             
, RuleAppliedOn int            
, CommissionRate float(53)          
, MinCommission float(53)          
, MaxCommission float(53)          
, IsCriteriaApplied bit            
, IsClearingFeeApplied bit           
, IsRoundOff bit        
, RoundOffValue int          
, RuleId uniqueidentifier            
, ClearingFeeCalculationBasedOn int            
, ClearingFeeRate Float(53)            
, MinClearingFee Float(53)             
, RuleAppliedOnForSoft int            
, CommissionRateForSoft float(53)          
, MinCommissionForSoft float(53)          
, MaxCommissionForSoft float(53)           
, IsCriteriaAppliedForSoft bit             
, IsRoundOffForSoft bit        
, RoundOffValueForSoft int         
, IsClearingBrokerFeeApplied bit 
, ClearingBrokerFeeCalculationBasedOn int            
, ClearingBrokerFeeRate Float(53)            
, MinClearingBrokerFee Float(53)
, IsCriteriaAppliedForClearingFee BIT
, IsCriteriaAppliedForClearingBrokerFee BIT            
)            
INSERT INTO #XmlItem            
(            
 RuleName             
, RuleDescription             
, ApplyRuleForTrade             
, RuleAppliedOn            
, CommissionRate             
, MinCommission          
, MaxCommission           
, IsCriteriaApplied            
, IsClearingFeeApplied            
, IsRoundOff         
, RoundOffValue           
, RuleId             
, ClearingFeeCalculationBasedOn            
, ClearingFeeRate            
, MinClearingFee             
, RuleAppliedOnForSoft
, CommissionRateForSoft
, MinCommissionForSoft
, MaxCommissionForSoft
, IsCriteriaAppliedForSoft
, IsRoundOffForSoft
, RoundOffValueForSoft
, IsClearingBrokerFeeApplied
, ClearingBrokerFeeCalculationBasedOn            
, ClearingBrokerFeeRate            
, MinClearingBrokerFee
, IsCriteriaAppliedForClearingFee
, IsCriteriaAppliedForClearingBrokerFee
)            
SELECT            
            
  RuleName             
, RuleDescription             
, ApplyRuleForTrade             
, RuleAppliedOn            
, CommissionRate             
, MinCommission          
, MaxCommission            
, IsCriteriaApplied            
, IsClearingFeeApplied         
, IsRoundOff         
, RoundOffValue           
, RuleId             
, ClearingFeeCalculationBasedOn            
, ClearingFeeRate            
, MinClearingFee             
, RuleAppliedOnForSoft
, CommissionRateForSoft
, MinCommissionForSoft
, MaxCommissionForSoft
, IsCriteriaAppliedForSoft
, IsRoundOffForSoft
, RoundOffValueForSoft
, IsClearingBrokerFeeApplied
, ClearingBrokerFeeCalculationBasedOn            
, ClearingBrokerFeeRate            
, MinClearingBrokerFee
, IsCriteriaAppliedForClearingFee
, IsCriteriaAppliedForClearingBrokerFee            
            
FROM            
OPENXML(@handle, '//CommissionRule', 2)                 
 WITH               
 (            
 RuleName Varchar(100)            
 ,RuleDescription varchar(500)     
 ,ApplyRuleForTrade int            
 ,RuleAppliedOn nchar(10) 'Commission/RuleAppliedOn'              
 ,CommissionRate float(53) 'Commission/CommissionRate'            
 ,MinCommission float(53) 'Commission/MinCommission'         
 ,MaxCommission float(53) 'Commission/MaxCommission'            
 ,IsCriteriaApplied bit 'Commission/IsCriteriaApplied'            
 ,IsClearingFeeApplied bit        
, IsRoundOff bit 'Commission/IsRoundOff'        
, RoundOffValue int 'Commission/RoundOffValue'             
 ,RuleID uniqueidentifier            
 , ClearingFeeCalculationBasedOn int 'ClearingFeeObj/RuleAppliedOn'           
 , ClearingFeeRate Float(53)	'ClearingFeeObj/ClearingFeeRate'           
 , MinClearingFee Float(53)     'ClearingFeeObj/MinClearingFee'          
, RuleAppliedOnForSoft nchar(10) 'SoftCommission/RuleAppliedOn'            
, CommissionRateForSoft float(53) 'SoftCommission/CommissionRate'         
, MinCommissionForSoft float(53)  'SoftCommission/MinCommission'         
, MaxCommissionForSoft float(53) 'SoftCommission/MaxCommission'           
, IsCriteriaAppliedForSoft bit 'SoftCommission/IsCriteriaApplied'            
, IsRoundOffForSoft bit 'SoftCommission/IsRoundOff'        
, RoundOffValueForSoft int 'SoftCommission/RoundOffValue'
, IsClearingBrokerFeeApplied bit 
, ClearingBrokerFeeCalculationBasedOn int   'ClearingBrokerFeeObj/RuleAppliedOn'        
, ClearingBrokerFeeRate Float(53)	'ClearingBrokerFeeObj/ClearingFeeRate'   
, MinClearingBrokerFee Float(53)	'ClearingBrokerFeeObj/MinClearingFee'   
, IsCriteriaAppliedForClearingFee BIT 'ClearingFeeObj/IsCriteriaApplied'
, IsCriteriaAppliedForClearingBrokerFee BIT 'ClearingBrokerFeeObj/IsCriteriaApplied'             
)            
 --This code inserts new data in to T_CommissionRules.              
DELETE FROM T_ClearingFee Where RuleId_FK In ( Select RuleId From #XmlItem)    
DELETE FROM T_ClearingFeeCriteria WHERE RuleId_FK In ( Select RuleId From #XmlItem)    
DELETE FROM T_CommissionRuleAssets  Where RuleID_FK In ( Select RuleId From #XmlItem)        
DELETE FROM T_CommissionCriteria  Where RuleID_FK In ( Select RuleId From #XmlItem)        
DELETE FROM T_CommissionRules Where RuleId In ( Select RuleId From #XmlItem)       
        
INSERT INTO              
  T_CommissionRules              
   (                   
  RuleId            
     ,RuleName               
     ,RuleDescription             
     ,ApplyRuleForTrade            
     ,CalculationBasedOn            
     ,CommissionRate            
     ,MinCommission            
     ,MaxCommission         
  ,IsCriteriaApplied            
  ,IsClearingFeeApplied         
, IsRoundOff         
, RoundOffValue             
, CalculationBasedOnForSoft
, CommissionRateForSoft
, MinCommissionForSoft
, MaxCommissionForSoft
, IsCriteriaAppliedForSoft
, IsRoundOffForSoft
, RoundOffValueForSoft
, IsClearingBrokerFeeApplied            
   )              
SELECT               
    RuleId               
 ,RuleName               
 ,RuleDescription            
 ,ApplyRuleForTrade            
 ,RuleAppliedOn            
 ,CommissionRate            
 ,MinCommission          
 ,MaxCommission           
 ,IsCriteriaApplied            
 ,IsClearingFeeApplied         
, IsRoundOff         
, RoundOffValue             
, RuleAppliedOnForSoft
, CommissionRateForSoft
, MinCommissionForSoft
, MaxCommissionForSoft
, IsCriteriaAppliedForSoft
, IsRoundOffForSoft
, RoundOffValueForSoft
, IsClearingBrokerFeeApplied            
            
FROM #XmlItem --WHERE RuleId Not In ( SELECT RuleId FROM T_CommissionRules )            
        
--This code inserts new data (Other Broker Fee) in to T_ClearingFee.              
INSERT INTO T_ClearingFee            
(            
  CalculationBasedOn            
 ,FeeRate            
 ,MinFee            
 ,RuleId_FK
 ,BrokerLevelFeeType 
 ,IsCriteriaApplied           
)            
SELECT            
   ClearingFeeCalculationBasedOn            
 , ClearingFeeRate            
 , MinClearingFee            
 , RuleId
 , 0
 , IsCriteriaAppliedForClearingFee             
            
FROM #XmlItem --Where IsClearingFeeApplied=1 --and RuleId Not In ( SELECT RuleId_FK FROM  T_ClearingFee )          

--This code inserts new data (Clearing Broker Fee) in to T_ClearingFee.              
INSERT INTO T_ClearingFee            
(            
  CalculationBasedOn            
 ,FeeRate            
 ,MinFee            
 ,RuleId_FK
 ,BrokerLevelFeeType
 , IsCriteriaApplied            
)            
SELECT            
   ClearingBrokerFeeCalculationBasedOn            
 , ClearingBrokerFeeRate            
 , MinClearingBrokerFee            
 , RuleId
 , 1
 , IsCriteriaAppliedForClearingBrokerFee             
            
FROM #XmlItem

CREATE TABLE #XmlItem1            
(            
              
  RuleId uniqueidentifier            
 ,AssetCategory int            
)            
            
INSERT INTO #XmlItem1            
(            
   RuleId             
  ,AssetCategory            
)            
SELECT            
  RuleID             
 ,AssetCategory          
           
FROM            
OPENXML(@handle, '/ArrayOfCommissionRule/CommissionRule/AssetIdList/AssetCategory',2)                 
WITH               
(            
  RuleID uniqueidentifier   '../../RuleID'          
 ,AssetCategory int '.'         
)        
            
          
--DELETE FROM T_CommissionRuleAssets  Where RuleID_FK In ( Select RuleId From #XmlItem1)        
          
INSERT INTO               
  T_CommissionRuleAssets              
   (                   
   RuleId_FK            
  ,AssetId_FK              
   )             
SELECT             
  RuleId             
 ,AssetCategory            
            
FROM  #XmlItem1 --Where RuleId Not In ( SELECT RuleId_FK From T_CommissionRuleAssets) and  AssetCategory Not In ( SELECT AssetId_FK From T_CommissionRuleAssets)            
          
CREATE TABLE #XmlItem2          
(           
   RuleId uniqueidentifier            
  ,ValueGreaterThan float           
  ,ValueLessThanOrEqual float        
  ,CommissionRate float
  ,CommissionType int           
)          
INSERT INTO #XmlItem2          
(          
   RuleId             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,CommissionRate
  ,CommissionType            
)          
SELECT          
          
   RuleId          
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,CommissionRate
  ,CommissionType            
FROM OPENXML (@handle, '//CommissionRuleCriteria',2)                 
WITH            
(          
  RuleID uniqueidentifier   '../../../RuleID'          
  ,ValueGreaterThan float           
  ,ValueLessThanOrEqual float           
  ,CommissionRate float
  ,CommissionType int             
)          
--DELETE FROM T_CommissionCriteria  Where RuleID_FK In ( Select RuleId From #XmlItem2)        
          
INSERT INTO T_CommissionCriteria          
(          
   RuleId_FK             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqualTo            
  ,CommissionRate
  ,CommissionType           
)           
SELECT           
   RuleId             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,CommissionRate
  ,CommissionType           
          
FROM #XmlItem2 --Where RuleId  In ( SELECT RuleId From T_CommissionRules where IsCriteriaApplied = 1 )    

CREATE TABLE #TempClearingFeeCriteria
(           
   RuleId uniqueidentifier            
  ,ValueGreaterThan float           
  ,ValueLessThanOrEqual float        
  ,ClearingFeeRate float
  ,ClearingFeeType int           
)          
INSERT INTO #TempClearingFeeCriteria          
(          
   RuleId             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,ClearingFeeRate
  ,ClearingFeeType            
)          
SELECT          
          
   RuleId          
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,ClearingFeeRate
  ,ClearingFeeType
FROM OPENXML (@handle, '//CommissionRule/ClearingBrokerFeeObj/ClearingFeeRuleCriteiaList/ClearingFeeCriteria',2)
WITH            
(          
  RuleID uniqueidentifier   '../../../RuleID' 
  ,ValueGreaterThan float           
  ,ValueLessThanOrEqual float           
  ,ClearingFeeRate float
  ,ClearingFeeType int             
)

INSERT INTO #TempClearingFeeCriteria          
(          
   RuleId             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,ClearingFeeRate
  ,ClearingFeeType            
)          
SELECT          
          
   RuleId          
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,ClearingFeeRate
  ,ClearingFeeType
FROM OPENXML (@handle, '//CommissionRule/ClearingFeeObj/ClearingFeeRuleCriteiaList/ClearingFeeCriteria',2)
WITH            
(          
  RuleID uniqueidentifier   '../../../RuleID' 
  ,ValueGreaterThan float           
  ,ValueLessThanOrEqual float           
  ,ClearingFeeRate float
  ,ClearingFeeType int             
)

INSERT INTO T_ClearingFeeCriteria          
(          
   RuleId_FK             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqualTo            
  ,ClearingFeeRate
  ,ClearingFeeType           
)           
SELECT           
   RuleId             
  ,ValueGreaterThan            
  ,ValueLessThanOrEqual            
  ,ClearingFeeRate
  ,ClearingFeeType           
          
FROM #TempClearingFeeCriteria
        
DROP Table #XmlItem             
DROP Table #XmlItem1            
DROP Table #XmlItem2
DROP Table #TempClearingFeeCriteria            
EXEC sp_xml_removedocument @handle              
        
            
        
COMMIT TRANSACTION TRAN1              
              
END TRY              
BEGIN CATCH               
 SET @ErrorMessage = ERROR_MESSAGE();              
 SET @ErrorNumber = Error_number();               
             
 ROLLBACK TRANSACTION TRAN1                 
END CATCH;

