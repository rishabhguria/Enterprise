<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes" />

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
        <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL126)" />
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($varNetPosition) and COL9= 'TRD'">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'NAV'" />
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL69" />
            </xsl:variable>
            <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL75)" />
            </xsl:variable>
			  <xsl:variable name="varISIN">
				  <xsl:value-of select="normalize-space(COL74)" />
			  </xsl:variable>

          
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
            </xsl:variable>
           
            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="contains(COL45,'Equities')">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
               <!-- <xsl:when test="$varCUSIP = ''"> -->
                  <!-- <xsl:value-of select="''" /> -->
                <!-- </xsl:when> -->
                 <xsl:when test="string-length($varCUSIP)=7">
                  <xsl:value-of select="''" />
                </xsl:when>
				  
                <xsl:otherwise>
                 <xsl:value-of select="normalize-space(substring-before(COL71,'US'))" />
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>
            <SEDOL>
              <xsl:choose>               
                <xsl:when test="string-length($varCUSIP)= 7">
                  <xsl:value-of select="$varCUSIP" />
                </xsl:when>      
								
                <xsl:otherwise>
				<xsl:value-of select="normalize-space(substring-before(COL71,'US'))" />
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>
			  
			

            <xsl:variable name="PB_FUND_NAME" select="COL15" />
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
            </xsl:variable>
            <AccountName>
              <xsl:choose>
			  
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME" />
                </xsl:when>
				 
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME" />
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>
          
           <xsl:variable name="Side" select="COL10"/>
            <SideTagValue>
               <xsl:choose>
                  
                    <xsl:when test="$Side = 'BUY' ">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$Side = 'SEL'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                 
                    <xsl:otherwise>
                      <xsl:value-of select="''" />
                    </xsl:otherwise>
                 </xsl:choose>
            </SideTagValue>


            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="$varNetPosition" />
                </xsl:when>
                <xsl:when test="$varNetPosition &lt; 0">
                  <xsl:value-of select="$varNetPosition * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
           
            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL128"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis" />
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL18)"/>
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

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL138)"/>
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
			
			<xsl:variable name="varfee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL144)"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varfee &gt; 0">
                  <xsl:value-of select="$varfee"/>
                </xsl:when>
                <xsl:when test="$varfee &lt; 0">
                  <xsl:value-of select="$varfee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>
			  
            <xsl:variable name="varOtherBrokerFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL149)"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$varOtherBrokerFees &gt; 0">
                  <xsl:value-of select="$varOtherBrokerFees"/>
                </xsl:when>
                <xsl:when test="$varOtherBrokerFees &lt; 0">
                  <xsl:value-of select="$varOtherBrokerFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>
			
			<xsl:variable name="varClearingFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL145)"/>
              </xsl:call-template>
            </xsl:variable>
            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$varClearingFee &gt; 0">
                  <xsl:value-of select="$varClearingFee"/>
                </xsl:when>
                <xsl:when test="$varClearingFee &lt; 0">
                  <xsl:value-of select="$varClearingFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClearingFee>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="varPositionStartDate" select="COL114"/>
            <!--<xsl:variable name="varPositionStartDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="normalize-space(COL9)"/>
              </xsl:call-template>
            </xsl:variable>-->
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>
            
            <xsl:variable name="varSDate" select="COL115"/>
            <PositionSettlementDate>
              <xsl:value-of select="$varSDate"/>
            </PositionSettlementDate>
			  
			
            <PBAssetType>
              <xsl:value-of select="$varAsset"/>
            </PBAssetType>
			        
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
 
</xsl:stylesheet>