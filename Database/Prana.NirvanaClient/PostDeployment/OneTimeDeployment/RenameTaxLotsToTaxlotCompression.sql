---Created by : Bharat Jangir
---Date: 12 July 2016
---Description: Rename TaxLots To Taxlot Compression

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_PMViews' AND COLUMN_NAME='PMViews')
BEGIN
	UPDATE T_PMViews SET PMViews = 'Taxlot' WHERE PMViews = 'TaxLots'
END
