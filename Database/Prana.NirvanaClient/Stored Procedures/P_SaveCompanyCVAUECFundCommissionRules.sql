
/****** Object:  Stored Procedure dbo.P_SaveCompanyCVAUECFundCommissionRules    Script Date: 10/16/2007 07:25:22 PM ******/        
CREATE PROCEDURE [dbo].[P_SaveCompanyCVAUECFundCommissionRules] (        
  @Xml nText,     
  @companyID int,       
--  @CompanyFundID int,        
--  @CompanyCounterPartyCVID int,        
--  @CVAUECID int,        
--  @SingleRuleID int,        
--  @BasketRuleID int,        
--  @result int       
   @ErrorMessage varchar(500) output        
 , @ErrorNumber int output                  
)        
AS         
Declare @total int         
Set @total = 0        
      
BEGIN TRY        
      
 DECLARE @handle int           
exec sp_xml_preparedocument @handle OUTPUT,@Xml           
    
DELETE FROM T_CommissionRulesForCVAUEC     
 WHERE CompanyID = @companyID    
    
        
--This code updates old data.        
INSERT INTO T_CommissionRulesForCVAUEC        
(      
 CVId_FK,      
 AUECId_FK,      
 FundId_FK,      
 SingleRuleId_FK,      
 BasketRuleId_FK,      
 CompanyID      
 )      
      
SELECT         
CVID--CVID,        
,AUECID--AUECID,        
,FundID--FundID,        
,SingleRule --'0697e656-6edd-4a7f-bcb4-91101c62f369'--SingleRule,      
,BasketRule --'0697e656-6edd-4a7f-bcb4-91101c62f369'--BasketRule,        
,CompanyID        
         
FROM         
 OPENXML(@handle, '//CVAUECFundCommissionRule', 2)           
 WITH         
  (CVID Integer, AUECID Integer, FundID Integer, SingleRule UniqueIdentifier 'SingleRule/RuleID', BasketRule UniqueIdentifier 'BasketRule/RuleID', CompanyID Integer)  XmlItem        
       
      
      
EXEC sp_xml_removedocument @handle        
        
--COMMIT TRANSACTION TRAN1        
        
END TRY        
BEGIN CATCH         
 SET @ErrorMessage = 0 --ERROR_MESSAGE();        
 SET @ErrorNumber = Error_number();         
-- ROLLBACK TRANSACTION TRAN1        
         
END CATCH;
