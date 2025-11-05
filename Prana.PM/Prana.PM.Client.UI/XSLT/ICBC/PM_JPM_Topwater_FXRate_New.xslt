<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <xsl:if test="normalize-space(COL1) != 'Family' and number(COL32) ">
          <PositionMaster>

            <BaseCurrency>
              <xsl:value-of select ="COL3"/>
            </BaseCurrency>

            <SettlementCurrency>
              <xsl:value-of select="'USD'"/>
            </SettlementCurrency>

            <xsl:choose>
              <xsl:when test ="number(COL32)">
                <ForexPrice>
                  <xsl:value-of select="COL32"/>
                </ForexPrice>
              </xsl:when >
              <xsl:otherwise>
                <ForexPrice>
                  <xsl:value-of select="0"/>
                </ForexPrice>
              </xsl:otherwise>
            </xsl:choose >
            <Date>
              <xsl:value-of select="COL11"/>
            </Date>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
