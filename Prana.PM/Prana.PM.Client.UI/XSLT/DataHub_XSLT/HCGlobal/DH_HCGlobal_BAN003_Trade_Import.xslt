<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


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


		<xsl:for-each select="//PositionMaster">
		  
        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL14"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity) and normalize-space(COL10)='Equity' ">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
			  
			  <xsl:variable name="varSymbol">
				  <xsl:value-of select="normalize-space(COL11)"/>
			  </xsl:variable>

			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="COL12"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
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
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

          

            <SideTagValue>
				<xsl:choose>
					<xsl:when test="$varSide='Buy'">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:when test="$varSide='Sell'">
						<xsl:value-of select="'2'"/>
					</xsl:when>
					<xsl:when test="$varSide='Cover Short'">
						<xsl:value-of select="'E'"/>
					</xsl:when>
					<xsl:when test="$varSide='Sell Short' ">
						<xsl:value-of select="'5'"/>
					</xsl:when>
				</xsl:choose>
				
            </SideTagValue>

            <!--<xsl:variable name="PB_FUND_NAME" select="COL2"/>-->

			  <xsl:variable name="PB_FUND_NAME" select="'Test'"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

         				

			<PositionStartDate>
				<xsl:value-of select="COL4"/>
			</PositionStartDate>


            <PositionSettlementDate>
              <xsl:value-of select="COL5"/>
            </PositionSettlementDate>
			
		

            <NetPosition>
              <xsl:choose>
                <xsl:when  test="number($varQuantity) &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test="number($varQuantity) &lt; 0">
                  <xsl:value-of select="$varQuantity * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:when>
                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>




			  <xsl:variable name="NetNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="normalize-space(COL16)"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <NetAmount>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalValue &gt; 0">
						  <xsl:value-of select="$NetNotionalValue"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalValue &lt; 0">
						  <xsl:value-of select="$NetNotionalValue * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetAmount>

			  <xsl:variable name="NetNotionalValueBase">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL17"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <NetAmountBase>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalValueBase &gt; 0">
						  <xsl:value-of select="$NetNotionalValueBase"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalValueBase &lt; 0">
						  <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetAmountBase>


			  <xsl:variable name="varCommission">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
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

			  <xsl:variable name="varSecFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL21"/>
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
			  
            <CurrencyID>
              <xsl:value-of select="'1'"/>
            </CurrencyID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


