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
        <xsl:variable name="PB_NAME">
          <xsl:value-of select="'GS'"/>
        </xsl:variable>

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="PB_FUND_NAME" select="COL26"/>

        <xsl:variable name ="PRANA_FUND_NAME">
          <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
        </xsl:variable>
        
        <xsl:if test="number($Quantity) and $Quantity!=0">

          <PositionMaster>          

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>




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
            
            
            
           

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>
			
			

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

					<xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


           <xsl:variable name ="Bloomberg" select="COL2"/>

            <Bloomberg>
              <xsl:choose>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Bloomberg>

         

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
                <xsl:with-param name="Number" select=" COL7"/>
              </xsl:call-template>
            </xsl:variable>


            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number($MarkPrice)">
                  <xsl:choose>
                    <xsl:when  test="normalize-space(COL16)='ZAR' or normalize-space(COL16)='GBP'">
                      <xsl:value-of select="$MarkPrice div 100"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$MarkPrice"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >


            </MarkPrice>

            <xsl:variable name="FXRate">
              <xsl:value-of select="COL46"/>
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

            <xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL32"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarketValueSwap1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL34"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarketValueSwap2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL40"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarketValueSwap">
              <xsl:value-of select="$varMarketValueSwap1 - $varMarketValueSwap2"/>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="contains(COL51,'EQUITY SWAP') or contains(COL51,'EQUITY SWAP FINANCING')">
                  <xsl:choose>
                    <xsl:when test="number($varMarketValueSwap)">
                      <xsl:value-of select="$varMarketValueSwap"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="number($varMarketValue)">
                      <xsl:value-of select="$varMarketValue"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>

            </MarketValue>

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varMarketValueBaseSwap1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL35"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarketValueBaseSwap2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL41"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarketValueBaseSwap">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$varMarketValueBaseSwap1 - $varMarketValueBaseSwap2"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="contains(COL51,'EQUITY SWAP') or contains(COL51,'EQUITY SWAP FINANCING')">
                  <xsl:choose>
                    <xsl:when test="number($varMarketValueBaseSwap)">
                      <xsl:value-of select="$varMarketValueBaseSwap"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="number($MarketValueBase)">
                      <xsl:value-of select="$MarketValueBase"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>
            



            <CompanyName>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </CompanyName>

            <xsl:variable name ="Date" select="''"/>


            <!--<xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before($Date,'/')-->



            <TradeDate>

              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
              <xsl:value-of select="$Date"/>
            </TradeDate>

            

            <CurrencySymbol>
              <xsl:value-of select="normalize-space(COL16)"/>
            </CurrencySymbol>

            



          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>