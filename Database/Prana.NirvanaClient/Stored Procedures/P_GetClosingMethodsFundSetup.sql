-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 25/03/14
--Purpose: Get the closing methods for the Fund Setup
-----------------------------------------------------------------
create procedure [dbo].[P_GetClosingMethodsFundSetup]
as
select AlgorithmId, AlgorithmAcronym
from T_ClosingAlgos
