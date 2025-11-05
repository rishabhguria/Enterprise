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
            <xsl:with-param name="Number" select="COL21"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity) ">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_FUND_NAME" select="COL2"/>-->

			  <xsl:variable name="PB_FUND_NAME" select="'The Purpleville Foundation: 14462208'"/>
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

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL9)"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL15"/>
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
					  <xsl:when test="$varSide = 'B' and COL9 ='L'">
						  <xsl:value-of select="'1'"/>
					  </xsl:when>
					  <xsl:when test="$varSide = 'S' and COL9 ='L'">
						  <xsl:value-of select="'2'"/>
					  </xsl:when>
					  <xsl:when test="$varSide = 'B' and COL9 ='S'">
						  <xsl:value-of select="'B'"/>
					  </xsl:when>
					  <xsl:when test="$varSide = 'S' and COL9 ='S'">
						  <xsl:value-of select="'5'"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
		

            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL19,1,4)"/>
            </xsl:variable>
			  
            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL19,5,2)"/>
            </xsl:variable>
			  
            <xsl:variable name="varDay">
               <xsl:value-of select="substring(COL19,7,2)"/>
            </xsl:variable>
			<PositionStartDate>
				<xsl:value-of select="concat($varYear,$varMonth, $varDay)"/>
			</PositionStartDate>
			  
             <xsl:variable name="varSYear">
              <xsl:value-of select="substring(COL20,1,4)"/>
            </xsl:variable>
			  
            <xsl:variable name="varSMonth">
              <xsl:value-of select="substring(COL20,5,2)"/>
            </xsl:variable>
			  
            <xsl:variable name="varSDay">
               <xsl:value-of select="substring(COL20,7,2)"/>
            </xsl:variable>
            <PositionSettlementDate>
             	<xsl:value-of select="concat($varSYear,$varSMonth, $varSDay)"/>
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
                <xsl:with-param name="Number" select="COL22"/>
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
					  <xsl:with-param name="Number" select="normalize-space(COL26)"/>
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
					  <xsl:with-param name="Number" select="''"/>
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
                <xsl:with-param name="Number" select="COL28"/>
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
                <xsl:with-param name="Number" select="COL29"/>
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
			  
			  <ExecutingBroker>
				  <xsl:choose>
					  <xsl:when test="COL34 = 'PB BNP'">
						  <xsl:value-of select="'BNP PB'"/>
					  </xsl:when>
					  <xsl:when test="COL34 = 'OTC PLC' ">
						  <xsl:value-of select="'PLC OTC'"/>
					  </xsl:when>
					  <xsl:when test="COL34 = 'PB ML'">
						  <xsl:value-of select="'ML PB'"/>
					  </xsl:when>
					  <xsl:when test="COL34 = 'PB ROTH' ">
						  <xsl:value-of select="'ROTH PB'"/>
					  </xsl:when>
					  <xsl:when test="COL34 = 'PB SSB' ">
						  <xsl:value-of select="'SSB PB'"/>
					  </xsl:when>
					  <xsl:when test="COL34 = 'PB JSFC / 09510-60-00000000301910' ">
						  <xsl:value-of select="'JSFC'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </ExecutingBroker>
			  
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


