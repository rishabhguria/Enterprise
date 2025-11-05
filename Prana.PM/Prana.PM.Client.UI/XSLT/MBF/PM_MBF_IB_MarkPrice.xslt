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

        <xsl:if test ="COL2='POST'">
          <PositionMaster>
            <xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='IB']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <!--  Symbol Region -->
            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test ="COL5='FUT'">
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
                      </xsl:when>
                      <xsl:otherwise>
                        <Symbol>
                          <xsl:value-of select="COL6"/>
                        </Symbol>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <Symbol>
                      <xsl:value-of select="COL6"/>
                    </Symbol>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when  test="COL10='MarkPrice' or COL10='*'">
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:when >
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="number(translate(COL10,',',''))"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose >
            <Date>
              <xsl:value-of select="''"/>
            </Date>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
