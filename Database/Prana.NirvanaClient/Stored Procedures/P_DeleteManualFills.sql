  
 CREATE       PROCEDURE [dbo].[P_DeleteManualFills]  
(  
 @clOrderID varchar(50)  
)  
AS  
--Declare @result int  
Delete From T_Fills  
where T_Fills.ClOrderID in   
(  
--Select S.ClOrderID   
--From T_Order as O join T_Sub as S on O.ParentClOrderID = S.ParentClOrderID  
select S.ClOrderID from T_Sub as S where  
S.ParentClOrderID = @clOrderID  
)  
  
/*  
Declare @total int   
Select @total = Count(*)  
From T_Order as O join T_Sub as S on O.ParentClOrderID = S.ParentClOrderID  
join T_Fills as F on S.ClOrderID = F.ClOrderID  
 Where O.ParentClOrderID = @ParentClOrderID  
if (@total > 0)  
begin  
Delete  from T_Fills  
where ClorderID = @clOrderID  
end  
*/  
--SET @result = scope_identity()  
--Select @result  
  
  