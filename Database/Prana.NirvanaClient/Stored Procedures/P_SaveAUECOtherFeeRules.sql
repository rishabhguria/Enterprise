Create PROCEDURE [dbo].[P_SaveAUECOtherFeeRules]      
 (        
  @Xml nText        
  ,@ErrorMessage varchar(500) output        
  ,@ErrorNumber int output    
 )        
AS        
SET @ErrorMessage = 'Success'        
SET @ErrorNumber = 0        
BEGIN TRAN TRAN1         
        
BEGIN TRY        
        
DECLARE @handle int           
exec sp_xml_preparedocument @handle OUTPUT,@Xml

CREATE TABLE #TEMPOtherFeeRules
(      
LongFeeRate float,                          
ShortFeeRate float ,        
LongCalculationBasis int ,                          
ShortCalculationBasis int ,        
RoundOffPrecision smallint ,        
MaxValue float  ,        
MinValue float ,      
AUECID int,
FeeTypeID int, 
RoundUpPrecision smallint,
RoundDownPrecision smallint,
FeePrecisionType smallint,
IsCriteriaApplied bit
) 

INSERT INTO #TEMPOtherFeeRules 
(      
LongFeeRate,                          
ShortFeeRate,        
LongCalculationBasis,                          
ShortCalculationBasis,        
RoundOffPrecision,        
MaxValue,        
MinValue,      
AUECID,
FeeTypeID,
RoundUpPrecision,
RoundDownPrecision,
FeePrecisionType,
IsCriteriaApplied
)
SELECT
LongRate,                          
ShortRate,        
LongCalculationBasis,                          
ShortCalculationBasis,        
RoundOffPrecision,        
MaxValue,        
MinValue,      
AUECID,
OtherFeeType,
RoundUpPrecision,
RoundDownPrecision,
FeePrecisionType,
IsCriteriaApplied
FROM         
   OPENXML(@handle, '//OtherFeeRule', 2)           
  WITH         
   (LongRate float, ShortRate float, LongCalculationBasis Integer, ShortCalculationBasis Integer, RoundOffPrecision smallint, MaxValue float, MinValue float, AUECID Integer, OtherFeeType Integer,RoundUpPrecision smallint,
RoundDownPrecision smallint,FeePrecisionType smallint,IsCriteriaApplied bit)  XmlItem      

CREATE TABLE #TEMPLongOtherFeeCriteria
(      
OtherFeesCriteriaId       BIGINT,    
LongValueGreaterThan       FLOAT ,
LongValueLessThanOrEqualTo FLOAT ,
LongFeeRate               FLOAT ,  
LongCalculationBasis       int,
AUECID							int,
FeeTypeID						int
--OtherFeeRuleId_FK         UNIQUEIDENTIFIER   
) 

INSERT INTO #TEMPLongOtherFeeCriteria 
(      
OtherFeesCriteriaId,       
LongValueGreaterThan,    
LongValueLessThanOrEqualTo,
LongFeeRate,    
LongCalculationBasis,            
AUECID,	
FeeTypeID
--OtherFeeRuleId_FK     
)
SELECT
OtherFeesCriteriaId,       
LongValueGreaterThan,    
LongValueLessThanOrEqual,
LongFeeRate, 
LongCalculationBasis,               
AUECID,	
OtherFeeType
--RuleID
FROM         
   OPENXML(@handle, '//LongFeeRuleCriteriaList//OtherFeesCriteria', 2)           
  WITH         
   (OtherFeesCriteriaId bigint, LongValueGreaterThan float, LongValueLessThanOrEqual float, LongFeeRate float,LongCalculationBasis integer,AUECID integer '../../AUECID',	
OtherFeeType integer '../../OtherFeeType')  XmlItem1   

--select * from #TEMPLongOtherFeeCriteria


CREATE TABLE #TEMPShortOtherFeeCriteria
(      
OtherFeesCriteriaId       BIGINT,    
ShortValueGreaterThan       FLOAT ,
ShortValueLessThanOrEqualTo FLOAT ,
ShortFeeRate               FLOAT ,  
ShortCalculationBasis       int, 
AUECID							int,
FeeTypeID						int
--OtherFeeRuleId_FK         UNIQUEIDENTIFIER     
) 

