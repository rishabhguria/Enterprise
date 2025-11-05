
CREATE PROC P_GetTaxLotForDate (
	@FromAllAUECDatesString VARCHAR(max)
	,@ToAllAUECDatesString VARCHAR(max)
	)
AS
DECLARE @ToAUECDatesTable TABLE (
	AUECID INT
	,CurrentAUECDate DATETIME
	)

INSERT INTO @ToAUECDatesTable
SELECT *
FROM dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)

DECLARE @FromAUECDatesTable TABLE (
	AUECID INT
	,CurrentAUECDate DATETIME
	)

INSERT INTO @FromAUECDatesTable
SELECT *
FROM dbo.GetAllAUECDatesFromString(@FromAllAUECDatesString)

SELECT taxlots.Symbol
	,taxlots.CounterPartyID
	,taxlots.VenueID
	,taxlots.OrderSideTagValue
	,taxlots.OrderTypeTagValue
	,taxlots.AUECID
	,taxlots.AssetID
	,taxlots.UnderLyingID
	,taxlots.ExchangeID
	,taxlots.CurrencyID
	,taxlots.TradingAccountID
	,taxlots.TaxLotQty
	,taxlots.CumQty
	,taxlots.AvgPrice
	,0 AS CompanyUserID
	,SecMaster.CompanyName
	,taxlots.FundID
	,taxlots.Level2ID
	,taxlots.TaxLotQty
	,taxlots.Commission
	,taxlots.OtherBrokerFees
	,taxlots.StampDuty
	,taxlots.TransactionLevy
	,taxlots.ClearingFee
	,taxlots.TaxOnCommissions
	,taxlots.MiscFees
	,taxlots.AuecLocalDate
	,taxlots.SettlementDate
	,SecMaster.ExpirationDate
	,taxlots.ProcessDate
	,taxlots.OriginalPurchaseDate
	,taxlots.SecFee
	,taxlots.OccFee
	,taxlots.OrfFee
	,taxlots.ClearingBrokerFee
	,taxlots.SoftCommission
	,taxlots.Quantity
	,COALESCE(SettlCurrency_Taxlot, SettlCurrency_Group, 0) AS SettlCurrency
FROM V_TaxLots taxlots
INNER JOIN @FromAUECDatesTable AS FromAUECDates ON FromAUECDates.AUECID = taxlots.AUECID
INNER JOIN @ToAUECDatesTable AS ToAUECDates ON ToAUECDates.AUECID = taxlots.AUECID
	AND DATEDIFF(d, taxlots.AuecLocalDate, FromAUECDates.CurrentAUECDate) <= 0
	AND DATEDIFF(d, taxlots.AuecLocalDate, ToAUECDates.CurrentAUECDate) > = 0
INNER JOIN V_SecMasterData SecMaster ON taxlots.Symbol = SecMaster.TickerSymbol