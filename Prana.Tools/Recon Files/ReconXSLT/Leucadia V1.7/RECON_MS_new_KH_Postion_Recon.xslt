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
            <xsl:with-param name="Number" select="COL27"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)">

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


            <xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="COL44"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="contains(COL50,'CALL') or contains(COL50,'PUT') or contains(COL50,'PUTL')">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL51,'FX FORWARDS')">
						  <xsl:value-of select="'FX'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL51,'EQUITY SWAP')">
						  <xsl:value-of select="'EquitySwap'"/>
					  </xsl:when>
					  
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <AssetNew>
				  <xsl:value-of select="$Asset"/>
			  </AssetNew>

            <xsl:variable name="Symbol">
              <xsl:choose>
                <xsl:when test="contains(COL8,'.')">
                  <xsl:value-of select="substring-before(COL8,'.')"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL8)"/>
                </xsl:otherwise>



              </xsl:choose>
            </xsl:variable>

            <Symbol>

				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					</xsl:when>

					<xsl:when test="$Asset='FX'">
						<xsl:value-of select="concat(translate(substring-before(COL6,' '),'/','-'),' ',translate(substring-after(COL6,' '),'/',''))"/>
					</xsl:when>

					<xsl:when test="$Asset='Option'">
						<xsl:value-of select="''"/>
					</xsl:when>

					<xsl:when test="$Asset='Equity'">
						<xsl:value-of select="concat(translate($Symbol,$lower_CONST,$upper_CONST),$PRANA_SUFFIX_NAME)"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="$PB_SYMBOL_NAME"/>
					</xsl:otherwise>

				</xsl:choose>


            </Symbol>

			  <xsl:variable name="Underlying" select="substring-before(COL8,'1')"/>
			  <xsl:variable name="undspaces">
				  <xsl:call-template name="spaces">
					  <xsl:with-param name="count" select="number(5 - string-length($Underlying))"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <xsl:variable name="IdcoLast" select="substring(COL8,string-length($Underlying)+1)"/>
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


			  <xsl:variable name="TradeDate" select="COL1"/>

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

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>

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


            <xsl:variable name="MarkPrice">
				<xsl:choose>
					<xsl:when test="$Asset='FX'">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL67"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL30"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
            
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


            <xsl:variable name="Side" select="COL29"/>

            <Side>
              <!--<xsl:choose>
								<xsl:when test="$Side='Buy' or $Side='Buy (TRD)' or $Side='ReceiveLong'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side='Sell' or $Side='Sell (TRD)' or $Side='DeliverLong'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$Side='SellShort'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$Side='CoverShort'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>-->

              <xsl:choose>
                <xsl:when test="$Side='L' ">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$Side='S'">
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
            </FXRate>-->

            <xsl:variable name="MarketValueBase">
				<xsl:choose>
					<xsl:when test="$Asset='FX'">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL33"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL35"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>              
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

            <xsl:variable name="MarketValue">
				<xsl:choose>
					<xsl:when test="$Asset='FX'">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL33"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL34"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
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
            </ISIN>-->

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