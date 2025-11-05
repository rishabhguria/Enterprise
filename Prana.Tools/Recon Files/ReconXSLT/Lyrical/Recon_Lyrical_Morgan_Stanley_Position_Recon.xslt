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

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring(COL1,65,19)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and contains(normalize-space(substring(COL1,28,36)),'BANK DEPOSIT PROGRAM')!='true'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Morgan Stanley'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="substring(COL1,28,36)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:variable name="Symbol" select="substring(COL1,189,21)"/>

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
            
            
            <xsl:variable name="Year" select="substring(COL1,378,4)"/>
            <xsl:variable name="Month" select="substring(COL1,383,2)"/>
            <xsl:variable name="Day" select="substring(COL1,386,2)"/>

            <xsl:variable name="Date" select="substring(COL1,378,10)"/>

            <TradeDate>

              <xsl:choose>
                <xsl:when test="contains(substring(COL1,378,10),'-')">
                  <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$Date"/>
                </xsl:otherwise>
              </xsl:choose>


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

            <xsl:variable name="PB_FUND_NAME" select="substring(COL1,7,9)"/>

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

            <xsl:variable name="Sign" select="substring(COL1,64,1)"/>

            <Quantity>
              <xsl:choose>
                <xsl:when test="$Sign = '+'">
                  <xsl:value-of select="$Quantity"/>

                </xsl:when>
                <xsl:when test="$Sign = '-'">
                  <xsl:value-of select="$Quantity * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
              
              <!--<xsl:choose>
                <xsl:when test="number($Quantity)">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>-->
            </Quantity>


			  <xsl:variable name="MarketValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="substring(COL1,105,19)"/>
				  </xsl:call-template>
			  </xsl:variable>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="substring(COL1,211,14)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="Cost">
              <xsl:value-of select="format-number($MarketValue div $Quantity,'#.##')"/>
            </xsl:variable>

			  format-number(substring(substring-before(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),' '),2),'#.00')"/>


			  <MarkPrice>
              <xsl:choose>
                <xsl:when test="$Cost &gt; 0">
                  <xsl:value-of select="$Cost"/>

                </xsl:when>
                <xsl:when test="$Cost&lt; 0">
                  <xsl:value-of select="$Cost * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>

            <xsl:variable name="Side" select="substring(COL1,325,2)"/>


            <Side>
              <xsl:choose>

                <!--<xsl:when test="$Side='sl'">
									<xsl:value-of select="'2'"/>
								</xsl:when>-->

                <xsl:when test="$Side='by'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>

                <xsl:when test="$Side='ss'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </Side>
            
            
            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <!--<SettlementDate>
							<xsl:value-of select="normalize-space(COL86)"/>
						</SettlementDate>-->

            <!--<xsl:variable name="Comm">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Comm &gt; 0">
									<xsl:value-of select="$Comm"/>

								</xsl:when>
								<xsl:when test="$Comm &lt; 0">
									<xsl:value-of select="$Comm * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL80"/>
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
						</StampDuty>-->

            <!--<xsl:variable name="COL58">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL58"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="COL59">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL59"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="FXRate">
              <xsl:value-of select="$COL58 div $COL59"/>
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

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL58"/>
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
            </MarketValueBase>-->

            <!--<xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="substring(COL1,105,19)"/>
              </xsl:call-template>
            </xsl:variable>-->

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

            <!--<xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL27"/>
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

            <xsl:variable name="NetNotionalBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL29"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalBase>
              <xsl:choose>
                <xsl:when test="number($NetNotionalBase)">
                  <xsl:value-of select="$NetNotionalBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalBase>-->



            <!--<CurrencySymbol>
              <xsl:value-of select="normalize-space(COL20)"/>
            </CurrencySymbol>

            <CUSIP>
              <xsl:value-of select="normalize-space(COL15)"/>
            </CUSIP>

            <SEDOL>
              <xsl:value-of select="normalize-space(COL37)"/>
            </SEDOL>

            <ISIN>
              <xsl:value-of select="normalize-space(COL19)"/>
            </ISIN>

            <SMRequest>
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