/*
Name : <P_GetAUECMarketFees>
Created By : <Kanupriya>
Purpose : <to fetch the sec fees for an auecID.>
Dated : <11/03/2006>
*/
CREATE PROCEDURE dbo.P_GetAUECMarketFees
	
	(
	@auecID int 
	)
	
AS
	SELECT      ExchangeID,PurchaseSecFees, SaleSecFees, PurchaseStamp, SaleStamp, PurchaseLevy, SaleLevy
	FROM         T_AUEC
	WHERE     (AUECID = @auecID)
