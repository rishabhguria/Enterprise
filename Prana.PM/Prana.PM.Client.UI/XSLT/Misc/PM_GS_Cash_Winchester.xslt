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
				<xsl:variable name = "varPortfolioID" >
					<xsl:value-of select="translate(COL1,'&quot;','')"/>
				</xsl:variable>
				<PositionMaster>
					<!--   Fund -->
					<xsl:choose>
						<xsl:when test="$varPortfolioID='Lucas Energy Total Return Master Fund, LP'">
							<FundName>
								<xsl:value-of select="'Letrol.002-36771-2'"/>
							</FundName>
						</xsl:when>
						<xsl:when test="$varPortfolioID='Lucas Energy Total Return Offshore, Ltd.'">
							<FundName>
								<xsl:value-of select="'Letrol.002-36771-2'"/>
							</FundName>
						</xsl:when>
						<xsl:otherwise>
							<FundName>
								<xsl:value-of select="' '"/>
							</FundName>
						</xsl:otherwise >
					</xsl:choose >

					<BaseCurrency>
						<xsl:value-of select="translate(COL6,'&quot;','')"/>
					</BaseCurrency>
					<LocalCurrency>
						<xsl:value-of select="translate(COL3,'&quot;','')"/>
					</LocalCurrency>
					<xsl:choose>
						<xsl:when test ="COL2 ='Cumulative Cash Value - Local '">
							<CashValueLocal>
								<xsl:value-of select="0"/>
							</CashValueLocal>
						</xsl:when>
						<xsl:otherwise>
							<CashValueLocal>
								<xsl:value-of select="COL2"/>
							</CashValueLocal>
						</xsl:otherwise >
					</xsl:choose >
					<xsl:choose>
						<xsl:when test ="COL5 ='Cumulative Cash Value - Base'">
							<CashValueBase>
								<xsl:value-of select="0"/>
							</CashValueBase>
						</xsl:when>
						<xsl:otherwise>
							<CashValueBase>
								<xsl:value-of select="COL5"/>
							</CashValueBase>
						</xsl:otherwise >
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="COL4 ='Date'">
							<Date>
								<xsl:value-of select="''"/>
							</Date>
						</xsl:when>
						<xsl:otherwise>
							<Date>
								<xsl:value-of select="translate(COL4,'&quot;','')"/>
							</Date>
						</xsl:otherwise >
					</xsl:choose >
					<PositionType>
						<xsl:value-of select="'Cash'"/>
					</PositionType>
				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
