<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">				
					<PositionMaster>
						
						<BaseCurrency>
							<xsl:value-of select ="'USD'"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select="'USD'"/>
						</SettlementCurrency>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL9))">
								<ForexPrice>
									<xsl:value-of select="COL9"/>
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:variable name ="varDateString" select="normalize-space(COL16)"/>
						<xsl:variable name ="varDate">
							<xsl:value-of select ="concat(substring($varDateString,1,4),'/',substring($varDateString,5,2),'/',substring($varDateString,7,2))"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varDate !=''">
								<PositionStartDate>
									<xsl:value-of select="$varDate"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="'rr'"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
					<!--</xsl:if>-->
				</xsl:for-each>
			</DocumentElement>
		</xsl:template>
	</xsl:stylesheet>


	