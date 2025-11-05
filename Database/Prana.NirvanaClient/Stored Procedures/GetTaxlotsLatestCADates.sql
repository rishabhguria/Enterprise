/*
Author  : Rajat Tandon       
Date   : 02 Sept 2008      
Description : Returns latest corporate action dates for the taxlots  
Modified By :Ishant Kathuria
Date:20th dec 2011

Modified By: Sandeep Singh
Modified Date: 29th Jan, 2020
Desc: Closing Status has beed added to update it on Allocation UI
*/
CREATE Procedure [dbo].[GetTaxlotsLatestCADates]      
As      

Select TaxlotID, EffectiveDate as AUECLocalDate,ClosingTaxlotId,ClosingID,
Case
	When ClosingId Is Not Null
	Then 1
	Else 0
End As ClosingStatus
Into #Temp_CorpAction
from V_corpActionData VcaData 
inner join  PM_CorpActionTaxlots CATaxlots on VcaData.corpactionId = CATaxlots.corpactionId
where (CATaxlots.corpactionId is not null And VcaData.IsApplied = 'True')
    

Update T1
Set ClosingStatus = 1
From #Temp_CorpAction T1
Inner Join #Temp_CorpAction T2 On T1.TaxlotId = T2.ClosingTaxlotId And T2.ClosingTaxlotId Is Not Null

Select 
TaxlotId, 
AUECLocalDate,
ClosingStatus  
From #Temp_CorpAction T1

Drop Table #Temp_CorpAction  