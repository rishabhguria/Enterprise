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


	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'JPY'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CHF'">
				<xsl:value-of select="'-SWX'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'EUR'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CAD'">
				<xsl:value-of select="'-TC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="varEntryCondition">
					<xsl:choose>
						<xsl:when test="COL2='Cash and Equivalents'">
							<xsl:value-of select="0"/>
						</xsl:when>
						<xsl:when test="COL2='Currency'">
							<xsl:value-of select="0"/>
						</xsl:when>
						<xsl:when test="COL2='FX Forward'">
							<xsl:choose>
								<xsl:when test="COL10!=0">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="1"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:choose>
					<xsl:when test="number($Quantity) and normalize-space(COL6) != 'Cash and Equivalents'">
						<PositionMaster>

							<xsl:variable name="varPBName">
								<xsl:value-of select="'Jeff'"/>
							</xsl:variable>

							<xsl:variable name = "PB_FUND_NAME">
								<xsl:value-of select="normalize-space(COL1)"/>
							</xsl:variable>

							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<xsl:variable name = "PB_Symbol_NAME" >
								<xsl:value-of select="normalize-space(COL4)"/>
							</xsl:variable>

							<xsl:variable name="PRANA_Symbol_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="varPositionStartDate">
								<xsl:value-of select="''"/>
							</xsl:variable>


							<xsl:variable name="varOptionSymbol">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varEquitySymbol">
								<xsl:value-of select="COL2"/>
							</xsl:variable>

							<xsl:variable name="varFutureSymbol">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varCUSIP">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varRIC">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varBloomberg">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varOSISymbol">
								<xsl:value-of select="COL19"/>
							</xsl:variable>

							<xsl:variable name="varOptionExpiry">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varPBSymbol">
								<xsl:value-of select="COL2"/>
							</xsl:variable>

							<xsl:variable name="CompanyName">
								<xsl:value-of select="COL4"/>
							</xsl:variable>

							<xsl:variable name="varMarkPrice">
								<xsl:value-of select="COL13"/>
							</xsl:variable>
							<xsl:variable name="varNetNotionalValue">
								<xsl:value-of select="COL11"/>
							</xsl:variable>

							<xsl:variable name="varNetNotionalValueBase">
								<xsl:value-of select="COL10"/>
							</xsl:variable>


							<xsl:variable name="varCounterPartyID">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varAssetType">
								<xsl:value-of select="COL6"/>
							</xsl:variable>

							<xsl:variable name="varCommission">
								<xsl:value-of select="0"/>
							</xsl:variable>

							<xsl:variable name="varMarketValue">
								<xsl:value-of select="COL17"/>
							</xsl:variable>

							<xsl:variable name="varMarketValueBase">
								<xsl:value-of select="COL16"/>
							</xsl:variable>

							<xsl:variable name="varSMRequest">
								<xsl:value-of select="'TRUE'"/>
							</xsl:variable>

							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<FundName>
										<xsl:value-of select='$PB_FUND_NAME'/>
									</FundName>
								</xsl:when>
								<xsl:otherwise>
									<FundName>
										<xsl:value-of select='$PRANA_FUND_NAME'/>
									</FundName>
								</xsl:otherwise>
							</xsl:choose>

							<PositionStartDate>
								<xsl:value-of select="$varPositionStartDate"/>
							</PositionStartDate>


							<!--<xsl:choose>
								<xsl:when test="COL6='Options'">

									<Symbol>
										<xsl:value-of select="''"/>
									</Symbol>

									<IDCOOptionSymbol-22>
										<xsl:value-of select="concat($varOSISymbol,'U')"/>
									</IDCOOptionSymbol-22>
								</xsl:when>-->

								<!--<xsl:when test="$varAssetType = 'Equity' or $varAssetType = 'Fixed Income'">
									<xsl:variable name="varSuffix">
										<xsl:call-template name="GetSuffix">
											<xsl:with-param name="Suffix" select="COL6"/>
										</xsl:call-template>
									</xsl:variable>-->

									<Symbol>
										<xsl:choose>
											<xsl:when test="$PRANA_Symbol_NAME != ''">
												<xsl:value-of select="$PRANA_Symbol_NAME"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:choose>
													<xsl:when test="COL5 != 'USD'">
														<xsl:value-of select="'$PB_Symbol_NAME'"/>
													</xsl:when>
													<xsl:when test="COL6 = 'Options'">
													<xsl:value-of select="''"/>
												</xsl:when>

													<xsl:otherwise>
														<xsl:value-of select="$varEquitySymbol"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:otherwise>
										</xsl:choose>
									</Symbol>

									<SEDOL>
										<xsl:choose>
											<xsl:when test="$PRANA_Symbol_NAME != ''">
												<xsl:value-of select="''"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:choose>
													<xsl:when test="COL5 != 'USD'">
														<xsl:value-of select="normalize-space(COL2)"/>
													</xsl:when>
													<xsl:when test="COL6 = 'Options'">
														<xsl:value-of select="''"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="''"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:otherwise>
										</xsl:choose>
									</SEDOL>

									<IDCOOptionSymbol>

										<xsl:choose>
											<xsl:when test="$PRANA_Symbol_NAME != ''">
												<xsl:value-of select="''"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:choose>
													<xsl:when test="COL5 != 'USD'">
														<xsl:value-of select="''"/>
													</xsl:when>
													<xsl:when test="COL6 = 'Options'">
														<xsl:value-of select="concat($varOSISymbol,'U')"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="''"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:otherwise>
										</xsl:choose>




									</IDCOOptionSymbol>

								<!--</xsl:when>
							</xsl:choose>-->


							<xsl:choose>
								<xsl:when test="$varAssetType = 'EQUITY'">
									<xsl:variable name="varSuffix">
										<xsl:call-template name="GetSuffix">
											<xsl:with-param name="Suffix" select="substring-after($varEquitySymbol, '.')"/>
										</xsl:call-template>
									</xsl:variable>
									<Symbol>
										<xsl:choose>
											<xsl:when test="contains($varEquitySymbol, '.') != false">
												<xsl:value-of select="concat(substring-before($varEquitySymbol, '.'), $varSuffix)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$varEquitySymbol"/>
											</xsl:otherwise>
										</xsl:choose>
									</Symbol>

									<IDCOOptionSymbol>
										<xsl:value-of select="''"/>
									</IDCOOptionSymbol>
								</xsl:when>
							</xsl:choose>

							<PBSymbol>
								<xsl:value-of select="$varPBSymbol"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="$CompanyName"/>
							</CompanyName>


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


							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>
								</xsl:choose>
							</Side>



							<MarkPrice>
								<xsl:choose>
									<xsl:when test ="boolean(number($varMarkPrice))">
										<xsl:value-of select="$varMarkPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPrice>



							<MarketValue>
								<xsl:choose>
									<xsl:when test ="number($varMarketValue) ">
										<xsl:value-of select="$varMarketValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test ="number($varMarketValueBase) ">
										<xsl:value-of select="$varMarketValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>
							<NetNotionalValue>
								<xsl:choose>
									<xsl:when test ="number($varNetNotionalValue) ">
										<xsl:value-of select="$varNetNotionalValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValue>

							<NetNotionalValueBase>
								<xsl:choose>
									<xsl:when test ="number($varNetNotionalValueBase) ">
										<xsl:value-of select="$varNetNotionalValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValueBase>

							<CUSIPSymbol>
								<xsl:value-of select="COL20"/>
							</CUSIPSymbol>

							<SEDOLSymbol>
								<xsl:value-of select="COL19"/>
							</SEDOLSymbol>

							<ISINSymbol>
								<xsl:value-of select="COL21"/>
							</ISINSymbol>

							<SMRequest>
								<xsl:value-of select="$varSMRequest"/>
							</SMRequest>

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>


							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>


							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<CUSIPSymbol>
								<xsl:value-of select="''"/>
							</CUSIPSymbol>

							<SEDOLSymbol>
								<xsl:value-of select="''"/>
							</SEDOLSymbol>

							<ISINSymbol>
								<xsl:value-of select="''"/>
							</ISINSymbol>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>


							<SMRequest>
								<xsl:value-of select="'True'"/>
							</SMRequest>



						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


