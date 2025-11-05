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


	<xsl:template name="Currency">
		<xsl:param name="varCurrency"/>
		<xsl:choose>
			<xsl:when test="$varCurrency='CANADIAN DOLLAR'">
				<xsl:value-of select="'CAD'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='NORWEGIAN KRONE'">
				<xsl:value-of select="'NOK'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='SWEDISH KRONA'">
				<xsl:value-of select="'SEK'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='U S DOLLAR'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
			<xsl:when test="$varCurrency='UK POUND STERLING'">
				<xsl:value-of select="'GBP'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Cash)">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'GS'"/>
							</xsl:variable>

							<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

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
								<xsl:call-template name="Currency">
									<xsl:with-param name="varCurrency" select="COL4"/>
								</xsl:call-template>
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

							<xsl:variable name="OpeningBalanceCR">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<OpeningBalanceCR>
								<xsl:choose>
									<xsl:when test="number($OpeningBalanceCR)">
										<xsl:value-of select="$OpeningBalanceCR"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceCR>

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