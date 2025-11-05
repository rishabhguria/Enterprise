<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL5)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Statestreet'"/>
              </xsl:variable>
              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL62)"/>
              </xsl:variable>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@SourceSymbol=$PB_SYMBOL_NAME]/@TargetSymbol"/>
              </xsl:variable>

              <xsl:variable name="varCUSIP" select="normalize-space(COL27)"/>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <CUSIP>
                <xsl:choose>

                  <!--<xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>-->

                  <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                    <xsl:value-of select="$varCUSIP"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </CUSIP>

              <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
              <!--<xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>-->
              <FundName>
                <!--<xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>-->
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  <!--</xsl:otherwise>
                </xsl:choose>-->
              </FundName>

              <xsl:variable name="varMarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL10)"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValue>
                <xsl:choose>
                  <xsl:when test="$varMarketValue &gt; 0">
                    <xsl:value-of select="$varMarketValue"/>
                  </xsl:when>
                  <xsl:when test="$varMarketValue &lt; 0">
                    <xsl:value-of select="$varMarketValue "/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <xsl:variable name="varMarketValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL11)"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test="$varMarketValueBase &gt; 0">
                    <xsl:value-of select="$varMarketValueBase"/>
                  </xsl:when>
                  <xsl:when test="$varMarketValueBase &lt; 0">
                    <xsl:value-of select="$varMarketValueBase "/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($Quantity)">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <xsl:variable name="varNetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL16)"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$varNetNotionalValue &gt; 0">
                    <xsl:value-of select="$varNetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$varNetNotionalValue &lt; 0">
                    <xsl:value-of select="$varNetNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

              <xsl:variable name="varNetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL17)"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$varNetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$varNetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$varNetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>

              <xsl:variable name="varMarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL8)"/>
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

              <xsl:variable name="varUnitCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL6)"/>
                </xsl:call-template>
              </xsl:variable>
              <UnitCost>
                <xsl:choose>
                  <xsl:when test="$varUnitCost &gt; 0">
                    <xsl:value-of select="$varUnitCost"/>
                  </xsl:when>
                  <xsl:when test="$varUnitCost &lt; 0">
                    <xsl:value-of select="$varUnitCost * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </UnitCost>

              <Side>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="'Sell short'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <TradeDate>
                <xsl:value-of select="concat(substring(normalize-space(COL1),5,2),'/',substring(normalize-space(COL1),7,2),'/',substring(normalize-space(COL1),1,4))"/>
              </TradeDate>

              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>

              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>

              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>

              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>

              <UnitCost>
                <xsl:value-of select="0"/>
              </UnitCost>

              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>