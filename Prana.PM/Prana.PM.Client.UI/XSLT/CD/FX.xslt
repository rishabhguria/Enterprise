<?xml version="1.0" encoding="utf-8" ?>


<!--Description: Seafearer , Alps
    Date       : 02-03-2012-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="(COL1 != 'FUNDID')">
				<PositionMaster>
						
						<BaseCurrency>
							<xsl:value-of select ="'USD'"/>
						</BaseCurrency>

						<SettlementCurrency>
              <xsl:if test="COL4 != ''">
                <xsl:value-of select="COL4"/>
              </xsl:if>
						</SettlementCurrency>

          <ForexPrice>
            <xsl:choose>
              <xsl:when test ="number(COL3)">
                <xsl:value-of select="COL3"/>
							</xsl:when >
							<xsl:otherwise>
									<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose >
          </ForexPrice>

          <Date>
            <xsl:value-of select="COL2"/>
          </Date>

        </PositionMaster>
					</xsl:if>
				</xsl:for-each>
			</DocumentElement>
		</xsl:template>
	</xsl:stylesheet>


	