<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
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

  <xsl:template name ="MonthCode">
    <xsl:param name ="varMonth"/>
    <xsl:param name ="varPutCall"/>
    <xsl:choose>
      <xsl:when  test ="$varMonth=1 and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=2 and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=3 and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=4 and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=5 and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=6 and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=7 and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=8 and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=9 and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when  test =" $varMonth=10 and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=11 and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=12  and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=1 and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=2 and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=3 and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=4 and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=5 and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=6 and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=7 and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=8 and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=9 and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=10 and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=11 and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when  test ="$varMonth=12 and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Option">

    <xsl:param name="varSymbol"/>

    <xsl:param name="varSecurityType"/>

    <xsl:variable name ="varUnderlyingSymbol">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select="substring-before($varSymbol,' ')"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select ="number(substring(substring-after($varSymbol,' '),1,2))"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select ="number(substring(substring-after($varSymbol,' '),3,2))"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varExDay">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select ="substring(substring-after($varSymbol,' '),5,2)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varPutCall">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select ="substring(substring-after($varSymbol,' '),7,1)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varStrikePriceInt">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select ="substring(substring-after($varSymbol,' '),8,5)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varStrikePriceDec">
      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION'">
          <xsl:value-of select ="substring(substring-after($varSymbol,' '),13,3)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varStrikePrice">
      <xsl:choose>
        <xsl:when test="number($varStrikePriceInt) or number($varStrikePriceDec)">
          <xsl:value-of select ="format-number(concat($varStrikePriceInt,'.',$varStrikePriceDec),'#.00')"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varMonthCode">
      <xsl:call-template name ="MonthCode">
        <xsl:with-param name ="varMonth" select ="number($varMonth)"/>
        <xsl:with-param name ="varPutCall" select="$varPutCall"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="varDays">
      <xsl:choose>
        <xsl:when test="substring($varExDay,1,1)='0'">
          <xsl:value-of select="substring($varExDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varExDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varThirdFriday">

      <xsl:choose>
        <xsl:when test ="$varSecurityType='OPTION' and number($varYear) and number($varMonth)">
          <xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
        </xsl:when>
      </xsl:choose>

    </xsl:variable>


    <xsl:choose>
      <xsl:when test ="$varSecurityType='OPTION'">
        <xsl:choose>
          <xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
            <xsl:choose>
              <xsl:when test="COL16='CAD'">
                <xsl:value-of select="concat('O:',$varUnderlyingSymbol,'-TC',' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="COL16='CAD'">
                <xsl:value-of select="concat('O:',$varUnderlyingSymbol,'-TC',' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL12)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varAsset" select="normalize-space(COL5)"/>
        
        <xsl:choose>
          <xsl:when test="number($Quantity) and $varAsset!='Currency'">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Scotia'"/>
              </xsl:variable>
              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL4)"/>
              </xsl:variable>

              <xsl:variable name="CUSIP" select="COL7"/>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:choose>
                  <xsl:when test="$varAsset='OPTION'">
                    <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varSuffix">
                <xsl:choose>
                  <xsl:when test="contains(substring-after(COL9,' '),'CT') or contains(substring-after(COL9,' '),'CN')">
                    <xsl:value-of select="'CAD'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'USD'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="PB_SUFFIX_NAME" select="$varSuffix"/>

              <xsl:variable name="PRANA_SUFFIX_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
              </xsl:variable>

              <xsl:variable name="varOptionTicker">
                <xsl:call-template name="Option">
                  <xsl:with-param name="varSymbol" select="normalize-space(COL10)"/>
                  <xsl:with-param name="varSecurityType" select="$varAsset"/>
                </xsl:call-template>
              </xsl:variable>
              
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test ="$varAsset='OPTION'">
                    <xsl:value-of select ="$varOptionTicker"/>
                  </xsl:when>

                  <xsl:when test ="COL9!='*'">
                    <xsl:choose>
                      <xsl:when test="contains(COL9,'/')">
                        <xsl:value-of select="concat(substring-before(COL9,'/'),'.',substring(substring-after(COL9,'/'),1,1),$PRANA_SUFFIX_NAME)"/>
                      </xsl:when>
                      <xsl:when test="contains(COL9,'-')">
                        <xsl:choose>
                          <xsl:when test="substring(substring-after(COL9,'-'),1,1)='U'">
                            <xsl:value-of select="concat(substring-before(COL9,'-'),'.','UN',$PRANA_SUFFIX_NAME)"/>
                          </xsl:when>
                          <xsl:when test="substring(substring-after(COL9,'-'),1,1)='W'">
                            <xsl:value-of select="concat(substring-before(COL9,'-'),'.','WT',$PRANA_SUFFIX_NAME)"/>
                          </xsl:when>

                        </xsl:choose>


                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select ="concat(substring-before(COL9,' '),$PRANA_SUFFIX_NAME)"/>
                      </xsl:otherwise>
                    </xsl:choose>

                  </xsl:when>
                  
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
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

              <xsl:variable name="MarketValue">
                <xsl:choose>
                  <xsl:when test="$varAsset='OPTION'">
                    <xsl:value-of select="number(translate(COL15,',',''))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="number(translate(COL15,',',''))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <MarketValue>
                <xsl:value-of select="$MarketValue"/>
              </MarketValue>

              <xsl:variable name="MarketValueBase">
                <xsl:choose>
                  <xsl:when test="number(MarketValue)">
                    <xsl:value-of select="$MarketValue * number(COL18)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <MarketValueBase>
                <xsl:value-of select="$MarketValueBase"/>
              </MarketValueBase>

              <xsl:variable name="Side" select ="normalize-space(COL18)"/>
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

              <xsl:variable name="varAvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL14"/>
                </xsl:call-template>
              </xsl:variable>
              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$varAvgPX &gt; 0">
                    <xsl:value-of select="$varAvgPX"/>
                  </xsl:when>
                  <xsl:when test="$varAvgPX &lt; 0">
                    <xsl:value-of select="$varAvgPX * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <xsl:variable name="CurrencySymbol" select="$varSuffix"/>
              <CurrencySymbol>
                <xsl:value-of select="$CurrencySymbol"/>
              </CurrencySymbol>

              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>

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

              <FundName>
                <xsl:value-of select="''"/>
              </FundName>
              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>

              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>

              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>