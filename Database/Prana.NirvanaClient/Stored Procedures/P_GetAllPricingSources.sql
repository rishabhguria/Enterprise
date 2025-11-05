-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 10-mar-14
--Purpose: Get the list of pricing sources for the company
-----------------------------------------------------------------
create procedure [dbo].[P_GetAllPricingSources]
as 
select sourceID, SourceName from T_PricingSource
