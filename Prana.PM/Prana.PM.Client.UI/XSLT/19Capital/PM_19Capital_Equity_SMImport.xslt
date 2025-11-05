<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varAsset">
          <xsl:value-of select="normalize-space(COL5)"/>
        </xsl:variable>

		<xsl:variable name="varQty">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>
		
        <xsl:if test="not(contains(COL21,'PUT')) and not(contains(COL21,'CALL')) and number($varQty)">

          <PositionMaster>

            <xsl:variable name="varTicker">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL21)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <TickerSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varTicker"/>
                </xsl:otherwise>
              </xsl:choose>
            </TickerSymbol>

            <UnderLyingSymbol>
              <xsl:value-of select="$varTicker"/>
            </UnderLyingSymbol>

            <LongName>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </LongName>

            <Multiplier>
              <xsl:value-of select="1"/>
            </Multiplier>

            <AUECID>
              <xsl:value-of select="'1'"/>
            </AUECID>

            <UnderLyingID>
              <xsl:value-of select="1"/>
            </UnderLyingID>

            <ExchangeID>
              <xsl:value-of select="21"/>
            </ExchangeID>

            <CurrencyID>
              <xsl:value-of select="1"/>
            </CurrencyID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>