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
    <xsl:if test="substring-before(COL11,' ')='CALL' or substring-before(COL11,' ')='PUT'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,' '),2,2)"/>
      </xsl:variable>
      <!--<xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after(normalize-space(COL1),' '),1,2)"/>
      </xsl:variable>-->
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,' '),4,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,' '),6),'#.00')"/>
      </xsl:variable>

      <!--<xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>-->
      <xsl:variable name="MonthCodeVar">
        <xsl:value-of select="substring(substring-after($Symbol,' '),1,1)"/>
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
            <xsl:with-param name="Number" select="COL77"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and COL73='Stock'">
            <PositionMaster>
              <xsl:variable name = "PB_NAME">
                <xsl:value-of select="'Bank of America Merrill Lynch'"/>
              </xsl:variable>

              <xsl:variable name = "PB_SYMBOL_NAME" >
                <xsl:value-of select ="COL25"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="Asset1">
                <xsl:choose>
                  <xsl:when test="substring-before(COL61,' ')='CALL' or substring-before(COL61,' ')='PUT'">
                    <xsl:value-of select="'Option'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Symbol" select="substring-before(COL36,' ')"/>
				<xsl:variable name="Sedol" select="COL28"/>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$Asset1='Option'">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="normalize-space(COL36)"/>
                      <xsl:with-param name="Suffix" select="''"/>
                    </xsl:call-template>
                  </xsl:when>
					<xsl:when test="COL49 !='US'">
						<xsl:value-of select="''"/>
					</xsl:when>

					<xsl:when test="$Symbol !=''">
                    <xsl:value-of select="translate($Symbol,$lower_CONST,$upper_CONST)"/>
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

						<xsl:when test="$Asset1='Option'">
							<xsl:call-template name="Option">
								<xsl:with-param name="Symbol" select="''"/>
								<xsl:with-param name="Suffix" select="''"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="COL49 !='US'">
							<xsl:value-of select="$Sedol"/>
						</xsl:when>

						<xsl:when test="$Symbol !=''">
							<xsl:value-of select="''"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</SEDOL>

              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="COL8"/>
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
                  <xsl:when test="$Asset1='Option'">
                    <xsl:choose>

                      <xsl:when test="$Quantity &gt; 0">
                        <xsl:value-of select="'Buy to open'"/>
                      </xsl:when>

                      <xsl:when test="$Quantity &gt; 1">
                        <xsl:value-of select="'Buy to close'"/>
                      </xsl:when>

                      <xsl:when test="$Quantity &lt; 0">
                        <xsl:value-of select="'Sell to open'"/>
                      </xsl:when>

                      <xsl:when test="$Quantity &lt; 1">
                        <xsl:value-of select="'Buy to close'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
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

              <xsl:variable name="MarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL85"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="MarketValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL87"/>
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

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <SettlementDate>
                <xsl:value-of select ="''"/>
              </SettlementDate>

              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL79"/>
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
              <xsl:variable name="MarkPriceBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL81"/>
                </xsl:call-template>
              </xsl:variable>
       
              <MarkPriceBase>
                <xsl:choose>
                  <xsl:when test="$MarkPriceBase &gt; 0">
                    <xsl:value-of select="$MarkPriceBase"/>
                  </xsl:when>
                  <xsl:when test="$MarkPriceBase &lt; 0">
                    <xsl:value-of select="$MarkPriceBase * (-1)"/>
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
                <xsl:value-of select ="''"/>
              </xsl:variable>
              <FXRate>
                <xsl:choose>
                  <xsl:when test ="$FXRate ">
                    <xsl:value-of select ="$FXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select ="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FXRate>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>


              <SMRequest>
                <xsl:value-of select="'TRUE'"/>
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
                <xsl:value-of select="'TRUE'"/>
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



