
CREATE Procedure [dbo].[P_OTC_SaveTemplateData]                                                      
(                                                      
 @xml nText,                                                      
 @ErrorMessage varchar(500) output,                                                                                               
 @ErrorNumber int output                                                                  
)                                                         
As         
--Declare @xml varchar(5000)                                                      
--Declare @ErrorMessage varchar(500)                                                                                               
--Declare @ErrorNumber int     
	                                                                     
--SET @ErrorNumber = 0                                                                          
--SET @ErrorMessage = 'Success'                                                       
--  set @xml = '<SecMasterEquitySwap xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
--  <Id>0</Id>
--  <Name>EquitySwap_6</Name>
--  <InstrumentType>1</InstrumentType>
--  <Description>12211</Description>
--  <UnderlyingAssetID>1</UnderlyingAssetID>
--  <ISDACounterParty>21</ISDACounterParty>
--  <CreatedBy>17</CreatedBy>
--  <CreationDate>2019-10-30T19:09:42</CreationDate>
--  <LastModifiedBy>17</LastModifiedBy>
--  <LastModifieDate>2019-10-30T19:54:03.0595998+05:30</LastModifieDate>
--  <DaysToSettle>3</DaysToSettle>
--  <EffectiveDate>0001-01-01T00:00:00</EffectiveDate>
--  <CustomFieldsString>[]</CustomFieldsString>
--  <EquitySwapId>0</EquitySwapId>
--  <OTCTemplateID>4</OTCTemplateID>
--  <EquityLeg_Frequency>1</EquityLeg_Frequency>
--  <EquityLeg_BulletSwap>true</EquityLeg_BulletSwap>
--  <EquityLeg_ExcludeDividends>true</EquityLeg_ExcludeDividends>
--  <EquityLeg_ImpliedCommission>true</EquityLeg_ImpliedCommission>
--  <CommissionBasis>5</CommissionBasis>
--  <HardCommissionRate>1</HardCommissionRate>
--  <SoftCommissionRate>1</SoftCommissionRate>
--  <FinanceLeg_InterestRate>4</FinanceLeg_InterestRate>
--  <FinanceLeg_SpreadBasisPoint>101</FinanceLeg_SpreadBasisPoint>
--  <FinanceLeg_FixedRate>10</FinanceLeg_FixedRate>
--  <FinanceLeg_Frequency>1</FinanceLeg_Frequency>
--  <FinanceLeg_DayCount>5</FinanceLeg_DayCount>
--  <CustomFields />
--  <EquityLeg_FirstPaymentDate>1800-01-01T12:00:00</EquityLeg_FirstPaymentDate>
--  <EquityLeg_ExpirationDate>1800-01-01T12:00:00</EquityLeg_ExpirationDate>
--  <FinanceLeg_FirstResetDate>1800-01-01T12:00:00</FinanceLeg_FirstResetDate>
--  <FinanceLeg_FirstPaymentDate>1800-01-01T12:00:00</FinanceLeg_FirstPaymentDate>
--</SecMasterEquitySwap>'                                     

--BEGIN TRY                                   
                                                      
