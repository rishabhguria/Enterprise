             
/* =============================================                              
 Author:  Sandeep Singh                 
 Create date: May 12,2010                            
 Description: Get Split Factor for a Taxlot between 2 date ranges                      
 Usage                
 Select dbo.[GetSplitFactorForReports]('05-01-2010','05-20-2010','100524182312011820')                      
-- =============================================                              
*/                      
                      
CREATE function [dbo].[GetSplitFactorForReports]                 
(                
 @StartDate DateTime,            
 @EndDate DateTime,            
 @TaxlotID Varchar(100)               
)                
returns float as                      
                      
Begin               
 Declare @splitFactor Float                
            
 Declare @Temp Table               
 (            
  splitFactor Float            
 )            
            
 Insert into @Temp            
 Select SplitFactor from V_CorpActionData VCA             
 Inner join PM_CorpActionTaxlots PMT On PMT.CorpActionID=VCA.CorpActionID            
 Where DateDiff(d,@StartDate,VCA.EffectiveDate) >=0 And DateDiff(d,VCA.EffectiveDate,@EndDate) >=0             
 and PMT.TaxlotID=@TaxlotID            
            
 Set @splitFactor = (SELECT EXP(SUM(LOG(splitFactor))) AS AMultiply            
      FROM @Temp)            
            
If(@splitFactor is null or @splitFactor =0)            
Begin            
Set @splitFactor=1            
End            
             
Return  @splitFactor                  
End 

