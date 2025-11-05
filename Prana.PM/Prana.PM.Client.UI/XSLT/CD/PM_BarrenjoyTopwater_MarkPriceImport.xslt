<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL23)">
          <PositionMaster>
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL35"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@CompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_SymbolCurrency_NAME" >
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name = "PB_Currency_NAME" >
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SymbolCurrency_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@CompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="COL19='USD'">
                  <xsl:value-of select="COL15"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$PRANA_SymbolCurrency_NAME=''">
                      <xsl:choose>
                        <xsl:when test="COL19='AUD'">
                          <xsl:value-of select="concat(substring-before(COL15,'.'),'-ASX')"/>
                        </xsl:when>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <PBSymbol>
              <xsl:value-of select="COL2"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL29)">
                  <xsl:value-of select="COL29"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <Date>
              <xsl:value-of select="concat(substring(COL1,5,2),'/',substring(COL1,7,2),'/',substring(COL1,1,4))"/>
            </Date>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
