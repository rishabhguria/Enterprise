---Created by : Bhupendra Singh Bora
---Date: 9 October 2015
---Description: Set the RoundLot value equals to 1 where RoundLot value is Null

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_AUEC' AND COLUMN_NAME='RoundLot')
BEGIN
	UPDATE T_AUEC SET RoundLot=1 WHERE RoundLot IS NULL
END
