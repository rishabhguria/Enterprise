-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Update the feeder funds of the company
-----------------------------------------------------------------
CREATE procedure [dbo].[P_UpdateFeederAccounts]
(@xmlDoc ntext)
as 
 
declare @handle int
exec sp_xml_preparedocument @handle output, @xmlDoc
update T_CompanyFeederFunds
SET T_CompanyFeederFunds.FeederFundName=ox.FeederFundName,
T_CompanyFeederFunds.FeederFundShortName=ox.FeederFundShortName,
T_CompanyFeederFunds.CompanyID=ox.CompanyID,
T_CompanyFeederFunds.Amount=ox.Amount, 
--T_CompanyFeederFunds.Currency=ox.Currency,
T_CompanyFeederFunds.RemainingAmount=ox.RemainingAmount
--SELECT FeederFundName, FeederFundShortName, CompanyID, Capital, Currency
from openxml(@handle, '/dsFeederFunds/dtFeederFunds',2)
with 
(FeederFundID int,
FeederFundName VARCHAR(100), 
FeederFundShortName VARCHAR(50), 
CompanyID INT , 
Amount DECIMAL(10,2), 
--Currency NVARCHAR(5),
RemainingAmount DECIMAL(10,2)) ox
where ox.FeederFundID=T_CompanyFeederFunds.FeederFundID
exec sp_xml_removedocument @handle 
