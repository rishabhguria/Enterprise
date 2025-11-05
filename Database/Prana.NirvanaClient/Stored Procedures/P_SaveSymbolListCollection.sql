

CREATE  procedure P_SaveSymbolListCollection
(
@symbolListID varchar(200),
@userID int,
@symbolListName varchar(50),
@symbolListCollection varchar(2000)

)

as

if((select count (*) from T_SymbolList where symbolListID=@symbolListID )=0)
insert into T_SymbolList (SymbolListID,symbolListName,symbolListCollection ,userID)
values(@symbolListID,@symbolListName,@symbolListCollection ,@userID)
else

update T_SymbolList 
set symbolListName=@symbolListName,
symbolListCollection =@symbolListCollection

where symbolListID=@symbolListID



