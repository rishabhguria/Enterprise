CREATE PROCEDURE [dbo].[P_SaveMasterFundAccountPreference]
	@xmlMasterFundPref xml,
	@xmlAccountPref xml
AS
DECLARE @hdoc int,@hhdoc int
	
SELECT *
INTO #Temp1
FROM T_PTTAccountPercentagePreference where AccountId in (select AccountId from T_PTTAccountPercentagePreference group by (AccountId) having count(AccountId)=1)

UPDATE #Temp1
SET PreferenceType = '1'

SELECT *
INTO #Temp2
FROM T_PTTAccountPercentagePreference where AccountId in (select AccountId from T_PTTAccountPercentagePreference group by (AccountId) having count(AccountId)=1)

UPDATE #Temp2
SET PreferenceType = '2'

INSERT INTO T_PTTAccountPercentagePreference
SELECT *
FROM #Temp1

INSERT INTO T_PTTAccountPercentagePreference
SELECT *
FROM #Temp2



	EXEC sp_xml_preparedocument @hdoc OUTPUT, @xmlAccountPref
	 Update T_PTTAccountPercentagePreference 
		set T_PTTAccountPercentagePreference.PercentInMasterFund=userPefsXml.Percentage,
		T_PTTAccountPercentagePreference.AccountFactor=userPefsXml.AccountFactor
			 From
			(Select * From OPENXML(@hdoc,'//AccountPreference',2)
					WITH(
						[AccountId] int,
						[Percentage] decimal(18,6),
						[AccountFactor] float,
						[PreferenceType] int
						)
			)
			AS userPefsXml Join T_PTTAccountPercentagePreference   on userPefsXml.AccountId=T_PTTAccountPercentagePreference.AccountId and userPefsXml.PreferenceType=T_PTTAccountPercentagePreference.PreferenceType

  EXEC sp_xml_removedocument @hdoc 

SELECT *
INTO #Temp3
FROM T_PTTMasterFundPreference where MasterFundId in (select MasterFundId from T_PTTMasterFundPreference group by (MasterFundId) having count(MasterFundId)=1)

UPDATE #Temp3
SET PreferenceType = '1'

SELECT *
INTO #Temp4
FROM T_PTTMasterFundPreference where MasterFundId in (select MasterFundId from T_PTTMasterFundPreference group by (MasterFundId) having count(MasterFundId)=1)

UPDATE #Temp4
SET PreferenceType = '2'

INSERT INTO T_PTTMasterFundPreference
SELECT *
FROM #Temp3

INSERT INTO T_PTTMasterFundPreference
SELECT *
FROM #Temp4

drop table #Temp1
drop table #Temp2
drop table #Temp3
drop table #Temp4

   EXEC sp_xml_preparedocument @hhdoc OUTPUT, @xmlMasterFundPref
	Update  T_PTTMasterFundPreference
		Set T_PTTMasterFundPreference.UseProrataPreference=userPrefMf.IsProrataPrefChecked
		From
		(Select * From OPENXML(@hhdoc,'//MasterFundPreference',2)
			WITH(
				[MasterFundId] int,
				[IsProrataPrefChecked] bit,
				[PreferenceType] int
				)		
		 )
  As userPrefMf Join T_PTTMasterFundPreference on userPrefMf.MasterFundId=T_PTTMasterFundPreference.MasterFundId and userPrefMf.PreferenceType=T_PTTMasterFundPreference.PreferenceType

RETURN 0
