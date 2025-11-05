





/****** Object:  Stored Procedure dbo.P_AddRowCurrency    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllFlagsByAUECID]
	
AS 
Select  T_AUEC.AUECID, T_CountryFlag.CountryFlagImage

from 

T_AUEC, T_CountryFlag Where T_AUEC.CountryFlagID = T_CountryFlag.CountryFlagID


