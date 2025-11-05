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

  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL34"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and contains (COL84,'Cash')!='true'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL17"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="COL28"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="COL84='Put - Listed' or COL84='Call - Listed'">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>             
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="Symbol">
              <xsl:choose>
                <xsl:when test="contains(COL25,' ')">
                  <xsl:value-of select="substring-before(COL25,' ')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL25)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Symbol>

				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					</xsl:when>


					<xsl:when test="$Asset='Option'">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:when test="$Asset='Equity'">
						<xsl:value-of select="concat(normalize-space($Symbol),$PRANA_SUFFIX_NAME)"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="$PB_SYMBOL_NAME"/>
					</xsl:otherwise>
				</xsl:choose>


            </Symbol>

			  <xsl:variable name="Underlying" select="substring-before(COL19,'1')"/>
			  <xsl:variable name="undspaces">
				  <xsl:if test="$Asset='Option'">
				  <xsl:call-template name="spaces">
					  <xsl:with-param name="count" select="number(5 - string-length($Underlying))"/>
				  </xsl:call-template>
				  </xsl:if>
			  </xsl:variable>
			  <xsl:variable name="IdcoLast" select="substring(COL19,string-length($Underlying)+1)"/>
			  <xsl:variable name="Idco">
				  <xsl:value-of select="concat($Underlying,$undspaces,$IdcoLast,'U')"/>
			  </xsl:variable>


			  <IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>
					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="$Idco"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>

			  <xsl:variable name="PB_COUNTER_PARTY" select="COL40"/>

			  <xsl:variable name="PRANA_COUNTER_PARTY">
				  <xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@PranaBrokerCode"/>
			  </xsl:variable>

			  <CounterPartyID>
				  <xsl:choose>

					  <xsl:when test ="$PRANA_COUNTER_PARTY!=''">
						  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </CounterPartyID>

            <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$Asset='Option'">
                  <xsl:value-of select="concat(COL6,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->

            <xsl:variable name="PB_FUND_NAME" select="COL5"/>

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

            <xsl:variable name="Side" select="COL13"/>

            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Asset='Option'">
                  <xsl:choose>
                    <xsl:when test="$Side='Buy'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when test="$Side='Buy Long'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when test="$Side='Sell'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>

                    <xsl:when test="$Side='Sell Long'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>

                    <xsl:when test="$Side='Buy to Cover'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    
                    <xsl:when test="$Side='Sell Short'">
                      <xsl:value-of select="'C'"/>
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

                    <xsl:when test="$Side='Buy Long'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when test="$Side='Sell Long'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>

                    <xsl:when test="$Side='Sell Short'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>

					  <xsl:when test="$Side='Buy to Cover'">
						  <xsl:value-of select="'B'"/>
					  </xsl:when>


                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>


                </xsl:otherwise>

              </xsl:choose>

              </SideTagValue>

              <xsl:variable name="TradePrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL71"/>
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

            <!--<xsl:variable name="CurrencyID" select="COL6"/>

            <xsl:variable name="COL6">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>-->


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="Date" select="COL36"/>

            <PositionStartDate>
              <xsl:value-of select="$Date"/>

            </PositionStartDate>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL73"/>
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

			  <xsl:variable name="SecFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL74"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <SecFee>


				  <xsl:choose>

					  <xsl:when test="$SecFee &gt; 0">
						  <xsl:value-of select="$SecFee"/>
					  </xsl:when>

					  <xsl:when test="$SecFee &lt; 0">
						  <xsl:value-of select="$SecFee * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
				  
			  </SecFee>

			  <xsl:variable name="OrfFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL80"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <OrfFee>

				  <xsl:choose>
					  <xsl:when test="$Asset='Option'">
						  <xsl:choose>

							  <xsl:when test="$OrfFee &gt; 0">
								  <xsl:value-of select="$OrfFee"/>
							  </xsl:when>

							  <xsl:when test="$OrfFee &lt; 0">
								  <xsl:value-of select="$OrfFee * (-1)"/>
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

				 

			  </OrfFee>

						<xsl:variable name="FXRate">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL74"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <FXRate>

				  <xsl:choose>
					  
					  <xsl:when test="not(contains(COL29,'USA'))">
						  <xsl:choose>
							  <xsl:when test ="number(COL58)">
								  <xsl:value-of select="(COL70 div COL58)"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="1"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="1"/>
					  </xsl:otherwise>
					  
				  </xsl:choose>
				  
			  </FXRate>





						<!--<xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValue>

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

            </NetNotionalValue>-->

						<!--<SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>-->

					</PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>