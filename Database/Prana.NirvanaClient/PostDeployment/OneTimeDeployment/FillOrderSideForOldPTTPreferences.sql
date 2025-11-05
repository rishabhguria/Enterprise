/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*
	ScriptType: General
	Description: Update order side in PTTDetails for old PTT Preferences
	Created By: Shubham Awasthi
	Dated: 22 MAY 2017
*/

--------------------------------------------------------------------------------------
*/
   IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_Sub') AND EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_Fills') 
   AND EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_PTTDetails')
	BEGIN
	IF (select count(1) FROM T_PTTDetails where OrderSideID is Null) <> 0
	BEGIN
	UPDATE T_PTTDetails
    Set OrderSideID=subFillsJoin.SideID From
    (SELECT SideID,OriginalAllocationPreferenceID from T_Sub inner join 
	T_Fills on T_Sub.ClOrderID=T_Fills.ClOrderID) AS subFillsJoin inner join 
	T_PTTDetails on subFillsJoin.OriginalAllocationPreferenceID=T_PTTDetails.PTTId
	END
	END
	
