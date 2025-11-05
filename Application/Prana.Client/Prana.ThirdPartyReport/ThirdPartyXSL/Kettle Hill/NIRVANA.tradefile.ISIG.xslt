<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection xmlns:xsi="http://www.w3.org/2001/XMLschema-instance" xsi:noNamespaceSchemaLocation="alan.xsd">

			<!--<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>-->
			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty='ISIG']">

				
				<!--<ThirdPartyFlatFileDetail emailto="midoffice@evercoreisi.com , rpd@nirvanasolutions.com, Charlie@rpdfund.com">-->
				 <ThirdPartyFlatFileDetail emailto="rpd@nirvanasolutions.com "> 
				
				
				
					<xsl:choose>
						<xsl:when test="Side='Buy to Open' or Side='Buy' ">
							<Side>
								<xsl:value-of select ="'B'"/>
							</Side>
						</xsl:when>
						<xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
							<Side>
								<xsl:value-of select ="'BTC'"/>
							</Side>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close' ">
							<Side>
								<xsl:value-of select ="'S'"/>
							</Side>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
							<Side>
								<xsl:value-of select ="'SS'"/>
							</Side>
						</xsl:when>
						<xsl:otherwise>
							<Side>
								<xsl:value-of select="Side"/>
							</Side>
						</xsl:otherwise>
					</xsl:choose>

					<Ticker>
						<xsl:value-of select="Symbol"/>
					</Ticker>
					
					<OSI>
						<xsl:value-of select="OSISymbol"/>
					</OSI>


					<CUSIP>
						<xsl:value-of select="CUSIPSymbol"/>
					</CUSIP>

					<RIC>
						<xsl:value-of select="ReutersSymbol"/>
					</RIC>

					<BBCode>
						<xsl:value-of select="BloombergSymbol"/>
					</BBCode>

					<ISIN>
						<xsl:value-of select="ISINSymbol"/>
					</ISIN>

					<Sedol>
						<xsl:value-of select="SEDOLSymbol"/>
					</Sedol>

					<OrderID>
						<xsl:value-of select ="TradeRefID"/>
					</OrderID>

					<OrderQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</OrderQuantity>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select ="SettlementDate"/>
					</SettlementDate>

					<ExecutionPrice>
						<xsl:value-of select="AveragePrice"/>
					</ExecutionPrice>

					<ExecutingBrokerCode>
						<xsl:value-of select ="CounterParty"/>
					</ExecutingBrokerCode>

					<!--<Account>-->
					<!--<xsl:value-of select="FundName"/>-->
					<!--</Account>-->


					<Account>
						<xsl:value-of select="AccountName"/>
					</Account>

					<TradeCommission>
						<xsl:value-of select="CommissionCharged"/>
					</TradeCommission>

					<SecFees>
						<xsl:value-of select="Secfee"/>
					</SecFees>

					<OtherFees>
						<xsl:value-of select="TaxOnCommissions + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
					</OtherFees>

					<StrikePrice>
						<xsl:value-of select="StrikePrice"/>
					</StrikePrice>

					<ExpirationDate>
						<xsl:value-of select="ExpirationDate"/>
					</ExpirationDate>

					<PutOrCall>
						<xsl:value-of select="PutOrCall"/>
					</PutOrCall>

					<UnderlyingSymbol>
						<xsl:value-of select="UnderlyingSymbol"/>
					</UnderlyingSymbol>

					<Exchange>
						<xsl:value-of select="Exchange"/>
					</Exchange>

					<TradedCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</TradedCurrency>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