INSERT INTO #TEMPShortOtherFeeCriteria 
(      
OtherFeesCriteriaId,       
ShortValueGreaterThan,    
ShortValueLessThanOrEqualTo,
ShortFeeRate,
ShortCalculationBasis,                
AUECID,	
FeeTypeID
--OtherFeeRuleId_FK     
)
SELECT
OtherFeesCriteriaId,       
ShortValueGreaterThan,    
ShortValueLessThanOrEqual,
ShortFeeRate, 
ShortCalculationBasis,               
AUECID,	
OtherFeeType
--RuleID
FROM         
   OPENXML(@handle, '//ShortFeeRuleCriteriaList//OtherFeesCriteria', 2)           
  WITH         
   (OtherFeesCriteriaId bigint, ShortValueGreaterThan float, ShortValueLessThanOrEqual float, ShortFeeRate float,ShortCalculationBasis integer,AUECID integer '../../AUECID',	
OtherFeeType integer '../../OtherFeeType')  XmlItem2  

--select * from #TEMPShortOtherFeeCriteria


CREATE TABLE #TEMPOtherFeeCriteria
(      
OtherFeesCriteriaId         BIGINT,    
LongValueGreaterThan        FLOAT ,
LongValueLessThanOrEqualTo  FLOAT ,
LongFeeRate                 FLOAT ,  
LongCalculationBasis        int,
LongAUECID					int,
LongFeeTypeID					int,
ShortValueGreaterThan       FLOAT ,
ShortValueLessThanOrEqualTo FLOAT ,
ShortFeeRate                FLOAT ,  
ShortCalculationBasis       int, 
ShortAUECID					int,
ShortFeeTypeID					int    
)

INSERT INTO #TEMPOtherFeeCriteria 
(  
OtherFeesCriteriaId,          
LongValueGreaterThan,      
LongValueLessThanOrEqualTo,  
LongFeeRate, 
LongCalculationBasis,
LongAUECID,
LongFeeTypeID,
ShortValueGreaterThan,
ShortValueLessThanOrEqualTo,
ShortFeeRate,
ShortCalculationBasis,	
ShortAUECID,
ShortFeeTypeID				 
)
SELECT     
LC.OtherFeesCriteriaId,
LC.LongValueGreaterThan,    
LC.LongValueLessThanOrEqualTo,
LC.LongFeeRate, 
LC.LongCalculationBasis,  
LC.AUECID,
LC.FeeTypeID,                  
SC.ShortValueGreaterThan,    
SC.ShortValueLessThanOrEqualTo,
SC.ShortFeeRate, 
SC.ShortCalculationBasis, 
SC.AUECID,
SC.FeeTypeID       
from #TEMPLongOtherFeeCriteria LC FULL OUTER JOIN  #TEMPShortOtherFeeCriteria SC 
on LC.AUECID=SC.AUECID and LC.FeeTypeID=SC.FeeTypeID and LC.OtherFeesCriteriaId=SC.OtherFeesCriteriaId

--select * from #TEMPOtherFeeCriteria
--select * from #TEMPOtherFeeRule

