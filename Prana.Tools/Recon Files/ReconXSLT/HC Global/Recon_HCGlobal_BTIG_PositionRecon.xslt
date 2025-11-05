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
            <xsl:with-param name="Number" select="normalize-space(COL4)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity) and contains(normalize-space(COL2),'Cash')">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!='*' or $varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
                <xsl:when  test="number($varQuantity)">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="varUnitCost">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL9)"/>
              </xsl:call-template>
            </xsl:variable>
            <UnitCost>
              <xsl:choose>
                <xsl:when test="number($varUnitCost)">
                  <xsl:value-of select="$varUnitCost"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </UnitCost>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
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
                <xsl:with-param name="Number" select="normalize-space(COL19)"/>
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

            <xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL10)"/>
              </xsl:call-template>
            </xsl:variable>
            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$varMarkPrice &gt; 0">
                  <xsl:value-of select="$varMarkPrice"/>

                </xsl:when>
                <xsl:when test="$varMarkPrice &lt; 0">
                  <xsl:value-of select="$varMarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>

            <xsl:variable name="varMarkPriceBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL16)"/>
              </xsl:call-template>
            </xsl:variable>
            <MarkPriceBase>
              <xsl:choose>
                <xsl:when test="$varMarkPriceBase &gt; 0">
                  <xsl:value-of select="$varMarkPriceBase"/>

                </xsl:when>
                <xsl:when test="$varMarkPriceBase &lt; 0">
                  <xsl:value-of select="$varMarkPriceBase * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPriceBase>

            <xsl:variable name="varNetNotional">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL11)"/>
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

            <xsl:variable name="varNetNotionalBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL18)"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalBase"/>

                </xsl:when>
                <xsl:when test="$varNetNotionalBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalBase * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetNotionalValueBase>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="$varSide='Long'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$varSide='Short'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

