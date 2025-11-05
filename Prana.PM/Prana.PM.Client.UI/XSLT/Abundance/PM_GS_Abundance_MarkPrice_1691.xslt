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

        <PositionMaster>
          <!--  Symbol Region -->
          <xsl:variable name = "varInstrumentType" >
            <xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
          </xsl:variable>

          <!--<xsl:variable name="OptionUnderlyingSymbol">
            <xsl:choose>
              <xsl:when test="$varInstrumentType='OPTION'">
                <xsl:variable name="OpraCode" select="normalize-space(COL3)"/>
                <xsl:value-of select="document('../ReconMappingXml/UnderlyingSymbolMapping.xml')/SymbolMapping/PB[@Name='ABUNDANCE']/SymbolData[@OPRASymbol=$OpraCode]/@UnderlyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="OptionMonth">
            <xsl:choose>
              <xsl:when test="$varInstrumentType='OPTION'">
                <xsl:value-of select ="substring(COL5,string-length(COL5) - 1,1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="Strike">
            <xsl:choose>
              <xsl:when test="$varInstrumentType='OPTION'">
                <xsl:variable name ="varStr" select ="normalize-space(substring(COL4,19,11))"/>
                <xsl:variable name ="varStrikeDecimal" select ="substring-after($varStr,'.')"/>
                <xsl:variable name ="varStrikeInt" select ="substring-before($varStr,'.')"/>
                <xsl:choose>
                  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
                    <xsl:value-of select ="concat($varStr,'0')"/>
                  </xsl:when>
                  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
                    <xsl:value-of select ="$varStr"/>
                  </xsl:when>
                  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
                    <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="concat($varStr,'.00')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="ExpYear">
            <xsl:choose>
              <xsl:when test="$varInstrumentType='OPTION'">
                <xsl:value-of select ="substring(COL4,17,2)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>-->

          <!--  Symbol Region -->

          <xsl:choose>
            <xsl:when test ="$varInstrumentType='EQUITY'">
              <Symbol>
                <xsl:value-of select="translate(COL3,'&quot;','')"/>
              </Symbol>
              <PBSymbol>
                <xsl:value-of select="translate(COL3,'&quot;','')"/>
              </PBSymbol>
              <IDCOOptionSymbol>
                <xsl:value-of select="''"/>
              </IDCOOptionSymbol>
            </xsl:when>
            <xsl:when test ="$varInstrumentType='OPTION'">
              <Symbol>
                <!--<xsl:value-of select="translate(COL5,'&quot;','')"/>-->
                <!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>-->
                <xsl:value-of select="''"/>
              </Symbol>
              <PBSymbol>
                <xsl:value-of select="COL17"/>
              </PBSymbol>
              <IDCOOptionSymbol>
                <xsl:value-of select="concat(COL17,'U')"/>
              </IDCOOptionSymbol>
            </xsl:when>
            <xsl:when test ="$varInstrumentType='FUTURE'">
              <xsl:variable name = "varLength" >
                <xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
              </xsl:variable>
              <xsl:choose>
                <xsl:when test ="$varLength &gt; 0 ">
                  <xsl:variable name = "varAfter" >
                    <xsl:value-of select="substring(COL6,($varLength)-1,2)"/>
                  </xsl:variable>
                  <xsl:variable name = "varBefore" >
                    <xsl:value-of select="substring(COL6,1,($varLength)-2)"/>
                  </xsl:variable>
                  <Symbol>
                    <xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
                  </Symbol>
                  <PBSymbol>
                    <xsl:value-of select="translate(COL6,'&quot;','')"/>
                  </PBSymbol>
                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>
                </xsl:when>
                <xsl:otherwise>
                  <Symbol>
                    <xsl:value-of select="''"/>
                  </Symbol>
                  <PBSymbol>
                    <xsl:value-of select="''"/>
                  </PBSymbol>
                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>
              <IDCOOptionSymbol>
                <xsl:value-of select="''"/>
              </IDCOOptionSymbol>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when  test="COL15='Local Currency Market Price' or COL15='*'">
              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>
            </xsl:when >
            <xsl:otherwise>
              <MarkPrice>
                <xsl:value-of select="COL15"/>
              </MarkPrice>
            </xsl:otherwise>
          </xsl:choose >

          <xsl:choose>
            <xsl:when test ="COL7='*' or COL7='Trade/Settlement/Value Date'">
              <Date>
                <xsl:value-of select="''"/>
              </Date>
            </xsl:when>
            <xsl:otherwise>
              <Date>
                <xsl:value-of select="COL7"/>
              </Date>
            </xsl:otherwise>
          </xsl:choose>
        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
