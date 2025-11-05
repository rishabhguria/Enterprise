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
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'%',''),$SingleQuote,''))"/>
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
            <xsl:with-param name="Number" select="COL14"/>
          </xsl:call-template>
        </xsl:variable>
        <!--<xsl:variable name="PB_Entity_Name" select="normalize-space(substring-before(substring-after(substring-after(//Comparision[contains(COL1,'Group : ')]/COL1, '   '),' '),'As of '))"/>-->
        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <!--<xsl:when test="number($Quantity) and (COL1!='Total : AIF' and COL1!='Insurance' and COL1!='AIF'  and COL1!='Total : Alternate Assets' and COL1!='Total : Bank Account' and COL1!='Total : Corporate Bonds' and COL1!='Total : Debt Fund' and COL1!='Total : Equity' and COL1!='Total : Equity Fund' and COL1!='Total : Hybrid Fund' and COL1!='Total : Index' and COL1!='Total : Insurance' and COL1!='Total : Liquid Fund' and COL1!='Total : Listed'  and COL1!='Total : Options' and COL1!='Total : Other Assets'  and COL1!='Total : Structured Products' and COL1!='Total : Unlisted'  and COL1!='Total : Unlisted Debt' and COL1!='Total : Unlisted PE shares' and COL1!='Total Portfolio'  and COL1!='Total : Fixed Income' and COL1!='Total : Hybrid'  and COL1!='Unlisted' and COL1!='Equity Fund' and COL1!='Corporate Bonds' and COL1!='Listed' and COL1!='Unlisted Debt' and COL1!='Index' and COL1!='Hybrid Fund' and COL1!='Debt Fund' and COL1!='Unlisted PE shares' and COL1!='Other Assets' and COL1!='Liquid Fund' and COL1!='Bank Account' and  COL1!='Total : Cash &amp; Equivalents' )">--> 
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Valuefy'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL10)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@SourceSymbol=$PB_SYMBOL_NAME]/@TargetSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
                <xsl:value-of select="normalize-space(COL9)"/>
              </xsl:variable>
                <xsl:variable name="Folio_Number">
                <xsl:value-of select="normalize-space(COL6)"/>
              </xsl:variable>

              <Counterparty>
                <xsl:value-of select="$Folio_Number"/>
              </Counterparty>
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
              <MasterFund>
                <xsl:value-of select="''"/>
              </MasterFund>

              <xsl:variable name="PB_FUND_NAME" select="''"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMappings.xml')/FundMapping/PB[@Name='Valuefy']/FundData[@SourceFund=$PB_FUND_NAME]/@TargetFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="'Lakshmi Machine Works'"/>
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

              <xsl:variable name="varRoundOffQty">
                <xsl:value-of select="round($Quantity *100) div 100"/>
              </xsl:variable>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($varRoundOffQty)">
                    <xsl:value-of select="$varRoundOffQty"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </Quantity>

              <xsl:variable name="varAvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="0"/>
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

              <xsl:variable name="varMarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL13"/>
                </xsl:call-template>
              </xsl:variable>
              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$varMarkPrice &gt; 0">
                    <xsl:value-of select="$varMarkPrice"/>
                  </xsl:when>
                  <xsl:when test="$varMarkPrice &lt; 0">
                    <xsl:value-of select="$varMarkPrice * (-1)"/>
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
                  
                  <xsl:when test="$varMarketValue &gt; 0">
                    <xsl:value-of select="$varMarketValue"/>
                  </xsl:when>
                  <xsl:when test="$varMarketValue &lt; 0">
                    <xsl:value-of select="$varMarketValue "/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL16"/>
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

              <xsl:variable name="FXRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="0"/>
                </xsl:call-template>
              </xsl:variable>
              <FXRate>
                <xsl:choose>
                  <xsl:when test="$FXRate &gt; 0">
                    <xsl:value-of select="$FXRate"/>
                  </xsl:when>
                  <xsl:when test="$FXRate &lt; 0">
                    <xsl:value-of select="$FXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FXRate>

              <xsl:variable name="Multiplier">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="0"/>
                </xsl:call-template>
              </xsl:variable>
              <Multiplier>
                <xsl:choose>
                  <xsl:when test="$Multiplier &gt; 0">
                    <xsl:value-of select="$Multiplier"/>
                  </xsl:when>
                  <xsl:when test="$Multiplier &lt; 0">
                    <xsl:value-of select="$Multiplier"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Multiplier>


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
              
              <MasterFund>
                <xsl:value-of select="''"/>
              </MasterFund>

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


            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>


      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>