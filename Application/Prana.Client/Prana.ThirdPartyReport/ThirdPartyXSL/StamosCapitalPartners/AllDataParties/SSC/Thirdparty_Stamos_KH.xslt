<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>



					<xsl:variable name="PB_NAME" select="'SSCEOD'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<FundID>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</FundID>

					<Blank1>
						<xsl:value-of select="''"/>
					</Blank1>

					<TradeType>
						<!--<xsl:choose>
							<xsl:when test="(Side='Buy' or Side='Buy to Open') and TaxLotState='Deleted'">
								<xsl:value-of select="'CANCELBL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell' or Side='Sell to Close') and TaxLotState='Deleted'">
								<xsl:value-of select="'CANCELSL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell short' or Side='Sell to Open') and TaxLotState='Deleted'">
								<xsl:value-of select="'CANCELSS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted' ">
								<xsl:value-of select="'CANCELBS'"/>
							</xsl:when>

							<xsl:when test="(Side='Buy' or Side='Buy to Open') and TaxLotState='Amended'">
								<xsl:value-of select="'UPDATEBL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell' or Side='Sell to Close') and TaxLotState='Amended'">
								<xsl:value-of select="'UPDATESL'"/>
							</xsl:when>
							<xsl:when test="(Side='Sell short' or Side='Sell to Open') and TaxLotState='Amended'">
								<xsl:value-of select="'UPDATESS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted' ">
								<xsl:value-of select="'UPDATEBS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' ">
										<xsl:value-of select="'BS'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'BL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'SL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' ">
								<xsl:value-of select="'BS'"/>
							</xsl:when>
						</xsl:choose>
					</TradeType>

					<!--<xsl:choose>
						<xsl:when test="TaxLotState='Allocated'">
							<xsl:value-of select ="'N'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Amemded'">
							<xsl:value-of select ="'A'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:value-of select ="'D'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>-->
					<NewCancelCorrect>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'UPDATE'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'CANCEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NewCancelCorrect>

					<PosType>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'STOCK'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'BOND'"/>
							</xsl:when>
							<xsl:when test="Asset='PrivateEquity'">
								<xsl:value-of select="'PrivateEquity'"/>
							</xsl:when>
						</xsl:choose>
					</PosType>

					<ID1>
						<xsl:choose>
							<xsl:when test="Asset='Equity'and CurrencySymbol!='USD'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='PrivateEquity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ID1>

					<Blank2>
						<xsl:value-of select="''"/>
					</Blank2>

					<Exchange>
						<xsl:value-of select="Exchange"/>
					</Exchange>

					<Qty>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Qty>

					<xsl:variable name="Price">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SettlCurrAmt"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>
					
					<Price>
						<xsl:value-of select="format-number($Price,'0.####')"/>
						
					</Price>

					<Currency>
						<xsl:value-of select="SettlCurrency"/>
					</Currency>

					<xsl:variable name="NetAmount">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="NetAmount * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="NetAmount div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					
					<NetAmt>
						
						<xsl:value-of select="format-number($NetAmount,'0.####')"/>
					</NetAmt>

					<Blank3>
						<xsl:value-of select="''"/>
					</Blank3>

					<Custodian>
						<xsl:value-of select="CounterParty"/>
					</Custodian>

					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</SettleDate>

					<Blank4>
						<xsl:value-of select="''"/>
					</Blank4>

					<TransactionID>
						<xsl:value-of select="EntityID"/>
					</TransactionID>

					<Blank5>
						<xsl:value-of select="''"/>
					</Blank5>

					<Blank6>
						<xsl:value-of select="''"/>
					</Blank6>
					<xsl:variable name="Commission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="COMM">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$Commission"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$Commission * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$Commission div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Commission>
						<xsl:value-of select="format-number($COMM,'0.####')"/>
						
					</Commission>

					<Blank7>
						<xsl:value-of select="''"/>
					</Blank7>

					<xsl:variable name="Fees">
						<xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee)"/>
					</xsl:variable>

					<xsl:variable name="FEE1">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$Fees"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$Fees * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$Fees div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Fees1>
						<xsl:value-of select="format-number($FEE1,'0.####')"/>						
					</Fees1>
					
					<xsl:variable name="FEE2">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="SecFee"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="SecFee * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="SecFee div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<Fees2>
						<xsl:value-of select="format-number($FEE2,'0.####')"/>						
					</Fees2>

					<Blank8>
						<xsl:value-of select="''"/>
					</Blank8>

					<Blank9>
						<xsl:value-of select="''"/>
					</Blank9>

					<Custodian1>
						<xsl:value-of select="CounterParty"/>
					</Custodian1>

					<Blank10>
						<xsl:value-of select="''"/>
					</Blank10>

					<Blank11>
						<xsl:value-of select="''"/>
					</Blank11>

					<Blank12>
						<xsl:value-of select="''"/>
					</Blank12>

					<Blank13>
						<xsl:value-of select="''"/>
					</Blank13>

					<Blank14>
						<xsl:value-of select="''"/>
					</Blank14>

					<Blank15>
						<xsl:value-of select="''"/>
					</Blank15>

					<Blank16>
						<xsl:value-of select="''"/>
					</Blank16>

					<BrokerSecurityID>
						<xsl:value-of select="EntityID"/>
					</BrokerSecurityID>

					<Broker>
						<xsl:choose>
							<xsl:when test="contains(AccountName,'JP Morgan')">
								<xsl:value-of select="'JPMS'"/>
							</xsl:when>
							<xsl:when test="contains(AccountName,'Charles')">
								<xsl:value-of select="'CHAS'"/>
							</xsl:when>				
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Broker>

					<SecurityName>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityName>

					<Blank17>
						<xsl:value-of select="''"/>
					</Blank17>

					<Blank18>
						<xsl:value-of select="''"/>
					</Blank18>

					<Currency1>
						<xsl:value-of select="SettlCurrency"/>
					</Currency1>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<SEDOL>
						<xsl:value-of select="SEDOL"/>
					</SEDOL>

					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<ID2>
						<xsl:choose>
							<xsl:when test="Asset='Equity'and CurrencySymbol!='USD'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='PrivateEquity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ID2>

					<FileHeader>
						<xsl:value-of select="'false'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'false'"/>
					</FileFooter>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>