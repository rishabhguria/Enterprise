CREATE procedure P_GetOrderSideForCache
(
@companyUserID int 
)
as
select distinct OS.CVAUECID,OS.SideID from T_CVAUECSide as OS
join V_GetAllCVAUEC on V_GetAllCVAUEC.CVAUECID=OS.CVAUECID
where V_GetAllCVAUEC.CompanyUserID=@companyUserID