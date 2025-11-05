                                                            
CREATE Procedure [dbo].[PMDeleteFundClosingForTaxlots]                                                             
(                                                                  
 @TaxLotClosingIDString  VARCHAR(MAX),      
@skipPM_TaxlotsUpdate bit       
                                                   
)                                                                      
As                                                                    
                                      
BEGIN                                      
BEGIN TRAN TRAN1                                                                                                                                                                                     
                                                                                                                                                                    
BEGIN TRY                           
                     
Select * into #Temp_TaxlotClosingIDs  from dbo.split(@TaxLotClosingIDString,',')                
                          
create table #PM_Taxlots                
(                
 Taxlot_pk bigint,                
 TaxlotClosingID_fk uniqueidentifier,                
 TaxlotID varchar(MAX),           
 GroupID varchar(100),          
 Symbol Varchar(50),          
 FundId int,               
 AuecModifiedDate DateTime              
)           
           
--insert taxlot details for which ClosingTaxlotId is passed to SP               
Insert into #PM_Taxlots                 
(                
 Taxlot_pk,                
 TaxlotClosingID_fk,                
 TaxlotID,          
 GroupID,          
 Symbol,          
 FundId,          
 AuecModifiedDate                
)                
SELECT                 
Taxlot_pk,                
TaxlotClosingID_fk,                
TaxlotID,          
GroupID,          
 Symbol,          
 FundId,          
AuecModifiedDate                
FROM PM_Taxlots PM             
inner join #Temp_TaxlotClosingIDs Temp           
on Temp.Items = PM.TaxlotClosingId_fk                   
            
--select and insert symbol and fund with max AuecModifiedDate          
Select Symbol,FundId,max(AuecModifiedDate) as AuecModifiedDate           
into           
#Temp_TaxlotWithFundSymbolAuecDate           
from #PM_Taxlots          
group by fundid,symbol            
           
--Select and insert all the taxlots corresponding to symbol and fund having greater AuecModifiedDate          
Insert into #PM_Taxlots                 
(                
 Taxlot_pk,                
 TaxlotClosingID_fk,                
 TaxlotID,          
 GroupID,      
 Symbol,          
 FundId,          
 AuecModifiedDate                
)             
SELECT                 
Taxlot_pk,                
TaxlotClosingID_fk,                
TaxlotID,          
GroupID,       
PM.Symbol,          
PM.FundId,          
PM.AuecModifiedDate                
FROM PM_Taxlots PM             
inner join #Temp_TaxlotWithFundSymbolAuecDate FSA           
on FSA.Symbol = PM.Symbol          
and FSA.FundId = PM.FundId          
where datediff(d,FSA.AuecModifiedDate,PM.AuecModifiedDate) > 0           
and TaxLotClosingId_FK is not null          
             
          
Create table #PM_TaxlotClosing                
(                    
PositionalTaxlotID varchar(MAX),              
ClosingTaxlotID varchar(MAX),              
TaxlotClosingID uniqueidentifier,                
Closingmode int,              
ClosedQty float                
)            
--insert taxlotclosingid with same symbol and fundid which have closing in future date.            
Insert into #PM_TaxlotClosing                
(                
PositionalTaxlotID,              
ClosingTaxlotID,              
TaxlotClosingID,                
Closingmode,              
ClosedQty                
)          
Select           
positionalTaxlotID,       
ClosingTaxlotID,          
TaxlotClosingID,          
Closingmode,          
ClosedQty          
from PM_TaxLotClosing PTC          
inner join #PM_Taxlots PM          
on PTC.TaxlotClosingID = PM.TaxlotClosingID_FK          
          
           
create table #Temp_GroupIDs                    
(           
 GroupID varchar(50)                     
)                       
                
create table #Temp_PositionalTaxlotIDs                
(                
  TaxlotID varchar(MAX),                
  ClosedQty float,                
  Taxlot_pk bigint                   
)                
                
Insert into #Temp_PositionalTaxlotIDs                
(                
TaxlotID,                
ClosedQty,                
Taxlot_pk                
)                
                
Select TaxlotID,                 
SUM(ClosedQty),                 
MAX(Taxlot_pk)                
FROM #PM_Taxlots PM             
inner join #PM_TaxlotClosing TEMP           
on PM.TaxlotClosingID_fk = TEMP.TaxlotClosingID           
and PM.TaxlotID = TEMP.PositionalTaxlotID                
GROUP BY PM.TaxlotID                
                
                
CREATE TABLE #Temp_ClosingTaxlotIDs                
(                
  TaxlotID varchar(MAX),                
  ClosedQty float,                
  Taxlot_pk bigint                   
)                
                
                
INSERT INTO #Temp_ClosingTaxlotIDs                
(                
TaxlotID,                
ClosedQty,                
Taxlot_pk                
)                
                
SELECT TaxlotID,                 
SUM(ClosedQty),                 
MAX(Taxlot_pk)                
FROM #PM_Taxlots PM             
inner join #PM_TaxlotClosing PMC           
on PM.TaxlotClosingID_fk = PMC.TaxlotClosingID           
and PM.TaxlotID = PMC.ClosingTaxlotID                
GROUP BY PM.TaxlotID              
          
