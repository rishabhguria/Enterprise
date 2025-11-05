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



  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(substring(substring-after(substring-after(substring-after(COL14,'/'),'/'),' '),1,1),'P') or contains(substring(substring-after(substring-after(substring-after(COL14,'/'),'/'),' '),1,1),'C')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL14),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL14),'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL14),' '),' '),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL14),'/'),'/'),' ')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL14),'/'),'/'),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(normalize-space(COL14),'/'),'/'),' '),2),'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name ="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL25"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and contains(COL12,'Cash') !='true' and not(contains(COL1,'Grand'))">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'MS'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL14"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>




              <xsl:variable name = "PB_SUFFIX_CODE" >
                <xsl:value-of select ="substring-after(COL7,' ')"/>
              </xsl:variable>

              <xsl:variable name ="PRANA_SUFFIX_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
              </xsl:variable>


              <xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="contains(COL12,'Option')">
                    <xsl:value-of select="'Option'"/>
                  </xsl:when>
                  <xsl:when test="contains(COL12,'FX')">
                    <xsl:value-of select="'FX'"/>
                  </xsl:when>
                  <xsl:when test="contains(COL11,'CFD')">
                    <xsl:value-of select="'EquitySwap'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <AssetNew>
                <xsl:value-of select="$Asset"/>
              </AssetNew>
              <xsl:variable name="Symbol" select="substring-before(COL13, ' ')"/>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$Asset='Option'">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="COL14"/>
                      <xsl:with-param name="Suffix" select="''"/>
                    </xsl:call-template>
                  </xsl:when>


                  <xsl:when test="$Asset='FX'">
                    <xsl:value-of select="concat(translate(substring-before(COL14,' '),'/','-'),' ',translate(substring-after(COL14,' '),'-',''))"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Equity'">
                    <xsl:value-of select="normalize-space($Symbol)"/>
                  </xsl:when>

                  <xsl:when test="$Asset='Swap'">
                    <xsl:value-of select="concat(substring-before(COL5,' '),$PRANA_SUFFIX_NAME)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

             

              <xsl:variable name="PB_FUND_NAME" select="COL21"/>
              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

              <Multiplier>
                <xsl:value-of select="''"/>
              </Multiplier>


              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>


              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>

              <PutOrCall>
                <xsl:value-of select="''"/>
              </PutOrCall>

              <xsl:variable name="varFxRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <Strategy>
                <xsl:choose>
                  <xsl:when test="number($varFxRate)">
                    <xsl:value-of select="$varFxRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Strategy>


              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>


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


              <xsl:variable name="UnitCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <UnitCost>
                <xsl:choose>
                  <xsl:when test="number($UnitCost)">
                    <xsl:value-of select="$UnitCost"/>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </UnitCost>


              <xsl:variable name="SettlCurrAmt">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlCurrAmt>

                <xsl:choose>
                  <xsl:when test="number($SettlCurrAmt)">
                    <xsl:value-of select="$SettlCurrAmt"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlCurrAmt>


              <xsl:variable name="SettlCurrFxRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlCurrFxRate>

                <xsl:choose>
                  <xsl:when test="number($SettlCurrFxRate)">
                    <xsl:value-of select="$SettlCurrFxRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlCurrFxRate>


              <xsl:variable name="SettlPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlPrice>

                <xsl:choose>
                  <xsl:when test="number($SettlPrice)">
                    <xsl:value-of select="$SettlPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlPrice>



              <xsl:variable name="SettlementCurrencyMarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlementCurrencyMarkPrice>

                <xsl:choose>
                  <xsl:when test="number($SettlPrice)">
                    <xsl:value-of select="$SettlPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlementCurrencyMarkPrice>


              <xsl:variable name="SettlementCurrencyCostBasis">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlementCurrencyCostBasis>

                <xsl:choose>
                  <xsl:when test="number($SettlementCurrencyCostBasis)">
                    <xsl:value-of select="$SettlementCurrencyCostBasis"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlementCurrencyCostBasis>

              <xsl:variable name="SettlementCurrencyMarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlementCurrencyMarketValue>

                <xsl:choose>
                  <xsl:when test="number($SettlementCurrencyMarketValue)">
                    <xsl:value-of select="$SettlementCurrencyMarketValue"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlementCurrencyMarketValue>


              <xsl:variable name="SettlementCurrencyTotalCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlementCurrencyTotalCost>

                <xsl:choose>
                  <xsl:when test="number($SettlementCurrencyTotalCost)">
                    <xsl:value-of select="$SettlementCurrencyTotalCost"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlementCurrencyTotalCost>


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


              <xsl:variable name="MarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValue>
                <xsl:choose>
                  <xsl:when test="$MarketValue &gt; 0">
                    <xsl:value-of select="$MarketValue"/>
                  </xsl:when>
                  <xsl:when test="$MarketValue &lt; 0">
                    <xsl:value-of select="$MarketValue "/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </MarketValue>

              <xsl:variable name="MarketValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test="$MarketValueBase &gt; 0">
                    <xsl:value-of select="$MarketValueBase"/>
                  </xsl:when>
                  <xsl:when test="$MarketValueBase &lt; 0">
                    <xsl:value-of select="$MarketValueBase "/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </MarketValueBase>


              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL31"/>
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


              <xsl:variable name="MarkPriceBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <MarkPriceBase>
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
              </MarkPriceBase>

              <xsl:variable name="AvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$AvgPX &gt; 0">
                    <xsl:value-of select="$AvgPX"/>

                  </xsl:when>
                  <xsl:when test="$AvgPX &lt; 0">
                    <xsl:value-of select="$AvgPX * (1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </AvgPX>


              <xsl:variable name="Side" select="COL24"/>
              <Side>
                <xsl:choose>
                  <xsl:when test="$Asset='Options'">
                    <xsl:choose>
                      <xsl:when test="$Side='L'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>

                      <xsl:when test="$Side='S'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$Side='L'">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>

                      <xsl:when test="$Side='S'">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <OriginalPurchaseDate>
                <xsl:value-of select="''"/>
              </OriginalPurchaseDate>

              <xsl:variable name="TradeDate" select="COL33"/>
              <TradeDate>
                <xsl:value-of select="$TradeDate"/>
              </TradeDate>

              <CompanyName>
                <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
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

              <Multiplier>
                <xsl:value-of select="0"/>
              </Multiplier>


              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>


              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>

              <PutOrCall>
                <xsl:value-of select="''"/>
              </PutOrCall>


              <Strategy>
                <xsl:value-of select="0"/>
              </Strategy>


              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>

              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>

              <UnitCost>
                <xsl:value-of select="0"/>
              </UnitCost>

              <BaseCurrency>
                <xsl:value-of select="''"/>>
              </BaseCurrency>


              <SettlCurrency>
                <xsl:value-of select="''"/>
              </SettlCurrency>

              <SettlCurrAmt>
                <xsl:value-of select="0"/>

              </SettlCurrAmt>

              <SettlCurrFxRate>
                <xsl:value-of select="0"/>
              </SettlCurrFxRate>


              <SettlPrice>
                <xsl:value-of select="0"/>
              </SettlPrice>



              <SettlementCurrencyMarkPrice>
                <xsl:value-of select="0"/>
              </SettlementCurrencyMarkPrice>


              <SettlementCurrencyCostBasis>
                <xsl:value-of select="0"/>
              </SettlementCurrencyCostBasis>

              <SettlementCurrencyMarketValue>
                <xsl:value-of select="0"/>
              </SettlementCurrencyMarketValue>


              <SettlementCurrencyTotalCost>
                <xsl:value-of select="0"/>
              </SettlementCurrencyTotalCost>


              <Quantity>
                <xsl:value-of select="0"/>

              </Quantity>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>


              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>

              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>

              <MarkPriceBase>
                <xsl:value-of select="0"/>
              </MarkPriceBase>

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <Side>
                <xsl:value-of select ="''"/>
              </Side>

              <OriginalPurchaseDate>
                <xsl:value-of select ="''"/>
              </OriginalPurchaseDate>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

              <CompanyName>
                <xsl:value-of select ="''"/>
              </CompanyName>
            
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

