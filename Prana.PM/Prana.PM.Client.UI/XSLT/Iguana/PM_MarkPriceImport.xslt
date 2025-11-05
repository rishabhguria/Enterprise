<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'T'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'OS'">
        <xsl:value-of select="'-OSE'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
      
          <PositionMaster>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>

            <!--<xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSuffix">
              <xsl:choose>
                <xsl:when test="contains(COL27, '.') != false">
                  <xsl:call-template name="GetSuffix">
                    <xsl:with-param name="Suffix" select="substring-after(COL27,'.')"/>
                  </xsl:call-template>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->

            <!--<Symbol>
              <xsl:choose>
                <xsl:when test=" contains(COL27,'.') != false">
                  <xsl:value-of select="concat(substring-before(COL27,'.'),$varSuffix)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL27"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>-->
			  <Symbol>
				  <xsl:value-of select="$PB_Symbol_NAME"/>
			  </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_Symbol_NAME"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL2)">
                  <xsl:value-of select="COL2"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <!--<Date>
              <xsl:value-of select="concat(substring(COL22,7,2),'/',substring(COL22,5,2), '/', substring(COL22,1,4))"/>
            </Date>-->

			  <Date>
				  <xsl:value-of select="''"/>
			  </Date>

          </PositionMaster>
      
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
