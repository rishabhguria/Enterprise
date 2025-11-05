<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL4"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Cash)">
						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'NT'"/>
							</xsl:variable>

							<xsl:variable name = "PB_SYMBOL_NAME" >
								<xsl:value-of select="'COL2'"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name="PB_FUND_NAME" select="'GMF_CITCO'"/>
							<xsl:variable name ="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<PortfolioAccount>
								<xsl:choose>

									<xsl:when test ="$PRANA_FUND_NAME!=''">
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="$PB_FUND_NAME"/>
									</xsl:otherwise>

								</xsl:choose>
							</PortfolioAccount>

							<xsl:variable name="varCurrency">
								<xsl:choose>
									<xsl:when test ="normalize-space(COL2)='U.S. Dollars'">
										<xsl:value-of select ="'USD'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Australian Dollar'">
										<xsl:value-of select ="'AUD'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Canadian Dollar'">
										<xsl:value-of select ="'CAD'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='New Israeli Sheqel'">
										<xsl:value-of select ="'ILS'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Swiss Franc'">
										<xsl:value-of select ="'CHF'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Japanese Yen'">
										<xsl:value-of select ="'JPY'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Brazilian Real'">
										<xsl:value-of select ="'BRL'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Danish Krone'">
										<xsl:value-of select ="'DKK'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Norwegian Krone'">
										<xsl:value-of select ="'NOK'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Euro'">
										<xsl:value-of select ="'EUR'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Hong Kong Dollar'">
										<xsl:value-of select ="'HKD'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Mexican Peso'">
										<xsl:value-of select ="'MXN'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Pound Sterling'">
										<xsl:value-of select ="'GBP'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Swedish Krona'">
										<xsl:value-of select ="'SEK'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Rand'">
										<xsl:value-of select ="'ZAR'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<Currency>
								<xsl:value-of select ="$varCurrency"/>
							</Currency>

						

							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="number($Cash)">
										<xsl:value-of select="$Cash"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceDR>

							<xsl:variable name="Cash1">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL8"/>
								</xsl:call-template>
							</xsl:variable>

							<OpeningBalanceCR>
								<xsl:choose>
									<xsl:when test="number($Cash1)">
										<xsl:value-of select="$Cash1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceCR>



							<xsl:variable name="Year">
								<xsl:value-of select="substring(COL4,1,2)"/>
							</xsl:variable>
							<xsl:variable name="Month">
								<xsl:value-of select="substring(COL4,3,2)"/>
							</xsl:variable>
							<xsl:variable name="Day">
								<xsl:value-of select="substring(COL4,5,2)"/>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

						
					</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<PortfolioAccount>
								<xsl:value-of select="''"/>
							</PortfolioAccount>


							<Currency>
								<xsl:value-of select="''"/>
							</Currency>


							<OpeningBalanceDR>
								<xsl:value-of select="0"/>
							</OpeningBalanceDR>

							<OpeningBalanceCR>
								<xsl:value-of select="0"/>
							</OpeningBalanceCR>



							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>