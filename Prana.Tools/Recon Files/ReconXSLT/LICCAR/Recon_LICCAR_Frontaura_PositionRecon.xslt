<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name ="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL3"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($varQuantity)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol" select="normalize-space(COL1)"/>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!='' and $varSymbol!='*'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <Quantity>
              <xsl:choose>
                <xsl:when  test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when  test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="varAvgPX1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL3"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varAvgPX2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varAvgPX">
              <xsl:value-of select="$varAvgPX2 div $varAvgPX1"/>
            </xsl:variable>
            <AvgPX>
              <xsl:choose>
                <xsl:when  test="number($varAvgPX)">
                  <xsl:value-of select="$varAvgPX"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>

            <xsl:variable name="varMarkPrice1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL3"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarkPrice2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL5"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="MarkPrice">
              <xsl:value-of select="$varMarkPrice2 div $varMarkPrice1"/>
            </xsl:variable>
            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>

                </xsl:when>
                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <xsl:variable name="varNetNotional">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNetNotional &gt; 0">
                  <xsl:value-of select="$varNetNotional"/>

                </xsl:when>
                <xsl:when test="$varNetNotional &lt; 0">
                  <xsl:value-of select="$varNetNotional * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL5"/>
              </xsl:call-template>
            </xsl:variable>
            <MarketValue>
              <xsl:choose>
                <xsl:when test="$MarketValue &gt; 0">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>

                <xsl:when test="$MarketValue &lt; 0">
                  <xsl:value-of select="$MarketValue * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <Side>
              <xsl:choose>
                <xsl:when  test="$varQuantity &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>

                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="'Sell Short'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <OriginalPurchaseDate>
              <xsl:value-of select="''"/>
            </OriginalPurchaseDate>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>