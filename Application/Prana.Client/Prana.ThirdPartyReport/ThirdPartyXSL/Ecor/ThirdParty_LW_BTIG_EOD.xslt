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

			<xsl:for-each select="ThirdPartyFlatFileDetail[not(contains(Symbol,'-')) or CounterParty != 'BTIG' or contains(Asset,'FX')]">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<xsl:variable name="PB_NAME" select="'SSC'"/>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountMappedName"/>

					<xsl:variable name="THIRDPARTY_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<Portfolio>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
								<xsl:value-of select="concat('TAO',$THIRDPARTY_FUND_NAME)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat('TAO',$PRANA_FUND_NAME)"/>
							</xsl:otherwise>
						</xsl:choose>
					</Portfolio>

					<Side>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'Short'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<xsl:value-of select="'Cover'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<Investment>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption' and CurrencySymbol='USD'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Investment>

					<Cusip>
						<xsl:value-of select="CUSIP"/>
					</Cusip>

					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>

					<EventDate>
						<xsl:value-of select="TradeDate"/>
					</EventDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<ClosingMethod>
						<xsl:value-of select="''"/>
					</ClosingMethod>

					<TradeFX>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:when test ="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<!--<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>-->
							<xsl:otherwise>
								<xsl:value-of select="''"/>
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

					<Commission>
						<xsl:value-of select="CommissionCharged"/>
					</Commission>

					<CommissionType>
						<xsl:value-of select="'absolute'"/>
					</CommissionType>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="PB_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>
					
					<Broker>
						<xsl:choose>
							<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Broker>

					<OptionsMultiplier>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="AssetMultiplier"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</OptionsMultiplier>

					<PutCall>
						<xsl:choose>
							<xsl:when test="PutOrCall='CALL'">
								<xsl:value-of select="'Call'"/>
							</xsl:when>
							<xsl:when test="PutOrCall='PUT'">
								<xsl:value-of select="'Put'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PutCall>

					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExpirationDate>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="number(StrikePrice)">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<Underlyer>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="UnderlyingSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Underlyer>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>