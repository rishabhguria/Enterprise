
/*
	Modified By: Ankit Gupta
	On: June 06, 2014
	To fetch ClosingDate for Unwinding for CA closed taxlots 
*/

CREATE Procedure [dbo].[P_GetClosingInfoForCA]      
(      
 @caIds varchar(max)      
)      
As      
      
Select CorpActionId, ClosingId, TaxlotId, caTaxlots.ClosingTaxlotId, Closing.AUECLocalDate from PM_Corpactiontaxlots caTaxlots    
Inner Join PM_TAxlotClosing Closing on Closing.TaxLotClosingID = caTaxlots.ClosingId  
inner join (Select Items from dbo.Split(@caIds,',')) as CaIds on caTaxlots.CorpActionId = CaIds.Items      
where ClosingId is not null    

