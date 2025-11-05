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

				<xsl:if test="number($Position) and contains(COL33,'CASH')!='true' and COL93!='CORPORATE ACTION'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JP Morgan'"/>	
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL22"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_Currency_NAME" >
							<xsl:value-of select ="substring-before(COL43,' ')"/>
						</xsl:variable>


						<xsl:variable name="PRANA_Currency_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyDesc=$PB_Currency_NAME]/@CurrencyName"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL18='OPTN'">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
								<xsl:when test="contains(COL106,'Foreign Currency')">
									<xsl:value-of select="'FX'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="Year" select="substring-before(COL17,'-')"/>
						<xsl:variable name="Month" select="substring-before(substring-after(COL17,'-'),'-')"/>
						<xsl:variable name="Date" select="substring-after(substring-after(COL17,'-'),'-')"/>

						<xsl:variable name="Date1">
							<xsl:value-of select="concat($Month,'/',$Date,'/',substring($Year,3))"/>
						</xsl:variable>

						<xsl:variable name="Bloomberg">
							<xsl:value-of select="concat(COL27,' ',$Date1,' ',COL16,format-number(COL24,'##'),' ',substring(COL15,1,2),' ','EQUITY')"/>
						</xsl:variable>


						<xsl:variable name="Symbol" select="COL32"/>


						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FX'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="COL30!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL36!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>


						</Symbol>


						<Bloomberg>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="$Bloomberg"/>
								</xsl:when>

								<xsl:when test="$Asset='FX'">
									<xsl:value-of select="concat($PRANA_Currency_NAME,'USD',' ','CURNCY')"/>
								</xsl:when>

								<xsl:when test="COL30!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL36!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Bloomberg>

						<CUSIP>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FX'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="COL30!='*'">
									<xsl:value-of select="COL30"/>
								</xsl:when>
								<xsl:when test="COL36!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</CUSIP>


						<ISIN>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Asset='FX'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL30!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL36!='*'">
									<xsl:value-of select="COL36"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ISIN>

						<Symbology>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>

								<xsl:when test="$Asset='FX'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>

								<xsl:when test="COL30!='*'">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>

								<xsl:when test="COL36!='*'">
									<xsl:value-of select="'ISIN'"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbology>


						<xsl:variable name="PB_FUND_NAME" select="COL2"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

								<xsl:when test="contains(COL59,'CXL') or contains(COL59,'CANCEL')">

									<xsl:choose>
										<xsl:when test="$Side='B'">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="$Side='S' ">
											<xsl:value-of select="'D'"/>
										</xsl:when>

										<xsl:when test="$Side='SHSC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='SHSZ'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:when test="$Side='CVS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='CVSC'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:when test="$Side='CVSZ'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='SHSX'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='CVSX'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:when test="$Side='SC'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='BC'">
											<xsl:value-of select="'2'"/>
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

										<xsl:when test="$Side='S' or $Side='BC'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$Side='SHS'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:when test="$Side='CVS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='XLO'">
											<xsl:value-of select="'D'"/>
										</xsl:when>

										<xsl:when test="$Side='DSS' and COL62='SELL'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="$Side='WTH' and COL62='SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$Side='DPG' and COL62='BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='WPG' and COL62='SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$Side='DDP' and $Position &gt; 0">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='DDP' and $Position &lt; 0">
											<xsl:value-of select="'2'"/>
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
								<xsl:when test="$Asset='FX'">
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

						<ExchangeCode>
							<xsl:choose>
								<xsl:when test="contains(COL3,'CAD')">
									<xsl:value-of select="'CN'"/>
								</xsl:when>

							</xsl:choose>
						</ExchangeCode>

						<!--<xsl:variable name="CurrencyID" select="COL6"/>

            <xsl:variable name="COL6">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>-->


						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="Year1" select="substring-before(COL56,'-')"/>
						<xsl:variable name="Month1" select="substring-before(substring-after(COL56,'-'),'-')"/>
						<xsl:variable name="Day" select="substring-after(substring-after(COL56,'-'),'-')"/>

						<xsl:variable name="Date2" select="COL56"/>

						<PositionStartDate>

							<xsl:choose>
								<xsl:when test="$Asset='FX'">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="contains(COL56,'-')">
											<xsl:value-of select="concat($Month1,'/',$Day,'/',$Year1)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$Date2"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>


						</PositionStartDate>

						<xsl:variable name="Year2" select="substring-before(COL57,'-')"/>
						<xsl:variable name="Month2" select="substring-before(substring-after(COL57,'-'),'-')"/>
						<xsl:variable name="Day1" select="substring-after(substring-after(COL57,'-'),'-')"/>

						<xsl:variable name="Date3" select="COL57"/>

						<PositionSettlementDate>

							<xsl:choose>
								<xsl:when test="contains(COL57,'-')">
									<xsl:value-of select="concat($Month2,'/',$Day1,'/',$Year2)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$Date3"/>
								</xsl:otherwise>
							</xsl:choose>


						</PositionSettlementDate>









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
								<xsl:when test="$COL107 &lt; 0">
									<xsl:value-of select="$COL107 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL107"/>
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
								<xsl:when test="contains(normalize-space(COL33),'FIXED INCOME')">
									<xsl:value-of select="0"/>
								</xsl:when>

								<!--<xsl:when test="normalize-space(COL58)='S' or normalize-space(COL58)='SHS' or normalize-space(COL58)='SC'">-->
								<xsl:when test="COL33='EQUITIES' and COL73!=0">
									<xsl:choose>
										<xsl:when test="COL87='B'">
											<xsl:value-of select="($varCOL107 * $varCOL44) + ($varCOL73) - ($varCOL108)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="($varCOL107 * $varCOL44) - ($varCOL73) - ($varCOL108)"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<StampDuty>
							<xsl:choose>
								<xsl:when test="number($StampDuty)">
									<xsl:choose>

										<xsl:when test="$StampDuty &gt; 0">
											<xsl:value-of select="format-number($StampDuty,'0.###')"/>
										</xsl:when>

										<xsl:when test="$StampDuty &lt; 0">
											<xsl:value-of select="format-number($StampDuty,'0.###') * (-1)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>



						</StampDuty>


						<xsl:variable name="Commission">
							<xsl:choose>
								<xsl:when test="contains(COL106,'Foreign Currency')">
									<xsl:value-of select="$COL73"/>
								</xsl:when>
								<xsl:when test="COL73=0 and contains(normalize-space(COL33),'FIXED INCOME')!='true' ">
									<xsl:value-of select="format-number(($varCOL107 * $varCOL44)  - ($varCOL108),'0.##')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$COL73"/>
								</xsl:otherwise>
							</xsl:choose>
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