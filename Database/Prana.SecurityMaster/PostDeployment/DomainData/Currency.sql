/*
This script merges the temp table into the main table WITHOUT modifing the current prefrences
*/

select * into #Temp_T_Currency from T_Currency where 0 = 1

Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(1,'US Dollar','USD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(2,'Hong Kong Dollar','HKD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(3,'Yen','JPY')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(4,'Pound Sterling','GBP')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(5,'United Arab Emirates, Dirhams','AED')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(6,'Brazilian Real','BRL')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(7,'Canadian Dollar','CAD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(8,'EURO','EUR')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(9,'Norwegian Kroner','NOK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(10,'Singapore Dollar','SGD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(11,'Multiple Currency','MUL')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(12,'South African Rand','ZAR')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(13,'Swedish Kronor','SEK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(14,'Australian Dollar','AUD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(15,'Chinese Renminbi','CNY')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(16,'Korean Won','KRW')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(17,'Taka','BDT')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(18,'Baht','THB')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(19,'VND','dong')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(20,'GBPPence','GBX')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(21,'Indian Rupee','INR')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(23,'CHF','CHF')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(24,'CLP','CLP')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(25,'COP','COP')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(26,'CZK','CZK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(27,'DKK','DKK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(28,'GHS','GHS')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(29,'HUF','HUF')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(30,'IDR','IDR')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(31,'ILS','ILS')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(32,'ISK','ISK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(33,'KZT','KZT')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(34,'LVL','LVL')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(35,'MXN','MXN')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(36,'NZD','NZD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(37,'PEN','PEN')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(38,'PLN','PLN')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(40,'RON','RON')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(41,'RUB','RUB')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(42,'SKK','SKK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(43,'TRY','TRY')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(44,'ARS','ARS')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(45,'UYU','UYU')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(46,'Taiwan Dollar','TWD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(47,'BMD','BMD')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(48,'EEK','EEK')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(49,'GEL','GEL')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(51,'MYR','MYR')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(52,'SIT','SIT')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(53,'XAF','XAF')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(54,'XOF','XOF')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(55,'New Manat','AZN')
Insert into #Temp_T_Currency(CurrencyID,CurrencyName,CurrencySymbol) VALUES(56,'Pakisthan rupee','PKR')


-- if not exist
		insert into T_Currency
		select 
		#Temp_T_Currency.*
		from #Temp_T_Currency 
		left outer join  T_Currency 
		on #Temp_T_Currency.CurrencyID = T_Currency.CurrencyID
		where 
		T_Currency.CurrencyID is null

drop table #Temp_T_Currency