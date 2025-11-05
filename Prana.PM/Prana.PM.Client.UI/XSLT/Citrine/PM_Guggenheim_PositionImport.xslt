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
      <xsl:when test="$Suffix = 'T'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'OS'">
        <xsl:value-of select="'-OSE'"/>
      </xsl:when>
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


            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="concat(substring(COL5,5,2),'/',substring(COL5,7,2),'/',substring(COL5,1,4))"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
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

            <xsl:variable name="varBBKey">
              <xsl:call-template name="substring-after-last">
                <xsl:with-param name="string" select="$varBloomberg" />
                <xsl:with-param name="delimiter" select="' '" />
              </xsl:call-template>
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
              <xsl:value-of select="format-number(COL10,'#.####')"/>
            </xsl:variable>

            <xsl:variable name="varStrike">
              <xsl:choose>
                <xsl:when test="normalize-space($varStrikeMul) = ''">
                  <xsl:value-of select="format-number(COL10,'#.####')"/>
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

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

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
              <xsl:choose>
                <xsl:when test="$varAssetType = 'FUTURE'">
                  <xsl:value-of select="concat($varBBTicker,' ',$varBBKey)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($varBBTicker, ' ',$varStrikeOld, ' ', $varBBKey)"/>
                </xsl:otherwise>
              </xsl:choose>
            </PBSymbol>

            <!--<UnderlyingSymbol/>-->

            <Bloomberg>
              <xsl:choose>
                <xsl:when test="$varAssetType = 'FUTURE'">
                  <xsl:value-of select="concat($varBBTicker,' ',$varBBKey)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($varBBTicker, ' ',$varStrikeOld, ' ', $varBBKey)"/>
                </xsl:otherwise>
              </xsl:choose>
            </Bloomberg>

            <!--<RIC>
              <xsl:value-of select="$varRIC"/>
            </RIC>

            <Bloomberg>
              <xsl:value-of select="$varBloomberg"/>
            </Bloomberg>-->

            <!--<Description>
              <xsl:value-of select="$varDescription"/>
            </Description>-->

            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="$varNetPosition &lt; 0">
                <NetPosition>
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </NetPosition>
              </xsl:when>
              <xsl:when test="$varNetPosition &gt; 0">
                <NetPosition>
                  <xsl:value-of select="$varNetPosition"/>
                </NetPosition>
              </xsl:when>
              <xsl:otherwise>
                <NetPosition>
                  <xsl:value-of select="0"/>
                </NetPosition>
              </xsl:otherwise>
            </xsl:choose>

            <!--Side-->

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'L'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'S'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <!--<xsl:when test="$varSide = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'BC'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>-->
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <CostBasis>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCostBasis))">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <SMMappingReq>
              <xsl:value-of select="'SecMasterMapping.xml'"/>
            </SMMappingReq>

            <!--<Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>-->

            <!--<SecFees>
              <xsl:choose>
                <xsl:when test="$varSecFees &gt; 0">
                  <xsl:value-of select="$varSecFees"/>
                </xsl:when>
                <xsl:when test="$varSecFees &lt; 0">
                  <xsl:value-of select="$varSecFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFees>-->

            <!--<MiscFees>
              <xsl:choose>
                <xsl:when test="$varMiscFee &gt; 0">
                  <xsl:value-of select="$varMiscFee"/>
                </xsl:when>
                <xsl:when test="$varMiscFee &lt; 0">
                  <xsl:value-of select="$varMiscFee*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
