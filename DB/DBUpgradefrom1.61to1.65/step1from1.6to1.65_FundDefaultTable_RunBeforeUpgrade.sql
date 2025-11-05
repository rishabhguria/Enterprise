Alter table T_FundDefault
drop CONSTRAINT PK_T_FundDefault
Alter table T_FundDefault
drop column DefaultID
Alter table T_FundDefault
Add DefaultID int
CONSTRAINT PK_T_FundDefault PRIMARY KEY (DefaultID) identity(100,1)
Alter table T_FundDefault
drop CONSTRAINT PK_T_FundDefault
Alter table T_FundDefault
ADD CONSTRAINT PK_T_FundDefault PRIMARY KEY (DefaultID)