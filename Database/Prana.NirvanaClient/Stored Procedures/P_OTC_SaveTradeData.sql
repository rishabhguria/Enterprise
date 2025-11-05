
CREATE Procedure [dbo].[P_OTC_SaveTradeData]                                                      
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
--  set @xml = '
--<SecMasterEquitySwap>
 
--  <Name>ytuytu</Name>
--  <InstrumentType>EquitySwap</InstrumentType>
--  <Description>tyuyt</Description>
--  <UnderlyingAssetID>1</UnderlyingAssetID>
--  <ISDACounterParty>0</ISDACounterParty>
--  <CreatedBy>1</CreatedBy>
--  <CreationDate>1900-01-26T00:00:00</CreationDate>
--  <LastModifiedBy>1</LastModifiedBy>
--  <LastModifieDate>1900-01-18T00:00:00</LastModifieDate>
--  <EquityLeg_Frequency>Quaterly</EquityLeg_Frequency>
--  <EquityLeg_BulletSwap>true</EquityLeg_BulletSwap>
--  <EquityLeg_ExcludeDividends>true</EquityLeg_ExcludeDividends>
--  <EquityLeg_ImpliedCommission>true</EquityLeg_ImpliedCommission>
--  <CommissionBasis>Notional</CommissionBasis>
--  <HardCommissionRate>55</HardCommissionRate>
--  <SoftCommissionRate>5</SoftCommissionRate>
--  <FinanceLeg_InterestRate>1</FinanceLeg_InterestRate>
--  <FinanceLeg_SpreadBasisPoint>56</FinanceLeg_SpreadBasisPoint>
--  <FinanceLeg_FixedRate>56</FinanceLeg_FixedRate>
--  <FinanceLeg_Frequency>Quaterly</FinanceLeg_Frequency>
--  <FinanceLeg_DayCount>1</FinanceLeg_DayCount>
--</SecMasterEquitySwap>'                                     

--BEGIN TRY                                   
                                                      
-- BEGIN TRAN TRAN1                                                                            
                                                                              
  DECLARE @handle int                                                                            
                                                                                                                           
  exec sp_xml_preparedocument @handle OUTPUT, @Xml 
  
  
  CREATE TABLE #TempTemplateData(
	
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
	[CustomFieldsString] nText
 
 )
                                                                                                                   
 Insert Into #TempTemplateData                                                                                                                                                                           
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
	[CustomFieldsString]                        
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
	[CustomFieldsString]                        
                                                              
  FROM OPENXML(@handle, '//SecMasterEquitySwap', 2)                                                                                                                           
  WITH                                                                                                
  (                                                        
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
	[CustomFieldsString] nText                                                        
  )                                                          

Insert Into T_OTC_EquitySwapTradeData                                                                                                                                                                           
   ( 
                                                                                                                                                                       
	[EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap],
	[EquityLeg_ImpliedCommission],
	[Commission_Basis] ,
	[Commission_HardCommissionRate],
	[Commission_SoftCommissionRate],
	[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] ,
	[FinanceLeg_Frequency],
	[CustomFields] 
	                    
  )                                                                               
  Select  
                               
   [EquityLeg_Frequency] ,
	[EquityLeg_BulletSwap],
	[EquityLeg_ImpliedCommission],
	[Commission_Basis] ,
	[Commission_HardCommissionRate],
	[Commission_SoftCommissionRate],
	[FinanceLeg_InterestRate],
	[FinanceLeg_SpreadBasisPoint] ,
	[FinanceLeg_DayCount] ,
	[FinanceLeg_Frequency],
	[CustomFieldsString] 
	
	from   #TempTemplateData

                 
Drop Table #TempTemplateData                      
                                                  
EXEC sp_xml_removedocument @handle                                                                                
                                                                                 
--COMMIT TRANSACTION TRAN1                                                                                
                                              
--END TRY                                                                                
--BEGIN CATCH                             
-- SET @ErrorMessage = ERROR_MESSAGE();                                                             
-- SET @ErrorNumber = Error_number();                                                                                 
-- ROLLBACK TRANSACTION TRAN1                                                                                   
--END CATCH;