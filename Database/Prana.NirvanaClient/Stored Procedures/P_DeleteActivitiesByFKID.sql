-- =============================================
-- Author:		<Author,,Nishant Kumar Jain>
-- Create date: <Create Date,,2015-05-07>
-- Description:	<Description,, To delete Journals and Activites>
-- =============================================
CREATE PROCEDURE P_DeleteActivitiesByFKID	
	@FKID VarChar(50),
	@TransactionNumber int = null
AS
BEGIN	
	SET NOCOUNT ON;
   
	DELETE FROM T_AllActivity  WHERE FKID = @FKID;
	
	DELETE FROM T_Journal WHERE TransactionId = @FKID AND TransactionNumber = @TransactionNumber;
	
	IF EXISTS(SELECT 1 FROM T_SymbolLevelAccrualsJournal  WHERE TransactionId = @FKID AND TransactionNumber = @TransactionNumber)
	BEGIN
		DELETE FROM T_SymbolLevelAccrualsJournal WHERE TransactionId = @FKID AND TransactionNumber = @TransactionNumber;
	END

END
