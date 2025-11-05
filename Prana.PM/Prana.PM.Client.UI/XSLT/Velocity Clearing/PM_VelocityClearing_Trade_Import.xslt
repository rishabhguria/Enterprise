<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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

  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:param name="varPutCall"/>
    <xsl:if test="$varPutCall='C'">
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
        <xsl:when test="$Month='07' ">
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
    <xsl:if test="$varPutCall='P'">
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
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($Symbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(normalize-space(translate($var,'0123456789','')),1,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')) div 1000,7,8),'#.00')"/>
    </xsl:variable>
    <xsl:variable name="MonthCode">
      <xsl:call-template name="MonthCodevar">
        <xsl:with-param name="Month" select="$ExpiryMonth"/>
        <xsl:with-param name="varPutCall" select="$PutORCall"/>
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

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL71)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition)" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL66)"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL17)"/>
            </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="normalize-space(COL15)='OPTION'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>

                <xsl:when test ="normalize-space(COL15)='EQUITY'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$varAsset='FixedIncome'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test ="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL239)"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test ="$varAsset='Equity'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            
            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$varAsset='FixedIncome' and COL19!=''">
                  <xsl:value-of select="normalize-space(COL19)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>

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

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL83)"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>
            
            <xsl:variable name="varAccruedInterest">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL85)"/>
              </xsl:call-template>
            </xsl:variable>
             <AccruedInterest>
              <xsl:choose>
                <xsl:when test="$varAccruedInterest &gt; 0">
                  <xsl:value-of select="$varAccruedInterest"/>

                </xsl:when>
                <xsl:when test="$varAccruedInterest &lt; 0">
                  <xsl:value-of select="$varAccruedInterest * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccruedInterest>  
            
            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL93)"/>
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
            
            <xsl:variable name="varSecFee1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL103)"/>
              </xsl:call-template>
            </xsl:variable>
              
            <xsl:variable name="varSecFee2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL105)"/>
              </xsl:call-template>
            </xsl:variable>
                
            <xsl:variable name="varSecFee3">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL107)"/>
              </xsl:call-template>
            </xsl:variable>
              
            <xsl:variable name="varSecFee">
              <xsl:value-of select="$varSecFee1 + $varSecFee2 + $varSecFee3"/>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>

                </xsl:when>
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </SecFee>

            <xsl:variable name="var1AUECFee1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL86)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="var2AUECFee1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL88)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="var3AUECFee1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL90)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varAUECFee1">
              <xsl:value-of select="$var1AUECFee1 + $var2AUECFee1 + $var3AUECFee1"/>
            </xsl:variable>
            <AUECFee1>
              <xsl:choose>
                <xsl:when test="$varAUECFee1 &gt; 0">
                  <xsl:value-of select="$varAUECFee1"/>

                </xsl:when>
                <xsl:when test="$varAUECFee1 &lt; 0">
                  <xsl:value-of select="$varAUECFee1 * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AUECFee1>

            <xsl:variable name="var1OccFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL95)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="var2OccFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL97)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="var3OccFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL99)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="var4OccFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL101)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOccFee">
              <xsl:value-of select="$var1OccFee + $var2OccFee + $var3OccFee + $var4OccFee"/>
            </xsl:variable>
            <OccFee>
              <xsl:choose>
                <xsl:when test="$varOccFee &gt; 0">
                  <xsl:value-of select="$varOccFee"/>

                </xsl:when>
                <xsl:when test="$varOccFee &lt; 0">
                  <xsl:value-of select="$varOccFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OccFee>

            <xsl:variable name="varBrokerFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL91)"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$varBrokerFee &gt; 0">
                  <xsl:value-of select="$varBrokerFee"/>

                </xsl:when>
                <xsl:when test="$varBrokerFee &lt; 0">
                  <xsl:value-of select="$varBrokerFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>

                    <xsl:when test="$NetPosition &gt; 0">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when test="$NetPosition &lt; 0">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                    
                  </xsl:choose>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:choose>

                    <xsl:when test="$NetPosition &gt; 0">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when test="$NetPosition &lt; 0">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL199)"/>
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

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <PositionSettlementDate>
              <xsl:value-of select="concat(substring(COL74,5,2),'/',substring(COL74,7,2),'/',substring(COL74,1,4))"/>
            </PositionSettlementDate>

            <PositionStartDate>
              <xsl:value-of select="concat(substring(COL73,5,2),'/',substring(COL73,7,2),'/',substring(COL73,1,4))"/>
            </PositionStartDate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>