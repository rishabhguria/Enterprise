 --Correct the data of the table T_PTTAccountPercentagePreference

SELECT *

INTO #Temp1

FROM T_PTTAccountPercentagePreference

UPDATE #Temp1

SET PreferenceType = '1'

--select * from #Temp1

SELECT *

INTO #Temp2

FROM T_PTTAccountPercentagePreference

UPDATE #Temp2

SET PreferenceType = '2'

--select * from #Temp2

INSERT INTO T_PTTAccountPercentagePreference

SELECT *

FROM #Temp1


INSERT INTO T_PTTAccountPercentagePreference

SELECT *

FROM #Temp2

--select * from T_PTTAccountPercentagePreference

 --Correct the data of the table T_PTTAccountPercentagePreference

SELECT *

INTO #Temp3

FROM T_PTTMasterFundPreference

UPDATE #Temp3

SET PreferenceType = '1'

SELECT *

INTO #Temp4

FROM T_PTTMasterFundPreference

UPDATE #Temp4

SET PreferenceType = '2'


INSERT INTO T_PTTMasterFundPreference

SELECT *

FROM #Temp3


INSERT INTO T_PTTMasterFundPreference

SELECT *

FROM #Temp4

--select * from T_PTTMasterFundPreference

--delete temp table 

drop table #Temp1

drop table #Temp2

drop table #Temp3

drop table #Temp4


