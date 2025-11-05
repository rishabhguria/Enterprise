<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public static string NowSwap(int year, int month, int date)
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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
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

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'UBS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="substring-before(COL6,' Equity')"/>
            </xsl:variable>

            <xsl:variable name="varBloombergSymbol">
              <xsl:value-of select="concat($varBloomberg,' EQUITY')"/>
            </xsl:variable>
            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varBloombergSymbol!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </Symbol>

            <Bloomberg>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varBloombergSymbol!='*'">
                  <xsl:value-of select="normalize-space($varBloombergSymbol)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>

            </Bloomberg>

            <xsl:variable name="PB_FUND_NAME" select="COL4"/>

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

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
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

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

            <xsl:variable name="Side" select="normalize-space(COL9)"/>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Side='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$Side='Sell short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:when test="$Side='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

                <xsl:when test="$Side='Buy to Open'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>

                <xsl:when test="$Side='Buy to Close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>

                <xsl:when test="$Side='Sell to Open'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>

                <xsl:when test="$Side='Sell to Close'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

           

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name="varPositionStartDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL1"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varPositionSettlementDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL2"/>
              </xsl:call-template>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

            <!--<OriginalPurchaseDate>
              <xsl:value-of select="$varOriginalPurchaseDate"/>
            </OriginalPurchaseDate>-->

            <PositionSettlementDate>
              <xsl:value-of select="$varPositionSettlementDate"/>
            </PositionSettlementDate>

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="COL3='EquitySwap'">
                  <xsl:value-of select="'EquitySwap'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varOrigTransDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL1"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:if test="$Asset='EquitySwap'">

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
                <xsl:value-of select ="$varOrigTransDate"/>
              </OrigTransDate>

             
              <xsl:variable name="varYear">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'/'),'/')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'-'),'-')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Day">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'/'),'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'/'),'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'/'),'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'-'),'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'-'),'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'-'),'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Month">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name="SettleDate">
                <xsl:value-of select='my:NowSwap(number($varYear),number($Month),number($Day))'/>
              </xsl:variable>

              <FirstResetDate>
                <xsl:value-of select ="$SettleDate"/>
              </FirstResetDate>
            </xsl:if>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>