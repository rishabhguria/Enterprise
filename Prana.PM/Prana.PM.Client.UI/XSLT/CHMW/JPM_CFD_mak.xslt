<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">
        <xsl:if test ="number(COL11)">
          <PositionMaster>

            <xsl:variable name = "PB_NAME">
              <xsl:value-of select="'JP Morgan'"/>
            </xsl:variable>
            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            
          
            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="COL29!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
               

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>


            <ISIN>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL29!='*'">
                  <xsl:value-of select="COL29"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </ISIN>


            <Symbology>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="COL29!='*'">
                  <xsl:value-of select="'ISIN'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </Symbology>

			  <SecurityRequestDescription>
				  <xsl:value-of select="'CFD'"/>
			  </SecurityRequestDescription>

			  <ExchangeCode>
				  <xsl:choose>
					  <xsl:when test="contains(COL4,'USD')">
						  <xsl:value-of select="'US'"/>
					  </xsl:when>
			       </xsl:choose>
			  </ExchangeCode>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name ="PRANA_FUND_NAME">
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

            <PositionStartDate>
              <xsl:value-of select ="COL9"/>
            </PositionStartDate>


            <PositionSettlementDate>
							<xsl:value-of select ="COL10"/>
						</PositionSettlementDate>

            <xsl:variable name ="NetPosition">
              <xsl:value-of select ="number(COL11)"/>
            </xsl:variable>

            <NetPosition>

              <xsl:choose>

                <xsl:when test ="$NetPosition &lt;0">
                  <xsl:value-of select ="$NetPosition*-1"/>
                </xsl:when>

                <xsl:when test ="$NetPosition &gt;0">
                  <xsl:value-of select ="$NetPosition"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </NetPosition>

            <SideTagValue>
              <xsl:choose>

                <xsl:when test ="COL7='SS'">
                  <xsl:value-of select ="'5'"/>
                </xsl:when>

                <xsl:when test ="COL7='S'">
                  <xsl:value-of select ="'2'"/>
                </xsl:when>


                <xsl:when test ="COL7='CS'">
                  <xsl:value-of select ="'B'"/>
                </xsl:when>

                <xsl:when test ="COL7='B'">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>

            </SideTagValue>

            <xsl:variable name ="varCostBasis">
              <xsl:value-of select ="COL14"/>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>

                <xsl:when test ="$varCostBasis &lt;0">
                  <xsl:value-of select ="$varCostBasis*-1"/>
                </xsl:when>

                <xsl:when test ="$varCostBasis &gt;0">
                  <xsl:value-of select ="$varCostBasis"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>
