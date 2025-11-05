<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <PositionMaster>
          <xsl:choose>
            <xsl:when test="COL2 != ''">
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>
              <CUSIP>
                <xsl:value-of select="COL2"/>
              </CUSIP>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>
              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>
            </xsl:otherwise>
          </xsl:choose>

          <PBSymbol>
            <xsl:value-of select="COL2"/>
          </PBSymbol>

          <xsl:choose>
            <xsl:when  test="boolean(number(COL11))">
              <MarkPrice>
                <xsl:value-of select="COL11"/>
              </MarkPrice>
            </xsl:when >
            <xsl:otherwise>
              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>
            </xsl:otherwise>
          </xsl:choose >
          <xsl:choose>
            <xsl:when test="COL37 != 'SR_BUS_DATE'">
              <Date>
                <xsl:value-of select="COL37"/>
              </Date>
            </xsl:when>
            <xsl:otherwise>
              <Date>
                <xsl:value-of select="''"/>
              </Date>
            </xsl:otherwise>
          </xsl:choose>
        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>


