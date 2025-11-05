

CREATE Procedure [dbo].[P_GetBorrowersDetails]
as
Select distinct  BorrowerFirmID ,BorrowerShortName
From T_CompanyBorrower as CB join T_CompanyUser as CU on
CB.CompanyID=CU.CompanyID

--select * from T_CompanyBorrower