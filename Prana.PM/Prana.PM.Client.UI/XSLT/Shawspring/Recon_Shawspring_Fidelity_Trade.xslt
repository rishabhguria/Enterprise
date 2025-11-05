<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test ="number(COL58) and COL35!='Currency Revaluation'">

          <xsl:variable name="varPBName">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <PositionMaster>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL75"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL22"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="COL12"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


            <xsl:variable name="varAsset">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>
            <Asset>
              <xsl:value-of select ="$varAsset"/>
              <!--<xsl:choose>
				       	<xsl:when test="$varAsset='COMMON STOCK'">
				       		<xsl:value-of select="'Equity'"/>
				       	</xsl:when>
				       	<xsl:otherwise>
				       		<xsl:value-of select="''"/>
				       	</xsl:otherwise>
				       </xsl:choose>-->
            </Asset>

            <xsl:variable name="varTradeDate">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <TradeDate>
              <xsl:value-of select="$varTradeDate"/>
            </TradeDate>


            <xsl:variable name="varSettlementDate">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
            </SettlementDate>


            <xsl:variable name="varQuantity">
              <xsl:value-of select="COL58 div 100000"/>
            </xsl:variable>
            <Quantity>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity*(-1)"/>
                </xsl:when>
              </xsl:choose>
            </Quantity>


            <xsl:variable name="varAvgPX">
              <xsl:value-of select="COL59 div 1000000000"/>
            </xsl:variable>
            <AvgPX>
              <xsl:value-of select="$varAvgPX"/>
            </AvgPX>


            <xsl:variable name="varCommission">
              <xsl:value-of select="COL66 div 100"/>
            </xsl:variable>
            <Commission>
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
            </Commission>


            <xsl:variable name="varSecFee">
              <xsl:value-of select="COL68 div 100"/>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>
            

            <xsl:variable name="varAUECFee1">
              <xsl:value-of select="COL70 div 100"/>
            </xsl:variable>
            <AUECFee1>
              <xsl:choose>
                <xsl:when test="$varAUECFee1 &gt; 0">
                  <xsl:value-of select="$varAUECFee1"/>
                </xsl:when>
                <xsl:when test="$varAUECFee1 &lt; 0">
                  <xsl:value-of select="$varAUECFee1*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AUECFee1>


            <xsl:variable name="varNetNotionalValueBase">
              <xsl:value-of select="COL71 div 100"/>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>


            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="$varSide='B'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$varSide='S'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
              </xsl:choose>
            </Side>

            <SMRequest>
              <xsl:value-of select="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>