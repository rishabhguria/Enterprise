/****** Object:  Stored Procedure dbo.P_SaveCVAUEC    Script Date: 12/22/2005 6:30:24 PM ******/  
CREATE PROCEDURE dbo.P_SaveCVAUEC (@counterPartyVenueID INT)  
AS  
DECLARE @total INT  
  
SET @total = 0  
  
SELECT @total = Count(*)  
FROM T_CVAUEC  
WHERE CounterPartyVenueID = @counterPartyVenueID  
  
IF (@total <= 0)  
BEGIN  
 INSERT INTO T_CVAUEC (  
  CounterPartyVenueID  
  ,AUECID  
  )  
 SELECT @counterPartyVenueID  
  ,AUECID  
 FROM T_AUEC  
END
ELSE
BEGIN
Update T_CVAUEC 
	Set CounterPartyVenueID = @counterPartyVenueID 	
	Where CounterPartyVenueID = @counterPartyVenueID
END