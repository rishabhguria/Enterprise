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
	
		
			
	<xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">
        
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Quantity)  and COL17 = 'Cash and Equivalents'">

            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <!--<xsl:variable name="PB_FUND_NAME" select="'BT Global - Direct Holdings'"/>-->
				<xsl:variable name="PB_FUND_NAME" select="COL2"/>
				
              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <xsl:variable name="varSymbol">
                  <xsl:value-of select="COL4"/>
              </xsl:variable>

              <xsl:variable name="PB_Symbol" select="COL5"/>
				
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
              </xsl:variable>
			
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>                  
                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>
              
              <Currency>
                <xsl:value-of select="normalize-space(COL3)"/>
              </Currency>

		 	  <xsl:variable name="varCashLocal">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL9"/>
                </xsl:call-template>
              </xsl:variable>
		
              <MarketValue>
                <xsl:choose>
                  <xsl:when test="number($varCashLocal)">
                    <xsl:value-of select="$varCashLocal"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

              <xsl:variable name="varCashBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL11"/>
                </xsl:call-template>
              </xsl:variable>

              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test="number($varCashBase)">
                    <xsl:value-of select="$varCashBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>


              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL7"/>
                </xsl:call-template>
              </xsl:variable>
				
              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>
                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarkPrice>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($Quantity)">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>
            
              <Side>          	      
				<xsl:choose>
					 <xsl:when test="$Quantity &gt; 0">
                            <xsl:value-of select ="'Buy'"/>
                      </xsl:when>
                     <xsl:when test="$Quantity &lt; 0">
                       <xsl:value-of select ="'Sell'"/>
                     </xsl:when>
                     <xsl:otherwise>
                       <xsl:value-of select="''"/>
                     </xsl:otherwise>
				</xsl:choose>
              </Side>
              
              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

          </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>

              <Currency>
                <xsl:value-of select="''"/>
              </Currency>
              
              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>
              
              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>
				
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>
              
              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
	</xsl:template>
</xsl:stylesheet>