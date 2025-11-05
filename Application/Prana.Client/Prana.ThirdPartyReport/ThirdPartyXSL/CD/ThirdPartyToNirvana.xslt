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

  <xsl:template name="GetMonth">
    <xsl:param name="varMonthNo"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonthNo='01' and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='02' and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='03' and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='04' and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='05' and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='06' and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='07' and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='08' and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='09' and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='10' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='11' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='12' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='01' and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='02' and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='03' and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='04' and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='05' and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='06' and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='07' and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='08' and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='09' and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='10' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='11' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='12' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="Symbol">
    <xsl:param name="varCurrency"/>
    <xsl:choose>
      <xsl:when test="$varCurrency = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'DKK'">
        <xsl:value-of select="'-OMX'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'GBP'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'HKD'">
        <xsl:value-of select="'-HKG'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'SGD'">
        <xsl:value-of select="'-SES'"/>
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="tempEquityExchangeOtherThanUS">
    <xsl:param name="paramExchange"/>
    <xsl:choose>
      <xsl:when test ="$paramExchange='CAD' ">
        <xsl:value-of select ="'TC'"/>
      </xsl:when>
      <xsl:when test ="$paramExchange='EUR' ">
        <xsl:value-of select ="'FRA'"/>
      </xsl:when>
      <xsl:when test ="$paramExchange='AUD'">
        <xsl:value-of select ="'ASX'"/>
      </xsl:when>
      <xsl:when test ="$paramExchange='HKD'">
        <xsl:value-of select ="'HKG'"/>
      </xsl:when>
      <xsl:when test ="$paramExchange='GBP'">
        <xsl:value-of select ="'LON'"/>
      </xsl:when>
      <xsl:when test ="$paramExchange='JPY'">
        <xsl:value-of select ="'TSE'"/>
      </xsl:when>
      <xsl:when test ="$paramExchange='SGD'">
        <xsl:value-of select ="'SES'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Month">
    <xsl:param name="varMonth"/>
    <!-- 2 characters for month code -->
    <!-- month Codes e.g. 01 represents 01 = January-->
    <xsl:choose>
      <xsl:when test ="$varMonth='A'">
        <xsl:value-of select ="'01'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='B'">
        <xsl:value-of select ="'02'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='C'">
        <xsl:value-of select ="'03'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='D'">
        <xsl:value-of select ="'04'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='E'">
        <xsl:value-of select ="'05'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='F'">
        <xsl:value-of select ="'06'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='G'">
        <xsl:value-of select ="'07'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='H'">
        <xsl:value-of select ="'08'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='I'">
        <xsl:value-of select ="'09'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='J'">
        <xsl:value-of select ="'10'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='K'">
        <xsl:value-of select ="'11'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='L'">
        <xsl:value-of select ="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:choose>
          <xsl:when test="number(COL23) and (normalize-space(COL4) = 'Topwater Opportunity Fund LP - Tigris' or normalize-space(COL4) = 'Topwater Opportunity Account LP-Jackdaw' or normalize-space(COL4) = 'Topwater Opportunity Qualified Account LP - Loeb' or normalize-space(COL4) = 'Topwater Opportunity Qualified Account LP - Loeb Asia' or normalize-space(COL4) = 'Topwater Opportunity Qualified Account LP-Cove')">

            <PositionMaster>
              
              <PositionStartDate>
                <xsl:value-of select="concat(substring(COL1,5,2),'/',substring(COL1,7,2),'/',substring(COL1,1,4))"/>
              </PositionStartDate>

              <PBSymbol>
                <xsl:value-of select="COL13"/>
              </PBSymbol>

              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <xsl:variable name = "PB_SymbolCurrency_NAME" >
                <xsl:value-of select="COL15"/>
              </xsl:variable>

              <xsl:variable name = "PB_Currency_NAME" >
                <xsl:value-of select="COL19"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SymbolCurrency_NAME">
                <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@CompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <FundName>
                <xsl:value-of select="substring-after(COL4, '-')"/>
              </FundName>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SymbolCurrency_NAME=''">
                    <xsl:choose>
                      <xsl:when test="COL19='USD' or COL12 = 'Equity Future'">
                        <xsl:value-of select="COL15"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test="COL19='AUD'">
                            <xsl:value-of select="concat(substring-before(COL15,'.'),'-ASX')"/>
                          </xsl:when>
                          <xsl:when test="COL19='NZD'">
                            <xsl:value-of select="concat(substring-before(COL15,'.'),'-NZX')"/>
                          </xsl:when>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>

                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="COL29 &gt; 0">
                    <xsl:value-of select="COL29"/>
                  </xsl:when>
                  <xsl:when test="COL29 &lt; 0">
                    <xsl:value-of select="COL29*(-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="COL23 &gt; 0">
                    <xsl:value-of select="COL23"/>
                  </xsl:when>
                  <xsl:when test="COL23 &lt; 0">
                    <xsl:value-of select="COL23*(-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <PBAssetName>
                <xsl:value-of select ="''"/>
              </PBAssetName>

              <Side>
                <xsl:choose>
                  <xsl:when test="COL23 &gt; 0">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="COL23 &lt; 0">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <MarketValue>
                <xsl:value-of select="COL33"/>
              </MarketValue>

              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>
              
            </PositionMaster>
            
          </xsl:when>
          
          <xsl:when test="(normalize-space(COL11)= 'EQ' or normalize-space(COL11)= 'PS' or normalize-space(COL11) = 'B') and COL10 = 'T' and contains(COL6,'Topwater Investment') !=false and COL3 != 'TOPTIG' and COL3 != '10608998' and (substring(COL3,1,3) = '106' or COL3 = 'GRVMF' or COL3 = 'PMFG' or COL3 = 'SAPMFLP' or COL3 = 'SENS2' or COL3 = 'SSAPMF' or COL3 = 'TOOQ' or COL3 = 'TOPBARREN' or COL3 = 'TOPJACKDAW' or COL3 = 'TOPLOEB' or COL3 = 'TOPTIG' or COL3 = 'TOQFLPP' or COL3 = 'TOQLOEB') ">
            <PositionMaster>
              
              <xsl:variable name="varSecurityType" select="COL12"/>
              <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL22)"/>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='DB']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
              </xsl:variable>


              <xsl:variable name ="varExchange">
                <xsl:choose>
                  <xsl:when test ="COL35 != 'USD'">
                    <xsl:call-template name="tempEquityExchangeOtherThanUS">
                      <xsl:with-param name="paramExchange" select="COL35" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name ="varSymbol">
                <xsl:choose>
                  <xsl:when test ="COL35 != 'USD'">
                    <xsl:choose>
                      <xsl:when test ="normalize-space(COL11)= 'B' ">
                        <xsl:value-of select ="COL16"/>
                      </xsl:when>

                      <xsl:when test ="contains(COL14,'.TO')!=False and normalize-space(COL11)!= 'B' ">
                        <xsl:value-of select ="concat(substring-before(COL14,'.'),'-TC')"/>
                      </xsl:when>

                      <xsl:when test ="contains(COL14,'.T')!=False and normalize-space(COL11)!= 'B' ">
                        <xsl:value-of select ="concat(substring-before(COL14,'.'),'-TSE')"/>
                      </xsl:when>

                      <xsl:when test ="contains(COL14,'.OS')!=False and normalize-space(COL11)!= 'B' ">
                        <xsl:value-of select ="concat(substring-before(COL14,'.'),'-OSE')"/>
                      </xsl:when>

                      <xsl:when test ="contains(COL14,'.OJ')!=False and normalize-space(COL11)!= 'B' ">
                        <xsl:value-of select ="concat(substring-before(COL14,'.'),'-JAQ')"/>
                      </xsl:when>

                      <xsl:when test ="contains(COL14,'.HK')!=False and normalize-space(COL11)!= 'B' ">
                        <xsl:value-of select ="concat(substring-before(COL14,'.'),'-HKG')"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select ="COL14"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test ="COL12='Corp Bond' or COL12='Govt Bond'">
                        <xsl:value-of select ="COL16"/>
                      </xsl:when>
                      <xsl:when test ="contains(COL14,'.')='false'">
                        <xsl:value-of select="substring-before(COL14,'.')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select ="COL14"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name ="varPBSymbol">
                <xsl:choose>
                  <xsl:when test ="contains($varSymbol,'_') != False ">
                    <xsl:value-of select ="concat(substring-before($varSymbol,'_'),'/',translate(substring-after($varSymbol,'_'),$vLowercaseChars_CONST,$vUppercaseChars_CONST))"/>
                  </xsl:when>
                  <xsl:when test ="contains($varSymbol,'_') = False">
                    <xsl:value-of select ="$varSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$varSymbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varPBSymbol_Length" select="string-length($varPBSymbol)"/>
              <xsl:variable name="varOption_Underlying" select="substring($varPBSymbol,1,$varPBSymbol_Length -10)"/>
              <xsl:variable name="varOption_Year" select="substring($varPBSymbol,$varPBSymbol_Length -6,2)"/>
              <xsl:variable name="varMonthCode" select="translate(substring($varPBSymbol,$varPBSymbol_Length -9,1),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>

              <xsl:variable name = "varOption_ExpirationMonth" >
                <xsl:call-template name="Month">
                  <xsl:with-param name="varMonth" select="$varMonthCode" />
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varOption_StrikePrice" >
                <xsl:if test="string-length(COL16) > 8">
                  <xsl:value-of select="substring(COL16,1,string-length(COL16)-8)"/>
                </xsl:if>
              </xsl:variable>

              <xsl:variable name="varOPTStrikePrice">
                <xsl:if test="$varOption_StrikePrice != ''">
                  <xsl:value-of select="substring(COL16,string-length($varOption_StrikePrice) + 1,8)"/>
                </xsl:if>
              </xsl:variable>

              <xsl:variable name="varStrikePrice">
                <xsl:if test="$varOPTStrikePrice != ''">
                  <xsl:value-of select="format-number(concat(substring($varOPTStrikePrice,1,5),'.',substring($varOPTStrikePrice,6,3)),'#.00')"/>
                </xsl:if>
              </xsl:variable>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME=''">
                  <xsl:choose>
                    <xsl:when test ="$varSecurityType='Equity Option'">
                      <Symbol>
                        <xsl:value-of select ="concat('O:',$varOption_Underlying,' ',$varOption_Year,$varMonthCode,$varStrikePrice)"/>
                      </Symbol>
                    </xsl:when>
                    <xsl:when test ="$varSecurityType !='Equity Option'">
                      <Symbol>
                        <xsl:value-of select ="$varPBSymbol"/>
                      </Symbol>
                    </xsl:when>
                    <xsl:otherwise>
                      <Symbol>
                        <xsl:value-of select="''"/>
                      </Symbol>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <Symbol>
                    <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                  </Symbol>
                </xsl:otherwise>
              </xsl:choose>

              <PBSymbol>
                <xsl:value-of select="$varPBSymbol"/>
              </PBSymbol>

              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="COL3"/>
              </xsl:variable>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

              <PositionStartDate>
                <xsl:value-of select="''"/>
              </PositionStartDate>


              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="number(COL29)">
                    <xsl:value-of select="COL29"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="COL26 &gt; 0">
                    <xsl:value-of select="COL26"/>
                  </xsl:when>

                  <xsl:when test="COL26 &lt; 0">
                    <xsl:value-of select="COL26*(-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <Side>
                <xsl:choose>
                  <xsl:when test="COL26 &gt;0">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>

                  <xsl:when test="COL26 &lt;0">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <PBAssetName>
                <xsl:value-of select ="COL12"/>
              </PBAssetName>

              <MarketValue>
                <xsl:value-of select ="COL30"/>
              </MarketValue>

              <MarketValueBase>
                <xsl:value-of select="COL40"/>
              </MarketValueBase>

        </PositionMaster>
          </xsl:when>

          <xsl:when test="number(COL21) and (COL2 = '10242007' or COL2 = '10242009' or COL2 = '10244034')">
            <PositionMaster>
              <!--   Fund -->
              <!--fundname section-->
              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <xsl:variable name = "PB_Symbol_NAME" >
                <xsl:value-of select="COL10"/>
              </xsl:variable>

              <xsl:variable name="PRANA_Symbol_NAME">
                <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='DB']/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
              </xsl:variable>



              <xsl:choose>

                <xsl:when test="normalize-space(COL4)='OPTIONS'">

                  <xsl:variable name="varExpirationDate">
                    <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL10),' '),' '),' ')"/>
                  </xsl:variable>

                  <xsl:variable name="varYear">
                    <xsl:value-of select="substring($varExpirationDate,7,2)"/>
                  </xsl:variable>

                  <xsl:variable name="varMonth">
                    <xsl:value-of select="substring($varExpirationDate,1,2)"/>
                  </xsl:variable>

                  <xsl:variable name="varDateNo">
                    <xsl:value-of select="substring($varExpirationDate,4,2)"/>
                  </xsl:variable>

                  <xsl:variable name="varThirdFriday">
                    <xsl:value-of select =" my:Now(number(concat('20',$varYear)),number($varMonth))"/>
                  </xsl:variable>


                  <xsl:variable name="varIsFlex">
                    <xsl:choose>
                      <xsl:when test="(substring-before(substring-after($varThirdFriday,'/'),'/') + 1) = number($varDateNo)">
                        <xsl:value-of select="0"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="1"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>

                  <xsl:variable name="varDate">
                    <xsl:choose>
                      <xsl:when test="substring(COL27,4,1)='0'">
                        <xsl:value-of select="substring(COL27,5,2)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring(COL27,4,2)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>

                  <xsl:variable name="MonthCode">
                    <xsl:call-template name="GetMonth">
                      <xsl:with-param name="varMonthNo" select="$varMonth"/>
                      <xsl:with-param name="varPutCall" select="COL26"/>
                    </xsl:call-template>
                  </xsl:variable>
                  <Symbol>
                    <xsl:choose>
                      <xsl:when test="$varIsFlex = 0">
                        <xsl:value-of select="concat('O:',normalize-space(COL16),' ',$varYear,$MonthCode,format-number(COL28,'#.00'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat('O:',normalize-space(COL16),' ',$varYear,$MonthCode,format-number(COL28,'#.00'),'D',$varDate)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Symbol>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:variable name="varSuffix">
                    <xsl:call-template name="Symbol">
                      <xsl:with-param name="varCurrency" select="COL3"/>
                    </xsl:call-template>
                  </xsl:variable>
                  <Symbol>
                    <xsl:choose>
                      <xsl:when test="$PRANA_Symbol_NAME = ''">
                        <xsl:choose>
                          <xsl:when test="string-length(normalize-space(COL12))=4 and (COL3 = 'HKD' or COL3 = 'SGD' or COL3 = 'JPY')">
                            <xsl:value-of select="concat(COL12,$varSuffix)"/>
                          </xsl:when>
                          <xsl:when test="string-length(normalize-space(COL12))=3 and (COL3 = 'HKD' or COL3 = 'SGD' or COL3 = 'JPY')">
                            <xsl:value-of select="concat('0',COL12,$varSuffix)"/>
                          </xsl:when>
                          <xsl:when test="string-length(normalize-space(COL12))=2 and (COL3 = 'HKD' or COL3 = 'SGD' or COL3 = 'JPY')">
                            <xsl:value-of select="concat('00',COL12,$varSuffix)"/>
                          </xsl:when>
                          <xsl:when test ="COL3 = 'CAD' and contains(COL12,'/') != false">
                            <xsl:value-of select ="concat(substring-before(COL12,'/'),'.',substring-after(COL12,'/'),$varSuffix)"/>
                          </xsl:when>
                          <xsl:when test ="normalize-space(COL4)='FIXED INCOME'">
                            <xsl:value-of select ="translate(normalize-space(COL15),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
                          </xsl:when>

                          <xsl:otherwise>
                            <xsl:value-of select ="concat(COL12,$varSuffix)"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$PRANA_Symbol_NAME"/>
                      </xsl:otherwise>

                    </xsl:choose>
                  </Symbol>
                </xsl:otherwise>

              </xsl:choose>


              <PBSymbol>
                <xsl:value-of select="COL10"/>
              </PBSymbol>


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

              <PositionStartDate>
                <xsl:value-of select="COL11"/>
              </PositionStartDate>

              <MarkPrice>
                <xsl:choose>
                  <xsl:when test ="boolean(number(COL22))">
                    <xsl:value-of select="COL22"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="COL21 &lt; 0">
                    <xsl:value-of select="COL21 * (-1)"/>
                  </xsl:when>
                  <xsl:when test="COL21 &gt; 0">
                    <xsl:value-of select="COL21"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <Side>
                <xsl:choose>
                  <xsl:when test="COL21 &lt; 0">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>

                  <xsl:when test="COL21 &lt;0">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <PBAssetName>
                <xsl:value-of select ="COL12"/>
              </PBAssetName>

              <MarketValue>
                <xsl:choose>
                  <xsl:when test ="boolean(number(COL23))">
                    <xsl:value-of select="COL23"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test ="boolean(number(COL30))">
                    <xsl:value-of select="COL30"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>

            </PositionMaster>
          </xsl:when>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  
</xsl:stylesheet>