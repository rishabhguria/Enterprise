/********************************************************************          
created by : omshiv
desc : get todays positions with master fund target ratio
Date: 16 jan 2014

Usage:          
 Exec GetTodayPositionsWithMFRatio '1/15/2014 12:00:00 AM'          
************************************************************************/          
CREATE Procedure [dbo].[GetTodayPositionsWithMFRatio]          
(          
  @date datetime          
)           
As        
      
        
--Declare @date datetime            
--            
--Set @date=GetDate()           
--            
Create Table #OpenTaxlots                                                          
 (                   
    Symbol varchar(200),                                                          
    Quantity decimal,            
    FundID Int    
 )   


--select * INTO #TEMP FROM T_ThirdPartyPermittedFunds


Create Table #OpenTaxlotsWithPB                                                          
 (                   
    Symbol varchar(200),                                                          
    Quantity decimal,            
    FundID Int ,
    ThirdPartyIDs Int ,  
 )             
            
 Insert Into #OpenTaxlots            
 Select             
 Symbol,            
 SUM(PT.TaxlotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)),            
 FundID             
 From PM_Taxlots PT 
           
 Where  taxlot_PK in                                                                       
 (                                                  
   Select Max(Taxlot_PK) from PM_Taxlots             
   Where Datediff(d, PM_Taxlots.AUECModifiedDate,@Date) >= 0                      
   Group By TaxlotID            
 )                                                                      
 and TaxLotOpenQty<>0 Group By Symbol,FundID

--SELECT * FROM #OpenTaxlots

INSERT into #OpenTaxlotsWithPB 
SELECT 
Symbol,
Quantity,
FundID,
TP.ThirdPartyID
from T_ThirdPartyPermittedFunds TP
INNER JOIN #OpenTaxlots OT ON TP.CopanyFundID = OT.FundID

select TP.ThirdPartyID,TP.CopanyFundID,P.ThirdPartyName into #TEMP from T_ThirdPartyPermittedFunds TP
inner join T_CompanyThirdParty CTP ON CTP.CompanyThirdPartyID = TP.ThirdPartyID
inner JOIN T_ThirdParty P on CTP.ThirdPartyID = P.ThirdPartyID
--
--SELECT * FROM T_CompanyThirdParty
--SELECT * FROM T_ThirdParty

select 
*
INTO #finalTable
FROM #TEMP t 
INNER JOIN #OpenTaxlotsWithPB o ON o.ThirdPartyIDs = t.ThirdPartyID

INNER Join T_CompanyMasterFundSubAccountAssociation CMF On t.copanyfundid=CMF.CompanyFundID
left Join T_AllocationMasterfundRatio MFR On MFR.MasterFundID=CMF.CompanyMasterFundID

--Select * from #TEMP

SELECT DISTINCT
FT.Symbol as COL1,
CF.FundName as COL2,
isnull(OT.Quantity,0) as COL3,
FT.TargetRatioPct as COL4,
FT.ThirdPartyName as COL5

from #finalTable FT
left JOIN #OpenTaxlots OT on OT.FundID = FT.copanyFundID AND OT.Symbol = FT.Symbol
INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = FT.copanyFundID
where FT.TargetRatioPct <>0 AND	OT.Quantity <>0
Order by COL1,COL2  
        
Drop Table #OpenTaxlots,#finalTable , #TEMP,#OpenTaxlotsWithPB  
  
  

