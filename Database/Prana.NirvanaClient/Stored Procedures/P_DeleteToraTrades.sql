

/* =============================================          
 Author:  Sumit kakra          
 Create date: 24 Dec 2007          
 Description: Deletes Order from T_Fill, T_Sub, T_Order
 Parameter: 
	OrderIDs: comma separated list of orders which needs to be deleted
 Usage:  
 Exec P_DeleteToraTrades OrderIds
-- =============================================          
*/  
CREATE PROCEDURE [dbo].[P_DeleteToraTrades] (
		@OrderIds varchar(8000)
	)
AS
Begin
    
	Delete from T_Fills where  OrderID in (Select CAST(ITEMS As Varchar(500)) from Split(@OrderIDs,','))
	Delete from T_Sub where  OrderID in (Select CAST(ITEMS As Varchar(500)) from Split(@OrderIDs,','))
	Select Count(*) from T_Order where ParentClOrderID Not In (Select ParentClOrderID from T_Sub)
	Delete from T_Order where ParentClOrderID Not In (Select ParentClOrderID from T_Sub)
End
