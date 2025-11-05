<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL20"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name = "PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name = "PB_SYMBOL_NAME" >
                <xsl:value-of select ="COL6"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <Side>
                <xsl:choose>
                      <xsl:when test="$Quantity &gt; 0">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:when test="$Quantity &lt; 0">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
              </Side>

              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>

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

              <xsl:variable name="AvgPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL74"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$AvgPrice &gt; 0">
                    <xsl:value-of select="$AvgPrice"/>

                  </xsl:when>
                  <xsl:when test="$AvgPrice &lt; 0">
                    <xsl:value-of select="$AvgPrice * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </AvgPX>


              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValue &gt; 0">
                    <xsl:value-of select="$NetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValue &lt; 0">
                    <xsl:value-of select="$NetNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

              <xsl:variable name="NetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$NetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>


              <xsl:variable name="MarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL36"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="MarketValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL38"/>
                </xsl:call-template>
              </xsl:variable>

              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test="number($MarketValueBase)">
                    <xsl:value-of select="$MarketValueBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>

              <MarketValue>
                <xsl:choose>
                  <xsl:when test="number($MarketValue)">
                    <xsl:value-of select="$MarketValue"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <xsl:variable name="varTradeDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL12"/>
                </xsl:call-template>
              </xsl:variable>

              <TradeDate>
                <xsl:value-of select="$varTradeDate"/>
              </TradeDate>

              <SettlementDate>
                <xsl:value-of select ="''"/>
              </SettlementDate>

              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL74"/>
                </xsl:call-template>
              </xsl:variable>

              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>
                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <MarkPriceBase>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>
                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPriceBase>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>


              <SMRequest>
                <xsl:value-of select="'false'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>

              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <FXRate>
                <xsl:value-of select="'0'"/>
              </FXRate>

              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>


              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>


              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>


              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>
              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

              <SettlementDate>
                <xsl:value-of select ="''"/>
              </SettlementDate>

              <MarkPriceBase>
                <xsl:value-of select="0"/>
              </MarkPriceBase>
              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>


              <UnitCost>
                <xsl:value-of select="0"/>
              </UnitCost>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
              <SMRequest>
                <xsl:value-of select="''"/>
              </SMRequest>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