-- BEGIN TRAN TRAN1                                                                            
                                                                              
                                                                               
  DECLARE @handle int                                                                            
                                                                                                                           
  exec sp_xml_preparedocument @handle OUTPUT, @Xml 
  
  
  CREATE TABLE #TempTemplateData(
	[Id]  [int], 
	[Name] [varchar](500),
	[InstrumentType] [varchar](100),
	[Description] [varchar](1000),
	[UnderlyingAssetID] [int],
	[ISDACounterParty] [int],
	[CreatedBy] [int],
	[CreationDate] [datetime] ,
	[LastModifiedBy] [int],
	[LastModifieDate] [datetime] ,
	[ISDAContract] [ntext],
	[EquityLeg_Frequency] [varchar](50),
	[EquityLeg_BulletSwap] [bit],
	[EquityLeg_ExcludeDividends] [bit],
	[EquityLeg_ImpliedCommission] [bit],
	[CommissionBasis] [varchar](50),
	[HardCommissionRate] [float],
	[SoftCommissionRate] [float],
	[FinanceLeg_InterestRate] [float],
	[FinanceLeg_SpreadBasisPoint] [float],
	[FinanceLeg_DayCount] [int],
	[FinanceLeg_Frequency] [varchar](50) ,
	[CustomFieldsString] nText,
    [DaysToSettle] [int],
	[FinanceLeg_FixedRate] [float]
 )
                                                                                                                   
 Insert Into #TempTemplateData                                                                                                                                                                           
 (    
    [Id],                                                                                                                                                                      
	 [Name],
	[InstrumentType],
	[Description],
	[UnderlyingAssetID] ,
	[ISDACounterParty],
	[CreatedBy],
	[CreationDate] ,
	[LastModifiedBy],
	[LastModifieDate],
	[ISDAContract] ,  
	[EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap],
	[EquityLeg_ExcludeDividends],
	[EquityLeg_ImpliedCommission],
	[CommissionBasis] ,
	[HardCommissionRate],
	[SoftCommissionRate],
	[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] ,
	[FinanceLeg_Frequency],
	[CustomFieldsString],
	[DaysToSettle]   ,
	[FinanceLeg_FixedRate]                              
  )                                                                               
  Select 
   [Id]  ,                               
    [Name],
	[InstrumentType],
	[Description],
	[UnderlyingAssetID] ,
	[ISDACounterParty],
	[CreatedBy],
	[CreationDate] ,
	[LastModifiedBy],
	[LastModifieDate],
	[ISDAContract] ,  
	[EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap],
	[EquityLeg_ExcludeDividends],
	[EquityLeg_ImpliedCommission],
	[CommissionBasis] ,
	[HardCommissionRate],
	[SoftCommissionRate],
	[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] ,
	[FinanceLeg_Frequency], 
	[CustomFieldsString],  
	[DaysToSettle]   ,
	[FinanceLeg_FixedRate]                       
                                                              
  FROM OPENXML(@handle, '//SecMasterEquitySwap', 2)                                                                                                                           
  WITH                                                                                                
  (    
   [Id]  [int],                                                     
    [Name] [varchar](500),
	[InstrumentType] [varchar](100),
	[Description] [varchar](1000),
	[UnderlyingAssetID] [int],
	[ISDACounterParty] [int],
	[CreatedBy] [int],
	[CreationDate] [datetime] ,
	[LastModifiedBy] [int],
	[LastModifieDate] [datetime] ,
	[ISDAContract] [ntext],
	[EquityLeg_Frequency] [varchar](50),
	[EquityLeg_BulletSwap] [bit],
	[EquityLeg_ExcludeDividends] [bit],
	[EquityLeg_ImpliedCommission] [bit],
	[CommissionBasis] [varchar](50),
	[HardCommissionRate] [float],
	[SoftCommissionRate] [float],
	[FinanceLeg_InterestRate] [float],
	[FinanceLeg_SpreadBasisPoint] [float],
	[FinanceLeg_DayCount] [int],
	[FinanceLeg_Frequency] [varchar](50),
	[CustomFieldsString] nText ,
	[DaysToSettle] [int],
	[FinanceLeg_FixedRate] [float]                                                         
  )  
  
  --select * from   #TempTemplateData                                                      

  update T_OTC_Templates
  Set
    [Name] = #TempTemplateData.[Name],
	[InstrumentType]= #TempTemplateData.[InstrumentType],
	[Description]=#TempTemplateData.[Description],
	[UnderlyingAssetID] =#TempTemplateData.[UnderlyingAssetID],
	[ISDACounterParty]=#TempTemplateData.[ISDACounterParty],
	[CreatedBy]=#TempTemplateData.[CreatedBy],
	[CreationDate] =#TempTemplateData.[CreationDate],
	[LastModifiedBy]=#TempTemplateData.[LastModifiedBy],
	[LastModifieDate]=#TempTemplateData.[LastModifieDate],
	[ISDAContract]  =#TempTemplateData.[ISDAContract],
	[DaysToSettle]  =#TempTemplateData.[DaysToSettle]
  from 
  T_OTC_Templates
  inner join #TempTemplateData on #TempTemplateData.Id = T_OTC_Templates.ID


  update T_OTC_EquitySwapData
  Set
                                                                                                                                                                        
	[EquityLeg_Frequency]=#TempTemplateData.[EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap]=#TempTemplateData.[EquityLeg_BulletSwap],
	[EquityLeg_ExcludeDividends]=#TempTemplateData.[EquityLeg_ExcludeDividends],
	[EquityLeg_ImpliedCommission]=#TempTemplateData.[EquityLeg_ImpliedCommission],
	[Commission_Basis] =#TempTemplateData.[CommissionBasis],
	[Commission_HardCommissionRate]=#TempTemplateData.[HardCommissionRate],
	[Commission_SoftCommissionRate]=#TempTemplateData.[SoftCommissionRate],
	[FinanceLeg_InterestRate]=#TempTemplateData.[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint]=#TempTemplateData.[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] =#TempTemplateData.[FinanceLeg_DayCount],
	[FinanceLeg_Frequency]=#TempTemplateData.[FinanceLeg_Frequency],
	[CustomFields]=#TempTemplateData.[CustomFieldsString],
	[FinanceLeg_FixedRate] = #TempTemplateData.[FinanceLeg_FixedRate]
  from T_OTC_EquitySwapData
  inner join T_OTC_Templates on T_OTC_Templates.ID = T_OTC_EquitySwapData.OTCTemplateID
  inner join #TempTemplateData on #TempTemplateData.Id = T_OTC_Templates.ID