--select * from #Temp_TaxlotWithFundSymbolAuecDate          
--select * from #PM_Taxlots          
--select * from #PM_TaxlotClosing           
--select * from #Temp_ClosingTaxlotIDs          
--select * from #Temp_PositionalTaxlotIDs          
          
               
DELETE PMC FROM PM_TaxLotClosing PMC  inner join  #PM_TaxlotClosing on  PMC.TaxlotClosingID =   #PM_TaxlotClosing.TaxlotClosingID                          
                                      
DELETE PM FROM PM_TaxLots PM  inner join  #PM_TaxlotClosing on  PM.TaxlotClosingID_fk =   #PM_TaxlotClosing.TaxlotClosingID                           
         
      
          
      
--Mukul-- The below update statements are only solving partial purpose as these are specifically meant to update any taxlots which are closed in future dates. This is done if user unwinds data       
IF(@skipPM_TaxlotsUpdate <>1)      
      
BEGIN       
--       
UPDATE PM_taxlots                                  
                                   
set                           
 OpenTotalCommissionandFees =   (CASE TaxlotOpenQty WHEN 0 THEN 0 ELSE OpenTotalCommissionandFees*((TaxlotOpenQty+TEMP.ClosedQty)/  TaxlotOpenQty )  END )                 
, TaxlotOpenQty = TaxlotOpenQty+TEMP.ClosedQty                                          
                
FROM PM_Taxlots PM                
inner join #Temp_PositionalTaxlotIDs TEMP ON TEMP.TaxlotID = PM.TaxlotID                 
WHERE   PM.Taxlot_pk > TEMP.Taxlot_pk                  
              
UPDATE PM_taxlots                                           
SET                                  
 OpenTotalCommissionandFees =   (CASE TaxlotOpenQty WHEN 0 THEN 0 ELSE   OpenTotalCommissionandFees*((TaxlotOpenQty+TMP.ClosedQty)/TaxlotOpenQty )  END  )                                       
, TaxlotOpenQty = TaxlotOpenQty+TMP.ClosedQty                     
                                 
FROM PM_Taxlots PM                
inner join #Temp_ClosingTaxlotIDs TMP on TMP.TaxlotID = TMP.TaxlotID                 
inner join #PM_TaxlotClosing PMC on PMC.taxlotClosingID = PM.TaxlotclosingID_fk                
WHERE   PM.Taxlot_pk > TMP.Taxlot_pk   and PMC.Closingmode =0           
      
END           
                
      
if EXISTS(select TaxlotClosingID from #PM_TaxlotClosing PMC where (PMC.Closingmode <>0 and PMC.Closingmode<>7 AND PMC.Closingmode <> 10) )      
BEGIN      
      
---Mukul// fetching all the Exercised underlying ..Need a better way as right now the only way we can       
--identify this is thorugh T_group where TaxlotclosingID for a particular Exercise/Assignment is set in Pm_TaxlotclosingID_fk field (in T_group) with underlying generated..      
      
INSERT INTO #Temp_GroupIDs (GroupID)                    
SELECT distinct (GroupID)                     
FROM T_group  inner join #PM_TaxlotClosing PMC on T_group.TaxlotClosingID_fk = PMC.taxlotClosingID                
WHERE PMC.Closingmode <>0 and PMC.Closingmode<>7 AND PMC.Closingmode <> 10               
              
      
--Mukul// fetching all the expired transactions..This can be handled using transaction type (Expiry)..      
Union              
   
Select distinct(#PM_Taxlots.GroupID) from #PM_Taxlots inner join #PM_TaxlotClosing PMC on PMC.ClosingTaxlotID  = #PM_Taxlots.TaxlotID          
WHERE PMC.Closingmode <>0 and PMC.Closingmode<>7   AND PMC.Closingmode <> 10        
      
END           
                       
--Added by Sandeep              
Update T_PBwiseTaxlotState                                                          
Set TaxlotState = 2                                               
where TaxlotID in  (Select ClosingTaxlotID FROM  #PM_TaxlotClosing)                                                      
and TaxlotState=1                
              
                                       
SELECT * FROM  #Temp_GroupIDs            
SELECT TaxlotClosingID FROM #PM_TaxlotClosing             
SELECT TaxlotID FROM #Temp_PositionalTaxlotIDs          
UNION ALL          
SELECT TaxlotID FROM #Temp_ClosingTaxlotIDs          
           
DROP TABLE #Temp_GroupIDs,#PM_Taxlots,#PM_TaxlotClosing,#Temp_ClosingTaxlotIDs,#Temp_PositionalTaxlotIDs,#Temp_TaxlotClosingIDs,#Temp_TaxlotWithFundSymbolAuecDate                
                                         
                                      
COMMIT TRANSACTION TRAN1                                         
                                                                         
 END TRY                                                                 
 BEGIN CATCH                                          
                                                                           
 ROLLBACK TRANSACTION TRAN1                                                                                                                                             
                                                                         
 END CATCH;                                                                                                                  
                                                                                                                                                  
END   

