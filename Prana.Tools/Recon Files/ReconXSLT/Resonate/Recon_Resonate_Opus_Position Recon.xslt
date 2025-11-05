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
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and contains(COL1,'CASH')!='true' ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Morgan Stanley'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="substring-after(COL27,'.')"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>-->

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="string-length(COL3) &gt; 16">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Symbol" >
              <xsl:choose>
                <xsl:when test="contains(COL3,' ')">
                  <xsl:value-of select="substring-before(COL3,' ')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="COL3"/>
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
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

				

            </Symbol>

			  <xsl:variable name="COL3">
				  <xsl:choose>
					  <xsl:when test="contains(normalize-space(COL3),' ')">
						  <xsl:value-of select="concat(substring-before(normalize-space(COL3),' '),substring-after(normalize-space(COL3),' '))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="COL3"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
            <xsl:variable name="Underlying" select="substring-before($COL3,'1')"/>
            <xsl:variable name="undspaces">
				
						<xsl:call-template name="spaces">
							<xsl:with-param name="count" select="number(5 - string-length($Underlying))"/>
						</xsl:call-template>
					          
            </xsl:variable>
            <!--<xsl:variable name="IdcoLast" select="substring(COL3,string-length($Underlying)+1)"/>-->
			  <xsl:variable name="IdcoLast">
				  <xsl:choose>
					  <xsl:when test="contains(normalize-space(COL3),' ')">
						  <xsl:value-of select="substring-after(normalize-space(COL3),' ')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="substring(COL3,string-length($Underlying)+1)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable> 
            <xsl:variable name="Idco">
              <xsl:value-of select="concat($Underlying,$undspaces,$IdcoLast,'U')"/>
            </xsl:variable>


            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$Asset='Option'">
                  <xsl:value-of select="$Idco"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>

            <xsl:variable name="TradeDate" select="''"/>

            <TradeDate>
              <xsl:value-of select="$TradeDate"/>
            </TradeDate>


            <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="string-length(COL41)=21">
                  <xsl:value-of select="concat(COL41,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>-->

            <xsl:variable name="PB_FUND_NAME" select="COL8"/>

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

                <xsl:when test="$Asset='Option'">
                  <xsl:choose>
                    <xsl:when test="$Side='Long'">
                      <xsl:value-of select="'Buy to open'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Short'">
                      <xsl:value-of select="'Sell to open'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </xsl:when>



                <xsl:otherwise>


                  <xsl:choose>
                    <xsl:when test="$Side='Long'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="$Side='Short'">
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
                <xsl:with-param name="Number" select="COL7"/>
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

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


          
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

			  <xsl:variable name="MarketValueBase">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL13"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <MarketValueBase>
				  <xsl:choose>
					  <xsl:when test="number(COL13)">
						  <xsl:value-of select="COL13"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </MarketValueBase>

			

            <SEDOL>
              <xsl:value-of select="normalize-space(COL4)"/>
            </SEDOL>

			  <ISINSymbol>
				  <xsl:value-of select="normalize-space(COL5)"/>
			  </ISINSymbol>

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