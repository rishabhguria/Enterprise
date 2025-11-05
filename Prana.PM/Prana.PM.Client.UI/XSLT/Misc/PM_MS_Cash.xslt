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

				<!--   Column48 gives the value of Position Type i.e. Currency or Cash -->
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL48, ' ' , ''),'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType='Currency' or $varInstrumentType='Cash'">
					
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='038C35642'">
								<FundName>
									<xsl:value-of select="'LP C/O'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='038C35741'">
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
							<xsl:value-of select="translate(COL19,'&quot;','')"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="translate(COL12,'&quot;','')"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when test ="COL7 &lt; 0 or COL7 &gt; 0 or COL7 = 0">
								<CashValueBase>
									<xsl:value-of select="COL7"/>
								</CashValueBase>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:choose>
							<xsl:when test ="COL10 &lt; 0 or COL10 &gt; 0 or COL10 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL10"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >
						<!-- Position Date mapped with the column 6 -->
						<Date>
							<xsl:value-of select="translate(COL51,'&quot;','')"/>
						</Date>
						<PositionType>
							<xsl:value-of select="translate(COL48,'&quot;','')"/>
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
