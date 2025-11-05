<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL28"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'HedgeServ'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL6"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol" select="COL8"/>

            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="normalize-space($Symbol)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>

			  <xsl:variable name="PB_COUNTER_PARTY" select="COL60"/>

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

            <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$AssetType='Options'">
                  <xsl:value-of select="concat($Symbol,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>

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

            <xsl:variable name="Side" select="COL29"/>


            <Side>


              <xsl:choose>
                <xsl:when test="$Side='L'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$Side='S'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

           


            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select=" COL30"/>
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

            <xsl:variable name="FXRate">
              <xsl:value-of select="COL46 "/>
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

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL32"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="number($MarketValue)">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="number($MarketValueBase)">
                  <xsl:value-of select="$MarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>
			  <xsl:variable name ="Bloomberg" select="COL14"/>
				  			  
			  <Bloomberg>
				  <xsl:choose>
					  <xsl:when test ="contains(COL14,'US')">
						  <xsl:value-of select="concat($Bloomberg,' ','EQUITY')"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose> 
			  </Bloomberg>



            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name ="Date" select="COL1"/>


            <!--<xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before($Date,'/')-->"/>



            <TradeDate>

              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
              <xsl:value-of select="$Date"/>
            </TradeDate>

            <CUSIP>
              <xsl:value-of select="normalize-space(COL7)"/>
            </CUSIP>


            <SEDOL>
              <xsl:value-of select="normalize-space(COL9)"/>
            </SEDOL>

			  <ISINSymbol>
				  <xsl:value-of select="normalize-space(COL10)"/>
			  </ISINSymbol>



          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>