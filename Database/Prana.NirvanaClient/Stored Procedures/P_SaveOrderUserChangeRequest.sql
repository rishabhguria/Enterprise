  
-- =============================================  
-- Author:  <Ashsih>  
-- Create date: <14th Sept, 2006>  
-- Description: <To Save the data corresponding to change in UserID of a particular trade>  
-- =============================================  
CREATE PROCEDURE [dbo].[P_SaveOrderUserChangeRequest]  
 -- Add the parameters for the stored procedure here  
( @ClOrderID varchar(50),   
 @TransactionTime varchar(50),  
 @UserID int            
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
declare  @oldUserID int  
Select @oldUserID =  UserID from T_Sub where ClOrderID = @ClOrderID   
Insert into T_OrderCustomRequestDetails   
(  
 ClOrderID,  
 TransactionTime,  
 CompanyUserID  
)  
values  
(  
 @ClOrderID ,  
 @TransactionTime,   
 @oldUserID  
)   
   
Update  T_Sub  
Set UserID =  @UserID  
where ClOrderID = @ClOrderID  
  
  
END  