create procedure P_GetExecutionInstForCache
(
@companyUserID int 
)
as
select distinct EI.CVAUECID,EI.ExecutionInstructionsID from T_CVAUECExecutionInstructions as EI
join V_GetAllCVAUEC on V_GetAllCVAUEC.CVAUECID=EI.CVAUECID
where V_GetAllCVAUEC.CompanyUserID=@companyUserID