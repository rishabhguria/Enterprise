


CREATE proc [dbo].[P_BTSaveFilterDetails]
(
@filterTypeID int,
@filterID varchar(200),
@benchmarkID int,
@operatorID int,
@percentage float
)
as
insert into T_BTFilterDetails
values(@filterTypeID,@filterID,@benchmarkID,@operatorID,@percentage)




