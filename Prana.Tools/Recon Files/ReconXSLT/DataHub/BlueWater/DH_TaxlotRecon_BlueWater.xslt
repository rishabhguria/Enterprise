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



  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and normalize-space(COL3) !='Cash and Equivalents'">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'DataHub'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_MASTER_FUND_NAME" select="''"/>
            <xsl:variable name="PRANA_MASTER_FUND_NAME">
              <xsl:value-of select ="document('../../ReconMappingXML/MasterFundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_MASTER_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <MasterFund>
              <xsl:choose>
                <xsl:when test ="$PRANA_MASTER_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_MASTER_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_MASTER_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </MasterFund>

            <xsl:variable name="PB_FUND_NAME" select="''"/>
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

            <xsl:variable name="Side">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test ="$Quantity &gt; 0">
                  <xsl:value-of select ="'Buy'"/>
                </xsl:when>
                <xsl:when test ="$Quantity &lt; 0">
                  <xsl:value-of select ="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>
            <Quantity>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12 div COL6"/>
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


            <xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
            <MarketValue>
              <xsl:choose>
                <xsl:when test="$varMarketValue &gt; 0">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:when test="$varMarketValue &lt; 0">
                  <xsl:value-of select="$varMarketValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <TradeDate>
              <xsl:value-of select ="COL8"/>
            </TradeDate>


            <xsl:variable name="UnitCost">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
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

            <CompanyName>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </CompanyName>

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:when>
        <xsl:otherwise>
          <PositionMaster>
            <Symbol>
                  <xsl:value-of select="''"/>
            </Symbol>

            <MasterFund>
                  <xsl:value-of select ="''"/>
            </MasterFund>


            <FundName>
                  <xsl:value-of select ="''"/>
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

            <MarketValue>
                  <xsl:value-of select="0"/>
            </MarketValue>

            <TradeDate>
              <xsl:value-of select ="''"/>
            </TradeDate>

            <UnitCost>
                  <xsl:value-of select="0"/>
            </UnitCost>

            <CompanyName>
              <xsl:value-of select="''"/>
            </CompanyName>

            <SMRequest>
              <xsl:value-of select="'false'"/>
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


