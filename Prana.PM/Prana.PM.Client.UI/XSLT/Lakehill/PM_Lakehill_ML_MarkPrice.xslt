<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="left-trim">
    <xsl:param name="s" />
    <xsl:choose>
      <xsl:when test="substring($s, 1, 1) = ''">
        <xsl:value-of select="$s"/>
      </xsl:when>
      <xsl:when test="normalize-space(substring($s, 1, 1)) = ''">
        <xsl:call-template name="left-trim">
          <xsl:with-param name="s" select="substring($s, 2)" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$s" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="right-trim">
    <xsl:param name="s" />
    <xsl:choose>
      <xsl:when test="substring($s, 1, 1) = ''">
        <xsl:value-of select="$s"/>
      </xsl:when>
      <xsl:when test="normalize-space(substring($s, string-length($s))) = ''">
        <xsl:call-template name="right-trim">
          <xsl:with-param name="s" select="substring($s, 1, string-length($s) - 1)" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$s" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="trim">
    <xsl:param name="s" />
    <xsl:call-template name="right-trim">
      <xsl:with-param name="s">
        <xsl:call-template name="left-trim">
          <xsl:with-param name="s" select="$s" />
        </xsl:call-template>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL5)"/>
        </xsl:variable>
        <xsl:if test ="$varInstrument='0' or $varInstrument='1' or $varInstrument='B' or $varInstrument='J'or $varInstrument='BL'">
          <PositionMaster>
            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_TRIM" >
              <xsl:call-template name="trim">
                <xsl:with-param name="s" select="translate(COL20,'&quot;','')" />
              </xsl:call-template>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrument = 'B' or $varInstrument = 'BL' or $varInstrument = 'J'">
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="concat($PB_SYMBOL_TRIM,'U')"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrument='0' or $varInstrument = '1'">
                <Symbol>
                  <xsl:value-of select="$PB_SYMBOL_TRIM"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="$PB_SYMBOL_TRIM"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:otherwise>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_TRIM"/>
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
