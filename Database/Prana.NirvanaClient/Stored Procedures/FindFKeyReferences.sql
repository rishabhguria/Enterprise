
-- Author : Rajat Tandon
-- Date : 09 Aug 08
-- Description : It returns all of the the foriegn key references for the supplied table name
Create Proc FindFKeyReferences
(
	@tableName varchar(max)
)
As

select object_name(fkeyid) referencing,object_name(rkeyid) referenced,* from sysreferences
where object_name(rkeyid) = @tableName