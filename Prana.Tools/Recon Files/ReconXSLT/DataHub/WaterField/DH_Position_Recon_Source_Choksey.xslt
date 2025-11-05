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


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL12"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and normalize-space(COL11) !='Direct Equity' ">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Valuefy'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL10"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@SourceSymbol=$PB_SYMBOL_NAME]/@TargetSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
                <xsl:value-of select="COL9"/>
              </xsl:variable>

              <xsl:variable name="PB_Entity_Name">
              </xsl:variable>


              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>



              <xsl:variable name="PB_FUND_NAME">
                <xsl:choose>
                  <xsl:when test="COL6 !='' or COL6 !='*'">
                    <xsl:value-of select="COL6"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="COL2"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Folio_Number">
                <xsl:value-of select="normalize-space(COL6)"/>
              </xsl:variable>

              <Counterparty>
                <xsl:value-of select="$Folio_Number"/>
              </Counterparty>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMappings.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@SourceFund=$PB_FUND_NAME]/@TargetFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="'Choksey Family- Account Not mapped'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <Side>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="'Sell Short'"/>
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
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$varAvgPX &gt; 0">
                    <xsl:value-of select="$varAvgPX"/>

                  </xsl:when>
                  <xsl:when test="$varAvgPX &lt; 0">
                    <xsl:value-of select="$varAvgPX * (1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </AvgPX>
              <xsl:variable name="varAsset">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <Asset>
                <xsl:value-of select="$varAsset"/>
              </Asset>

              <xsl:variable name="varNav">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL16"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL13"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varMarkPrice">
                <xsl:choose>
                  <xsl:when test="COL16 !='' or COL16 !='*'">
                    <xsl:value-of select="$varNav"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="number($varMarkPrice)">
                    <xsl:value-of select="$varMarkPrice"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <xsl:variable name="varMarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL14"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValue>
                <xsl:choose>
                  <xsl:when test="number($varMarketValue)">
                    <xsl:value-of select="$varMarketValue"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

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


              <xsl:variable name="varDate">
                <xsl:value-of select="''"/>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="$varDate"/>
              </TradeDate>


              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>
              <PrimeBroker>
                <xsl:value-of select="COL1"/>
              </PrimeBroker>

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

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>


      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


