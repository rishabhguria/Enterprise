CREATE procedure [dbo].[P_SaveTradTicketRiskValidationSett]  
  
(  
@ISRISKCHECKED varchar(6),  
@ISVALIDATESYMBOLCHECKED varchar(6),  
@RISKVALUE float,  
@CompanyUserID int,  
@LIMITPRICECHECKED varchar(6),
@SetExecutedQtytoZero varchar(6) 
)  
as  
if((select count(*) from T_TTRiskNValidationSetting where CompanyUserID = @CompanyUserID)=0)  
insert into   
 T_TTRiskNValidationSetting  
 (  
 IsRiskChecked,  
 IsValidateSymbolChecked,  
 RiskValue,   
 CompanyUserID,  
 LimitPriceChecked,
SetExecutedQtytoZero  
 )   
   
values  
(  
@ISRISKCHECKED,  
@ISVALIDATESYMBOLCHECKED,  
@RISKVALUE,  
@CompanyUserID,  
@LIMITPRICECHECKED,
@SetExecutedQtytoZero  
)  
else  
update T_TTRiskNValidationSetting  
set   
 IsRiskChecked=@ISRISKCHECKED,  
 IsValidateSymbolChecked=@ISVALIDATESYMBOLCHECKED,  
 RiskValue=@RISKVALUE,  
 LimitPriceChecked = @LIMITPRICECHECKED ,
SetExecutedQtytoZero =  @SetExecutedQtytoZero
 where CompanyUserID = @CompanyUserID  

