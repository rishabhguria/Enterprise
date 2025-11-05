  
  
 CREATE procedure [dbo].[P_SaveTradingTicketPrefrencesPopUps]  
  
(  
@ISNewOrder varchar(6),  
@ISCXL varchar(6),  
@ISCXLReplace varchar(6),  
@ISManualOrder varchar(6),
@CompanyUserID int  
)  
as  
if((select count(*) from T_ConfirmationPopUp where CompanyUserID = @CompanyUserID)=0)  
insert into   
 T_ConfirmationPopUp  
 (  
 ISNewOrder,  
 ISCXL,  
 ISCXLReplace,
 ISManualOrder, 
 CompanyUserID  
 )   
   
values  
(  
@ISNewOrder,  
@ISCXL,  
@ISCXLReplace,
@ISManualOrder,  
@CompanyUserID  
)  
else  
update T_ConfirmationPopUp  
set   
 ISNewOrder=@ISNewOrder,  
 ISCXL=@ISCXL,  
 ISCXLReplace=@ISCXLReplace,
 ISManualOrder = @ISManualOrder
where   
 CompanyUserID = @CompanyUserID  
  
  