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
			<xsl:for-each select ="//PositionMaster">
			<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL107"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and contains(COL33,'CASH')!='true'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MAK'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL22"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_Currency_NAME" >
							<xsl:value-of select ="substring-before(COL43,' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Currency_NAME">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_Currency_NAME]/@PranaCurrency"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL106,'Foreign Currency')">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="Symbol" select="COL13"/>


						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="concat($PRANA_Currency_NAME,'-','USD',' ','SPOT')"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="normalize-space($Symbol)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>


						</Symbol>

						<!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$Asset='Option'">
                  <xsl:value-of select="concat(COL6,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->

						<xsl:variable name="PB_FUND_NAME" >
							<xsl:choose>
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL2"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

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

						<NetPosition>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Side" select="normalize-space(COL58)"/>



						<SideTagValue>

							<xsl:choose>

								<xsl:when test="contains(COL59,'CXL')">
									<xsl:choose>
										<xsl:when test="$Side='BC'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$Side='B'">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="$Side='S'">
											<xsl:value-of select="'D'"/>
										</xsl:when>

										<xsl:when test="$Side='SHSC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='CVS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='CVSC'">
											<xsl:value-of select="'5'"/>
										</xsl:when>


										<xsl:when test="$Side='SC'">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='B'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='S'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$Side='SHS'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="$Side='CVS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="TradePrice">
							<xsl:choose>
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL108 div COL107"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL44"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test="$TradePrice &gt; 0">
									<xsl:value-of select="$TradePrice"/>
								</xsl:when>

								<xsl:when test="$TradePrice &lt; 0">
									<xsl:value-of select="$TradePrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<!--<xsl:variable name="CurrencyID" select="COL6"/>

            <xsl:variable name="COL6">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>-->

					<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>
					<xsl:variable name="Year" select="substring-before(COL56,'-')"/>
						<xsl:variable name="Month" select="substring-before(substring-after(COL56,'-'),'-')"/>
						<xsl:variable name="Day" select="substring-after(substring-after(COL56,'-'),'-')"/>
					<xsl:variable name="Date" select="COL56"/>
						<PositionStartDate>
							<!--<xsl:choose>
                <xsl:when test="contains(COL56,'-')">
                  <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$Date"/>
                </xsl:otherwise>
              </xsl:choose>-->
							<xsl:choose>
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="contains(COL56,'-')">
											<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
										</xsl:when>
									<xsl:otherwise>
											<xsl:value-of select="$Date"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</PositionStartDate>
						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(COL18,'FI')">
									<xsl:choose>
										<xsl:when test="contains(COL56,'-')">
											<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$Date"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL57"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionSettlementDate>
						<xsl:variable name="AccruedInterest">
							<xsl:choose>
								<xsl:when test="COL18='FI'">
									<xsl:value-of select="COL86"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="$AccruedInterest &gt; 0">
									<xsl:value-of select="$AccruedInterest"/>
								</xsl:when>
								<xsl:when test="$AccruedInterest &lt; 0">
									<xsl:value-of select="$AccruedInterest*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccruedInterest>
						<xsl:variable name ="COL107">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL107"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL44">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL44"/>
							</xsl:call-template>
						</xsl:variable>
					<xsl:variable name ="COL73">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL73"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL108">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL108"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varCOL107">
							<xsl:choose>
								<xsl:when test="COL107 &lt; 0">
									<xsl:value-of select="COL107 * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL107"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varCOL44">
							<xsl:choose>
								<xsl:when test="$COL44 &lt; 0">
									<xsl:value-of select="$COL44 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL44"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varCOL73">
							<xsl:choose>
								<xsl:when test="$COL73 &lt; 0">
									<xsl:value-of select="$COL73 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL73"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varCOL108">
							<xsl:choose>
								<xsl:when test="$COL108 &lt; 0">
									<xsl:value-of select="$COL108 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL108"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="StampDuty">
							<xsl:choose>
								<xsl:when test="contains(normalize-space(COL25),'FIXED INCOME')">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:when test="COL3='USD' and normalize-space(COL58)='B'">
									<xsl:value-of select="(COL107 * $varCOL44) + ($varCOL73) - ($varCOL108)"/>
								</xsl:when>
								<xsl:when test="COL3='USD' and normalize-space(COL58)='S'">
									<xsl:value-of select="(COL107 * $varCOL44) + ($varCOL73) + ($varCOL108)"/>
								</xsl:when>
								<xsl:when test="COL3='USD' and normalize-space(COL101)='SELL SHORT'">
									<xsl:value-of select="(COL107 * $varCOL44) + ($varCOL73) + ($varCOL108)"/>
								</xsl:when>
								<xsl:when test="COL3='USD' and  normalize-space(COL58)='SC'">
									<xsl:value-of select="(COL107 * $varCOL44) + ($varCOL73) - ($varCOL108)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<StampDuty>

							<xsl:choose>
								<xsl:when test ="COL18='FI'">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:when test="contains(COL59,'CXL SELL')">
									<xsl:choose>
										<xsl:when test="$StampDuty &gt; 0">
											<xsl:value-of select="$StampDuty * -1"/>
										</xsl:when>

										<xsl:when test="$StampDuty &lt; 0">
											<xsl:value-of select="$StampDuty"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="$StampDuty &gt; 0">
											<xsl:value-of select="$StampDuty "/>
										</xsl:when>

										<xsl:when test="$StampDuty &lt; 0">
											<xsl:value-of select="$StampDuty *(-1)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>



						</StampDuty>

						<xsl:variable name="Commission1">
							<xsl:choose>
								<xsl:when test="contains(normalize-space(COL25),'FIXED INCOME')">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:when test="COL3!='USD' and (normalize-space(COL58)='S' or normalize-space(COL58)='SHS' or normalize-space(COL58)='SC'  or normalize-space(COL58)='B')">
									<xsl:value-of select="($varCOL107 * $varCOL44) - ($varCOL73) - ($varCOL108)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL60"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$Commission1"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>

							<xsl:choose>

								<xsl:when test="contains(COL59,'CXL')">
									<xsl:choose>
										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission * -1"/>
										</xsl:when>

										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>

									<xsl:choose>
										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission "/>
										</xsl:when>

										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission *(-1)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>

						</Commission>



						<!--<xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValueBase>

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
                <xsl:with-param name="Number" select="COL14"/>
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



            <xsl:variable name="GrossNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL20"/>
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
            

            <ISIN>
              <xsl:value-of select="normalize-space(COL21)"/>
            </ISIN>

            <CUSIP>
              <xsl:value-of select="normalize-space(COL9)"/>
            </CUSIP>-->

						<!--<SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>-->

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>