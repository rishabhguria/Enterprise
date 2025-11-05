<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public static string Now(int year, int month, int date)
    {
    DateTime weekEnd= new DateTime(year, month, date);
    weekEnd = weekEnd.AddDays(1);
    while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
    {
    weekEnd = weekEnd.AddDays(1);
    }
    return weekEnd.ToString();
    }

  </msxsl:script>

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



  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
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

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'FP'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'NA'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'AU'">
        <xsl:value-of select="'-ASX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'BZ'">
        <xsl:value-of select="'-BSP'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CN'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'LN'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'GR'">
        <xsl:value-of select="'-FRA'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'NO'">
        <xsl:value-of select="'-OSL'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'SM'">
        <xsl:value-of select="'-MAC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
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

  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>
    <xsl:variable name="varUSymbol">
      <xsl:choose>
        <xsl:when test="substring($varBBCode,2,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,2,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,3,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,3,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,4,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,4,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,5,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,5,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,6,1) = '1'">
          <xsl:value-of select="substring-before($varBBCode,'1')"/>
        </xsl:when>
        <xsl:when test="substring($varBBCode,6,1) = '2'">
          <xsl:value-of select="substring-before($varBBCode,'2')"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="varUnderlyingLength">
      <xsl:value-of select="string-length($varUSymbol)"/>
    </xsl:variable>
    <xsl:variable name="varExYear">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
    </xsl:variable>
    <xsl:variable name="varStrike1">
      <xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +8)), '#.00')"/>
    </xsl:variable>
    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(($varStrike1 div 1000), '#.00')"/>
    </xsl:variable>
    <xsl:variable name="varExDay">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +5),2)"/>
    </xsl:variable>
    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength + 3),2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength + 7),1)"/>
    </xsl:variable>
    <xsl:variable name="MonthCodeVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="number($varMonthCode)"/>
        <xsl:with-param name="PutOrCall" select="$PutORCall"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="varExpiryDay">
      <xsl:choose>
        <xsl:when test="substring($varExDay,1,1)= '0'">
          <xsl:value-of select="substring($varExDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varExDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="normalize-space(concat('O:', $varUSymbol, ' ', $varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
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
          <xsl:when test="number($Quantity) and contains(COL50,'CASH')!='true' and COL2='038CAB8P2'">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
              </xsl:variable>
              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL14"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="PB_CountnerParty" select="COL5"/>
              <xsl:variable name="PRANA_CounterPartyID">
                <xsl:value-of select="document('../../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
              </xsl:variable>


              <xsl:variable name="varPBSymbol">
                <xsl:value-of select="COL14"/>
              </xsl:variable>

              <xsl:variable name="varDescription">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varNetPosition">
                <xsl:value-of select="COL27"/>
              </xsl:variable>



              <xsl:variable name="varFXConversionMethodOperator">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varFXRate">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varCommission">
                <xsl:value-of select="COL12"/>
              </xsl:variable>

              <xsl:variable name="varFees">
                <xsl:value-of select="0"/>
              </xsl:variable>

              <xsl:variable name="varMiscFees">
                <xsl:value-of select="COL16"/>
              </xsl:variable>

              <xsl:variable name="varClearingFee">
                <xsl:value-of select="COL22"/>
              </xsl:variable>

              <xsl:variable name="varStampDuty">
                <xsl:value-of select="COL23"/>
              </xsl:variable>



              <xsl:variable name="varMarketValue">
                <xsl:choose>
                  <xsl:when test="COL51 ='EQUITY SWAP'">
                    <xsl:value-of select="COL32"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="COL34"/>
                  </xsl:otherwise>
                </xsl:choose>

              </xsl:variable>

              <xsl:variable name="varMarketValueBase">
                <xsl:choose>
                  <xsl:when test="COL51 ='EQUITY SWAP'">
                    <xsl:value-of select="COL33"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="COL35"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varSuffix">
                <xsl:call-template name="GetSuffix">
                  <xsl:with-param name="Suffix" select="substring-after(COL14, ' ')"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <FundName>
                    <xsl:value-of select='$PB_FUND_NAME'/>
                  </FundName>
                </xsl:when>
                <xsl:otherwise>
                  <FundName>
                    <xsl:value-of select='$PRANA_FUND_NAME'/>
                  </FundName>
                </xsl:otherwise>
              </xsl:choose>


              <Asset>
                <xsl:choose>

                  <xsl:when test='substring-before(COL51, " ") = "Call" or substring-before(COL51, " ") = "Put"'>
                    <xsl:value-of select='Option'/>
                  </xsl:when>
                  <xsl:when test="COL51 ='EQUITY SWAP'">
                    <xsl:value-of select="'EquitySwap'"/>
                  </xsl:when>
                  <xsl:when test="COL51 ='Convertible Bond'">
                    <xsl:value-of select="'FixedIncome'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Asset>

              <xsl:variable name="varOption">
                <xsl:call-template name="ConvertBBCodetoTicker">
                  <xsl:with-param name="varBBCode" select="normalize-space(COL8)"/>
                </xsl:call-template>
              </xsl:variable>
              <Symbol>
                <xsl:choose>
                  <xsl:when test='$PRANA_SYMBOL_NAME != ""'>
                    <xsl:value-of select='$PRANA_SYMBOL_NAME'/>
                  </xsl:when>

                  <xsl:when test="contains(COL6,'PUT') or contains(COL6,'CALL')">
                    <xsl:value-of select="$varOption"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test='$varSuffix = "" and substring-after(COL14, " ") != "US"'>
                        <xsl:value-of select='COL14'/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select='concat(substring-before(COL14, " "), $varSuffix)'/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>


              <!--Side-->

              <Side>
                <xsl:choose>
                  <xsl:when test="COL50='PUTL'">
                    <xsl:choose>
                      <xsl:when test="COL29 = 'L'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:when test="COL29 = 'S'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:choose>

                      <xsl:when test="COL29 = 'L'">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:when test="COL29 = 'S'">
                        <xsl:value-of select="'Sell short'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>


                </xsl:choose>
              </Side>

              <!--QUANTITY-->

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($varNetPosition)">
                    <xsl:value-of select="$varNetPosition"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>


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

              <CurrencySymbol>
                <xsl:value-of select ="COL44"/>
              </CurrencySymbol>

              <MarketValue>
                <xsl:choose>
                  <xsl:when test ="boolean(number($varMarketValue))">
                    <xsl:value-of select="$varMarketValue"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test ="boolean(number($varMarketValueBase))">
                    <xsl:value-of select="$varMarketValueBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>



              <SEDOL>
                <xsl:value-of select="normalize-space(COL9)"/>
              </SEDOL>



              <xsl:if test="COL51 ='EQUITY SWAP'">

                <IsSwapped>
                  <xsl:value-of select ="1"/>
                </IsSwapped>

                <SwapDescription>
                  <xsl:value-of select ="'SWAP'"/>
                </SwapDescription>

                <DayCount>
                  <xsl:value-of select ="365"/>
                </DayCount>

                <ResetFrequency>
                  <xsl:value-of select ="'Monthly'"/>
                </ResetFrequency>

                <OrigTransDate>

                  <xsl:value-of select="COL53"/>
                </OrigTransDate>


                <xsl:variable name="varYear">
                  <xsl:choose>
                    <xsl:when test="contains(COL53,'/')">
                      <xsl:value-of select="substring-after(substring-after(COL53,'/'),'/')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after(substring-after(COL53,'-'),'-')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="Day">
                  <xsl:choose>
                    <xsl:when test="contains(COL53,'/')">
                      <xsl:choose>
                        <xsl:when test="string-length(number(substring-before(substring-after(COL53,'/'),'/'))) = 1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL53,'/'),'/'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL53,'/'),'/')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test="string-length(number(substring-before(substring-after(COL53,'-'),'-'))) = 1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL53,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL53,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="Month">
                  <xsl:choose>
                    <xsl:when test="contains(COL53,'/')">
                      <xsl:choose>
                        <xsl:when test="string-length(number(substring-before(COL53,'/'))) = 1">
                          <xsl:value-of select="concat(0,substring-before(COL53,'/'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL53,'/')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test="string-length(number(substring-before(COL53,'-'))) = 1">
                          <xsl:value-of select="concat(0,substring-before(COL53,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL53,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


                <xsl:variable name="SettleDate">
                  <xsl:value-of select='my:Now(number($varYear),number($Month),number($Day))'/>
                </xsl:variable>

                <FirstResetDate>
                  <xsl:value-of select ="substring-before($SettleDate,' ')"/>
                </FirstResetDate>

              </xsl:if>


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

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


