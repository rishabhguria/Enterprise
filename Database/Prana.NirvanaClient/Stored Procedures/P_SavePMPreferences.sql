CREATE procedure P_SavePMPreferences  
(  
@userId int,  
@useClosingmark Varchar(10) ,  
@XPercentofAvgVolume float,
@IsShowPMToolbar Bit
)  
AS  
  
   
if ((select count(*) from PM_Preferences) =0)  
begin  
Insert into PM_Preferences(UserID,XPercentofAvgVolume,useClosingMark, IsShowPMToolbar)   
Values(@userID,@XPercentofAvgVolume,@useclosingmark, @IsShowPMToolbar)  
end  
ELSE  
begin  
Update PM_Preferences   
set XPercentofAvgVolume=@XPercentofAvgVolume, useClosingMark= @useClosingmark, IsShowPMToolbar= @IsShowPMToolbar

END  