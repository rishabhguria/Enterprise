<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL2,'&quot;',''),' ','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType = 'Cash'">
					<PositionMaster>

						<xsl:variable name = "varPortfolioID">
							<xsl:value-of select="translate(COL1,' ','')"/>
						</xsl:variable>
						<!--   Fund -->
						<xsl:choose>
							<xsl:when test="$varPortfolioID='SecondCurveVisionFundLP'">
								<FundName>
									<xsl:value-of select="'SC VISION FUND LP'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='SecondCurvePartnersIILP'">
								<FundName>
									<xsl:value-of select="'SC PARTNERS II LP'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='SecondCurvePartnersIntlLtd'">
								<FundName>
									<xsl:value-of select="'SC PARTNERS INTL LTD'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='SecondCurvePartnersLP'">
								<FundName>
									<xsl:value-of select="'SC PARTNERS LP'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='SecondCurveVisionFundInter'">
								<FundName>
									<xsl:value-of select="'SC VISION FUND INTL'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='SecondCurveOpp.FdIntlLtd'">
								<FundName>
									<xsl:value-of select="'SC INTERNATIONAL FUND'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='SecondCurveOpp.Fd'">
								<FundName>
									<xsl:value-of select="'SC OPPORTUNITY FUND'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >

						<BaseCurrency>
							<xsl:value-of select="translate(COL22,' ','')"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="translate(COL21,' ','')"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="COL16 &lt; 0 or COL16 &gt; 0 or COL16 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL16"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:choose>
							<xsl:when test ="COL17 &lt; 0 or COL17 &gt; 0 or COL17 = 0">
								<CashValueBase>
									<xsl:value-of select="COL17"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="''"/>
						</Date>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
