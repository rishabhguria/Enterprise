<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:if test ="normalize-space(COL3)='EQY - LST COMMON'">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <TickerSymbol>
              <xsl:value-of select="''"/>
            </TickerSymbol>

            <Multiplier>
              <xsl:value-of select="'1'"/>
            </Multiplier>

            <UnderLyingSymbol>
              <xsl:value-of select="''"/>
            </UnderLyingSymbol>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            
            <CusipSymbol>
              <xsl:value-of select="$varCUSIP"/>
            </CusipSymbol>
            
            <AUECID>
              <xsl:value-of select="1"/>
            </AUECID>

            <LongName>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </LongName>


          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

