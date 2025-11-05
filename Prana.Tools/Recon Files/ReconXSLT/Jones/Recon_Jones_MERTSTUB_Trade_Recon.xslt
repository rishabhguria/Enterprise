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
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
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
            <xsl:with-param name="Number" select="COL13"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Quantity) and not(contains(COL21,'CASH'))">
        <PositionMaster>
          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>
          <xsl:variable name = "PB_COMPANY_NAME" >
            <xsl:value-of select="COL9"/>
          </xsl:variable>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
          </xsl:variable>
          <xsl:variable name = "PB_FUND_NAME" >
            <xsl:value-of select="COL1"/>
          </xsl:variable>
          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <AccountName>
            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME!=''">
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountName>
          
          <xsl:variable name="Symbol">
            <xsl:value-of select="COL80"/>
          </xsl:variable>
          <Symbol>
            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
              </xsl:when>
           
              <xsl:when test="$Symbol!='*'">
                <xsl:value-of select="$Symbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COMPANY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>
        


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
              <xsl:with-param name="Number" select="COL60"/>
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
          <xsl:variable name="Side" select="COL16"/>
          <Side>           
                <xsl:choose>
                  <xsl:when test="$Side='B'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="$Side='S'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="$Side='SS'">
                    <xsl:value-of select="'Sell Short'"/>
                  </xsl:when>
                  <xsl:when test="$Side='BC'">
                    <xsl:value-of select="'Buy to Close'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>              
            </xsl:choose>
          </Side>
          <xsl:variable name="Secfee">
            <xsl:call-template name="Translate">
              <xsl:with-param name="Number" select="COL63"/>
            </xsl:call-template>
          </xsl:variable>
          <SecFee>
            <xsl:choose>
              <xsl:when test="$Secfee &gt; 0">
                <xsl:value-of select="$Secfee"/>
              </xsl:when>
              <xsl:when test="$Secfee &lt; 0">
                <xsl:value-of select="$Secfee * (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecFee>

          <xsl:variable name="PB_BROKER_NAME">
            <xsl:value-of select="COL19"/>
          </xsl:variable>
          <xsl:variable name="PRANA_BROKER_ID">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
          </xsl:variable>
          <CounterPartyID>
            <xsl:choose>
              <xsl:when test="number($PRANA_BROKER_ID)">
                <xsl:value-of select="$PRANA_BROKER_ID"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </CounterPartyID>

          <xsl:variable name="Fees">
            <xsl:call-template name="Translate">
              <xsl:with-param name="Number" select="''"/>
            </xsl:call-template>
          </xsl:variable>
          <OtherBrokerFees>
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
          </OtherBrokerFees>


          <xsl:variable name="varPositionStartDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="DateTime" select="COL24"/>
            </xsl:call-template>
          </xsl:variable>
          <xsl:variable name="varMonth">
            <xsl:value-of select="substring-before(substring-after(COL24,'-'),'-')"/>
          </xsl:variable>
          <xsl:variable name="varDay">
            <xsl:value-of select="substring-before(COL24,'-')"/>
          </xsl:variable>
          <xsl:variable name="varYear">
            <xsl:value-of select="substring-after(substring-after(COL24,'-'),'-')"/>
          </xsl:variable>

          <TradeDate>
            <xsl:value-of select="$varPositionStartDate"/>
          </TradeDate>

          <xsl:variable name="varPositionSettlementDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="DateTime" select="COL29"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varSMonth">
            <xsl:value-of select="substring-before(substring-after(COL29,'-'),'-')"/>
          </xsl:variable>
          <xsl:variable name="varSDay">
            <xsl:value-of select="substring-before(COL29,'-')"/>
          </xsl:variable>
          <xsl:variable name="varSYear">
            <xsl:value-of select="substring-after(substring-after(COL29,'-'),'-')"/>
          </xsl:variable>
          <SettlementDate>
            <xsl:value-of select="$varPositionSettlementDate"/>
          </SettlementDate>

          <xsl:variable name="Commission">
            <xsl:call-template name="Translate">
              <xsl:with-param name="Number" select="COL62"/>
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

        
          <PBSymbol>
            <xsl:value-of select="$PB_COMPANY_NAME"/>
          </PBSymbol>
          
          <xsl:variable name="NetNotionalValue">
            <xsl:call-template name="Translate">
              <xsl:with-param name="Number" select="COL49"/>
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

          <!--<CounterParty>
            <xsl:choose>
              <xsl:when test="COL67='Jones Trading'">
                <xsl:value-of select="'JONE'"/>
              </xsl:when>


              <xsl:otherwise>
                <xsl:value-of select="COL67"/>
              </xsl:otherwise>

            </xsl:choose>
          </CounterParty>-->

        </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>