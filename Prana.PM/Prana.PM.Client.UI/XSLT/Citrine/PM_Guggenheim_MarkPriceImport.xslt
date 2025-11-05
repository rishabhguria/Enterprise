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

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

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

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="COL12 != 'Quantity'">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="COL39"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRootSymbol">
              <xsl:value-of select="substring(COL39,1,2)"/>
            </xsl:variable>

            <xsl:variable name="varExchangeCode">
              <xsl:value-of select="COL31"/>
            </xsl:variable>

            <xsl:variable name="varExchangeName">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varYellowTag">
              <xsl:value-of select="substring-after(substring-after(COL39,' '),' ')"/>
            </xsl:variable>


            <xsl:variable name="varImportDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL39"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="COL7"/>
            </xsl:variable>


            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPutCall">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="COL34"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varSecFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varMiscFee">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varBBTicker">
              <xsl:value-of select="COL38"/>
            </xsl:variable>


            <!--<xsl:variable name="PB_Currency_Name">
              <xsl:value-of select="COL14"/>
            </xsl:variable>
            <xsl:variable name="PB_Suffix">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name = $varPBName]/SymbolData[@TickerSuffixCode = $PB_Currency_Name]/@PBSuffixCode"/>
            </xsl:variable>-->


            <xsl:variable name="varPrana_Root">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@ExchangeCode = $varExchangeCode]/@UnderlyingCode"/>
            </xsl:variable>

            <xsl:variable name="varStrikeMul">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@ExchangeCode = $varExchangeCode]/@StrikeMul"/>
            </xsl:variable>

            <xsl:variable name="RootSymbol">
              <xsl:choose>
                <xsl:when test="$varPrana_Root = ''">
                  <xsl:value-of select="normalize-space($varRootSymbol)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varPrana_Root"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varStrikeOld">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varStrike">
              <xsl:choose>
                <xsl:when test="normalize-space($varStrikeMul) = ''">
                  <xsl:value-of select="COL10"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(COL10*($varStrikeMul),1,4)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonthYearFlag">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@ExchangeCode = $varExchangeCode]/@ExpFlag"/>
            </xsl:variable>

            <xsl:variable name="varMonthYear">
              <xsl:choose>
                <xsl:when test="$varMonthYearFlag = '1'">
                  <xsl:value-of select="concat(substring(COL39,4,1),substring(COL39,3,1))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(COL39,3,2)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varPrana_Exchange">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $varPBName]/SymbolData[@ExchangeCode = $varExchangeCode and @ExchangeName = $varExchangeName]/@ExchangeSuffix"/>
            </xsl:variable>



            <Symbol>
              <xsl:choose>
                <xsl:when test="$varAssetType = 'FUTURE'">
                  <xsl:value-of select="concat($RootSymbol,' ', $varMonthYear, $varPrana_Exchange)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($RootSymbol,' ', $varMonthYear,$varPutCall,$varStrike,$varPrana_Exchange)"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number($varMarkPrice)">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <Date>
              <xsl:value-of select="$varImportDate"/>
            </Date>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>