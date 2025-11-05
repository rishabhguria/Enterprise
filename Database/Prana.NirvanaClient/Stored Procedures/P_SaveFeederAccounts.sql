-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 29/03/14
--Purpose: Save the feeder funds of the company
-----------------------------------------------------------------

Create procedure [dbo].[P_SaveFeederAccounts]
(@xmlDoc ntext)
as 
declare @handle int
exec sp_xml_preparedocument @handle output, @xmlDoc
insert INTO T_CompanyFeederFunds
(FeederFundName, FeederFundShortName, CompanyID, Amount, RemainingAmount, Currency)
SELECT FeederFundName, FeederFundShortName, CompanyID, Amount, Amount, Currency
from openxml(@handle, '/dsFeederFunds/dtFeederFunds',2)
with 
(FeederFundName VARCHAR(100), 
FeederFundShortName VARCHAR(50), 
CompanyID INT , 
Amount DECIMAL(10,2),
Currency int)
exec sp_xml_removedocument @handle 
