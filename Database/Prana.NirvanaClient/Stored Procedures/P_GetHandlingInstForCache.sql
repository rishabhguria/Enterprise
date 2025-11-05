create procedure P_GetHandlingInstForCache
(
@companyUserID int 
)
as
select distinct HI.CVAUECID,HI.HandlingInstructionsID from T_CVAUECHandlingInstructions as HI
join V_GetAllCVAUEC on V_GetAllCVAUEC.CVAUECID=HI.CVAUECID
where V_GetAllCVAUEC.CompanyUserID=@companyUserID