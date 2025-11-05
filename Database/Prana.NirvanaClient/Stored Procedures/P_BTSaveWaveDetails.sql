


CREATE Procedure [dbo].[P_BTSaveWaveDetails]
(
@waveID varchar(50),
@basketID varchar(50),
@percentage float
)
as
insert into  T_BTWave(waveID,BasketID,Percentage) 
values(@waveID,@basketID,@percentage )


