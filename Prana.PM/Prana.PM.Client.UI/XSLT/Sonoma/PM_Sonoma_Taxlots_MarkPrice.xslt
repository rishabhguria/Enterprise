<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
        <xsl:for-each select="//PositionMaster">

          <xsl:variable name ="varAsset">
            <xsl:value-of select ="COL2"/>
          </xsl:variable>
          <xsl:if test ="$varAsset='Equity' or $varAsset='Equity Option' or $varAsset = 'Fund' or $varAsset ='Bond Convertible' or $varAsset = 'Bond Corporate' or $varAsset = 'warrants'">

            <PositionMaster>
              <xsl:variable name="PB_COMPANY_NAME" select="COL5"/>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Sonoma']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                  <Symbol>
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </Symbol>
                </xsl:when>
                <xsl:when test ="$varAsset='Equity' or $varAsset='Fund'">
                  <Symbol>
                    <xsl:value-of select ="substring-before(COL22,' ')"/>
                  </Symbol>
                </xsl:when>
                <xsl:when test ="$varAsset='Equity Option'">
                  <Symbol>
                    <xsl:value-of select ="translate(COL22,'+',' ')"/>
                  </Symbol>
                </xsl:when>
                <xsl:when test ="$varAsset='Bond Convertible' or $varAsset='Bond Corporate' or $varAsset='warrants'">
                  <Symbol>
                    <xsl:value-of select ="COL5"/>
                  </Symbol>
                </xsl:when>
                <xsl:otherwise>
                  <Symbol>
                    <xsl:value-of select="COL22"/>
                  </Symbol>
                </xsl:otherwise>
              </xsl:choose >

              <PBSymbol>
                <xsl:value-of select="COL22"/>
              </PBSymbol>

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

              <xsl:choose>
                <xsl:when test ="COL10 = 'Lot Date' or COL10 = '*'">
                  <Date>
                    <xsl:value-of select="''"/>
                  </Date>
                </xsl:when>
                <xsl:otherwise>
                  <Date>
                    <xsl:value-of select="COL10"/>
                  </Date>
                </xsl:otherwise>
              </xsl:choose>

            </PositionMaster>
          </xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
