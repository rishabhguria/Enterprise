/*

Post-Deployment Script Template	

*/


TRUNCATE TABLE T_SAPISnapshotRequestField;

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
VALUES
('VWAP', 'VWAP', 1, 1, 0, 1, 0, 0, 0),
('CusipNo', 'ID_CUSIP', 1, 1, 1, 0, 0, 1, 0),
('SedolSymbol', 'ID_SEDOL1', 1, 0, 0, 0, 0, 1, 0),
('ISIN', 'ID_ISIN', 1, 0, 0, 1, 0, 1, 0),
('AverageVolume20Day', 'VOLUME_AVG_20D', 0, 0, 0, 0, 0, 0, 0),
('Volume10DAvg', 'VOLUME_AVG_10D', 0, 0, 0, 0, 0, 0, 0),
('PutOrCall', 'OPT_PUT_CALL', 0, 1, 0, 1, 0, 0, 0),
('OpenInterest', 'OPEN_INT', 0, 1, 1, 1, 0, 0, 0),
('ExpirationDate', 'OPT_EXPIRE_DT', 0, 1, 1, 1, 0, 0, 0),
('StrikePrice', 'OPT_STRIKE_PX', 0, 1, 0, 1, 0, 0, 0),
('LastPrice', 'PX_LAST', 1, 1, 1, 1, 1, 1, 1),
('TotalVolume', 'PX_VOLUME', 1, 1, 1, 1, 0, 1, 0),
('TradeVolume', 'PX_VOLUME', 0, 0, 0, 0, 0, 0, 0),
('Bid', 'PX_BID', 1, 1, 0, 1, 1, 1, 1),
('BidSize', 'BID_SIZE_CR', 0, 0, 0, 0, 0, 0, 0),
('MarketCapitalization ', 'CUR_MKT_CAP', 1, 0, 0, 0, 0, 0, 0),
('BidExchange', 'PX_EXCH_CODE_BID_ALL_SESSION', 0, 0, 0, 0, 0, 0, 0),
('Ask', 'PX_ASK', 1, 1, 0, 1, 1, 1, 1),
('AskSize', 'ASK_SIZE_CR', 0, 0, 0, 0, 0, 0, 0),
('AskExchange', 'PX_EXCH_CODE_ASK_ALL_SESSION', 0, 0, 0, 0, 0, 0, 0),
('High', 'PX_HIGH', 1, 1, 0, 0, 1, 1, 0),
('Low', 'PX_LOW', 1, 1, 0, 0, 1, 1, 0),
('High52W', 'HIGH_52WEEK', 0, 0, 0, 0, 0, 0, 0),
('Low52W', 'LOW_52WEEK', 0, 0, 0, 0, 0, 0, 0),
('OpenInterestDelta', 'DELTA', 0, 0, 0, 0, 0, 0, 0),
('SharesOutstanding', 'EQY_SH_OUT', 1, 0, 0, 0, 0, 0, 0),
('CountryId', 'COUNTRY_ISO', 1, 0, 1, 1, 1, 1, 1),
('Previous', 'PREV_CLOSE_VAL', 1, 1, 1, 1, 1, 1, 0),
('FullCompanyName', 'NAME', 1, 1, 1, 1, 1, 1, 1),
('OSIOptionSymbol', 'OCC_SYMBOL', 0, 0, 0, 0, 1, 0, 0),
('UpdateTime', 'LAST_UPDATE', 0, 0, 0, 0, 0, 0, 0),
('LastTick', 'LAST_TICK_DIRECTION', 0, 0, 0, 0, 0, 0, 0),
('Change', 'NET_CHANGE_ON_DAY_TODAY_RT', 1, 1, 1, 1, 1, 1, 1),
('DividendYield', 'DIVIDEND_INDICATED_YIELD', 1, 1, 0, 0, 0, 0, 0),
('Dividend', 'LAST_DPS_GROSS', 1, 0, 0, 0, 0, 0, 0),
('DividendInsterval', 'DVD_FREQ', 1, 0, 0, 0, 0, 0, 0),
('XDividendDate', 'DVD_EX_DT', 1, 0, 0, 0, 0, 0, 0),
('AnnualDividend', 'DVD_SH_12M', 1, 0, 0, 0, 0, 0, 0),
('CurrencyCode', 'CRNCY', 1, 1, 1, 1, 1, 1, 1),
('RoundLot', 'PX_ROUND_LOT_SIZE', 1, 0, 0, 0, 0, 0, 0),
('UnderlyingSymbol', 'OPT_UNDL_TICKER', 0, 1, 0, 1, 0, 0, 0),
('Exchange', 'EQY_PRIM_EXCH_SHRT', 1, 1, 1, 1, 0, 0, 0),
('Future Contract Size(Multiplier)', 'FUT_CONT_SIZE', 0, 0, 1, 0, 0, 0, 0),
('Futures Contract Expiration Date', 'FUT_CONTRACT_DT', 0, 0, 1, 0, 0, 0, 0),
('Bloomberg Composite Code', 'COMPOSITE_EXCH_CODE', 1, 0, 0, 0, 0, 0, 0),
('Issue Date', 'ISSUE_DT', 0, 0, 0, 0, 0, 1, 0),
('Coupon', 'CPN', 0, 0, 0, 0, 0, 1, 0),
('MaturityDate', 'MATURITY', 0, 0, 0, 0, 0, 1, 0),
('Accrual Basis', 'DAY_CNT_DES', 0, 0, 0, 0, 0, 1, 0),
('Coupon Frequency', 'CPN_FREQ', 0, 0, 0, 0, 0, 1, 0),
('Bond type', 'MARKET_SECTOR_DES', 0, 0, 0, 0, 0, 1, 0),
('First Coupon Date', 'REAL_FIRST_CPN_DT', 0, 0, 0, 0, 0, 1, 0),
('OptionMultiplier', 'OPT_MULTIPLIER', 0, 1, 0, 0, 0, 0, 0);

