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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
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

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space(COL15),' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(substring-after(normalize-space(COL15),' '),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-after(normalize-space(COL15),' '),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(substring-after(normalize-space(COL15),' '),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(substring-after(normalize-space(COL15),' '),7,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL15),' '),8) div 1000,'##.00')"/>
    </xsl:variable>
    <xsl:variable name="MonthCodVar">
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

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

  </xsl:template>

  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL17"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Position)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name = "PB_COMPANY_NAME">
              <xsl:value-of select="normalize-space(COL16)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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



            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL65='OPTIONS'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varSymbol" >
              <xsl:value-of select="normalize-space(COL15)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL15"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL120"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varSecFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL118"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFees &gt; 0">
                  <xsl:value-of select="$varSecFees"/>
                </xsl:when>
                <xsl:when test="$varSecFees &lt; 0">
                  <xsl:value-of select="$varSecFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>


            <xsl:variable name="varOrfFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL114"/>
              </xsl:call-template>
            </xsl:variable>
            <OrfFee>
              <xsl:choose>
                <xsl:when test="$varOrfFee &gt; 0">
                  <xsl:value-of select="$varOrfFee"/>
                </xsl:when>
                <xsl:when test="$varOrfFee &lt; 0">
                  <xsl:value-of select="$varOrfFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>

            <xsl:variable name="varNetMoney">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL21"/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="varOtherBrokerFee">
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="($varNetMoney +(($Position * $varCostBasis) + $varCommission + $varSecFees + $varOrfFee)) * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="($varNetMoney +((($Position * $varCostBasis) * 100) + $varCommission + $varSecFees + $varOrfFee)) * (-1)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            

            <xsl:variable name="varOtherFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$varOtherBrokerFee"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:value-of select="$varOtherFees"/>
            </Fees>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$varSide='BUY'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='SELL'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='COVER SHORT'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <!--<xsl:when test="$varSide='SELL DVP'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='RECEIVE LONG'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>-->
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varSide='BUY'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='SELL'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='COVER SHORT'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='SELL DVP'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='RECEIVE LONG'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>



            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL13)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>



            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL9"/>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

         
            <xsl:variable name="varPositionSettlementDate">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <PositionSettlementDate>
              <xsl:value-of select="$varPositionSettlementDate"/>
            </PositionSettlementDate>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>
          
        </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


