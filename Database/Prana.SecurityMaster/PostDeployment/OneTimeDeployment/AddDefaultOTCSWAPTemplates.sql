--Created By: Sachin Mishra
--Purpose: Script to add the default OTC temples in the OTC workflow table

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_OTC_CustomFields')
BEGIN
If((select count(*) from T_OTC_CustomFields) =0 )
Begin
DBCC CHECKIDENT ( T_OTC_CustomFields , RESEED , 1 ) 
INSERT INTO
   [dbo].[T_OTC_CustomFields] ( [InstrumentType] , [Name] , [DefaultValue] , [DataType] , [UIOrder] ) 
   SELECT      1,	'Target close :',	'Close Position by 2020', 	 '1',   1   
   UNION ALL
   SELECT      1,      'Flag Date :',      '10/29/2019',      '3',      2   
   UNION ALL
   SELECT      1,      'Reason for Holding :',      'Access to Market',      '1',      3 
   UNION ALL
   SELECT      1,      'Yes/No :',      'Yes',      '4',      4   
   UNION ALL
   SELECT      1,      'Ratings :',      '9',      '2',      5
   UNION ALL
   SELECT      2,      'CFD Triat 1 :',      'CFD Custom 1',      '1',      1 
   UNION ALL
   SELECT      2,      'CFD Triat 2 :',      'CFD Custom 2',      '1',      2 
   UNION ALL
   SELECT      2,      'Margin Max for Risk :',      '.80',      '2',      3
end
END

--Step 2
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_OTC_Templates')
BEGIN
If((select count(*) from T_OTC_Templates) =0 )
Begin
DBCC CHECKIDENT (T_OTC_Templates, RESEED, 1)
SET IDENTITY_INSERT T_OTC_Templates ON ;

INSERT INTO [dbo].[T_OTC_Templates]
 ([ID] ,[Name] ,[InstrumentType] ,[Description] ,[UnderlyingAssetID] ,[ISDACounterParty] ,[CreatedBy] ,[CreationDate] ,[LastModifiedBy] ,[LastModifieDate] ,[ISDAContract] ,[DaysToSettle])
 
 select     1 ,'EquitySwap' ,'1' ,'Equity Swap Default Template' ,1 ,47 ,0 ,GETDATE() ,0 ,GETDATE() ,Null ,3
 Union All 
 select     2 ,'CFD' ,'2' ,'CFD Default Template' ,1 ,47 ,0 ,GETDATE() ,0 ,GETDATE() ,Null ,3
  Union All 
 select     3 ,'ConvertibleBond' ,'3' ,'ConvertibleBond Default Template' ,1 ,47 ,0 ,GETDATE() ,0 ,GETDATE() ,Null ,3
End
End

---Step 3 
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_OTC_EquitySwapData')
BEGIN
If((select count(*) from T_OTC_EquitySwapData) =0 )
Begin
DBCC CHECKIDENT (T_OTC_EquitySwapData, RESEED, 1)
INSERT INTO [dbo].[T_OTC_EquitySwapData]
 ([OTCTemplateID]
 ,[EquityLeg_Frequency]
 ,[EquityLeg_BulletSwap]
 ,[EquityLeg_ExcludeDividends]
 ,[EquityLeg_ImpliedCommission]
 ,[Commission_Basis]
 ,[Commission_HardCommissionRate]
 ,[Commission_SoftCommissionRate]
 ,[FinanceLeg_InterestRate]
 ,[FinanceLeg_SpreadBasisPoint]
 ,[FinanceLeg_DayCount]
 ,[FinanceLeg_Frequency]
 ,[FinanceLeg_FixedRate]
 ,[CustomFields]
 )
  Select 1 ,1 ,1 ,1 ,1 ,1 ,convert(float,.02) ,convert(float,.005) ,2 ,10.50 ,3 ,1,convert(float,.02)
 ,'[{"ID":1,"Name":"Target close :","DefaultValue":"Close Position by 2020","DataType":"1","UIOrder":0,"InstrumentType":"1"},{"ID":2,"Name":"Flag Date :","DefaultValue":"10/29/2019","DataType":"3","UIOrder":0,"InstrumentType":"1"},{"ID":3,"Name":"Reason for Holding :","DefaultValue":"Access to Market","DataType":"1","UIOrder":0,"InstrumentType":"1"},{"ID":4,"Name":"Yes/No :","DefaultValue":"Yes","DataType":"4","UIOrder":0,"InstrumentType":"1"},{"ID":5,"Name":"Ratings :","DefaultValue":"9","DataType":"2","UIOrder":0,"InstrumentType":"1"}]'
  End
