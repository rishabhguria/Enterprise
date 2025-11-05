                        
CREATE Procedure P_UpdateTaxlotsToIgnoreState                        
(          
@thirdPartyID int,                      
@taxlotxml varchar(max)                        
)                        
as                        
DECLARE @handle int                     
Declare @IsL1Data varchar(10)                       
exec sp_xml_preparedocument @handle OUTPUT,@taxlotxml                            
create table #Temp_TaxLOT                        
(            
TaxLotID varchar(50),            
TaxLotState int                 
)                        
                        
insert into #Temp_TaxLOT                        
                        
select                         
TaxLotID,            
TaxLotState                  
 From                       
 openxml(@handle,'/TaxLots/TaxLot',1)                        
 with                        
 (                        
 TaxLotID varchar(50) '@TaxLotID',             
 TaxLotState int '@TaxLotState'                  
 )                        
                  
select @IsL1Data=IsL1Data from openxml(@handle,'/TaxLots',2)                    
with                  
(                        
IsL1Data varchar(50) '@IsL1Data'                  
)                        
                  
If(@IsL1Data='True')                  
 Begin              
                 
	Update T_PBWiseTaxlotState 
	Set TaxlotState=#Temp_TaxLOT.TaxLotState            
	From T_PBWiseTaxlotState,#Temp_TaxLOT 
	Where T_PBWiseTaxlotState.TaxlotID=#Temp_TaxLOT.TaxlotID        
	 and T_PBWiseTaxlotState.PBID=@thirdPartyID                     
	                   
	Update T_DeletedTaxlots set TaxlotState=#Temp_TaxLOT.TaxLotState                       
	from T_DeletedTaxlots,#Temp_TaxLOT where T_DeletedTaxlots.TaxlotID=#Temp_TaxLOT.TaxlotID                 
                  
 End                  
                  
Else                  
 Begin                     
                
          
--update T_PBWiseTaxlotState set T_PBWiseTaxlotState.TaxlotState=#Temp_TaxLOT.TaxLotState              
--  from T_PBWiseTaxlotState,#Temp_TaxLOT where T_PBWiseTaxlotState.TaxlotID in     
-- (select TaxLotID from T_Level2Allocation where Level1AllocationID in (select TaxLotID from #Temp_TaxLOT))        
--  and T_PBWiseTaxlotState.PBID=@thirdPartyID       

update T_PBWiseTaxlotState 
set T_PBWiseTaxlotState.TaxlotState=#Temp_TaxLOT.TaxLotState  
From  #Temp_TaxLOT
Inner Join  T_Level2Allocation L2 on L2.Level1AllocationID=#Temp_TaxLOT.TaxLotID
Inner Join T_PBWiseTaxlotState On T_PBWiseTaxlotState.TaxlotID=L2.TaxLotID 
Where  T_PBWiseTaxlotState.PBID=@thirdPartyID          
                    
update T_DeletedTaxlots set TaxlotState=#Temp_TaxLOT.TaxLotState                         
from T_DeletedTaxlots,#Temp_TaxLOT where T_DeletedTaxlots.Level1AllocationID in (select TaxLotID from #Temp_TaxLOT)             
            
                 
               
 End                  
                  
--select * from #Temp_TaxLOT                
drop table #Temp_TaxLOT 
exec sp_xml_removedocument @handle