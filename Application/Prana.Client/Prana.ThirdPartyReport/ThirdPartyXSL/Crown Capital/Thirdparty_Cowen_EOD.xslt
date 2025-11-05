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

					<Symbol>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<xsl:variable name="PB_NAME" select="'KH'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					
					<Account>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Account>

					<Side>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'Sell_short'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'Buy_cover'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

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

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<Currency>
						<xsl:value-of select="'USD'"/>
					</Currency>

					<ExecutionTime>
						<xsl:value-of select="''"/>
					</ExecutionTime>

					<Underlying>
						<xsl:value-of select="UnderLying"/>
					</Underlying>

					<Expiration>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'), substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Expiration>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test ="number(StrikePrice)">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<CallPutIndicator>
						<xsl:choose>
							<xsl:when test="PutOrCall='Call'">
								<xsl:value-of select="'Call'"/>
							</xsl:when>
							<xsl:when test="PutOrCall='Put'">
								<xsl:value-of select="'Put'"/>
							</xsl:when>
						</xsl:choose>
					</CallPutIndicator>

					<RootCode>
						<xsl:value-of select="''"/>
					</RootCode>

					<SecurityType>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'Option'"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'Equity'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
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
					
					<ExecBroker>
						<xsl:value-of select="$Broker"/>
					</ExecBroker>

					<BookingCategory>
						<xsl:value-of select="'CPS-Away'"/>
					</BookingCategory>

					<CommissionCentsPerLot>
						<xsl:value-of select="''"/>
					</CommissionCentsPerLot>

					<CommissionFlatFee>
						<xsl:value-of select="''"/>
					</CommissionFlatFee>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>

					<SecuritySubType>
						<xsl:value-of select="''"/>
					</SecuritySubType>

					<LocateID>
						<xsl:value-of select="''"/>
					</LocateID>

					<PositionEffect>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'Open'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Close'"/>
							</xsl:otherwise>
						</xsl:choose>
					</PositionEffect>

					<ClOrdID>
						<xsl:value-of select="CLOrderID"/>
					</ClOrdID>

					<CoveredOrUncovered>
						<xsl:value-of select="''"/>
					</CoveredOrUncovered>

					<LastMarket>
						<xsl:value-of select="''"/>
					</LastMarket>
				

					<xsl:variable name = "PRANA_EXCHANGE_NAME">
						<xsl:value-of select="Exchange"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_EXCHANGE_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE_NAME]/@PBExchangeName"/>
					</xsl:variable>
					
					<Exchange>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_EXCHANGE_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_EXCHANGE_CODE"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Exchange>

					<ExecutionState>
						<xsl:value-of select="''"/>
					</ExecutionState>

					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<SEDOL>
						<xsl:value-of select="SEDOL"/>	
					</SEDOL>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>