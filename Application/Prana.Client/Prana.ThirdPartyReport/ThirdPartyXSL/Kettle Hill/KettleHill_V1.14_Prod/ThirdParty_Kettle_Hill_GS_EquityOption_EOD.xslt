<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>
					<xsl:variable name="PB_NAME" select="'GS'"/>

					<xsl:variable name = "PRANA_FUND_NAME1">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PranaFund=$PRANA_FUND_NAME1]/@PBFundCode"/>
					</xsl:variable>
					<SIDE>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SIDE>





					<SYMBOL>
						<xsl:value-of select="UnderlyingSymbol"/>
					</SYMBOL>

					<QUANTITY>
						<xsl:value-of select="AllocatedQty"/>
					</QUANTITY>


					<xsl:variable name="varSettFxAmt">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AveragePrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PRICE>
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varSettFxAmt,'####.0000')"/>
							</xsl:otherwise>

						</xsl:choose>
					</PRICE>

					<EXCHOUR>
						<xsl:value-of select="''"/>
					</EXCHOUR>

					<EXCMIN>
						<xsl:value-of select="''"/>
					</EXCMIN>

					<EXCSEC>
						<xsl:value-of select="''"/>
					</EXCSEC>




					<ACCOUNT>
						<xsl:value-of select="AccountNo"/>

					</ACCOUNT>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>



					<xsl:variable name="THIRDPARTY_BROKER">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name='GS']/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@DTCCode"/>
					</xsl:variable>

					<BROKER>

						<xsl:value-of select="$THIRDPARTY_BROKER"/>

					</BROKER>

					<EXPYEAR>
						<xsl:value-of select="substring(ExpirationDate, string-length(ExpirationDate) -1)"/>
					</EXPYEAR>

					<EXPMONTH>
						<xsl:value-of select="substring-before(ExpirationDate,'/')"/>
					</EXPMONTH>

					<EXPDAY>
						<xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>
					</EXPDAY>

					<PUTCALL>
						<xsl:choose>
						<xsl:when test="PutOrCall='PUT'">
							<xsl:value-of select="'P'"/>
						</xsl:when>
						<xsl:when test="PutOrCall='CALL'">
							<xsl:value-of select="'C'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
						</xsl:choose>
					</PUTCALL>
					
					<STRIKEPRICE>
						<xsl:value-of select="StrikePrice"/>
					</STRIKEPRICE>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
