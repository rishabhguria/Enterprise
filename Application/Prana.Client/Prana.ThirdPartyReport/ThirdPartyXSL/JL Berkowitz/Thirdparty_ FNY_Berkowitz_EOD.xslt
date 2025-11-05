<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<OMSCode>
						<xsl:choose>
							<xsl:when test="Asset='Equity' or Asset='EquityOption'">
								<xsl:value-of select="'COB-M-T'"/>
							</xsl:when>
							<xsl:when test="Asset='Future' or Asset='FutureOption'">
								<xsl:value-of select="'COB-P-FR'"/>
							</xsl:when>
						</xsl:choose>
					</OMSCode>

					<Side>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="Side='Buy to Open'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'Cover'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'Cover'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>

						</xsl:choose>
					</Side>

					<Symbol>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
						</xsl:choose>
					</Symbol>

					<Sedol>
						<xsl:value-of select="Sedol"/>
					</Sedol>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<Quantity>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<TradeFX>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeFX>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<TradeCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</TradeCurrency>

					<SettleCurrency>
						<xsl:value-of select="SettlCurrency"/>
					</SettleCurrency>

					<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<Commission>
						<xsl:value-of select="$COMM"/>
					</Commission>

					<xsl:variable name="PB_NAME" select="Berkowitz"/>
					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Broker>
						<xsl:value-of select="$Broker"/>
					</Broker>

					<OptionsMultiplier>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="AssetMultiplier"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</OptionsMultiplier>

					<PutCall>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="PutOrCall"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PutCall>

					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExpirationDate>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<Underlyer>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="UnderlyingSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Underlyer>


					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
