-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 10-mar-2014
--Purpose: Get the list of pricing types for the pricing rule
-----------------------------------------------------------------
create procedure [dbo].[P_GetAllPricingTypes]
as
select DataTypeID, DataTypeName from T_PricingDataType;
