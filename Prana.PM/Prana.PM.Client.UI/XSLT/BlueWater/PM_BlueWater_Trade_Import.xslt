<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  
  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    
    <xsl:variable name="varUnderlying">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(normalize-space(substring-after($Symbol,' ')),1,2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring(normalize-space(substring-after($Symbol,' ')),7) div 1000, '#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(normalize-space(substring-after($Symbol,' ')),5,2)"/>
    </xsl:variable>

    <xsl:variable name="varExMonth">
      <xsl:value-of select="substring(normalize-space(substring-after($Symbol,' ')),3,2)"/>
    </xsl:variable>

    <xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varExMonth,$varStrike,'D',$varExDay))"/>
  </xsl:template>
  
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
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity) ">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            
            <xsl:variable name = "PB_SYMBOL" >
              <xsl:value-of select="COL7"/>
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

            <xsl:variable name="varOption">
              <xsl:call-template name="Option">
                <xsl:with-param name="Symbol" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name = "varAsset" >
              <xsl:choose>
                <xsl:when test ="string-length(COL6) >= 15">
                  <xsl:value-of select ="'Option'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'NotOption'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "varSEDOL" >
              <xsl:value-of select="COL6"/>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:when test ="$varAsset='Option' and $varOption != ''">
                  <xsl:value-of select ="$varOption"/>
                </xsl:when>
                <xsl:when test ="COL8 != 'USD_C'">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            
            <SEDOL>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                <xsl:when test ="$varAsset='Option' and $varOption != ''">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                <xsl:when test ="COL8 != 'USD_C'">
                  <xsl:value-of select ="$varSEDOL"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

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


            <SideTagValue>
              <xsl:choose>
                <xsl:when test="COL4='BUY' and $varAsset='Option'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="COL4='BTC' and $varAsset='Option'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="COL4='SEL' and $varAsset='Option'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>
                <xsl:when test="COL4='SSL' and $varAsset='Option'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
                <xsl:when test="COL4='BUY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="COL4='SEL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="COL4='BTC'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="COL4='SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="varFXrate1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varFXrate2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varFXRate">
              <xsl:choose>
                <xsl:when test ="number($varFXrate2)">
                  <xsl:value-of select ="$varFXrate1 div $varFXrate2"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test ="COL8 != 'USD_C'">
                  <xsl:value-of select ="$varCommission div $varFXRate"/>
                </xsl:when>
                <xsl:when test ="number($varCommission)">
                  <xsl:value-of select ="$varCommission"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test ="COL8 != 'USD_C'">
                  <xsl:value-of select ="$varFees div $varFXRate"/>
                </xsl:when>
                <xsl:when test ="number($varFees)">
                  <xsl:value-of select ="$varFees"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>
            
            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
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


            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL24)"/>
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

         
            <FXRate>
                  <xsl:value-of select ="$varFXRate"/> 
            </FXRate>

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


