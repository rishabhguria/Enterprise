EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_AL_AllocationBase

	INSERT INTO T_AL_AllocationBase(Id, AllocationBase, Description) VALUES(1,'CumQuantity','Allocation will be based on CumQuantity');
	INSERT INTO T_AL_AllocationBase(Id, AllocationBase, Description) VALUES(2,'Notional','Allocation will be based on Notional. Known as fair pricing');

EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";