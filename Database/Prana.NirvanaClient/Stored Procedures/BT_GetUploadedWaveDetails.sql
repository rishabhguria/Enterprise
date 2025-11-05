create procedure [dbo].[BT_GetUploadedWaveDetails]
(
@basketID varchar(50)
)
as
select waveID from T_BTWave as TW
where BasketID=@basketID



