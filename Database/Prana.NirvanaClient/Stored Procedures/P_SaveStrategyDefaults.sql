


CREATE   procedure P_SaveStrategyDefaults
(
@DefaultID varchar(200),
@UserID int,
@defaultName varchar(20),
@fundIDs   varchar(200),
@fundPercentages  varchar(200)
)
as
if((select count(*)from T_StrategyDefault where DefaultID=@DefaultID)=0)
insert into T_StrategyDefault(DefaultID,CompanyUserID,DefaultName,strategies,StrategyValues) 
values(@DefaultID,@UserID,@defaultName,@fundIDs,@fundPercentages)
else
update  T_StrategyDefault
set 
DefaultName=@defaultName,
Strategies=@fundIDs,
StrategyValues=@fundPercentages
where DefaultID=@DefaultID




