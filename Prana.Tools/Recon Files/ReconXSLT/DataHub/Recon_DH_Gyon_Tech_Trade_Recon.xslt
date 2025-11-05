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
    <xsl:if test="contains(COL6,Option)">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(COL7,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(COL27,7,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(COL27,5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(COL27,3,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(COL28,1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(COL29,'#.00')"/>
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

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'MS_SWAP'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL7)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="contains(COL6,'Option')">
                    <xsl:value-of select="'EquityOption'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varSEDOL">
                <xsl:value-of select="COL9"/>
              </xsl:variable>

              <xsl:variable name="Symbol" select="COL7"/>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="COL30"/>
                      <xsl:with-param name="Suffix" select="''"/>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:when test="$Symbol !='*' or $Symbol !=''">
                    <xsl:value-of select="$Symbol"/>
                  </xsl:when>
                  <xsl:when test="$varSEDOL !=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>
              <xsl:variable name="varOptionPart">
                <xsl:value-of select="substring-before(COL7,' ')"/>
              </xsl:variable>

              <xsl:variable name="varOptionPart2">
                <xsl:value-of select="substring-after(COL7,'   ')"/>
              </xsl:variable>

              <xsl:variable name="varEQOption">
                <xsl:value-of select="COL7"/>
              </xsl:variable>
              <IDCOOptionSymbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varSEDOL !=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$Symbol !='*' or $Symbol !=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </IDCOOptionSymbol>

              <SEDOL>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>


                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varSEDOL !=''">
                    <xsl:value-of select="$varSEDOL"/>
                  </xsl:when>

                  <xsl:when test="$Symbol !='*' or $Symbol !=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>
              
              <xsl:variable name="PB_FUND_NAME" select="COL1"/>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME!='' and COL6='Swap'">
                    <xsl:value-of select="concat($PRANA_FUND_NAME,' Swap')"/>
                  </xsl:when>
                  <xsl:when test="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>
              <xsl:variable name="PB_BROKER_NAME">
                <xsl:value-of select="COL26"/>
              </xsl:variable>
              <xsl:variable name="PRANA_BROKER_ID">
                <xsl:value-of select="document('../../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
              </xsl:variable>

              <CounterParty>
                <xsl:value-of select="COL26"/>
              </CounterParty>
              
              <Quantity>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>

                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="$Quantity * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <xsl:variable name="AvgPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL17"/>
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

              <xsl:variable name="Commission">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL19"/>
                </xsl:call-template>
              </xsl:variable>
              <Commission>
                <xsl:choose>
                  <xsl:when test="$Commission &gt; 0">
                    <xsl:value-of select="$Commission"/>
                  </xsl:when>
                  <xsl:when test="$Commission &lt; 0">
                    <xsl:value-of select="$Commission * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Commission>

              <xsl:variable name="FxRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL31"/>
                </xsl:call-template>
              </xsl:variable>
              <FxRate>
                <xsl:choose>
                  <xsl:when test="number($FxRate)">
                    <xsl:value-of select="$FxRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FxRate>

              <xsl:variable name="OtherBrokerFee">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL21"/>
                </xsl:call-template>
              </xsl:variable>
              <Fees>
                <xsl:choose>
                  <xsl:when test="$OtherBrokerFee &gt; 0">
                    <xsl:value-of select="$OtherBrokerFee"/>
                  </xsl:when>
                  <xsl:when test="$OtherBrokerFee &lt; 0">
                    <xsl:value-of select="$OtherBrokerFee * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Fees>

          
              <UnderlyingSymbol>
                <xsl:value-of select="''"/>
              </UnderlyingSymbol>

              <Bloomberg>
                <xsl:value-of select="''"/>
              </Bloomberg>

              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>

              <xsl:variable name="GrossNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL23"/>
                </xsl:call-template>
              </xsl:variable>
              <GrossNotionalValue>
                <xsl:choose>
                  <xsl:when test="$GrossNotionalValue &gt; 0">
                    <xsl:value-of select="$GrossNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$GrossNotionalValue &lt; 0">
                    <xsl:value-of select="$GrossNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </GrossNotionalValue>

              <xsl:variable name="varSecFee">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL20"/>
                </xsl:call-template>
              </xsl:variable>

              <SecFees>
                <xsl:choose>
                  <xsl:when test="$varSecFee &gt; 0">
                    <xsl:value-of select="$varSecFee"/>
                  </xsl:when>
                  <xsl:when test="$varSecFee &lt; 0">
                    <xsl:value-of select="$varSecFee * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SecFees>

              <xsl:variable name="TradeDate" select="COL2"/>

              <TradeDate>
                <xsl:value-of select="$TradeDate"/>
              </TradeDate>

              <OriginalPurchaseDate>
                <xsl:value-of select ="COL13"/>
              </OriginalPurchaseDate>

              <SettlementDate>
                <xsl:value-of select="COL14"/>
              </SettlementDate>

              <ExpirationDate>
                <xsl:value-of select ="''"/>
              </ExpirationDate>

              <ProcessDate>
                <xsl:value-of select ="''"/>
              </ProcessDate>

              <CurrencySymbol>
                <xsl:value-of select ="''"/>
              </CurrencySymbol>
              <xsl:variable name ="Side" select="COL4"/>
              <Side>
                <xsl:choose>
                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:choose>
                      <xsl:when test="$Side='BC'">
                        <xsl:value-of select="'B'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Buy'">
                        <xsl:value-of select="'A'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Sell'">
                        <xsl:value-of select="'D'"/>
                      </xsl:when>
                      <xsl:when test="$Side='SHRT'">
                        <xsl:value-of select="'C'"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$Side='BuyCover'">
                        <xsl:value-of select="'B'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Buy'">
                        <xsl:value-of select="'1'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Sell'">
                        <xsl:value-of select="'2'"/>
                      </xsl:when>
                      <xsl:when test="$Side='SellShort'">
                        <xsl:value-of select="'5'"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>

              <SMRequest>
                <xsl:value-of select="'true'"/>
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

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <Commission>
                <xsl:value-of select="0"/>
              </Commission>

              <SecFees>
                <xsl:value-of select="0"/>
              </SecFees>
              <FxRate>
                <xsl:value-of select="0"/>
              </FxRate>

              <Fees>
                <xsl:value-of select="0"/>
              </Fees>

              <UnderlyingSymbol>
                <xsl:value-of select="''"/>
              </UnderlyingSymbol>

              <Bloomberg>
                <xsl:value-of select="''"/>
              </Bloomberg>

              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>

              <Asset>
                <xsl:value-of select="''"/>
              </Asset>

              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>

              <GrossNotionalValue>
                <xsl:value-of select="0"/>
              </GrossNotionalValue>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <OriginalPurchaseDate>
                <xsl:value-of select="''"/>
              </OriginalPurchaseDate>

              <SettlementDate>
                <xsl:value-of select="''"/>
              </SettlementDate>

              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>

              <ProcessDate>
                <xsl:value-of select="''"/>
              </ProcessDate>

              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>

              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

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

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



