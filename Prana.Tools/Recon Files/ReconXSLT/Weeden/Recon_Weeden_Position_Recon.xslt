<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07 ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08 ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
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
    <xsl:if test="COL15='OPTION'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="normalize-space(COL17)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="normalize-space(COL61)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="normalize-space(COL60)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(COL59,3,2)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number((translate(COL62,'+','')),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="COL63"/>
      </xsl:variable>

      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>
  
<xsl:template name="FutureOption">
    <xsl:param name="Symbol"/>
    <xsl:if test="normalize-space(COL15)='FUTFOP'">
	 <xsl:variable name="varFutureSymbolLength">
        <xsl:value-of select="string-length($Symbol)-3"/>
     </xsl:variable>
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring($Symbol,1,$varFutureSymbolLength)"/>
      </xsl:variable>
	   <xsl:variable name="Month">
        <xsl:value-of select="substring($Symbol,$varFutureSymbolLength+1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number((translate(COL62,'+','')),'#.##')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="normalize-space(COL63)"/>
      </xsl:variable>

      <xsl:value-of select="concat($UnderlyingSymbol,' ',$Month,'20',$PutORCall,$StrikePrice)"/>
    </xsl:if>
  </xsl:template>
	<xsl:template match="/">
	
	<DocumentElement>
	
	<xsl:for-each select ="//Comparision">

		

			 <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="translate(COL74,'+','')"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and not(starts-with(COL4,'5F'))">

					<PositionMaster>

						 <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="concat(normalize-space(COL66),normalize-space(COL67))"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="normalize-space(COL15)='OPTION'">
                  <xsl:value-of select="'EqutiyOption'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL15)='BOND'">
                  <xsl:value-of select="'FixedIncome'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL15)='EQUITY'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL15)='FUTFOP'">
                  <xsl:value-of select="'FutureOption'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL15)='FUTURE'">
                  <xsl:value-of select="'Future'"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL15)='MISC.'">
                  <xsl:value-of select="'SPOT'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <PBAssetType>

              <xsl:value-of select="$varAsset"/>

            </PBAssetType>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL17)"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="normalize-space(COL23)"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL19)"/>
            </xsl:variable>
            <xsl:variable name="varFutureSym">
              <xsl:value-of select="normalize-space(COL21)"/>
            </xsl:variable>
            
            <xsl:variable name="varFutureSymbolLength">
              <xsl:value-of select="string-length($varFutureSym)-2"/>
            </xsl:variable>
            <xsl:variable name="varFutureSymbolLength2">
              <xsl:value-of select="string-length($varFutureSym)-1"/>
            </xsl:variable>
            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="normalize-space(substring($varFutureSym,1,$varFutureSymbolLength))"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol2">
              <xsl:value-of select="normalize-space(substring($varFutureSym,$varFutureSymbolLength2,1))"/>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL17)"/>
                  </xsl:call-template>
                </xsl:when>
				<xsl:when test="$varAsset='FutureOption'">
                  <xsl:call-template name="FutureOption">
                    <xsl:with-param name="Symbol" select="substring-before(normalize-space(COL23),' ')"/>
                  </xsl:call-template>
                </xsl:when>
                <xsl:when test="$varAsset='FixedIncome'">
                  <xsl:value-of select="normalize-space(COL19)"/>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varSEDOL!=''">
                      <xsl:value-of select="''"/>
                 </xsl:when>
				 
                 <xsl:when test="$varAsset='Equity' and $varCUSIP!=''">
                      <xsl:value-of select="''"/>
                 </xsl:when>
				 
                <xsl:when test="$varAsset='Future'">
                  <xsl:value-of select="concat($varFutureSymbol,' ',$varFutureSymbol2,'20')"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
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

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="''"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="$varAsset='Equity' and $varSEDOL!=''">
                      <xsl:value-of select="$varSEDOL"/>
                 </xsl:when>
				 
                 <xsl:when test="$varAsset='Equity' and $varCUSIP!=''">
                      <xsl:value-of select="''"/>
                 </xsl:when>

                <xsl:when test="$varAsset='FixedIncome'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="''"/>
                  </xsl:call-template>
                </xsl:when>

               <xsl:when test="$varAsset='Equity' and $varSEDOL!=''">
                      <xsl:value-of select="''"/>
                 </xsl:when>
				 
                 <xsl:when test="$varAsset='Equity' and $varCUSIP!=''">
                      <xsl:value-of select="$varCUSIP"/>
                 </xsl:when>

                <xsl:when test="$varAsset='FixedIncome'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME4" select="substring(normalize-space(COL4),1,4)"/>

            <xsl:variable name="PRANA_FUND_NAME4">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME4]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:when test ="$PRANA_FUND_NAME4!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME4"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>
			
			<CurrencySymbol>
							
									<xsl:value-of select="COL76"/>
								
						</CurrencySymbol>


            <Quantity>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

					<xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="translate(COL77,'+','')"/>
              </xsl:call-template>
            </xsl:variable>

            <MarkPrice>
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
            </MarkPrice>

						



						<Side>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="MarketValueLocalL">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="translate(COL78,'+','')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="MarketValueLocalS">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="translate(COL92,'+','')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<MarketValue>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$MarketValueLocalL"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$MarketValueLocalL"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>
						
						<MarketValueBase>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$MarketValueLocalS"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$MarketValueLocalS"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>
						
						
			<NetNotionalValue>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$MarketValueLocalL"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$MarketValueLocalL"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>
						
						<NetNotionalValueBase>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$MarketValueLocalS"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$MarketValueLocalS"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

              <SMRequest>
               <xsl:value-of select="'true'"/>
             </SMRequest>

					</PositionMaster>


				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>