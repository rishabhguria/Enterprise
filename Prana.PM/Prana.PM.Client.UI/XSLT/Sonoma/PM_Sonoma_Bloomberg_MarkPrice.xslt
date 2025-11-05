<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <!-- variable declaration for lower to upper case -->

        <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
        <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

        <!--<xsl:if test ="$varAsset='EQTY' or $varAsset='EQTYOPT' or $varAsset = 'FUND' or $varAsset ='CONV' or $varAsset = 'CORP' or $varAsset = 'WRNT'">-->

        <PositionMaster>
          <xsl:variable name="PB_COMPANY_NAME" select="COL2"/>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Bloomberg']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="$PRANA_SYMBOL_NAME != ''">
              <Symbol>
                <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
              </Symbol>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="translate(COL2,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
              </Symbol>
            </xsl:otherwise>
          </xsl:choose >

          <PBSymbol>
            <xsl:value-of select="COL2"/>
          </PBSymbol>

          <xsl:choose>
            <xsl:when  test="boolean(number(COL3))">
              <MarkPrice>
                <xsl:value-of select="COL3"/>
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

        </PositionMaster>
        <!--</xsl:if >-->
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
