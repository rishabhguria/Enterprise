<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
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

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL28"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and contains(COL3,'CUSHING') ">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley'"/>
              </xsl:variable>

              <xsl:variable name = "PB_SYMBOL_NAME" >
                <xsl:value-of select ="COL6"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>


              <xsl:variable name="Symbol" select="COL8"/>
              <Symbol>
                <xsl:choose>

                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$Symbol!='*'">
                    <xsl:value-of select="normalize-space($Symbol)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>


              <xsl:variable name="PB_COUNTER_PARTY" select="COL60"/>

              <xsl:variable name="PRANA_COUNTER_PARTY">
                <xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@MLPBroker"/>
              </xsl:variable>

              <CounterParty>
                <xsl:choose>

                  <xsl:when test ="$PRANA_COUNTER_PARTY!='' ">
                    <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_COUNTER_PARTY"/>
                  </xsl:otherwise>

                </xsl:choose>
              </CounterParty>

              <CUSIP>
                <xsl:value-of select="normalize-space(COL7)"/>
              </CUSIP>

              <SEDOL>
                <xsl:value-of select="normalize-space(COL9)"/>
              </SEDOL>

              <ISIN>
                <xsl:value-of select="normalize-space(COL10)"/>
              </ISIN>

              <xsl:variable name ="Bloomberg" select="COL14"/>
              <Bloomberg>
                <xsl:choose>
                  <xsl:when test ="contains(COL14,'US')">
                    <xsl:value-of select="concat($Bloomberg,' ','EQUITY')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Bloomberg>

              <xsl:variable name="PB_FUND_NAME" select="COL2"/>
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



              <xsl:variable name="Side" select="COL29"/>

              <Side>
                <xsl:choose>
                  <xsl:when test="$Side='L'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="$Side='S'">
                    <xsl:value-of select="'Sell short'"/>
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
                  <xsl:with-param name="Number" select="''"/>
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

              <xsl:variable name="MarketValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL33"/>
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

              <xsl:variable name="MarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL32"/>
                </xsl:call-template>
              </xsl:variable>
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

              <xsl:variable name ="Date" select="COL1"/>

              <TradeDate>
                <xsl:value-of select="$Date"/>
              </TradeDate>


              <SettlementDate>
                <xsl:value-of select ="''"/>
              </SettlementDate>



              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL30"/>
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


              <xsl:variable name="varMarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <MarkPriceBase>
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
              </MarkPriceBase>



              <xsl:variable name="UnitCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <UnitCost>
                <xsl:choose>
                  <xsl:when test="$UnitCost &gt; 0">
                    <xsl:value-of select="$UnitCost"/>
                  </xsl:when>
                  <xsl:when test="$UnitCost &lt; 0">
                    <xsl:value-of select="$UnitCost * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </UnitCost>

              <xsl:variable name ="FXRate">
                <xsl:value-of select ="COL46"/>
              </xsl:variable>
              <FXRate>
                <xsl:choose>
                  <xsl:when test ="$FXRate ">
                    <xsl:value-of select ="$FXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select ="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FXRate>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>


              <SMRequest>
                <xsl:value-of select="''"/>
              </SMRequest>
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>


              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>

              <ISIN>
                <xsl:value-of select="''"/>
              </ISIN>
              <Bloomberg>
                <xsl:value-of select="''"/>
              </Bloomberg>
              <IDCOOptionSymbol>
                <xsl:value-of select="''"/>
              </IDCOOptionSymbol>

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

</xsl:stylesheet>