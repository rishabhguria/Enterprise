<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
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

  <xsl:template name="MonthsCode">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth=01">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth=02">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth=03">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth=04">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth=05">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth=06">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth=07">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth=08">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth=09">
        <xsl:value-of select="'U'"/>
      </xsl:when>
      <xsl:when test="$varMonth=10">
        <xsl:value-of select="'V'"/>
      </xsl:when>
      <xsl:when test="$varMonth=11">
        <xsl:value-of select="'X'"/>
      </xsl:when>
      <xsl:when test="$varMonth=12">
        <xsl:value-of select="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(COL18,'/')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(COL9,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),2,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),4,2)"/>
      </xsl:variable>


      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL9),' '),8),'##.##')"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$ExpiryMonth,$StrikePrice,'D',$Day)"/>
    </xsl:if>
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
            <xsl:with-param name="Number" select="COL14"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) ">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'DavidCapital'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_ROOT_NAME">
              <xsl:value-of select="COL34"/>
            </xsl:variable>         

            <xsl:variable name ="UnderlyingCounterParty">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@CounterParty"/>
            </xsl:variable>

            <xsl:variable name ="PRANA_ROOT_NAME">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
            </xsl:variable>

            <xsl:variable name ="FUTURE_EXCHANGE_CODE">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExchangeCode"/>
            </xsl:variable>

            <xsl:variable  name="FUTURE_FLAG">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
            </xsl:variable>

            <xsl:variable name="MonthCode">              
              <xsl:value-of select="substring(COL2,string-length(COL2)-1,1)"/>
            </xsl:variable>

            <xsl:variable name="Year" select="substring(COL2,string-length(COL2),1)"/>

            <xsl:variable name="MonthYearCode">
              <xsl:choose>
                <xsl:when test="$FUTURE_FLAG!=''">
                  <xsl:value-of select="concat($Year,$MonthCode)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($MonthCode,$Year)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Underlying">
              <xsl:choose>
                <xsl:when test="$PRANA_ROOT_NAME!=''">
                  <xsl:value-of select="$PRANA_ROOT_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_ROOT_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->


            <xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test="contains(COL8,' ')">
                  <xsl:value-of select="substring-before(COL8,' ')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL8"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varSedol">
              <xsl:value-of select="COL12"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol =''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSymbol !=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSymbol =''">
                  <xsl:value-of select="$varSedol"/>
                </xsl:when>
                <xsl:when test="$varSymbol !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>


            <xsl:variable name="PB_FUND_NAME" select="COL19"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>

                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL16"/>
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
            <xsl:variable name="varCommission1">
              <xsl:choose>
                <xsl:when test="COL31 &gt; 0">
                  <xsl:value-of select="COL31"/>
                </xsl:when>
                <xsl:when test="COL31 &lt; 0">
                  <xsl:value-of select="COL31 * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="varCommission2">
              <xsl:choose>
                <xsl:when test="COL28 &gt; 0">
                  <xsl:value-of select="COL28"/>
                </xsl:when>
                <xsl:when test="COL28 &lt; 0">
                  <xsl:value-of select="COL28 * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="varCommission">
              <xsl:value-of select="$varCommission1 - $varCommission2"/>
            </xsl:variable>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$varCommission"/>
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


            <xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>
            <MiscFees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>
                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

            <xsl:variable name="StampFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
              </xsl:call-template>
            </xsl:variable>
            <StampDuty>
              <xsl:choose>
                <xsl:when test="$StampFees &gt; 0">
                  <xsl:value-of select="$StampFees"/>
                </xsl:when>
                <xsl:when test="$StampFees &lt; 0">
                  <xsl:value-of select="$StampFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>


            <xsl:variable name="varClearingFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>
            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$varClearingFee &gt; 0">
                  <xsl:value-of select="$varClearingFee"/>
                </xsl:when>
                <xsl:when test="$varClearingFee &lt; 0">
                  <xsl:value-of select="$varClearingFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClearingFee>

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL25"/>
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


            <xsl:variable name="varNetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
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


            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL1,33,2)"/>
            </xsl:variable>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL1,35,2)"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL1,37,4)"/>
            </xsl:variable>

            <xsl:variable name="varOriginalDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL1"/>
              </xsl:call-template>
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="$varOriginalDate"/>
            </TradeDate>

            <SettlementDate>
              <xsl:value-of select ="''"/>
            </SettlementDate>

            <xsl:variable name ="Side" select="COL3"/>
            <Side>
              <xsl:choose>
                <xsl:when test="$Side='Buy to Cover'">
                  <xsl:value-of select="'Buy to Close'"/>
                </xsl:when>
                <xsl:when test="$Side='Buy'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$Side='Sell'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:when test="$Side='Sell Short'">
                  <xsl:value-of select="'Sell Short'"/>
                </xsl:when>
              </xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


