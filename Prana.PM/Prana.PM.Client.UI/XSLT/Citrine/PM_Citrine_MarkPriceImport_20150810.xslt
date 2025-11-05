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

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='1' ">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='2' ">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='3'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='4' ">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='5' ">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='6'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='7' ">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='8' ">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='9' ">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' ">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12'">
        <xsl:value-of select ="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL9)">
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
              <xsl:value-of select="COL1"/>
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

           
            <xsl:variable name="varExchangeCode">
              <xsl:value-of select="COL31"/>
            </xsl:variable>

            <xsl:variable name="varExchangeName">
              <xsl:value-of select="COL6"/>
            </xsl:variable>


            <xsl:variable name="varImportDate">
              <xsl:value-of select="COL2"/>
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
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPutCall">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:choose>
                <xsl:when test="COL7 = '*'">
                  <xsl:value-of select="'FUT'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'FOP'"/>
                </xsl:otherwise>
              </xsl:choose>
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


            <xsl:variable name="varRootSymbol">
              <xsl:choose>
                <xsl:when test="$varAssetType = 'FUT'">
                  <xsl:value-of select="substring-after(COL3,' ')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(COL3,string-length(substring-before(COL3,' '))-2,3)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <xsl:variable name="varStrike">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varExpiryMonth">
              <xsl:value-of select="substring-before(COL4,'/')"/>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="$varExpiryMonth"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varExpiryYear">
              <xsl:value-of select="substring(substring-after(substring-after(COL4,'/'),'/'),4,1)"/>
            </xsl:variable>

            <xsl:variable name="varExpiryDay">
              <xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varExDay">
              <xsl:choose>
                <xsl:when test="string-length($varExpiryDay) = 1">
                  <xsl:value-of select="concat('0',$varExpiryDay)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varExpiryDay"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <Symbol>
              <xsl:choose>
                <xsl:when test="$varAssetType = 'FUT'">
                  <xsl:value-of select="concat($varRootSymbol,' ',$varExpiryYear, $varMonthCode, $varExDay, '-LME')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($varRootSymbol,' ',$varExpiryYear, $varMonthCode, $varExDay, $varPutCall, format-number($varStrike,'#'), '-LME')"/>
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