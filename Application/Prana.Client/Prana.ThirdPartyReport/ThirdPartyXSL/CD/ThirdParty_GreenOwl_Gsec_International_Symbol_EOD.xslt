<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

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
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<ACCOUNT>
					<xsl:value-of select="'Account'"/>
				</ACCOUNT>

				<SIDE>
					<xsl:value-of select="'Side'"/>
				</SIDE>

				<QUANTITY>
					<xsl:value-of select="'Quantity'"/>
				</QUANTITY>

				<SYMBOL>
					<xsl:value-of select="'Symbol'"/>
				</SYMBOL>

				<PRICE>
					<xsl:value-of select="'Price'"/>
				</PRICE>

				<DONEAWAYCOMM>
					<xsl:value-of select="'DONEAWAYCOMM'"/>
				</DONEAWAYCOMM>

				<GSBROKER>
					<xsl:value-of select="'GSBROKER'"/>
				</GSBROKER>

				<YEAR>
					<xsl:value-of select="'YEAR'"/>
				</YEAR>

				<MONTH>
					<xsl:value-of select="'MONTH'"/>
				</MONTH>

				<DAY>
					<xsl:value-of select="'DAY'"/>
				</DAY>

				<SYEAR>
					<xsl:value-of select="'SYEAR'"/>
				</SYEAR>

				<SMONTH>
					<xsl:value-of select="'SMONTH'"/>
				</SMONTH>

				<SDAY>
					<xsl:value-of select="'SDAY'"/>
				</SDAY>

				<MEMO>
					<xsl:value-of select="'MEMO'"/>
				</MEMO>

				<CURRENCY>
					<xsl:value-of select="'CURRENCY'"/>
				</CURRENCY>

				<!--<GLOBALSYMPREFIX>
					<xsl:value-of select="'GLOBALSYMPREFIX'"/>
				</GLOBALSYMPREFIX>

				<GLOBALEXCH>
					<xsl:value-of select="'GLOBALEXCH'"/>
				</GLOBALEXCH>-->


				<!-- system use only-->

				<FromDeleted>
					<xsl:value-of select ="'FromDeleted'"/>
				</FromDeleted>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="Asset='Equity'">
					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<!--for system internal use-->
						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<ACCOUNT>
							<xsl:value-of select="AccountNo"/>
						</ACCOUNT>

						<xsl:variable name="varSide">
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="Side='Buy to Close'">
									<xsl:value-of select="'BC'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side='Sell to Open'">
									<xsl:value-of select="'SS'"/>
								</xsl:when>
								<xsl:when test="Side='Sell to Close' or Side='Sell'">
									<xsl:value-of select="'S'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short exempt'">
									<xsl:value-of select="'SSE'"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<SIDE>
							<xsl:value-of select="$varSide"/>
						</SIDE>

						<QUANTITY>
							<xsl:value-of select="AllocatedQty"/>
						</QUANTITY>

						<SYMBOL>
							<xsl:choose>
								<xsl:when test="SEDOL!=''">
									<xsl:value-of select="concat(SEDOL,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SYMBOL>

						<PRICE>
							<xsl:value-of select="AveragePrice"/>
						</PRICE>

						<DONEAWAYCOMM>
							<xsl:value-of select="CommissionCharged"/>
						</DONEAWAYCOMM>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GSEC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_BROKER_NAME" >
							<xsl:value-of select ="CounterParty"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<GSBROKER>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="concat('034',$PRANA_SYMBOL_NAME)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</GSBROKER>

						<YEAR>
							<xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
						</YEAR>

						<MONTH>
							<xsl:value-of select="substring-before(TradeDate,'/')"/>
						</MONTH>

						<DAY>
							<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
						</DAY>

						<SYEAR>
							<xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
						</SYEAR>

						<SMONTH>
							<xsl:value-of select="substring-before(SettlementDate,'/')"/>
						</SMONTH>

						<SDAY>
							<xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
						</SDAY>

						<MEMO>
							<xsl:value-of select="'C@N@'"/>
						</MEMO>

						<CURRENCY>
							<xsl:value-of select="CurrencySymbol"/>
						</CURRENCY>

						<!--<GLOBALSYMPREFIX>
							<xsl:value-of select="'R'"/>
						</GLOBALSYMPREFIX>

						<GLOBALEXCH>
							<xsl:value-of select="'C'"/>
						</GLOBALEXCH>-->

						<!-- system use only-->
						<FromDeleted>
							<xsl:value-of select ="FromDeleted"/>
						</FromDeleted>

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
