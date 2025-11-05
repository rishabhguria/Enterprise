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
				<!--<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL4,'&quot;','')"/>
				</xsl:variable>-->
				<xsl:variable name = "varCurrLen" >
					<xsl:value-of select="string-length(translate(COL3,'&quot;',''))"/>
				</xsl:variable>
				<xsl:if test=" $varCurrLen=3 and translate(COL3,'&quot;','') != 'USD' and translate(COL5,'&quot;','') = '*'">
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='26088'">
								<FundName>
									<xsl:value-of select="'LR LP'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='30502'">
								<FundName>
									<xsl:value-of select="'LR F LTD'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='P 86500'">
								<FundName>
									<xsl:value-of select="'LR LP'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='P 86501' or $varPortfolioID='P 86502'">
								<FundName>
									<xsl:value-of select="'LR F LTD'"/>
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
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="COL7 &lt; 0 or COL7 &gt; 0 or COL7 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL7"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >
						
						<!-- Position Date mapped with the column 10 -->
						<Date>
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
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
