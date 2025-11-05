IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_Sub' AND COLUMN_NAME='IsHidden')
BEGIN
	declare @ParentClOrderid Table (ParentClOrderid varchar(max))
		
	insert into @ParentClOrderid (ParentClOrderid)
	Select ParentClOrderID from T_Sub where IsHidden = 1 and StagedOrderID = ''

	Update T_Sub
	set IsHidden = 1
	where 
	ParentClOrderID in (Select ParentClOrderID from @ParentClOrderid)  -- For replace orders
	or StagedOrderID in (Select ParentClOrderID from @ParentClOrderid) -- for child orders

	DELETE FROM @ParentClOrderid

	insert into @ParentClOrderid (ParentClOrderid)
	Select ParentClOrderID from T_Sub where IsHidden = 1 and StagedOrderID <> ''

	Update T_Sub
	set IsHidden = 1
	where 
	ParentClOrderID in (Select ParentClOrderID from @ParentClOrderid) -- For replace orders
	or StagedOrderID in (Select ParentClOrderID from @ParentClOrderid) -- for grand child orders
END