<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL5)"/>
        </xsl:variable>
        <xsl:if test ="$varInstrument='0' or $varInstrument='B'">
          <PositionMaster>
            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="COL9"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="COL9"/>
            </PBSymbol>

            <xsl:choose>
              <xsl:when  test="boolean(number(COL13))">
                <MarkPrice>
                  <xsl:value-of select="COL13"/>
                </MarkPrice>
              </xsl:when >
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose >

            <xsl:choose>
              <xsl:when test ="COL1='Trade Date' or COL1='*'">
                <Date>
                  <xsl:value-of select="''"/>
                </Date>
              </xsl:when>
              <xsl:otherwise>
                <Date>
                  <xsl:value-of select="COL3"/>
                </Date>
              </xsl:otherwise>
            </xsl:choose>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
