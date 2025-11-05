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

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)and contains(COL2,'Cash')!='true'">
			
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'BTIG'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="Symbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>
			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="string-length(COL6) &gt; 20">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
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

                <xsl:when test="string-length(COL6)=7 or string-length(COL6) &gt; 7">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="COL6!='*'">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


			  <IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>


					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="concat(COL6,'U')"/>
					  </xsl:when>

					  <xsl:when test="string-length(COL6)=7 or string-length(COL6) &gt; 7">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="COL6!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>


			  <SEDOL>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

				  <xsl:when test="$Asset='Option'">
					  <xsl:value-of select="''"/>
				  </xsl:when>
				  
                <xsl:when test="string-length(COL6)=7 or string-length(COL6) &gt; 7">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
                <xsl:when test="COL6!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>

            </SEDOL>





            <!--<xsl:variable name="PB_Account_NAME" select="Prospect - LP - BTIG"/>

            <xsl:variable name ="PRANA_Account_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/AccountData[@PBAccountCode=$PB_Account_NAME]/@PranaAccount"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_Account_NAME!=''">
                  <xsl:value-of select ="$PRANA_Account_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_Account_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>-->

			  <xsl:variable name="PB_FUND_NAME" select="COL1"/>

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
			  
			  <xsl:variable name="NetNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL11"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <NetNotionalValue>
				  <xsl:choose>
					  <xsl:when test="number($NetNotionalValue)">
						  <xsl:value-of select="$NetNotionalValue"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </NetNotionalValue>

			  <xsl:variable name="FXRate">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name="NetNotionalValueBase">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test="number($NetNotionalValueBase)">
						  <xsl:value-of select="$NetNotionalValueBase"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </NetNotionalValueBase>

			

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



            <xsl:variable name="Side" select="COL2"/>


            <Side>
              <xsl:choose>

                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>

                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </Side>

            <xsl:variable name ="COL4">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>

         
            <xsl:variable name="MarkPrice">
              <xsl:value-of select="COL10"/>
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




            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>


         

            <TradeDate>
              <xsl:value-of select="COL25"/>
            </TradeDate>

           
            <xsl:variable name="MarketValue1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>

              <xsl:choose>
                <xsl:when test="number($MarketValue1)">
                  <xsl:value-of select="$MarketValue1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </MarketValueBase>


            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
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

            <SettlCurrency>
              <xsl:value-of select="COL7"/>
            </SettlCurrency>

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>