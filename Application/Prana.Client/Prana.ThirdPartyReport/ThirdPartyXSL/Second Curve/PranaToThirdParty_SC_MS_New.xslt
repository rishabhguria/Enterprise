<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<MSAccount>
						<xsl:value-of select ="FundMappedName"/>
					</MSAccount>

					<Trade_Reference>
						<xsl:value-of select="TradeRefID"/>
					</Trade_Reference>
					<xsl:variable name = "varTradeMth" >
						<xsl:value-of select="substring(TradeDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeDay" >
						<xsl:value-of select="substring(TradeDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeYR" >
						<xsl:value-of select="substring(TradeDate,7,4)"/>
					</xsl:variable>
					<TradeDate>
						<xsl:value-of select="concat($varTradeYR,'',$varTradeMth,'',$varTradeDay)"/>
					</TradeDate>

					<xsl:variable name = "varSettleMth" >
						<xsl:value-of select="substring(SettlementDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleDay" >
						<xsl:value-of select="substring(SettlementDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleYR" >
						<xsl:value-of select="substring(SettlementDate,7,4)"/>
					</xsl:variable>
					<SettleDate>
						<xsl:value-of select="concat($varSettleYR,'',$varSettleMth,'',$varSettleDay)"/>
					</SettleDate>

					<!--   Side     -->

					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<ActionCode>
								<xsl:value-of select="'BUY'"/>
							</ActionCode>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<ActionCode>
								<xsl:value-of select="'BC'"/>
							</ActionCode>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<ActionCode>
								<xsl:value-of select="'SELL'"/>
							</ActionCode>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<ActionCode>
								<xsl:value-of select="'SS'"/>
							</ActionCode>
						</xsl:when>
						<xsl:otherwise>
							<ActionCode>
								<xsl:value-of select="''"/>
							</ActionCode>
						</xsl:otherwise>
					</xsl:choose>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<NetAmmount>
						<xsl:value-of select="GrossAmount"/>
					</NetAmmount>

					<SettleCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettleCurrency>

					<SecurityID>
						<xsl:value-of select ="Symbol"/>
					</SecurityID>

					<Commission>
						<xsl:value-of select="CommissionCharged + TaxOnCommissions"/>
					</Commission>

					<!--   Side End    -->
					<xsl:choose>
            <xsl:when test ="CounterParty='WEED'">
              <BrokerCode>
                <xsl:value-of select="'WEEE'"/>
              </BrokerCode>
            </xsl:when>
            <xsl:when test ="CounterParty='CUTTONE' or CounterParty='CUTN'">
              <BrokerCode>
                <xsl:value-of select="'CUTE'"/>
              </BrokerCode>
						</xsl:when>
						<xsl:otherwise>
							<ExecBrokerCode>
								<xsl:value-of select="CounterParty"/>
							</ExecBrokerCode>
						</xsl:otherwise>
					</xsl:choose>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
