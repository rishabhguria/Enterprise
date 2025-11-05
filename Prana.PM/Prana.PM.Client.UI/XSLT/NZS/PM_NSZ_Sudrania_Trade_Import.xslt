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
  
  <xsl:variable name ="smallcase" select ="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name ="Uppercase" select ="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  
  <xsl:template name="Upper">
    <xsl:param name="Text" />
        <xsl:value-of select="translate(substring-before(normalize-space(COL7),' '),$smallcase,$Uppercase)" />      
  </xsl:template>

  
  <xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C' or $PutOrCall='c'">
			<xsl:choose>
				<xsl:when test="$Month=01 or $Month=1">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 or $Month=2">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 or $Month=3">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 or $Month=4">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 or $Month=5">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 or $Month=6">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07 or $Month=7">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08 or $Month=8">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 or $Month=9">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P' or $PutOrCall='p'">
			<xsl:choose>
				<xsl:when test="$Month=01 or $Month=1">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 or $Month=2">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 or $Month=3">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 or $Month=4">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 or $Month=5">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 or $Month=6">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07 or $Month=7">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08 or $Month=8">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 or $Month=9">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
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
     <xsl:call-template name="Upper">
      <xsl:with-param name="Text" select="substring-before(normalize-space(COL7),' ')"/>
     </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring-before(substring-after(normalize-space(COL7),'/'),'/')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-before(normalize-space(COL7),'/'),string-length(substring-before(normalize-space(COL7),'/'))-1)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL7),'/'),'/'),' '),string-length(substring-before(substring-after(substring-after(normalize-space(COL7),'/'),'/'),' '))-1)"/>
    </xsl:variable>
	<xsl:variable name="PutORCall">
		<xsl:value-of select="substring(normalize-space(COL3),1,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(normalize-space(COL9),'#.00')"/>
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
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL18)" />
          </xsl:call-template>
        </xsl:variable>
      
        <xsl:if test="number($varNetPosition)">
		 
          <PositionMaster>
		  
	       
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''" />
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL7" />
            </xsl:variable>
			
            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL1)" />
            </xsl:variable>
			
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
            </xsl:variable>
			
            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="normalize-space(COL2)='Equity'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="EquityOption" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="COL23" />
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
                <xsl:when test="$varSEDOL!='*' or $varSEDOL!=''">
                  <xsl:value-of select="''" />
                </xsl:when>
                <!--<xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL1)" />
                  </xsl:call-template>
                </xsl:when>-->
                <xsl:when test="$varSymbol !='*' or $varSymbol !=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME" />
                </xsl:otherwise>
              </xsl:choose>          
            </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSEDOL!='*' or $varSEDOL!=''">
                  <xsl:value-of select="$varSEDOL" />
                </xsl:when>
                <xsl:when test="$varSymbol !='*' or $varSymbol !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <xsl:variable name="PB_FUND_NAME" select="COL21" />
			
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

            <xsl:variable name="Side" select="normalize-space(COL15)"/>
			
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$Side='Buy to Open'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Buy to Close'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Sell to Open'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Sell to Close'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Side='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
			
            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL4" />
            </xsl:variable>
			
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate" />
            </PositionStartDate>
            
            <xsl:variable name="varSettlementDate">
              <xsl:value-of select="COL6" />
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
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
         
            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
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

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
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
            
            <xsl:variable name="varFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
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

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'" />
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
</xsl:stylesheet>