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
            <xsl:with-param name="Number" select="COL3"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and COL2!= 'Cash'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL8"/>
            </xsl:variable>
           
            <xsl:variable name="varISIN">
              <xsl:value-of select="COL8"/>
            </xsl:variable>
			  
            <xsl:variable name="varSedol">
              <xsl:value-of select="COL8"/>
            </xsl:variable>


            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:when test="string-length($varISIN) &gt; 11">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="string-length($varSedol)= 7">
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
				  <xsl:when test="string-length($varISIN) &gt; 11">
					  <xsl:value-of select="''"/>
				  </xsl:when>
				  <xsl:when test="string-length($varSedol)= 7">
					  <xsl:value-of select="$varSedol"/>
				  </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <ISINSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				  <xsl:when test="string-length($varISIN) &gt; 11">
					  <xsl:value-of select="$varISIN"/>
				  </xsl:when>
				  <xsl:when test="string-length($varSedol)= 7">
					  <xsl:value-of select="''"/>
				  </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISINSymbol>



            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
                    <xsl:when test="number($Quantity) &gt; 0">
                      <xsl:value-of select="'Long'"/>
                    </xsl:when>
                    <xsl:when test="number($Quantity) &lt; 0">
                      <xsl:value-of select="'Short'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>               
              </xsl:choose>
            </Side>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

           <xsl:variable name="varCostbasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>

            <UnitCost>
              <xsl:choose>
                <xsl:when test ="number($varCostbasis) ">
                  <xsl:value-of select="$varCostbasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </UnitCost>

            <!--<xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
			  
            <TotalCost>
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
            </TotalCost>-->

            <CurrencySymbol>
              <xsl:value-of select="COL1"/>
            </CurrencySymbol>

            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="varOriginalPurchaseDate">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

			  <OriginalPurchaseDate>
              <xsl:value-of select="$varOriginalPurchaseDate"/>
            </OriginalPurchaseDate>
			
			 <xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
              </xsl:call-template>
            </xsl:variable>
              <MarkPrice>
              <xsl:choose>
                <xsl:when test ="number($varMarkPrice) ">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <xsl:variable name="varMarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL15)"/>
              </xsl:call-template>
            </xsl:variable>
            <MarketValueBase>
              <xsl:choose>
                <xsl:when test ="number($varMarketValueBase) ">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>
			
			 <xsl:variable name="varNetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL13)"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test ="number($varNetNotionalValueBase) ">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>
			
			

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  
</xsl:stylesheet>