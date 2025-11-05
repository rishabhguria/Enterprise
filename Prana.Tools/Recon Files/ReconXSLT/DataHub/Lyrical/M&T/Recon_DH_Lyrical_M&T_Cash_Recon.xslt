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
			<xsl:variable name ="PB_NAME">
				<xsl:value-of select ="'M and T'"/>
			</xsl:variable>
			<xsl:variable name = "PB_FUND_NAME" >
				<xsl:value-of select="normalize-space(substring(COL1,2,11))"/>
			</xsl:variable>
			
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>


				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$PRANA_FUND_NAME!='' and  normalize-space(substring(COL1,221,11))='WGOXX' or normalize-space(substring(COL1,221,11))='TOIXX'">
						<PositionMaster>
							
							
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
								<xsl:value-of select="'USD'"/>
							</xsl:variable>
							<Currency>
								<xsl:value-of select ="$varCurrency"/>
							</Currency>

							<xsl:variable name="varCashValueLocal">
								<xsl:choose>
									<xsl:when test="contains(normalize-space(substring(COL1,2,11)),'31048172')">
										<xsl:value-of select="normalize-space(substring(COL1,105,7)) + normalize-space(substring(COL1,127,16))"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="number(substring(COL1,127,16))"/>
									</xsl:otherwise>
								</xsl:choose>
								<!--<xsl:value-of select="normalize-space(substring(COL1,89,9)) + normalize-space(substring(COL1,105,7)) + normalize-space(substring(COL1,127,16))"/>-->
							</xsl:variable>
							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="number($varCashValueLocal)">
										<xsl:value-of select="$varCashValueLocal"/>
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
								<xsl:value-of select ="concat(substring(COL1,663,2),'/',substring(COL1,665,2),'/',substring(COL1,659,4))"/>
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