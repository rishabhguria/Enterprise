CREATE PROCEDURE [dbo].[P_DeleteCVAUECs]
	(
		@counterPartyVenueID int,
		@cvAUECID varchar(MAX) = ''
	)
AS

Declare @result int
	if(@cvAUECID = '') 
	begin
		Delete T_CVAUEC
			Where CounterPartyVenueID = @counterPartyVenueID	
	end
	else
BEGIN TRY
 	Begin		
	BEGIN TRAN
		Delete T_CVSymbolMapping Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)
		
		Delete T_CVAUECSide Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)
		
		Delete T_CVAUECCompliance Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)

		Delete T_CVAUECExecutionInstructions Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)			    
			    
		Delete T_CVAUECHandlingInstructions Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)
			    
		Delete T_CVAUECOrderTypes Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)
			    
		Delete T_CVAUECTimeInForce Where CVAUECID NOT IN(SELECT CVAUECID FROM T_CVAUEC)
		
		exec ('Delete T_CVAUEC
		Where convert(varchar, CVAUECID) NOT IN(' + @cvAUECID + ') AND CounterPartyVenueID = ' + @counterPartyVenueID)
        
	COMMIT TRAN
	set @result=1
	End
END TRY


BEGIN CATCH
   	ROLLBACK TRAN
		Set @result=-2	
END CATCH

Select @result

