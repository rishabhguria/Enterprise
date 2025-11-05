<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:when test="$Number='*'">
        <xsl:value-of select="0"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity) ">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL" >
              <xsl:value-of select="COL6"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>


            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL"/>
            </PBSymbol>

            <xsl:variable name="varPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when  test="number($varPrice)">
                  <xsl:value-of select="$varPrice"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CostBasis>

            <PositionStartDate>
              <xsl:value-of select="COL2"/>
            </PositionStartDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when  test=" $varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test=" $varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity * (-1)"/>
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
                    <xsl:when test="$varSide='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Buy to Open'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Sell short'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Buy to Close'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Sell to Open'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
              </xsl:choose>

            </SideTagValue>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test ="number($varCommission)">
                  <xsl:value-of select ="$varCommission"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varOrfFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>
            <OrfFee>
              <xsl:choose>
                <xsl:when test ="number($varOrfFees)">
                  <xsl:value-of select ="$varOrfFees"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>

            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test ="$varSecFee &gt; 0">
                  <xsl:value-of select ="$varSecFee"/>
                </xsl:when>
                <xsl:when test ="$varSecFee &lt; 0">
                  <xsl:value-of select ="$varSecFee * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name="varOtherBrokerFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <OtherBrokerFees>
              <xsl:choose>
                <xsl:when test ="$varOtherBrokerFee &gt; 0">
                  <xsl:value-of select ="$varOtherBrokerFee"/>
                </xsl:when>
                <xsl:when test ="$varOtherBrokerFee &lt; 0">
                  <xsl:value-of select ="$varOtherBrokerFee * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OtherBrokerFees>

            <xsl:variable name="varStampDuty">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>
            <StampDuty>
              <xsl:choose>
                <xsl:when test ="$varStampDuty &gt; 0">
                  <xsl:value-of select ="$varStampDuty"/>
                </xsl:when>
                <xsl:when test ="$varStampDuty &lt; 0">
                  <xsl:value-of select ="$varStampDuty * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>
            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL16)"/>
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

            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
              </xsl:call-template>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test ="$varFXRate &gt; 0">
                  <xsl:value-of select ="$varFXRate"/>
                </xsl:when>
                <xsl:when test ="$varFXRate &lt; 0">
                  <xsl:value-of select ="$varFXRate * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

            <xsl:variable name="varFXConversionRate">
              <xsl:value-of select="normalize-space(COL21)"/>
            </xsl:variable>
            <FXConversionMethodOperator>
              <xsl:choose>
                <xsl:when test ="$varFXConversionRate !=''">
                  <xsl:value-of select ="$varFXConversionRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="'M'"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXConversionMethodOperator>

            <xsl:variable name="varAccuredIntrest">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL31"/>
              </xsl:call-template>
            </xsl:variable>
            <AccruedInterest>
              <xsl:choose>
                <xsl:when test ="$varAccuredIntrest &gt; 0">
                  <xsl:value-of select ="$varAccuredIntrest"/>
                </xsl:when>
                <xsl:when test ="$varAccuredIntrest &lt; 0">
                  <xsl:value-of select ="$varAccuredIntrest * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccruedInterest>
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


