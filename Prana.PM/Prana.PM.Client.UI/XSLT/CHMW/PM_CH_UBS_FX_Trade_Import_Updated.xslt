<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="COL1">
          <xsl:choose>
            <xsl:when test="substring(COL1,1,3)='USD'">
              <xsl:value-of select="concat(substring(COL1,4,3),substring(COL1,1,3))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="DealtAmt">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="ContraAmt">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Position">
          <xsl:choose>
            <xsl:when test ="substring($COL1,1,3) = normalize-space(COL6)">
              <xsl:value-of select="$DealtAmt"/>
            </xsl:when>
            <xsl:when test="substring($COL1,1,3) = normalize-space(COL8)">
              <xsl:value-of select="$ContraAmt"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'UBS AG - Stamford'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>


			  <xsl:variable name="varExpMon">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL10,'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(COL10,'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(COL10,'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExpDay">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(COL10,'/'),'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(substring-after(COL10,'/'),'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after(COL10,'/'),'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

			  <xsl:variable name="varMonth">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL10,'/'))=1">
						  <xsl:value-of select="concat('0',substring-before(COL10,'/'))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="substring-before(COL10,'/')"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <xsl:variable name="ExpirationDate">
              <!--<xsl:value-of select="concat(substring-after(substring-after(COL10,'/'),'/'),$varExpMon,$varExpDay)"/>-->
				<xsl:value-of select="concat($varMonth,'/',$varExpDay,'/',substring(substring-after(substring-after(COL10,'/'),'/'),3))"/>
            </xsl:variable>

            <xsl:variable name="TradeDay" select="number(substring-before(substring-after(COL15,'/'),'/'))"/>
            <xsl:variable name="SettleDay" select="number(substring-before(substring-after(COL12,'/'),'/'))"/>

            <xsl:variable name="PreSymbol" select="substring($COL1,1,3)"/>
            <xsl:variable name="PostSymbol" select="substring($COL1,4)"/>

            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="translate($PRANA_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
                </xsl:when>

				  <xsl:when test="$PreSymbol != ''">
					  <xsl:value-of select="''"/>
				  </xsl:when>

				  <xsl:otherwise>
					  <xsl:value-of select="translate($PB_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
				  </xsl:otherwise>

			  </xsl:choose>

		  </Symbol>

		  <Bloomberg>
			  <xsl:choose>
				  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
					  <xsl:value-of select="translate($PRANA_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
				  </xsl:when>
				  <xsl:when test="contains(COL2,'Swap')">
					  <xsl:value-of select="concat($PreSymbol,$PostSymbol,' ','CURNCY')"/>
				  </xsl:when>

				  <xsl:when test="contains(COL2,'Spot')">
						  <xsl:value-of select="concat($PreSymbol,$PostSymbol,' ','CURNCY')"/>
					  </xsl:when>

					  <xsl:when test="contains(COL2,'Forward')">
						  <xsl:value-of select="concat($PreSymbol,'/',$PostSymbol,' ','N',' ',$ExpirationDate,' ','CURNCY')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Bloomberg>

			  <Symbology>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="'Symbol'"/>
					  </xsl:when>
					  <xsl:when test="$PreSymbol != ''">
						  <xsl:value-of select="'Bloomberg'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbology>

            <xsl:variable name="PB_FUND_NAME" select="COL23"/>
			  <xsl:variable name ="PRANA_FUND_NAME">
				  <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:variable name="CostBasis">
              <!--<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>-->
              <!--<xsl:choose>
								<xsl:when test="substring(COL1,1,3)='USD'">
									<xsl:choose>
										<xsl:when test="number(COL9)">
											<xsl:value-of select="1 div COL9"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL9"/>
								</xsl:otherwise>
							</xsl:choose>-->
              <xsl:choose>
                <xsl:when test="contains(COL6,'USD')">
                  <xsl:choose>
                    <xsl:when test="number(COL7)">
                      <xsl:value-of select="COL5 div COL7"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test="contains(COL8,'USD')">
                  <xsl:choose>
                    <xsl:when test="number(COL5)">
                      <xsl:value-of select="COL7 div COL5"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <CostBasis>
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
            </CostBasis>

            <xsl:variable name="Side" select="normalize-space(COL4)"/>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
			
			<CounterPartyID>
			<xsl:value-of select="6"/>
			</CounterPartyID>
            <PositionStartDate>
              <xsl:value-of select="COL15"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="COL12"/>
            </PositionSettlementDate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>