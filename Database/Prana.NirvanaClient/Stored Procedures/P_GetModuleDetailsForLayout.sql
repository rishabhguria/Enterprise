CREATE PROC P_GetModuleDetailsForLayout  
(  
@layoutID int,  
@modouleName varchar(50)  
)  
AS  
  
SELECT  
Height,  
Width,
WindowState,
LeftX,
RightY
FROM  
T_LayoutComponentDetails WITH (NOLOCK)   
WHERE LayoutID = @layoutID   
  AND ModuleID = (Select ComponentID from T_LayoutComponents where ComponentName = @modouleName)