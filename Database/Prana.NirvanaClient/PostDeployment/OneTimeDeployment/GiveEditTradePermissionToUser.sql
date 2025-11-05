/*
Purpose:- To grant the Edit Trades permission for Company/User first time if he/she has the permission of allocation already.
Created By:- Kuldeep Agrawal.
Creation Date: - April 12, 2016.

Test Cases:-
1. Company do not have permission for both allocation and Edit trades while some of its users have permission for None/Allocation/Edit Trades/both (atleast 1 user with each).
2. Company has permission for allocation but not Edit trades while some of its users have permission for None/Allocation/Edit Trades/both (atleast 1 user with each).
3. Company has permission for Edit Trades but not Allocation while some of its users have permission for None/Allocation/Edit Trades/both (atleast 1 user with each).
4. Company has permission for both Edit Trades and Allocation while some of its users have permission for None/Allocation/Edit Trades/both (atleast 1 user with each).
*/

SELECT ModuleID,ModuleName into #Modules from T_Module where ModuleName in ('Allocation','EditTrade')

DECLARE @CompanyID int
DECLARE @AllocationModuleID int
DECLARE @EditTradeModuleID int
DECLARE @AllocationCompanyModuleID int
DECLARE @EditTradeCompanyModuleID int

SELECT top 1 @CompanyID =  CompanyId from T_Company where CompanyID>0
SELECT @AllocationModuleID = moduleID from #Modules where modulename ='Allocation'
SELECT @EditTradeModuleID = moduleID from #Modules where modulename ='EditTrade'
set @AllocationCompanyModuleID = 0
Set @EditTradeCompanyModuleID = 0

SELECT @AllocationCompanyModuleID = companyModuleID from T_CompanyModule where ModuleID = @AllocationModuleID and CompanyID = @CompanyID
SELECT @EditTradeCompanyModuleID = companyModuleID from T_CompanyModule where ModuleID = @EditTradeModuleID and CompanyID = @CompanyID

IF ((Select @AllocationCompanyModuleID) > 0 and (Select @EditTradeCompanyModuleID) = 0)
BEGIN
INSERT INTO T_CompanyModule
VALUES(@CompanyID,@EditTradeModuleID,1)
END

SELECT @AllocationCompanyModuleID = companyModuleID from T_CompanyModule where ModuleID = @AllocationModuleID and CompanyID = @CompanyID
SELECT @EditTradeCompanyModuleID = companyModuleID from T_CompanyModule where ModuleID = @EditTradeModuleID and CompanyID = @CompanyID


SELECT CompanyUserID into #Users from T_CompanyUserModule inner join 
T_CompanyModule on T_CompanyUserModule.CompanyModuleID = T_CompanyModule.CompanyModuleID
WHERE T_CompanyModule.CompanyModuleID in(@AllocationCompanyModuleID,@EditTradeCompanyModuleID)
group by CompanyUserID
having max(T_CompanyUserModule.CompanyModuleID) = @AllocationCompanyModuleID


IF((SELECT @AllocationCompanyModuleID) > 0 and (SELECT @EditTradeCompanyModuleID) > 0)
BEGIN
Insert into T_CompanyUserModule
Select @EditTradeCompanyModuleID,CompanyUserID,1
from #Users
END

DROP TABLE #Modules
DROP TABLE #Users
