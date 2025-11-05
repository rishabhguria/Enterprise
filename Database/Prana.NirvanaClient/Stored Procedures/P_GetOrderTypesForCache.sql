create procedure P_GetOrderTypesForCache
(
@companyUserID int 
)
as
select distinct OT.CVAUECID,OT.OrderTypesID from T_CVAUECOrderTypes as OT
join V_GetAllCVAUEC on V_GetAllCVAUEC.CVAUECID=OT.CVAUECID
where V_GetAllCVAUEC.CompanyUserID=@companyUserID