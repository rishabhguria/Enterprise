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

        <xsl:if test="number($Quantity) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'WELLS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="substring-after(COL43,' ')"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>-->

			  

            <xsl:variable name="AssetType">
              <xsl:choose>
                
                <xsl:when test="contains(COL14,'Call') or contains(COL14,'Put') ">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Symbol">
              <xsl:value-of select="normalize-space(COL13)"/>
            </xsl:variable>

			  <Asset>
				  <xsl:value-of select="$AssetType"/>
			  </Asset>

            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                
                   <xsl:when test="string-length(COL13) &gt; 20">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>


            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="string-length(COL13) &gt; 20">
                  <xsl:value-of select="concat(COL13, 'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>


            <xsl:variable name="PB_FUND_NAME" select="COL6"/>

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


            <!--<xsl:variable name="PB_COUNTER_PARTY" select="COL9"/>

            <xsl:variable name="PRANA_COUNTER_PARTY">
              <xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTER_PARTY]/@PranaBroker"/>
            </xsl:variable>

            <CounterParty>
              <xsl:choose>

                <xsl:when test ="$PRANA_COUNTER_PARTY != ''">
                  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_COUNTER_PARTY"/>
                </xsl:otherwise>

              </xsl:choose>
            </CounterParty>-->


            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
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


            <xsl:variable name="Side" select="COL18"/>

            <Side>

     

              <xsl:choose>

                <xsl:when test="$AssetType='Equity'">
                  <xsl:choose>


                    <xsl:when test="$Side='BY'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SS'">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>

                    <xsl:when test="$Side='CS' ">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>
					  <xsl:when test="$Side='SL' ">
						  <xsl:value-of select="'Sell'"/>
					  </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>

                <xsl:when test="$AssetType='Option'">
                  <xsl:choose>


                    <xsl:when test="$Side='BY'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SS'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                    
                     <xsl:when test="$Side='CS' ">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>
					  <xsl:when test="$Side='SL' ">
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

            <xsl:variable name="Date" select="COL8"/>

            <TradeDate>

              <xsl:value-of select="$Date"/>

            </TradeDate>


            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL37"/>
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
					  <xsl:with-param name="Number" select="COL46"/>
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

			  <xsl:variable name="GrossNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL36"/>
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


			  <xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL44"/>
              </xsl:call-template>
            </xsl:variable>


            <Fees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>

                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Fees>

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



            <xsl:variable name="FXRate">
              <xsl:value-of select="COL54"/>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="number($FXRate)">
                  <xsl:value-of select="$FXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

         

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL40"/>
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
                <xsl:with-param name="Number" select="COL42"/>
              </xsl:call-template>
            </xsl:variable>

            <StampDuty>
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
            </StampDuty>

			  <CurrencySymbol>
				  <xsl:value-of select="COL32"/>
			  </CurrencySymbol>

			  <xsl:variable name="TotalCommissionandFees" select="$Commission + $SecFee + $Fees"/>

			  <TotalCommissionandFees>
				  <xsl:choose>
					  <xsl:when test="$TotalCommissionandFees &gt; 0">
						  <xsl:value-of select="$TotalCommissionandFees"/>

					  </xsl:when>
					  <xsl:when test="$TotalCommissionandFees &lt; 0">
						  <xsl:value-of select="$TotalCommissionandFees * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </TotalCommissionandFees>

			  <CUSIP>
				  <xsl:value-of select="normalize-space(COL15)"/>
			  </CUSIP>


			  <!--<SEDOL>
              <xsl:value-of select="normalize-space(COL14)"/>
            </SEDOL>

            <ISIN>
              <xsl:value-of select="normalize-space(COL15)"/>
            </ISIN>-->

            <!--<SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>-->

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>