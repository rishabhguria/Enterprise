<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--<IsCaptionChangeRequired>
								<xsl:value-of select ="'false'"/>
							</IsCaptionChangeRequired>-->

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<OriginalTransactionType>
						<xsl:value-of select="'BS'"/>
					</OriginalTransactionType>

					<Account>
						<xsl:value-of select="AccountNo"/>
					</Account>

					<Trade_Reference>
						<xsl:value-of select="EntityID"/>
					</Trade_Reference>

					<TradeDate>
						<xsl:value-of select="concat(substring(TradeDate,7,4), substring(TradeDate,1,2), substring(TradeDate,4,2))"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="concat(substring(SettlementDate,7,4), substring(SettlementDate,1,2), substring(SettlementDate,4,2))"/>
					</SettlementDate>

					<ActionCode>
						<xsl:choose>
							<xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side = 'Sell' or Side = 'Sell to Close'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side = 'Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ActionCode>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<NetAmount>
						<xsl:value-of select="NetAmount"/>
					</NetAmount>

					<SettlementCcy>
						<xsl:value-of select="CurrencySymbol"/>
					</SettlementCcy>

					<SecurityID>
						<xsl:choose>
							<xsl:when test ="Asset = 'EquityOption'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SEDOL"/>

							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<ExecBrokerCode>
						<xsl:value-of select="CounterParty"/>
					</ExecBrokerCode>

					<Commission>
						<xsl:value-of select="CommissionCharged"/>
					</Commission>

					<BuyCurrency>
						<xsl:value-of select="''"/>
					</BuyCurrency>

					<SellCurrency>
						<xsl:value-of select="''"/>
					</SellCurrency>

					<BuyAmount>
						<xsl:value-of select="''"/>
					</BuyAmount>

					<SellAmount>
						<xsl:value-of select="''"/>
					</SellAmount>

					<Rate>
						<xsl:value-of select="''"/>
					</Rate>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
