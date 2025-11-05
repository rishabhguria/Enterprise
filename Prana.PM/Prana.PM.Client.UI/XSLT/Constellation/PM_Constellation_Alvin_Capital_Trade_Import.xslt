<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}

	</msxsl:script>
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
						<xsl:with-param name="Number" select="COL20"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>
						<xsl:variable name = "PB_COMPANY_NAME" >
							<xsl:value-of select="COL8"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
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


						<xsl:variable name="varAsset">
							<xsl:choose>
								<xsl:when test="COL6='Option'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL6='FX'">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:when test="COL6='Future'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL6='Equity'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL6='Swap'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="VarFuture">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL9,' ')) = '5'">
									<xsl:value-of select="concat(substring(substring-before(COL9,' '),1,3),' ',substring(substring-before(COL9,' '),4,2))"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(COL9,' ')) = '4'">
									<xsl:value-of select="concat(substring(substring-before(COL9,' '),1,2),' ',substring(substring-before(COL9,' '),3,2))"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						
						
						

						<xsl:variable name="Symbol">
							<xsl:value-of select="COL8"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$varAsset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="COL11!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								
								<xsl:when test="$varAsset='Future'">
									<xsl:value-of select="$VarFuture"/>
								</xsl:when>
								<xsl:when test="$varAsset='FXForward'">
									<xsl:value-of select="concat(COL31,'/',COL32,' ',COL18)"/>
								</xsl:when>

								<xsl:when test="$varAsset='Equity'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="COL8"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="COL11!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="$varAsset='EquityOption'">
									<xsl:value-of select="concat(COL8,'U')"/>
								</xsl:when>

								<xsl:when test="$varAsset='Future'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varAsset='FXForward'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$varAsset='Equity'">
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

                <xsl:when test="COL11!='*'">
                  <xsl:value-of select="COL11"/>
                </xsl:when>


				  <xsl:when test="$varAsset='EquityOption'">
					  <xsl:value-of select="''"/>
				  </xsl:when>


				  <xsl:when test="$varAsset='Future'">
					  <xsl:value-of select="''"/>
				  </xsl:when>
				  <xsl:when test="$varAsset='FXForward'">
					  <xsl:value-of select="''"/>
				  </xsl:when>

				  <xsl:when test="$varAsset='Equity'">
					  <xsl:value-of select="''"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SEDOL>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL22"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL24"/>
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
						</Commission>						
						
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
						<xsl:variable name="Side">
							<xsl:value-of select="COL4"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side='Sell'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$Side='SellShort'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="$Side='Buy to Close' or $Side='CoverShort'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='Sell'">
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
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>					
													
							
						</SideTagValue>
						<xsl:variable name="varStampDuty">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test ="$varStampDuty &gt; 0">
									<xsl:value-of select ="$varStampDuty"/>
								</xsl:when>
								<xsl:when test ="$varStampDuty &lt; 0">
									<xsl:value-of select ="$varStampDuty * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						<!--<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="COL33"/>
						</xsl:variable>
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>-->
						<xsl:variable name="NetNotional">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Fees">
							<xsl:choose>
								<xsl:when test="($Side='Buy' or $Side='CoverShort') and $varAsset='Equity'">
									<xsl:value-of select="format-number(($NetNotional - $Position * $CostBasis - $Commission),'0.##') "/>
								</xsl:when>
								<xsl:when test="($Side='Buy' or $Side='CoverShort') and $varAsset='EquityOption'">
									<xsl:value-of select="format-number(($NetNotional - ($Position * 100) * $CostBasis - $Commission),'0.##') "/>
								</xsl:when>
								<xsl:when test="($Side='Sell' or $Side='SellShort') and $varAsset='Equity'">
									<xsl:value-of select="format-number(($Position * $CostBasis - $Commission  - $NetNotional),'0.##') "/>
								</xsl:when>
								<xsl:when test="($Side='Sell' or $Side='SellShort')  and $varAsset='EquityOption'">
									<xsl:value-of select="format-number((($Position * 100) * $CostBasis  - $Commission - $NetNotional),'0.##')"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<Fees>

							<xsl:choose>

								<xsl:when test="$Fees &gt; 0">
									<xsl:value-of select="$Fees"/>
								</xsl:when>

								<xsl:when test="$Fees &lt; 0">
									<xsl:value-of select="$Fees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Fees>
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL33)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='MS']/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>
						<PositionStartDate>
							<xsl:value-of select="COL17"/>
						</PositionStartDate>
						<PositionSettlementDate>
							<xsl:value-of select="COL18"/>
						</PositionSettlementDate>
						
						
						
						
						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</PBSymbol>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


