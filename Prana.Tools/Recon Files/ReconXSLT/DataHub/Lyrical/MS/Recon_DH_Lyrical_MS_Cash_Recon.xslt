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

				<xsl:variable name="Cash" >
					<xsl:choose>
						<xsl:when test="contains(normalize-space(substring(COL1,28,4)),'$$$$')">
							<xsl:value-of select="number(substring(COL1,85,19))"/>
						</xsl:when>
						
						<xsl:otherwise>
							<xsl:value-of select="number(substring(COL1,338,17))"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="varCash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$Cash"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($varCash) and contains(normalize-space(substring(COL1,28,36)),'BANK DEPOSIT PROGRAM')='true' or contains(normalize-space(substring(COL1,28,4)),'$$$$')='true' or contains(normalize-space(substring(COL1,28,28)),'MORGAN STANLEY SICAV US$ LIQ')='true'">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'GS'"/>
							</xsl:variable>

							<xsl:variable name="PB_FUND_NAME" select="substring(COL1,7,9)"/>

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
								<xsl:value-of select ="substring(COL1,189,21)"/>
							</xsl:variable>
							<Currency>
								<xsl:value-of select ="$varCurrency"/>
							</Currency>
							<xsl:variable name="Sign" select="substring(COL1,337,1)"/>

							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="contains(normalize-space(substring(COL1,28,4)),'$$$$')">
										<xsl:choose>
											<xsl:when test="$Sign ='+'">
												<xsl:value-of select="$varCash*(-1)"/>
											</xsl:when>
											<xsl:when test="$Sign ='-'">
												<xsl:value-of select="$varCash"/>
											</xsl:when>

											<xsl:otherwise>
												<xsl:value-of select="0"/>
											</xsl:otherwise>

										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$Sign ='+'">
												<xsl:value-of select="$varCash"/>
											</xsl:when>
											<xsl:when test="$Sign ='-'">
												<xsl:value-of select="$varCash * (-1)"/>
											</xsl:when>
										</xsl:choose>
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

							<xsl:variable name="Year" select="substring(COL1,378,4)"/>
							<xsl:variable name="Month" select="substring(COL1,383,2)"/>
							<xsl:variable name="Day" select="substring(COL1,386,2)"/>

							<xsl:variable name="Date" select="substring(COL1,378,10)"/>

							<TradeDate>

								<xsl:choose>
									<xsl:when test="contains(substring(COL1,378,10),'-')">
										<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$Date"/>
									</xsl:otherwise>
								</xsl:choose>


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