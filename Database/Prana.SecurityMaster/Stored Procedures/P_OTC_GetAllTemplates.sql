
CREATE Procedure [dbo].[P_OTC_GetAllTemplates]  (
@InstrumentType int =0      
)
AS
if(@InstrumentType<>0)
	Begin
SELECT [ID]
      ,[Name]
      ,[InstrumentType]
      ,[Description]
      ,[UnderlyingAssetID]
      ,[ISDACounterParty]
      ,[CreatedBy]
      ,[CreationDate]
      ,[LastModifiedBy]
      ,[LastModifieDate]
	  ,[ISDAContract]
	  ,[DaysToSettle]
  FROM [dbo].[T_OTC_Templates]
  where InstrumentType = @InstrumentType 
  End
  Else
  Begin
  SELECT [ID]
      ,[Name]
      ,[InstrumentType]
      ,[Description]
      ,[UnderlyingAssetID]
      ,[ISDACounterParty]
      ,[CreatedBy]
      ,[CreationDate]
      ,[LastModifiedBy]
      ,[LastModifieDate]
	  ,[ISDAContract]
	  ,[DaysToSettle]
  FROM [dbo].[T_OTC_Templates]
  End