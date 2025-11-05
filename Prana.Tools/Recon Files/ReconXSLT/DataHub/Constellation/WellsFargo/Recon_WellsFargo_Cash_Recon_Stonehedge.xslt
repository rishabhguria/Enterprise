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
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:choose>
				<xsl:when test="number($Cash) and contains(COL1,'Cash')='true' ">
					<PositionMaster>

						<xsl:variable name ="varPBName">
							<xsl:value-of select ="'WellsFargo'"/>
						</xsl:variable>
						<xsl:variable name="PB_FUND_NAME" select="'Stonehedge-WF'"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="COL3='GVIXX' ">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								
								<xsl:when test="COL3='JPY_CUR'">
									<xsl:value-of select="'JPY'"/>
								</xsl:when>

								<xsl:when test="COL3='USD'">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL3,'_')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Currency>
							
						<xsl:choose>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
							</xsl:choose>
						</Currency>

						<!--<xsl:variable name="CashvalueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>
						<CashValueBase>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueBase>-->
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
						<!--<Currency>
							<xsl:value-of select="substring-before(COL3,3)"/>
						</Currency>-->
						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>
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

							<SMRequest>
								<xsl:value-of select="''"/>
							</SMRequest>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>