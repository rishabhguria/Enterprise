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
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='JAN' and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='FEB' and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAR' and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='APR' and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAY' and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUN' and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUL' and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='AUG' and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='SEP' and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='OCT' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='NOV' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='DEC' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JAN' and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='FEB' and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAR' and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='APR' and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAY' and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUN' and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUL' and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='AUG' and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='SEP' and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='OCT' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='NOV' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='DEC' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="FutureMonthCode">
    <xsl:param name="varMonth"/>

    <!-- Call FUTURE month Codes -->
    <xsl:choose>
      <xsl:when test ="$varMonth= 1 ">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 2">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 3">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 4">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 5">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 6">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 7">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 8">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 9">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 10">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 11">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=12">
        <xsl:value-of select ="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varPBName">
          <xsl:value-of select="'Fidelity'"/>
        </xsl:variable>

        <xsl:if test ="number(COL12) and COL9!='FCASH' and COL9!='FDUXX' and COL9!='FZFXX' and COL9!='FDRXX'">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select ="COL10"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[normalize-space(@PBCompanyName)=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="PB_CountnerParty" select="COL5"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="COL26"/>
            </xsl:variable>

            <xsl:variable name="varBBCode">
              <xsl:value-of select="normalize-space(substring($varBloomberg, 1, 2))"/>
            </xsl:variable>

            <xsl:variable name ="varBBKey">
              <xsl:call-template name="substring-after-last">
                <xsl:with-param name="string" select="$varBloomberg"/>
                <xsl:with-param name="delimiter" select="' '"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMappedUnderlying">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
            </xsl:variable>

            <xsl:variable name="varUnderlying">
              <xsl:choose>
                <xsl:when test="$varMappedUnderlying = ''">
                  <xsl:value-of select="$varBBCode"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varMappedUnderlying"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name ="varStrikePrice">
              <xsl:choose>
                <xsl:when test ="COL2 = 'OPTION'">
                  <xsl:value-of select ="substring-before(substring-after(COL26,' '),' ')"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varStrikeMul">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@StrikeMul"/>
            </xsl:variable>

            <xsl:variable name ="varStrikeOld">
              <xsl:choose>
                <xsl:when test ="$varStrikeMul = ''">
                  <xsl:value-of select ="$varStrikePrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varStrikePrice*($varStrikeMul)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name ="varStrike">
              <xsl:choose>
                <xsl:when test = "contains($varStrikeOld, '.')">
                  <xsl:value-of select ="substring-before($varStrikeOld, '.')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varStrikeOld"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varExchangeSuffix">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeSuffix"/>
            </xsl:variable>

            <xsl:variable name="varFutureMonth">
              <xsl:call-template name="FutureMonthCode">
                <xsl:with-param name="varMonth" select="substring(COL4,7,2)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="COL29"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:value-of select="COL13"/>
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

            <xsl:variable name="varForwardExpiry">
              <xsl:choose>
                <xsl:when test ="contains(COL20,'--') != false">
                  <xsl:value-of select="substring-before(substring-after(COL20,'--'),'-')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after(substring-after(COL20,'-'),'-'),'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="COL26"/>
            </xsl:variable>


            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>


            <Symbol>
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
					</xsl:when>
					<xsl:when test ="COL9!=''">
						<xsl:value-of select ="normalize-space(COL9)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="$PB_Symbol"/>
					</xsl:otherwise>
				</xsl:choose>
            </Symbol>


            <!--Side-->

            <Side>
              <xsl:choose>
                <xsl:when test="COL12 &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="COL12 &lt; 0">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
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


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="number($varCostBasis) &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test ="number($varCostBasis) &lt; 0">
                  <xsl:value-of select="$varCostBasis*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>


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

            <!--<MarketValueBase>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarketValueBase))">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
