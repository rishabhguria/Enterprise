

-- =============================================
-- Author:		<Rajat>
-- Create date: <06 Oct 2006>
-- Description:	<Gets the Keyvaluepair for currency>
-- =============================================
Create PROCEDURE [dbo].[P_GetKeyValuePairCurrencies]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CurrencyID, CurrencySymbol from T_Currency
END

