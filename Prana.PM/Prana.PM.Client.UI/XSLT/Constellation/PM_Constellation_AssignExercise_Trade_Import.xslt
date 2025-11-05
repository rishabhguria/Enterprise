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
  <xsl:template name="MonthCode">
    <xsl:param name="Month" />
    <xsl:param name="PutOrCall" />
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'X'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
 
  
  
<xsl:template name="Option">
  <xsl:param name="varSymbol" />

  <!-- Underlying symbol: first letters before first digit -->
  <xsl:variable name="UnderlyingSymbol">
<xsl:value-of select="substring-before(substring-after(COL5, ' '), ' ')" />
  </xsl:variable>

  <!-- Extract the numeric and call/put part -->
  <xsl:variable name="symbolRest">
    <xsl:value-of select="substring-after($varSymbol, $UnderlyingSymbol)" />
  </xsl:variable>

  <!-- Extract Expiry Year, Month, Day -->
  <xsl:variable name="ExpiryYear" select="substring($symbolRest, 1, 2)" />
  <xsl:variable name="ExpiryMonth" select="substring($symbolRest, 3, 2)" />
  <xsl:variable name="ExpiryDay" select="substring($symbolRest, 5, 2)" />

  <!-- Call or Put -->
  <xsl:variable name="PutORCall" select="substring($symbolRest, 7, 1)" />

  <!-- Strike Price -->
  <xsl:variable name="StrikePriceRaw" select="substring($symbolRest, 8)" />
  <xsl:variable name="StrikePrice" select="format-number($StrikePriceRaw, '###.00')" />

  <!-- Month Code -->
  <xsl:variable name="MonthCodVar">
    <xsl:call-template name="MonthCode">
      <xsl:with-param name="Month" select="$ExpiryMonth" />
      <xsl:with-param name="PutOrCall" select="$PutORCall" />
    </xsl:call-template>
  </xsl:variable>

  <!-- Format Day (remove leading 0 if present) -->
  <xsl:variable name="Day">
    <xsl:choose>
      <xsl:when test="starts-with($ExpiryDay, '0')">
        <xsl:value-of select="substring($ExpiryDay, 2, 1)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$ExpiryDay" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <!-- Final Option Symbol -->
  <xsl:value-of select="concat('O:', $UnderlyingSymbol, ' ', $ExpiryYear, $MonthCodVar, $StrikePrice, 'D', $Day)" />
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

	<xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
      <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL8)" />
          </xsl:call-template>
        </xsl:variable>
		  
			   <!--<xsl:variable name="varNetPosition">
              <xsl:value-of select="COL8" />
            </xsl:variable>-->
        <xsl:if test="number($varNetPosition)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''" />
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL5" />
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="(normalize-space(COL4))" />
            </xsl:variable>
            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="contains(COL7,'Assign')">
                  <xsl:value-of select="'EquityOption'" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>             
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
              <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="varSymbol" select="normalize-space(COL4)" />
                  </xsl:call-template>
                </xsl:when>
                <xsl:when test="$varSymbol!='*'">
                  <xsl:value-of select="$varSymbol" />
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME" />
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
			  
			  <PBSymbol>
				       <xsl:value-of select="$PB_SYMBOL_NAME" />			  
			  </PBSymbol>
            <xsl:variable name="PB_FUND_NAME" select="COL1" />
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
          
            <SideTagValue>
            
			   <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                 <xsl:when test="contains(COL7, 'AssignSellShort') or contains(COL7, 'AssignSell') or contains(COL7, 'AssignCoverShort') or contains(COL7, 'ExerciseBuy')">
                      <xsl:value-of select="'B'" />
                    </xsl:when>
                      <xsl:when test="COL7 = 'Sell'">
                      <xsl:value-of select="'2'" />
                    </xsl:when>
		              <xsl:when test="COL7 = 'Buy'">
		            	   <xsl:value-of select="'1'" />
		              </xsl:when>
                   <xsl:when test="COL7 = 'SellShort'">
                      <xsl:value-of select="'C'" />
                    </xsl:when>
					   <xsl:when test="COL7 = 'CoverShort'">
		            	   <xsl:value-of select="'B'" />
		              </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''" />
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                     <xsl:when test="COL7 = 'Sell'">
                      <xsl:value-of select="'2'" />
                    </xsl:when>
		              <xsl:when test="COL7 = 'Buy'">
		            	   <xsl:value-of select="'1'" />
		              </xsl:when>
				     <xsl:when test="COL7 = 'SellShort'">
                      <xsl:value-of select="'5'" />
                    </xsl:when>
				  <xsl:when test="COL7 = 'CoverShort'">
		            	   <xsl:value-of select="'B'" />
		              </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
             <xsl:variable name="varTradeDate">
				  <xsl:call-template name="FormatDate">
					  <xsl:with-param name="DateTime" select="COL2"/>
				  </xsl:call-template>
			  </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varTradeDate" />
            </PositionStartDate>
			  
             <xsl:variable name="varSTradeDate">
				  <xsl:call-template name="FormatDate">
					  <xsl:with-param name="DateTime" select="COL3"/>
				  </xsl:call-template>
			  </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="COL3" />
            </PositionSettlementDate>
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
            <xsl:variable name="varPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9" />
              </xsl:call-template>
            </xsl:variable>
           
           
            <xsl:variable name="varNetAmount">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11" />
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varCostBasis">
              <xsl:choose>
                 <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="$varPrice" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varNetAmount div $varNetPosition" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis" />
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * -1" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>


			  <TransactionType>
				  <xsl:choose>
					  <xsl:when test="contains(COL7,'Assign')">
						  <xsl:value-of select="'Assignment'" />
					  </xsl:when>					 
					  <xsl:otherwise>
						  <xsl:value-of select="'Exercise'" />
					  </xsl:otherwise>
				  </xsl:choose>
			  </TransactionType>
			  
			  <TransactionSource>
				    <xsl:value-of select="'Closing'" />			  
			  </TransactionSource>
		  
		<CounterPartyID>
          <xsl:choose>
           <xsl:when test="number(16)">
           <xsl:value-of select="16"/>
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
</xsl:stylesheet>