DELETE FROM T_OtherFeesCriteria
WHERE OtherFeeRuleId_FK IN (SELECT OtherFeeRuleID FROM T_OtherFeeRules
WHERE AUECID IN (SELECT AUECID FROM #TEMPOtherFeeRules))

DELETE T_OtherFeeRules
WHERE AUECID IN (SELECT AUECID FROM #TEMPOtherFeeRules)
AND FeeTypeID NOT IN (SELECT FeeTypeID FROM #TEMPOtherFeeRules)
    
    
----This code updates old data.        
UPDATE T_OtherFeeRules        
SET         
LongFeeRate = TOFR.LongFeeRate, 
ShortFeeRate = TOFR.ShortFeeRate,  
LongCalculationBasis = TOFR.LongCalculationBasis,  
 ShortCalculationBasis = TOFR.ShortCalculationBasis,  
RoundOffPrecision = TOFR.RoundOffPrecision,        
 MaxValue = TOFR.MaxValue,  
 MinValue = TOFR.MinValue,
RoundUpPrecision=TOFR.RoundUpPrecision,
RoundDownPrecision=TOFR.RoundDownPrecision,
FeePrecisionType=TOFR.FeePrecisionType,
IsCriteriaApplied=TOFR.IsCriteriaApplied

  FROM         
   #TEMPOtherFeeRules TOFR INNER JOIN T_OtherFeeRules OFR ON TOFR.AUECID = OFR.AUECID
 Where TOFR.AUECID = OFR.AUECID
	AND TOFR.FeeTypeID = OFR.FeeTypeID       
    
--This code inserts new data.        
        
Insert Into         
 T_OtherFeeRules        
 (        
 OtherFeeRuleID,         
 LongFeeRate,  
 ShortFeeRate,  
 LongCalculationBasis,  
 ShortCalculationBasis,  
 RoundOffPrecision,  
 MaxValue,  
 MinValue,  
 AUECID,  
 FeeTypeID,
 RoundUpPrecision,
 RoundDownPrecision,
 FeePrecisionType,
 IsCriteriaApplied     
   )        
  SELECT         
 NewID(),  
 LongFeeRate,         
 ShortFeeRate,  
 LongCalculationBasis,  
 ShortCalculationBasis,  
 RoundOffPrecision,  
 MaxValue,  
 MinValue,  
 AUECID,  
 FeeTypeID,
 RoundUpPrecision,
 RoundDownPrecision,
 FeePrecisionType,
 IsCriteriaApplied        
  FROM    
 #TEMPOtherFeeRules TOFR 
  Where /*TOFR.AUECID NOT IN (Select AUECID from T_OtherFeeRules) 
	AND*/ TOFR.FeeTypeID Not IN (Select FeeTypeID from T_OtherFeeRules WHERE TOFR.AUECID = AUECID)
     
--   OPENXML(@handle, '//OtherFeeRule', 2)           
--  WITH         
--   (LongRate float, ShortRate float, LongCalculationBasis Integer, ShortCalculationBasis Integer, RoundOffPrecision smallint, MaxValue float, MinValue float, AUECID Integer, OtherFeeType Integer, RoundUpPrecision smallint, RoundDownPrecision smallint, FeePrecisionType smallint)  XmlItem        
-- Where AUECID IN (Select AUECID from T_OtherFeeRules) 
--	AND XmlItem.OtherFeeType Not IN (Select FeeTypeID from T_OtherFeeRules)         
        

--This code inserts fee Criteria data.  
INSERT INTO T_OtherFeesCriteria 
(            
LongValueGreaterThan,      
LongValueLessThanOrEqualTo,  
LongFeeRate, 
LongCalculationBasis,
OtherFeeRuleId_FK,
ShortValueGreaterThan,
ShortValueLessThanOrEqualTo,
ShortFeeRate,
ShortCalculationBasis				 
)
select 
ISNULL(OFC.LongValueGreaterThan,0) as LongValueGreaterThan,      
ISNULL(OFC.LongValueLessThanOrEqualTo,0) as LongValueLessThanOrEqualTo,  
ISNULL(OFC.LongFeeRate,0) as LongFeeRate, 
ISNULL(OFC.LongCalculationBasis,0) as LongCalculationBasis,
OFR.OtherFeeRuleId,
ISNULL(OFC.ShortValueGreaterThan,0) as ShortValueGreaterThan,      
ISNULL(OFC.ShortValueLessThanOrEqualTo,0) as ShortValueLessThanOrEqualTo,  
ISNULL(OFC.ShortFeeRate,0) as ShortFeeRate, 
ISNULL(OFC.ShortCalculationBasis,0) as ShortCalculationBasis
From #TEMPOtherFeeCriteria OFC INNER JOIN (select * from T_OtherFeeRules WHERE IsCriteriaApplied=1) OFR
ON (OFC.LongAUECID=OFR.AUECID AND OFC.LongFeeTypeID=OFR.FeeTypeID) OR (OFC.ShortAUECID=OFR.AUECID AND OFC.ShortFeeTypeID=OFR.FeeTypeID)

--select * from T_OtherFeesCriteria
DROP Table #TEMPLongOtherFeeCriteria
DROP Table #TEMPShortOtherFeeCriteria
DROP Table #TEMPOtherFeeCriteria   
EXEC sp_xml_removedocument @handle        
        
COMMIT TRANSACTION TRAN1        
        
END TRY        
BEGIN CATCH         
 SET @ErrorMessage = ERROR_MESSAGE();        
 SET @ErrorNumber = Error_number();         
 ROLLBACK TRANSACTION TRAN1        
         
END CATCH;    
 --Insert Data        
--  Insert Into T_AUECWeeklyHolidays(WeeklyHolidayID, AUECID)        
--  Values(@weeklyHolidayID, @auecID)        

