<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<BuySell>
					<xsl:value-of select="'B/S'"/>
				</BuySell>

				<TickerSymbol>
					<xsl:value-of select="'Ticker Symbol'"/>
				</TickerSymbol>

				<SecurityDesc>
					<xsl:value-of select="'Security Description Name'"/>
				</SecurityDesc>

				<TradeQuantity>
					<xsl:value-of select="'Trade Quantity'"/>
				</TradeQuantity>

				<NetAmountLocal>
					<xsl:value-of select="'Net Amount (Local)'"/>
				</NetAmountLocal>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>


				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>
				
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
		
					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<!--for system internal use-->
						<BuySell>
							<xsl:choose>
								<xsl:when test ="Side='Sell short' or Side='Sell to Open'">
									<xsl:value-of select ="'SS'"/>
								</xsl:when>
								<xsl:when test ="Side='Buy to Cover' or Side='Buy to Close'">
									<xsl:value-of select ="'CB'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(Side,1,1)"/>		
								</xsl:otherwise>
							</xsl:choose>							
						</BuySell>

						<TickerSymbol>
							<xsl:value-of select="Symbol"/>
						</TickerSymbol>

						<SecurityDesc>
							<xsl:value-of select="FullSecurityName"/>
						</SecurityDesc>

						<TradeQuantity>
							<xsl:value-of select="AllocatedQty"/>
						</TradeQuantity>

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

						<NetAmountLocal>
							<xsl:value-of select="$NetAmount"/>
						</NetAmountLocal>

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
							<xsl:value-of select="$Price"/>
						</Price>

						<xsl:variable name="PB_NAME" select="'MapleRock'"/>

						<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

						<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<Broker>
							<xsl:choose>
								<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
									<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Broker>

						<Currency>
							<xsl:value-of select="SettlCurrency"/>
						</Currency>
						

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
			
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>