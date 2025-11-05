
-----------------------------------------------------------------
--Created BY: OM shiv
--Date: 20-May-14
--Purpose: Get the list of secondary pricing sources for the company
-----------------------------------------------------------------
Create procedure [dbo].[P_GetAllSecondaryPricingSources]
as 
select SourceID,Description from T_PricingSecondarySource

