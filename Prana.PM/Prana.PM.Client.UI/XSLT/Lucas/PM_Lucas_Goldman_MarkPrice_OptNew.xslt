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

        <xsl:variable name = "varInstrumentType" >
          <xsl:value-of select="translate(COL5,'&quot;','')"/>
        </xsl:variable>

        <xsl:if test="$varInstrumentType='EQUITY' or $varInstrumentType='OPTION'">
          <PositionMaster>


            <!--<xsl:variable name="OptionUnderlyingSymbol">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:variable name ="varafterSlash" select ="normalize-space(substring-after(COL7,'/'))"/>
                  <xsl:variable name ="varUnderlying" select ="normalize-space(substring-before($varafterSlash,'('))"/>
                  <xsl:value-of select ="$varUnderlying"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="OptionMonth">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:variable name ="varLength" select ="string-length(COL8)"/>
                  <xsl:value-of select ="substring(COL8,$varLength - 1,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Strike">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:variable name ="varafter" select ="substring-after(COL7,'@')"/>
                  <xsl:variable name ="varStr" select ="normalize-space(substring-before($varafter,'EXP'))"/>
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
                  <xsl:variable name ="varafter" select ="substring-after(COL7,'EXP')"/>
                  <xsl:value-of select ="substring($varafter,10,2)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->



            <!--  Symbol Region -->

            <xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$varInstrumentType='EQUITY' and $PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test="$varInstrumentType='OPTION'">
                <!-- $PXPHO-->
                <!--<xsl:variable name="varAfterDollar" >
									<xsl:value-of select="substring-after(COL8,'$')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterDollar)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterDollar,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterDollar,1,($varLength)-2)"/>
								</xsl:variable>-->
                <Symbol>
                  <!--<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>-->
                  <!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>-->
                  <xsl:value-of select="''"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="concat(COL8,'U')"/>
                </IDCOOptionSymbol>
              </xsl:when >
              <xsl:otherwise>
                <xsl:variable name="varAfterAstric" >
                  <xsl:value-of select="substring-after(COL8,'*')"/>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test ="$varAfterAstric =''">
                    <Symbol>
                      <xsl:value-of select="COL8"/>
                    </Symbol>
                    <IDCOOptionSymbol>
                      <xsl:value-of select="''"/>
                    </IDCOOptionSymbol>
                  </xsl:when>
                  <xsl:otherwise>
                    <Symbol>
                      <xsl:value-of select="$varAfterAstric"/>
                    </Symbol>
                    <IDCOOptionSymbol>
                      <xsl:value-of select="''"/>
                    </IDCOOptionSymbol>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when  test="boolean(number(COL12))">
                <MarkPrice>
                  <xsl:value-of select="COL12"/>
                </MarkPrice>
              </xsl:when >
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose >

            <Date>
              <xsl:value-of select="''"/>
            </Date>

            <PBSymbol>
              <xsl:value-of select="COL8"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
