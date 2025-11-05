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
  <xsl:template name="Date">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="1"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="2"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="3"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="4"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="5"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="7"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="8"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="9"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>



  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL34"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and contains(COL26,'Trade')='true'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Morgan Stanley'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL17"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
			  
			  <xsl:variable name="AssetType">
				  <xsl:choose>

					  <xsl:when test="contains(COL15,'CALL') or contains(COL15,'PUT')">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>



					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


            <xsl:variable name="Symbol" select="COL19"/>
            <Symbol>


              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

				  <xsl:when test="AssetType='EquityOption'">
					  <xsl:value-of select="''"/>
				  </xsl:when>

                <xsl:when test="AssetType='Equity'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

			  <IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="AssetType='EquityOption'">
						  <xsl:value-of select="concat(COL19,'U')"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>
			  
            <xsl:variable name="PB_FUND_NAME" select="COL3"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
				  <xsl:when test="$Quantity &lt; 0">
					  <xsl:value-of select="$Quantity * (-1)"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL71"/>
              </xsl:call-template>
            </xsl:variable>


            <AvgPX>
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
            </AvgPX>


            <xsl:variable name="Side" select="COL14"/>


            <Side>
				<xsl:choose>

					<xsl:when test="$AssetType='Equity'">
						<xsl:choose>


							<xsl:when test="$Side='Buy Long'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>

							<xsl:when test="$Side='Sell Short'">
								<xsl:value-of select="'Sell short'"/>
							</xsl:when>

							<xsl:when test="$Side='Buy to Cover'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>

							<xsl:when test="$Side='Sell Long'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>


						</xsl:choose>
					</xsl:when>

					<xsl:when test="$AssetType='EquityOption'">
						<xsl:choose>


							<xsl:when test="$Side='Buy Long'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>

							<xsl:when test="$Side='Sell Short'">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>

							<xsl:when test="$Side='Buy to Cover'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>

							<xsl:when test="$Side='Sell Long'">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>


						</xsl:choose>
					</xsl:when>

				</xsl:choose>

            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>


            </PBSymbol>



            <!--<xsl:variable name="Year" select="substring(COL7,1,4)"/>
            <xsl:variable name="Month" select="substring(COL7,5,2)"/>
            <xsl:variable name="Day" select="substring(COL7,7,2)"/>-->

            <xsl:variable name="Date" select="COL36"/>

            <TradeDate>

              <xsl:value-of select="$Date"/>

            </TradeDate>

            <xsl:variable name="Date1" select="COL37"/>

            <SettlementDate>

              <xsl:value-of select="$Date1"/>

            </SettlementDate>


            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL82"/>
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

            </NetNotionalValue>

            <xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL115"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValueBase>

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

            </NetNotionalValueBase>

          
            <!--<xsl:variable name="MiscFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL59"/>
              </xsl:call-template>
            </xsl:variable>


            <MiscFees>
              <xsl:choose>
                <xsl:when test="$MiscFees &gt; 0">
                  <xsl:value-of select="$MiscFees"/>

                </xsl:when>
                <xsl:when test="$MiscFees &lt; 0">
                  <xsl:value-of select="$MiscFees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MiscFees>-->

            <xsl:variable name="GrossNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL72"/>
              </xsl:call-template>
            </xsl:variable>

            <GrossNotionalValue>

              <xsl:choose>

                <xsl:when test="$GrossNotionalValue &gt; 0">
                  <xsl:value-of select="$GrossNotionalValue"/>
                </xsl:when>

                <xsl:when test="$GrossNotionalValue &lt; 0">
                  <xsl:value-of select="$GrossNotionalValue * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </GrossNotionalValue>
            
            <xsl:variable name="GrossNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL107"/>
              </xsl:call-template>
            </xsl:variable>

            <GrossNotionalValueBase>

              <xsl:choose>

                <xsl:when test="$GrossNotionalValueBase &gt; 0">
                  <xsl:value-of select="$GrossNotionalValueBase"/>
                </xsl:when>

                <xsl:when test="$GrossNotionalValueBase &lt; 0">
                  <xsl:value-of select="$GrossNotionalValueBase * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </GrossNotionalValueBase>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL73"/>
              </xsl:call-template>
            </xsl:variable>

			  <TotalCommission>

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

			  </TotalCommission>

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

			  <CUSIP>
              <xsl:value-of select="normalize-space(COL18)"/>
            </CUSIP>


            <SEDOL>
              <xsl:value-of select="normalize-space(COL20)"/>
            </SEDOL>

            <ISIN>
              <xsl:value-of select="normalize-space(COL22)"/>
            </ISIN>

			  <xsl:variable name="PB_COUNTER_PARTY" select="COL40"/>

			  <xsl:variable name="PRANA_COUNTER_PARTY">
				  <xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@MLPBroker"/>
			  </xsl:variable>

			  <CounterParty>
				  <xsl:choose>

					  <xsl:when test ="$PRANA_COUNTER_PARTY!='' ">
						  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_COUNTER_PARTY"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </CounterParty>

			 

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