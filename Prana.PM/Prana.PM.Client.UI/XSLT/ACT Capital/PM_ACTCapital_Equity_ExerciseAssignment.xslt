<?xml version="1.0" encoding="utf-8" ?>
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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime"/>
    <!--  converts date time double number to 18/12/2009  -->
    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019"/>
    </xsl:variable>
    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))"/>
    </xsl:variable>
    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
    </xsl:variable>
    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
    </xsl:variable>
    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
    </xsl:variable>
    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))"/>
    </xsl:variable>
    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
    </xsl:variable>
    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))"/>
    </xsl:variable>
    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
    </xsl:variable>
    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
    </xsl:variable>
    <xsl:variable name="varMonthUpdated">
      <xsl:choose>
        <xsl:when test="string-length($nMonth) = 1">
          <xsl:value-of select="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="nDayUpdated">
      <xsl:choose>
        <xsl:when test="string-length($nDay) = 1">
          <xsl:value-of select="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:if test="(normalize-space(COL15)='COMMON STOCK' and normalize-space(COL19)!='0') and (normalize-space(COL7)='AssignBuy' 
              or normalize-space(COL7)='AssignSell' or normalize-space(COL7)='AssignCoverShort' or normalize-space(COL7)='AssignSellShort')">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!='' or $varSymbol!='*'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL31)"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when  test="number($varCostBasis)">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when >
                
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CostBasis>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL27)"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when  test="number($varCommission)">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </Commission>

            <xsl:variable name="varOrfFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL27)"/>
              </xsl:call-template>
            </xsl:variable>
            <OrfFee>
              <xsl:choose>
                <xsl:when  test="number($varOrfFee)">
                  <xsl:value-of select="$varOrfFee"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </OrfFee>

            <PositionStartDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="normalize-space(COL5)"/>
              </xsl:call-template>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="normalize-space(COL6)"/>
              </xsl:call-template>
            </PositionSettlementDate>


            <xsl:variable name="varQty">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL19)"/>
              </xsl:call-template>
            </xsl:variable>
            <NetPosition>
              <xsl:choose>
                <xsl:when  test="number($varQty) and $varQty &gt; 0">
                  <xsl:value-of select="$varQty"/>
                </xsl:when>
                <xsl:when test="number($varQty) and $varQty &lt; 0">
                  <xsl:value-of select="$varQty * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='AssignBuy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$varSide='AssignSell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

                <xsl:when test="$varSide='AssignSellShort'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:when test="$varSide='AssignCoverShort'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <TransactionType>
              <xsl:choose>
                <xsl:when  test="contains($varSide,'Assign')">
                  <xsl:value-of select="'Assignment'"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionType>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL33)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PB_BROKER_NAME]/@PranaBroker"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID!='')">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


