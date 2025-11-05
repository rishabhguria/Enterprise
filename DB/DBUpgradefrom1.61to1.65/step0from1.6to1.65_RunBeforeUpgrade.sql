select * into T_Group_Old from T_Group
Exec SP_Rename 'T_Group.AUECLocalDate', 'AllocationDate','COLUMN'
Exec SP_Rename 'T_Group.CreationDate', 'AUECLocalDate','COLUMN'
drop table T_Group_Old