End

--Step 4
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_OTC_CFDData')
BEGIN
If((select count(*) from T_OTC_CFDData) =0 )
Begin
DBCC CHECKIDENT (T_OTC_CFDData, RESEED, 1)
INSERT INTO [dbo].[T_OTC_CFDData]
 ([OTCTemplateID]
 ,[Commission_Basis]
 ,[Commission_HardCommissionRate]
 ,[Commission_SoftCommissionRate]
 ,[FinanceLeg_InterestRate]
 ,[FinanceLeg_SpreadBasisPoint]
 ,[FinanceLeg_DayCount]
 ,[FinanceLeg_FixedRate]
 ,[FinanceLeg_ScriptLeadingFee]
 ,[Collateral_Margin]
 ,[Collateral_Rate]
 ,[Collateral_DayCount]
 ,[CustomFields])
     VALUES
 (
 2 ,'1' ,convert(float,.02) ,convert(float,.005) ,2 ,0 ,5 ,convert(float,3.25 ),0 ,100 ,convert(float,1.25 ),3
 ,'[{"ID":6,"Name":"CFD Triat 1 :","DefaultValue":"CFD Custom 1","DataType":"1","UIOrder":0,"InstrumentType":"2"},{"ID":7,"Name":"CFD Triat 2 :","DefaultValue":"CFD Custom 2","DataType":"1","UIOrder":0,"InstrumentType":"2"},{"ID":8,"Name":"Margin Max for Risk :","DefaultValue":".80","DataType":"2","UIOrder":0,"InstrumentType":"2"}]'
 )
End
End



--Step 4
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_OTC_ConvertibleBondData')
BEGIN
If((select count(*) from T_OTC_ConvertibleBondData) =0 )
Begin
DBCC CHECKIDENT (T_OTC_ConvertibleBondData, RESEED, 1)
INSERT INTO [dbo].[T_OTC_ConvertibleBondData]
 (    [OTCTemplateID]
      ,[EquityLeg_ConversionRatio]
      ,[FinanceLeg_ZeroCoupon]
      ,[FinanceLeg_IRBenchMark]
      ,[FinanceLeg_FXRate]
      ,[FinanceLeg_SBPoint]
      ,[FinanceLeg_DayCount]
      ,[FinanceLeg_CouponFreq]
      ,[Commission_Basis]
      ,[Commission_HardCommissionRate]
      ,[Commission_SoftCommissionRate]
      ,[CustomFields]
	  )
     VALUES
 (
 3 
 ,convert(float,.02) 
  ,0 
   ,2 
   ,convert(float,.02) 
   ,convert(float,.02) 
    ,1
   ,1 
   ,'1'
   ,convert(float,.02)
   ,convert(float,.02)
 ,'[{"ID":6,"Name":"CFD Triat 1 :","DefaultValue":"CFD Custom 1","DataType":"1","UIOrder":0,"InstrumentType":"3"},{"ID":7,"Name":"CFD Triat 2 :","DefaultValue":"CFD Custom 2","DataType":"1","UIOrder":0,"InstrumentType":"3"},{"ID":8,"Name":"Margin Max for Risk :","DefaultValue":".80","DataType":"2","UIOrder":0,"InstrumentType":"3"}]'
 )
End
End