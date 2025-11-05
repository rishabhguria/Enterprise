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
      <xsl:value-of select="substring-before(normalize-space(COL10),' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(substring-after(normalize-space(COL10),' '),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-after(normalize-space(COL10),' '),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(substring-after(normalize-space(COL10),' '),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(substring-after(normalize-space(COL10),' '),7,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL10),' '),8) div 1000,'##.00')"/>
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
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL15)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition)" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL9)"/>
            </xsl:variable>



            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL8='OPT'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL10"/>
                  </xsl:call-template>
                </xsl:when>               
                <xsl:when test ="$varSymbol!=''">
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
                <xsl:with-param name="Number" select="normalize-space(COL16)"/>
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

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>
			  
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$varSide='1'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='2'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='5'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Z'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varSide='1'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='2'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='5'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Z'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="varODay">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL4),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL4,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL4),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL4,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL4,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varOMonth">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL4),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL4,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL4,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL4),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL4,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL4,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varOyear">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL4,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL4,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <OriginalPurchaseDate>
              <xsl:value-of select="concat($varOMonth,'/',$varODay,'/',$varOyear)"/>
            </OriginalPurchaseDate>

            <xsl:variable name="varSettleDay">
              <xsl:choose>
                <xsl:when test="contains(COL6,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL6),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL6,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL6,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL6,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL6),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL6,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL6,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varSettleMonth">
              <xsl:choose>
                <xsl:when test="contains(COL6,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL6),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL6,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL6,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL6,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL6),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL6,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL6,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSettleyear">
              <xsl:choose>
                <xsl:when test="contains(COL6,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL6,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL6,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL6,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <PositionSettlementDate>
              <xsl:value-of select="concat($varSettleMonth,'/',$varSettleDay,'/',$varSettleyear)"/>
            </PositionSettlementDate>

            <xsl:variable name="varTradeDay">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL4),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL4,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL4),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL4,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL4,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varTradeMonth">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL4),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL4,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL4,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL4),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL4,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL4,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varTradeyear">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL4,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL4,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="concat($varTradeMonth,'/',$varTradeDay,'/',$varTradeyear)"/>
            </PositionStartDate>
			
			 <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL17)"/>
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
			
			<xsl:variable name="varCommission">
              <xsl:value-of select="COL23"/>
            </xsl:variable>			  

			<ClearingFee>
			 <xsl:choose>
                <xsl:when test="$varCommission=0">
                  <xsl:value-of select="COL15 * 0.0065"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL23"/>
                </xsl:otherwise>
              </xsl:choose>
			</ClearingFee>
		  
			  <!--<AUECFee1>
             <xsl:choose>
                <xsl:when test="$varCommission=0">
                  <xsl:value-of select="COL15 * 0.0065"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL23"/>
                </xsl:otherwise>
              </xsl:choose>				  
		     </AUECFee1>-->
				  
			<xsl:variable name="varExchangeFee">
                  <xsl:value-of select="COL22"/>
             </xsl:variable>
			
			<SecFee>
			 <xsl:value-of select="$varExchangeFee"/>
			</SecFee>
			
			<!-- <AccruedInterest> -->
             
               <!-- <xsl:value-of select="COL28"/> -->
			
			<!-- </AccruedInterest> -->
			
          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>