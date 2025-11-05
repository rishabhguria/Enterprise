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
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
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
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
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
    <xsl:if test="contains(COL6,'OPTN')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:choose>
          <xsl:when test="substring(COL11,2,1) = '1'">
            <xsl:value-of select="substring-before(COL11,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,2,1) = '2'">
            <xsl:value-of select="substring-before(COL11,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,3,1) = '1'">
            <xsl:value-of select="substring-before(COL11,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,3,1) = '2'">
            <xsl:value-of select="substring-before(COL11,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,4,1) = '1'">
            <xsl:value-of select="substring-before(COL11,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,4,1) = '2'">
            <xsl:value-of select="substring-before(COL11,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,5,1) = '1'">
            <xsl:value-of select="substring-before(COL11,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,5,1) = '2'">
            <xsl:value-of select="substring-before(COL11,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,6,1) = '1'">
            <xsl:value-of select="substring-before(COL11,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL11,6,1) = '2'">
            <xsl:value-of select="substring-before(COL11,'2')"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8) div 1000  ,'#.00')"/>
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
      <xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

    </xsl:if>
  </xsl:template>

  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name ="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL27"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)  and contains(COL51,'CASH') !='true' and not (contains(COL51,'MONEY MARKET')) and (normalize-space(COL2)='038CAAPD2' or normalize-space(COL2)='038CDKDA6')">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'MS'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL6"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>



              <xsl:variable name = "varSymbol" >
                <xsl:value-of select="normalize-space(COL5)"/>
              </xsl:variable>
              <xsl:variable name = "varCusip" >
                <xsl:value-of select="normalize-space(COL15)"/>
              </xsl:variable>


              <xsl:variable name = "PB_SUFFIX_CODE" >
                <xsl:value-of select ="COL44"/>
              </xsl:variable>

              <xsl:variable name ="PRANA_SUFFIX_NAME">
                <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
              </xsl:variable>



              <xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="contains(COL50,'CALL') or contains(COL50,'PUT') or contains(COL50,'PUTL')">
                    <xsl:value-of select="'EquityOption'"/>
                  </xsl:when>
                  <xsl:when test="contains(COL51,'FX FORWARDS')">
                    <xsl:value-of select="'FXForward'"/>
                  </xsl:when>
                  <xsl:when test="contains(COL51,'EQUITY SWAP')">
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

              <xsl:variable name="Symbol">
                <xsl:choose>
                  <xsl:when test="contains(COL8,'.')">
                    <xsl:value-of select="substring-before(COL8,'.')"/>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:value-of select="normalize-space(COL8)"/>
                  </xsl:otherwise>



                </xsl:choose>
              </xsl:variable>

              <Symbol>

                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$Asset='FX'">
                    <xsl:value-of select="concat(translate(substring-before(COL6,' '),'/','-'),' ',translate(substring-after(COL6,' '),'/',''))"/>
                  </xsl:when>

                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$Asset='Equity'">
                    <xsl:value-of select="concat(translate($Symbol,$lower_CONST,$upper_CONST),$PRANA_SUFFIX_NAME)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>

                </xsl:choose>


              </Symbol>

              <xsl:variable name="Underlying" select="substring-before(COL8,'1')"/>
              <xsl:variable name="undspaces">
                <xsl:call-template name="spaces">
                  <xsl:with-param name="count" select="number(5 - string-length($Underlying))"/>
                </xsl:call-template>
              </xsl:variable>
              <xsl:variable name="IdcoLast" select="substring(COL8,string-length($Underlying)+1)"/>
              <xsl:variable name="Idco">
                <xsl:value-of select="concat($Underlying,$undspaces,$IdcoLast,'U')"/>
              </xsl:variable>


              <IDCOOptionSymbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:value-of select="$Idco"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </IDCOOptionSymbol>


              <xsl:variable name="PB_FUND_NAME" select="COL4"/>
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


              <xsl:variable name="MarketValueBase">
                <xsl:choose>
                  <xsl:when test="$Asset='FX'">
                    <xsl:call-template name="Translate">
                      <xsl:with-param name="Number" select="COL33"/>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="Translate">
                      <xsl:with-param name="Number" select="COL35"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
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
                <xsl:choose>
                  <xsl:when test="$Asset='FX'">
                    <xsl:call-template name="Translate">
                      <xsl:with-param name="Number" select="COL33"/>
                    </xsl:call-template>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:call-template name="Translate">
                      <xsl:with-param name="Number" select="COL34"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
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
              
              <xsl:variable name="MarkPrice">
                <xsl:choose>
                  <xsl:when test="$Asset='FX'">
                    <xsl:call-template name="Translate">
                      <xsl:with-param name="Number" select="COL67"/>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="Translate">
                      <xsl:with-param name="Number" select="COL30"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
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


              <xsl:variable name="Side" select="COL29"/>
              <Side>
                <xsl:choose>
                  <xsl:when test="$Side='L' ">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="$Side='S'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <OriginalPurchaseDate>
                <xsl:value-of select="''"/>
              </OriginalPurchaseDate>

              <xsl:variable name="TradeDate" select="COL1"/>
              <TradeDate>
                <xsl:value-of select="$TradeDate"/>
              </TradeDate>

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

