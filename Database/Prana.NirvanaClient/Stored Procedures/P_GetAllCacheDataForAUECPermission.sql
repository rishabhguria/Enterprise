



CREATE procedure [dbo].[P_GetAllCacheDataForAUECPermission]
(
@companyUserID int
)
as

delete from T_TempCache
declare @CVAUECID varchar(5)
declare @CounterpartyID int
declare @venueID int
declare @AUECID int 
DECLARE @orderSide varchar(100)
DECLARE @orderType varchar(100)
DECLARE @handlingInst varchar(100)
DECLARE @executinInst varchar(100)
DECLARE @tif varchar(100)
declare @key varchar(25)

DECLARE crs_CVAUEC CURSOR FAST_FORWARD
FOR SELECT CVAUECID,CounterPartyID,VenueID,AUECID
FROM V_GetAllCVAUEC
WHERE CompanyUserID=@companyUserID

OPEN crs_CVAUEC
FETCH NEXT FROM crs_CVAUEC INTO @CVAUECID,@CounterpartyID,@venueID,@AUECID

WHILE @@FETCH_STATUS = 0
BEGIN

SELECT @orderSide= COALESCE(@orderSide +',','')+cast(S.SideTagValue as varchar(2)) from T_CVAUECSide as CVS
join T_Side as S on S.SideID=CVS.SideID 
where CVS.CVAUECID=@CVAUECID

SELECT @orderType= COALESCE(@orderType +',','')+cast(O.OrderTypeTagValue as varchar(2)) from T_CVAUECOrderTypes  as CVO
join T_OrderType as O on O.OrderTypesID=CVO.OrderTypesID
where CVO.CVAUECID=@CVAUECID


SELECT @handlingInst= COALESCE(@handlingInst +',','')+cast(H.HandlingInstructionsTagValue as varchar(2)) from T_CVAUECHandlingInstructions as CVH
join T_HandlingInstructions as H on H.HandlingInstructionsID=CVH.HandlingInstructionsID
where CVH.CVAUECID=@CVAUECID

SELECT @executinInst= COALESCE(@executinInst +',','')+cast(E.ExecutionInstructionsTagValue as varchar(2)) from T_CVAUECExecutionInstructions as CVE
join T_ExecutionInstructions as E on E.ExecutionInstructionsID=CVE.ExecutionInstructionsID
where CVE.CVAUECID=@CVAUECID

SELECT @tif= COALESCE(@tif +',','')+cast(TIF.TimeInForceTagValue as varchar(2)) from T_CVAUECTimeInForce as CVTIF
join T_TimeInForce as TIF on TIF.TimeInForceID=CVTIF.TimeInForceID
where CVTIF.CVAUECID=@CVAUECID


set @key  = 'AUEC'+cast(@AUECID as varchar(3))+':C'+cast(@CounterpartyID as varchar(3))+':V'+cast(@venueID as varchar(5))
insert into T_TempCache values(@key,@orderSide,@orderType ,@handlingInst,@executinInst,@tif)


set @orderSide=''
set @orderType=''
set @handlingInst=''
set @executinInst=''
set @tif=''
FETCH NEXT FROM crs_CVAUEC INTO @CVAUECID,@CounterpartyID,@venueID,@AUECID

END
select * from T_TempCache
CLOSE crs_CVAUEC
DEALLOCATE crs_CVAUEC


