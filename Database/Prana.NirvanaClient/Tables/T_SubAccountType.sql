create table T_SubAccountType (
	SubAccountTypeId  INT			primary key identity(1,1),
	SubAccountType	  varchar(100)  unique														NOT NULL, 
);