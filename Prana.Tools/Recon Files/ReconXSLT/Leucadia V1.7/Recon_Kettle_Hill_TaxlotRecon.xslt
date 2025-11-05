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
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>


				<xsl:if test="number($Quantity) and COL6!='CASH'">

				
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SSC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME">
							<xsl:value-of select="substring-after(COL,'.')"/>
						</xsl:variable>


						

						<xsl:variable name="Symbol">

							<xsl:value-of select="COL3"/>

						</xsl:variable>


						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>





						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

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


						<xsl:variable name="TradeDate" select="COL13"/>

						<TradeDate>
							<xsl:value-of select="$TradeDate"/>
						</TradeDate>

						<OriginalPurchaseDate>
							<xsl:value-of select="$TradeDate"/>
						</OriginalPurchaseDate>


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
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
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


						<xsl:variable name="Side" select="COL21"/>

						<Side>
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

						</Side>
						
						<xsl:variable name="COL20">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL20"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL29">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="FXRate">
							<xsl:choose>
								<xsl:when test="COL10='AUD'">
									<xsl:value-of select="$COL20 div $COL29"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="number($FXRate)">
									<xsl:value-of select="$FXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</FXRate>


						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>

							<xsl:choose>

								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValue>


						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>

							<xsl:choose>

								<xsl:when test="$MarketValueBase &gt; 0">
									<xsl:value-of select="$MarketValueBase"/>
								</xsl:when>

								<xsl:when test="$MarketValueBase &lt; 0">
									<xsl:value-of select="$MarketValueBase * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValueBase>

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL20"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>

							<xsl:choose>

								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetNotionalValue>
						
						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValueBase>

							<xsl:choose>

								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetNotionalValueBase>

						<xsl:variable name="SettlementCurrencyMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>

						<SettlementCurrencyMarketValue>

							<xsl:choose>

								<xsl:when test="$SettlementCurrencyMarketValue &gt; 0">
									<xsl:value-of select="$SettlementCurrencyMarketValue"/>
								</xsl:when>

								<xsl:when test="$SettlementCurrencyMarketValue &lt; 0">
									<xsl:value-of select="$SettlementCurrencyMarketValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</SettlementCurrencyMarketValue>
						
						<xsl:variable name="SettlementCurrencyTotalCost">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>

						<SettlementCurrencyTotalCost>

							<xsl:choose>

								<xsl:when test="$SettlementCurrencyTotalCost &gt; 0">
									<xsl:value-of select="$SettlementCurrencyTotalCost"/>
								</xsl:when>

								<xsl:when test="$SettlementCurrencyTotalCost &lt; 0">
									<xsl:value-of select="$SettlementCurrencyTotalCost * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</SettlementCurrencyTotalCost>




						<!--<xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL16)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterParty>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterParty>-->


						<CurrencySymbol>
							<xsl:value-of select="COL16"/>
						</CurrencySymbol>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>


						</PBSymbol>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>