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

			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset='Equity' and CurrencySymbol!='USD']">

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
					<ACCOUNT>
						<xsl:value-of select="AccountNo"/>

					</ACCOUNT>


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

					<QUANTITY>
						<xsl:value-of select="AllocatedQty"/>
					</QUANTITY>
					
					<SYMBOL>
						<xsl:value-of select="concat(SEDOL,'.')"/>
					</SYMBOL>


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

				

					<xsl:variable name="Commission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="varFXRate">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<DONEAWAYCOMM>
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="format-number($Commission,'##.0000')"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
										<xsl:value-of select="format-number($Commission * $varFXRate,'##.0000')"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
										<xsl:value-of select="format-number($Commission div $varFXRate,'##.0000')"/>
									</xsl:when>
								</xsl:choose>

							</xsl:otherwise>
						</xsl:choose>
					</DONEAWAYCOMM>



					<GSBROKER>

						<xsl:value-of select="'039153440'"/>

					</GSBROKER>

					<YEAR>
						<xsl:value-of select="substring(TradeDate, string-length(TradeDate) -1)"/>
					</YEAR>

					<MONTH>
						<xsl:value-of select="substring-before(TradeDate,'/')"/>
					</MONTH>

					<DAY>
						<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
					</DAY>

					<SYEAR>
						<xsl:value-of select="substring(SettlementDate, string-length(SettlementDate) -1)"/>
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
						<xsl:choose>
							<xsl:when test="SettlCurrency='USD'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='GBP'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='JPY'">
								<xsl:value-of select="'3'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='MXN'">
								<xsl:value-of select="'4'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='CHF'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='EUR'">
								<xsl:value-of select="'6'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='CAD'">
								<xsl:value-of select="'7'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='NZD'">
								<xsl:value-of select="'8'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='ZAR'">
								<xsl:value-of select="'9'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='AUD'">
								<xsl:value-of select="'10'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='HKD'">
								<xsl:value-of select="'11'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='DKK'">
								<xsl:value-of select="'12'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='CZK'">
								<xsl:value-of select="'13'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='HUF'">
								<xsl:value-of select="'14'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='NOK'">
								<xsl:value-of select="'15'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='PLN'">
								<xsl:value-of select="'16'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='SEK'">
								<xsl:value-of select="'17'"/>
							</xsl:when>
							<xsl:when test="SettlCurrency='SGD'">
								<xsl:value-of select="'18'"/>
							</xsl:when>
						

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="SettlCurrency=''">
										<xsl:value-of select="''"/>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>

						</xsl:choose>
					</CURRENCY>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
