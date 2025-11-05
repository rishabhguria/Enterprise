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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime"/>

    <xsl:variable name="varDate">
      <xsl:choose>
        <xsl:when test="contains($DateTime,';')">
          <xsl:value-of select="substring-before($DateTime,';')"/>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$DateTime"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varDate"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name ="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($varQuantity)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL17)"/>
            </xsl:variable>

            <xsl:variable name="varISIN">
              <xsl:value-of select="normalize-space(COL18)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol" select="normalize-space(COL2)"/>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varISIN!='' and $varISIN!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSymbol!='' and $varSymbol!='*'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                  <xsl:value-of select="$varCUSIP"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <ISINSymbol>
              <xsl:choose>
                <xsl:when test="$varISIN!='' and $varISIN!='*'">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISINSymbol>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>

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

            <xsl:variable name="varAvgPX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL31"/>
              </xsl:call-template>
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

            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
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
                <xsl:with-param name="Number" select="COL32"/>
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
                <xsl:with-param name="Number" select="COL7"/>
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

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>
            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="$MarketValueBase &gt; 0">
                  <xsl:value-of select="$MarketValueBase"/>
                </xsl:when>

                <xsl:when test="$MarketValueBase &lt; 0">
                  <xsl:value-of select="$MarketValueBase * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>

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

            <xsl:variable name="varOriginalPurchaseDate">
              <xsl:choose>
                <xsl:when test="COL39!='' and COL39!='*'">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="normalize-space(COL39)"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <OriginalPurchaseDate>
              <xsl:value-of select="$varOriginalPurchaseDate"/>
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