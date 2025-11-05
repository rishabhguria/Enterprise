<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
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
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  
  <xsl:template name="MonthsCode">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth=01">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth=02">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth=03">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth=04">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth=05">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth=06">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth=07">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth=08">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth=09">
        <xsl:value-of select="'U'"/>
      </xsl:when>
      <xsl:when test="$varMonth=10">
        <xsl:value-of select="'V'"/>
      </xsl:when>
      <xsl:when test="$varMonth=11">
        <xsl:value-of select="'X'"/>
      </xsl:when>
      <xsl:when test="$varMonth=12">
        <xsl:value-of select="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
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
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(substring(COL39,1,1),'P') or contains(substring(COL39,1,1),'C')">
      
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="normalize-space(COL37)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(COL41,'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(COL41,'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(substring-after(COL41,'/'),'/'),3,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(COL39,1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(COL38,'#.00')"/>
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
    </xsl:if>
  </xsl:template>
  
  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($NetPosition) and COL4='NEW' and COL8='GSCO'">
          <PositionMaster>
			  <xsl:variable name="PB_NAME">
				  <xsl:value-of select="'MS_SWAP'"/>
			  </xsl:variable>
		
            <xsl:variable name="MappedPBName" select="'MS_SWAP'"/>
              
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:variable name="Asste">
              <xsl:choose>
                <xsl:when test="contains(substring(COL39,1,1),'P') or contains(substring(COL39,1,1),'C')">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:when test="COL25='EQUITY SWAPS'">
                  <xsl:value-of select="'EquitySwap'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="Symbol" select="normalize-space(COL30)"/>

            <xsl:variable name ="varBaseCurrency" select="COL26"/>

            <xsl:variable name ="varSettleCurrency" select="COL17"/>

            <xsl:variable name="varPBFxRate" select="COL24"/>

            <xsl:variable name="varFXRate">
              <xsl:choose>
                <xsl:when test ="($varBaseCurrency = 'EUR') or ($varBaseCurrency = 'GBP') or ($varBaseCurrency = 'AUD') or ($varBaseCurrency = 'NZD') ">
                  <xsl:value-of select ="number($varPBFxRate)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="number(1 div $varPBFxRate)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

           <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$Asste ='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL37"/>
                    <xsl:with-param name="Suffix" select="''"/>
                  </xsl:call-template>
                </xsl:when>

				  <xsl:when test="$Symbol!='' and not(contains(COL1,'SWAP'))">
					  <xsl:value-of select="''"/>
				  </xsl:when>

				  <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$Asste ='EquityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>
				  <xsl:when test="$Symbol=''">
					  <xsl:value-of select="COL1"/>
				  </xsl:when>
				  <xsl:when test="$Symbol!='' and not(contains(COL1,'SWAP'))">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <xsl:variable name="PB_FUND_NAME" select="COL8"/>
            <xsl:variable name="PB_MASTER_FUND_NAME">
              <xsl:value-of select="COL9"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:choose>
                <xsl:when test ="($PB_NAME = $MappedPBName)">
                  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME and @PBFundName=$PB_MASTER_FUND_NAME]/@PranaFund"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <xsl:variable name ="varAccountName">
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <FundName>
             <xsl:choose>
                <xsl:when test="contains(COL1, 'SWAP')">
                  <xsl:value-of select="concat($varAccountName,' ','Swap')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varAccountName"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>

            <xsl:variable name="Asset">
              <xsl:choose>

                <xsl:when test="contains(COL1,'SWAP')">
                  <xsl:value-of select="'EquitySwap'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <PBAssetType>
              <xsl:value-of select ="$Asset"/>
            </PBAssetType>
            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="COL7"/>
            </xsl:variable>
            <!--<ExecutingBroker>
                  <xsl:value-of select="PB_BROKER_NAME"/>
            </ExecutingBroker>-->

			  <xsl:variable name="PB_COUNTER_PARTY" select="COL7"/>
			  <xsl:variable name="PRANA_COUNTER_PARTY">
				  <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTER_PARTY]/@PranaBroker"/>
			  </xsl:variable>

			  <ExecutingBroker>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_COUNTER_PARTY != ''">
						  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_COUNTER_PARTY"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </ExecutingBroker>
			  
            <PositionStartDate>
              <xsl:value-of select="COL5"/>
            </PositionStartDate>
            <PositionSettlementDate>
              <xsl:value-of select="COL6"/>
            </PositionSettlementDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition&gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition&lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="Side" select="COL3"/>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$Side='Buy To Cover'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Buy Long'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Sell Long'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Sell Short'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Side='Cover'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Short'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>


             <xsl:variable name="varCostBasis" select="number(COL11)"/>
            <xsl:variable name="CostBasis">
              <xsl:choose>
                <xsl:when test ="($varBaseCurrency=$varSettleCurrency)">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varCostBasis div $varFXRate"/>
                </xsl:otherwise>
              </xsl:choose>
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

            <xsl:variable name="varCommission" select="number(COL14)"/>
            
            <xsl:variable name="Commission">
              <xsl:choose>
                <xsl:when test ="($varBaseCurrency=$varSettleCurrency)">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varCommission div $varFXRate"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>
                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
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
			  <!--String1 as settle Currency-->
			  <String1>
				  <xsl:if test ="($varSettleCurrency = 'USD') and ($varBaseCurrency != 'USD')">
                <xsl:value-of select="'USD'"/>
              </xsl:if>
            </String1>

						<xsl:if test="$Asset='EquitySwap'">

              <IsSwapped>
                <xsl:value-of select ="1"/>
              </IsSwapped>

              <SwapDescription>
                <xsl:value-of select ="'SWAP'"/>
              </SwapDescription>

              <DayCount>
                <xsl:value-of select ="365"/>
              </DayCount>

              <ResetFrequency>
                <xsl:value-of select ="'Monthly'"/>
              </ResetFrequency>

              <OrigTransDate>
                <xsl:value-of select ="COL5"/>
              </OrigTransDate>

              <xsl:variable name="varPrevMonth">
                <xsl:choose>
                  <xsl:when test ="number(substring-before(COL5,'/')) = 1">
                    <xsl:value-of select ="12"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="number(substring-before(COL5,'/'))-1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name ="varYear">
                <xsl:choose>
                  <xsl:when test ="number(substring-before(COL5,'/')) = 1">
                    <xsl:value-of select ="number(substring-after(substring-after(COL5,'/'),'/')) -1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="number(substring-after(substring-after(COL5,'/'),'/'))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <FirstResetDate>
                <xsl:value-of select ="concat($varPrevMonth,'/28/',$varYear)"/>
              </FirstResetDate>
            </xsl:if>
            
            <FXRate>
              <xsl:value-of select="$varFXRate"/>
            </FXRate>

            <PBSymbol>
              <xsl:value-of select="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>