IF exists(select * from   #TempTemplateData
	where #TempTemplateData.Id not in (select ID from T_OTC_Templates) )
BEGIN
		                
 Insert Into T_OTC_Templates                                                                                                                                                                           
 (                                                                                                                                                                          
	[Name],
	[InstrumentType],
	[Description],
	[UnderlyingAssetID] ,
	[ISDACounterParty],
	[CreatedBy],
	[CreationDate] ,
	[LastModifiedBy],
	[LastModifieDate],
	[ISDAContract] , 
	  [DaysToSettle]                  
  )                                                                               
  Select                                  
   [Name],
	[InstrumentType],
	[Description],
	[UnderlyingAssetID] ,
	[ISDACounterParty],
	[CreatedBy],
	[CreationDate] ,
	[LastModifiedBy],
	[LastModifieDate],
	[ISDAContract] ,
	[DaysToSettle]
	from   #TempTemplateData
	where #TempTemplateData.ID not in (select ID from T_OTC_Templates)

declare @templateID int
SELECT @templateID = SCOPE_IDENTITY()

  Insert Into T_OTC_EquitySwapData                                                                                                                                                                           
   ( 
   [OTCTemplateID] ,                                                                                                                                                                       
	[EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap],
	[EquityLeg_ExcludeDividends],
	[EquityLeg_ImpliedCommission],
	[Commission_Basis] ,
	[Commission_HardCommissionRate],
	[Commission_SoftCommissionRate],
	[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] ,
	[FinanceLeg_Frequency],
	[CustomFields] ,
	 [FinanceLeg_FixedRate]                   
  )                                                                               
  Select  
    @templateID,                                
    [EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap],
	[EquityLeg_ExcludeDividends],
	[EquityLeg_ImpliedCommission],
	[CommissionBasis] ,
	[HardCommissionRate],
	[SoftCommissionRate],
	[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] ,
	[FinanceLeg_Frequency] ,
	[CustomFieldsString],
	[FinanceLeg_FixedRate]
	from   #TempTemplateData

END                

Drop Table #TempTemplateData                      
                                                  
EXEC sp_xml_removedocument @handle                                                                                
                                                                                 
--COMMIT TRANSACTION TRAN1                                                                                
                                              
--END TRY                                                                                
--BEGIN CATCH                             
-- SET @ErrorMessage = ERROR_MESSAGE();                                                             
-- SET @ErrorNumber = Error_number();                                                                                 
-- ROLLBACK TRANSACTION TRAN1                                                                                   
--END CATCH;