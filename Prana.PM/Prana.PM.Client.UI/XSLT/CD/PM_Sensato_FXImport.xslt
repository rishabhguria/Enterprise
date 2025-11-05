<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <xsl:value-of select="translate(doc, $smallcase, $uppercase)" />
  </xsl:template>
  <xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'" />
  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

          <PositionMaster>

            <BaseCurrency>
              <xsl:value-of select ="translate(substring(COL1,4,3),$smallcase, $uppercase)"/>
            </BaseCurrency>

            <SettlementCurrency>
              <xsl:value-of select="translate(substring(COL1,1,3),$smallcase, $uppercase)"/>
            </SettlementCurrency>

            <xsl:choose>
              <xsl:when test ="number(COL2)">
                <ForexPrice>
                  <xsl:value-of select="1 div COL2"/>
                </ForexPrice>
              </xsl:when >
              <xsl:otherwise>
                <ForexPrice>
                  <xsl:value-of select="0"/>
                </ForexPrice>
              </xsl:otherwise>
            </xsl:choose >
            <Date>
              <xsl:value-of select="''"/>
            </Date>
          </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
