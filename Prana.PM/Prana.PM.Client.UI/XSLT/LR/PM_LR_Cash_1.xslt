<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">			
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL4,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType='JPMORGAN LIQUIDITY-PREMIERE (45BPS)' and COL8 &gt; 0">					
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='P 86500'">
								<FundName>
									<xsl:value-of select="'LP C/O'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='P 86501'">
								<FundName>
									<xsl:value-of select="'OFFSHORE'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="' '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="COL27 &lt; 0 or COL27 &gt; 0 or COL27 = 0">
								<CashValueBase>
									<xsl:value-of select="COL27"/>
								</CashValueBase>
								<CashValueLocal>
									<xsl:value-of select="COL27"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >

						<!-- Position Date mapped with the column 10 -->
						<Date>
							<xsl:value-of select="translate(COL10,'&quot;','')"/>
						</Date>
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
