<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes" />
  <xsl:template name="Translate">
    <xsl:param name="Number" />
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
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
      <xsl:choose>
        <xsl:when test="contains(COL3,'_')">
          <xsl:value-of select="substring-before(normalize-space(COL3),'_')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring(normalize-space(COL3),1,3)"/>
        </xsl:otherwise>
      </xsl:choose>

    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:choose>
        <xsl:when test="contains(COL3,'_')">
          <xsl:value-of select="substring(substring-after(normalize-space(COL3),'___'),5,2)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring(normalize-space(COL3),8,2)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL4),'@ '),' '),1,3)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring-after(substring-after(substring-after(normalize-space(COL4),'@ '),' '),' ')"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring-after(substring-before(normalize-space(COL4),' @'),'TRUST ')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring-before(substring-after(normalize-space(COL4),'@ '),' '),'#.00')"/>
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
    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  <xsl:template match="/">
  
    <DocumentElement>
	
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>
		
        <xsl:if test="number($varNetPosition) and COL7 !='Cash'">
		
          <PositionMaster>
		  
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'UBS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="varCusip">
              <xsl:value-of select="normalize-space(COL13)"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:choose>
                <xsl:when test="contains($varAsset,'Options')">
                  <xsl:value-of select="'EquityOption'" />
                </xsl:when>
                <xsl:when test="contains($varAsset,'Equity')">
                  <xsl:value-of select="'Equity'" />
                </xsl:when>
                <xsl:when test="contains($varAsset,'Bond')">
                  <xsl:value-of select="'FixedIncome'" />
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <!--<xsl:when test="$varAssetType='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL10)"/>
                  </xsl:call-template>
                </xsl:when>-->
                <xsl:when test="$varSymbol !='*' or $varSymbol !=''">
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
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varCusip !=''">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>
            
            <xsl:variable name="varSedol">
              <xsl:value-of select="normalize-space(COL31)"/>
            </xsl:variable>
            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSedol!=''">
                  <xsl:value-of select="$varSedol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>
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

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAssetType='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$varSide='Buy'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='CoverShort'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>                    
                    <xsl:when test="$varSide='SellShort'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
					       <xsl:when test="$varSide='Sell'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>                   
                    <xsl:when test="$varSide='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='SellShort'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when test="$varSide='Buy to Close'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
					          <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="varTradeDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>


            <PositionStartDate>
              <xsl:value-of select="$varTradeDate"/>
            </PositionStartDate>

            <xsl:variable name="varSettlementDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL5"/>
              </xsl:call-template>
            </xsl:variable>


            <!--<xsl:variable name="varSettlementDate">
              <xsl:value-of select="COL5"/>
            </xsl:variable>-->
            <PositionSettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
            </PositionSettlementDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
                <xsl:when test="$varNetPosition &lt; 0">
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL16)"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varPrice &gt; 0">
                  <xsl:value-of select="$varPrice"/>
                </xsl:when>
                <xsl:when test="$varPrice &lt; 0">
                  <xsl:value-of select="$varPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>
									
            <xsl:variable name="PB_BROKER_NAME">
				      <xsl:value-of select="normalize-space(COL14)"/>
			      </xsl:variable>
			
			<xsl:variable name="PRANA_BROKER_ID">
				<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
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

		     	<xsl:variable name="varComm">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL17)"/>
              </xsl:call-template>
            </xsl:variable>
			
			  <Commission>
			       <xsl:choose>
                <xsl:when test="$varComm &gt; 0">
                  <xsl:value-of select="$varComm"/>
                </xsl:when>
                <xsl:when test="$varComm &lt; 0">
                  <xsl:value-of select="$varComm * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
		    	</Commission>
				
				<xsl:variable name="varSecFee">
                            <xsl:call-template name="Translate">
                                <xsl:with-param name="Number" select="COL18"/>
                            </xsl:call-template>
                        </xsl:variable>
                        <SecFee>
                            <xsl:choose>
                                <xsl:when test =" $varSecFee &gt; 0">
                                    <xsl:value-of select ="$varSecFee"/>
                                </xsl:when>
                                <xsl:when test ="$varSecFee &lt; 0">
                                    <xsl:value-of select ="$varSecFee* -1"/>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select ="0"/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </SecFee>
			
			      <xsl:variable name="varFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
			
			    <Fees>
			       <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
		    	</Fees>
			
			    
            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
</xsl:stylesheet>