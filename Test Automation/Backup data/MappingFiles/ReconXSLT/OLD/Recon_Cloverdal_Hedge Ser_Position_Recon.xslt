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
            <xsl:with-param name="Number" select="COL25"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and  contains(COL12,'Cash') !='true' and not(contains(COL1,'Grand'))">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL7,' ')"/>

			  <xsl:variable name="PRANA_SUFFIX_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>	  
			  </xsl:variable>

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="contains(COL12,'Option')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>
                <xsl:when test="contains(COL12,'FX')">
                  <xsl:value-of select="'FX'"/>
                </xsl:when>
				  <xsl:when test="contains(COL11,'CFD')">
					  <xsl:value-of select="'Swap'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Symbol" select="substring-before(COL13, ' ')"/>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Asset='Option'">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$Asset='FX'">
                  <xsl:value-of select="concat(translate(substring-before(COL14,' '),'/','-'),' ',translate(substring-after(COL14,' '),'-',''))"/>
                </xsl:when>
                <xsl:when test="$Asset='Equity'">
                  <xsl:value-of select="normalize-space($Symbol)"/>
                </xsl:when>

				  <xsl:when test="$Asset='Swap'">
					  <xsl:value-of select="concat(substring-before(COL5,' '),$PRANA_SUFFIX_NAME)"/>
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
                <xsl:when test="$Asset='Option'">
                  <xsl:value-of select="concat(COL16,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>

            <xsl:variable name="PB_FUND_NAME" select="COL21"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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



            <xsl:variable name="Side" select="COL24"/>


            <Side>
              <xsl:choose>

                <xsl:when test="$Asset='Options'">

                  <xsl:choose>
                    <xsl:when test="$Side='L'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>

                    <xsl:when test="$Side='S'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>



                <xsl:otherwise>


                  <xsl:choose>
                    <xsl:when test="$Side='L'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>

                    <xsl:when test="$Side='S'">
                      <xsl:value-of select="'Sell'"/>
                    </xsl:when>


                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>


                </xsl:otherwise>

              </xsl:choose>


            </Side>


            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
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

			  <MarkPriceBase>
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
			  </MarkPriceBase>


            <!--<xsl:variable name="CurrencyID" select="COL6"/>

            <xsl:variable name="COL6">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>-->


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name="TradeDate" select="COL33"/>

            <TradeDate>
              <xsl:value-of select="$TradeDate"/>
            </TradeDate>

            <!--<xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
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

            </Commission>-->

            <!--<xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL34"/>
              </xsl:call-template>
            </xsl:variable>-->

            <!--<NetNotionalValueBase>

              <xsl:choose>

                <xsl:when test="$NetNotionalValueBase&gt; 0">
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

            <xsl:variable name="NetNotionalValue">
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

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>