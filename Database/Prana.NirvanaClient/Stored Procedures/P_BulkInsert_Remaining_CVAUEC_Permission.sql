Create Procedure P_BulkInsert_Remaining_CVAUEC_Permission 
(@CPVID varchar(200)=NULL)
As
Insert into T_CVAUEC (CounterPartyVenueID,AUECID)
Select counterpartyvenueid,auecid from T_CounterPartyVenue, T_AUEC where CAST(counterpartyvenueid AS VARCHAR(10)) + '_' + CAST(auecid AS VARCHAR(10)) not in
(Select CAST(counterpartyvenueid AS VARCHAR(10)) + '_' + CAST(auecid AS VARCHAR(10)) from T_CVAUEC)
and CounterPartyVenueID in (Select * from dbo.Split(@CPVID,','))
