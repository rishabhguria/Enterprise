
Create PROCEDURE [dbo].[P_CA_SaveOverridePreCheckAndPowerUser]    
 @companyId int,   
 @userId int,    
 @powerUser bit,
 @applyToManual bit ,    
 @overridePermission bit,
 @preTradeCheck bit,
 @trading bit,
 @staging bit,
 @defaultPrePopup bit,   
 @defaultPostPopup bit,
 @defaultRuleOverrideType int,
 @enableBasketComplianceCheck bit
 
 as   
 delete from T_CA_OtherCompliancePermission where userId=@userId  and companyId=@companyId  
 insert into T_CA_OtherCompliancePermission 
 (
 CompanyId,
 UserId,
 IsPreTradeEnabled,
 IsOverridePermission,
 PowerUser,
 IsApplyToManual,
 Trading,
 Staging,
 DefaultPrePopUp,
 DefaultPostPopUp,
 DefaultRuleOverrideType,
 EnableBasketComplianceCheck) 
 values
 (
 @companyId,
  @userId,
  @preTradeCheck,
  @overridePermission,
  @powerUser,
  @applyToManual,
  @trading, 
  @staging,
  @defaultPrePopup,
  @defaultPostPopup,
  @defaultRuleOverrideType,
  @enableBasketComplianceCheck
  )    


