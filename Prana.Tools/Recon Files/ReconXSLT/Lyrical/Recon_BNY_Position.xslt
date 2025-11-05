<?xml version="1.0" encoding="utf-8"?>

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


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:if test="number(COL27)">
          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name="varPBName">
              <xsl:value-of select="'BNY'"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL22"/>
            </xsl:variable>

            <xsl:variable name="varQuantity">
              <xsl:value-of select="COL27"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL38"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL31"/>
            </xsl:variable>

            <xsl:variable name="varCounterPartyID">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <!--<xsl:variable name="varCommission">
              <xsl:value-of select="COL18"/>
            </xsl:variable>

           

            <xsl:variable name="varMarketValueBase">
              <xsl:value-of select="COL13"/>
            </xsl:variable>-->

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="COL32"/>
            </xsl:variable>
            
            <!--<xsl:variable name="varSMRequest">
              <xsl:value-of select="'TRUE'"/>
            </xsl:variable>-->
            
            

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

            <xsl:variable name="PB_Currency_Name">
              <xsl:value-of select="substring-after(COL57, ' ')"/>
            </xsl:variable>
            <xsl:variable name="PB_Suffix">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name = $varPBName]/SymbolData[@TickerSuffixCode = $PB_Currency_Name]/@PBSuffixCode"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_Symbol_NAME != ''">
                  <xsl:value-of select="$PRANA_Symbol_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$PB_Currency_Name = 'USD'">
                      <xsl:value-of select="substring-before($varEquitySymbol, ' ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat(substring-before($varEquitySymbol, ' '), $PB_Suffix)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>


            <!--QUANTITY-->

            <Quantity>
              <xsl:value-of select="$varQuantity"/>
            </Quantity>

            <!--Side-->

            <Side/>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarkPrice))">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>


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

</xsl:stylesheet>
