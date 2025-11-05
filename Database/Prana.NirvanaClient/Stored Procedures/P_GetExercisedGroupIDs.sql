Create Procedure [dbo].[P_GetExercisedGroupIDs]         
(      
  @FromAllAUECDatesString VARCHAR(MAX)             
)      
    
    
as     
    
Select PM_taxlotClosing.positionalTaxlotID,PM_taxlotClosing.ClosingTaxlotID,T_group.GroupID into #Temp_IDs from PM_taxlotClosing inner join T_group on PM_taxlotClosing.TaxlotClosingID = T_group.TaxlotClosingID_fk        
    
where Datediff(d,T_group.ProcessDate,CONVERT(datetime, @FromAllAUECDatesString)) <= 0    
      
select #Temp_IDs.positionalTaxlotID,#Temp_IDs.GroupID,T_level2Allocation.groupID from T_level2Allocation inner join #Temp_IDs on T_level2Allocation.taxlotID = #Temp_IDs.ClosingTaxlotID       
      
drop table #Temp_IDs      
      