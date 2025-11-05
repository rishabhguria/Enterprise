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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>

    <xsl:variable name="var">
      <xsl:value-of select="string-length(translate($Symbol,'0123456789.',''))"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring(translate($Symbol,'0123456789.',''),1,$var - 1)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($Symbol,translate($Symbol, '0123456789.', ''), '')),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($Symbol,translate($Symbol, '0123456789.', ''), '')),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(translate($Symbol,translate($Symbol, '0123456789.', ''), ''),1,2)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(translate($Symbol,translate($Symbol, '0123456789.', ''), ''),7,5),'##.00')"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(translate($Symbol,'0123456789.',''),$var)"/>
    </xsl:variable>
    
    <xsl:variable name="Month">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$ExpiryMonth"/>
        <xsl:with-param name="PutOrCall" select="$PutORCall"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="substring($ExpiryDay,1,1)='0'">
          <xsl:value-of select="substring($ExpiryDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ExpiryDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$Month,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        
      <xsl:if test="normalize-space(COL15)='CALL OPTION' and COL7!='Expire'">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL14"/>
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
                
                <xsl:when test="normalize-space(COL15)='CALL OPTION'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL10"/>
                  </xsl:call-template>
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
              <xsl:value-of select="''"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="''"/>
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

                <xsl:when test="$varSide='ExerciseBuy'">
                  <xsl:value-of select="'1'"/>
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
                <xsl:when  test="contains($varSide,'Exercise')">
                  <xsl:value-of select="'Exercise'"/>
                </xsl:when>
                <xsl:when  test="contains($varSide,'Expire')">
                  <xsl:value-of select="'Expire'"/>
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


