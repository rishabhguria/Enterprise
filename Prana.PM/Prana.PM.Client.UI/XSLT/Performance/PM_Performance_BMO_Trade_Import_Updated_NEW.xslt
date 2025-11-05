<?xml version="1.0" encoding="UTF-8"?>

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
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL12"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'NewBMO'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL9"/>
            </xsl:variable>

            <!--<xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>-->
			  
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>
			  
            <xsl:variable name="Symbol">
              <xsl:choose>
                <xsl:when test="contains(COL9,'.')">
                  <xsl:value-of select="concat(substring-before(normalize-space(COL9),'.'),'-TC')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL9)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

            <!--<xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>-->

			  <xsl:variable name ="PRANA_FUND_NAME">
				  <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

            <FundName>
              <!--<xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>-->
              <xsl:value-of select="'DKAM BMO'"/>
            </FundName>

			  <xsl:variable name="PB_BROKER_NAME">
				  <xsl:value-of select="normalize-space(COL21)"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_BROKER_ID">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
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

            <NetPosition>
              <xsl:choose>

                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>

                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetPosition>

            <xsl:variable name="Side" select="COL8"/>


            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Side='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
				  <xsl:when test="$Side='Buy Cancel'">
					  <xsl:value-of select="'2'"/>
				  </xsl:when>
                <xsl:when test="$Side='Buy'">
					<xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="TradePrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>

                <xsl:when test="$TradePrice &gt; 0">
                  <xsl:value-of select="$TradePrice"/>
                </xsl:when>

                <xsl:when test="$TradePrice &lt; 0">
                  <xsl:value-of select="$TradePrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="Date" select="COL6"/>

            <PositionStartDate>
              <xsl:value-of select="$Date"/>
            </PositionStartDate>


			  <xsl:variable name="SettlementDate" select="COL7"/>
			  <PositionSettlementDate>
				  <xsl:value-of select="$SettlementDate"/>
			  </PositionSettlementDate>
			  
			  <!--<xsl:variable name="Year1">
				  <xsl:value-of select="substring(substring-after(substring-after(COL4,'/'),'/'),1,4)"/>
			  </xsl:variable>

			  <xsl:variable name="Month1">
				  <xsl:value-of select="substring(substring-before(substring-after(COL4,'/'),'/'),1,2)"/>
			  </xsl:variable>
			  <xsl:variable name="Day1">
				  <xsl:value-of select="substring(normalize-space(COL4),1,2)"/>
			  </xsl:variable>

			  <PositionStartDate>
				  <xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year1)"/>
			  </PositionStartDate>

			  <xsl:variable name="Year2">
				  <xsl:value-of select="substring(substring-after(substring-after(COL5,'/'),'/'),1,4)"/>
			  </xsl:variable>

			  <xsl:variable name="Month2">
				  <xsl:value-of select="substring(substring-before(substring-after(COL5,'/'),'/'),1,2)"/>
			  </xsl:variable>

			  <xsl:variable name="Day2">
				  <xsl:value-of select="substring(normalize-space(COL5),1,2)"/>
			  </xsl:variable>

			  <PositionSettlementDate>
				  <xsl:value-of select="concat($Month2,'/',$Day2,'/',$Year2)"/>
			  </PositionSettlementDate>-->


			  <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
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

			  <xsl:variable name="Fee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL15"/>
				  </xsl:call-template>
			  </xsl:variable>


			  <Fees>

				  <xsl:choose>

					  <xsl:when test="$Fee &gt; 0">
						  <xsl:value-of select="$Fee"/>
					  </xsl:when>

					  <xsl:when test="$Fee &lt; 0">
						  <xsl:value-of select="$Fee * (-1)"/>
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

</xsl:stylesheet>