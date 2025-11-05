---Created by : Bhupendra Singh Bora
---Date: 9 October 2015
---Description: Get AUECID and RoundLot value from T_AUEC

CREATE PROCEDURE [dbo].[P_GetKeyValuePairAUECRoundLots]        
AS        
BEGIN              
 SET NOCOUNT ON;        
               
 SELECT AUECID, RoundLot from T_AUEC        
 END
