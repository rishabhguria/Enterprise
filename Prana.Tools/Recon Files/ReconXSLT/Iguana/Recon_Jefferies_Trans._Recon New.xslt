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

	<xsl:template name="GetBrokerID">
		<xsl:param name="BrokerName"/>
		<xsl:choose>
			<xsl:when test="$BrokerName = 'JETS'">
				<xsl:value-of select="2"/>
			</xsl:when>
			<xsl:when test="$BrokerName = 'JEFF'">
				<xsl:value-of select="1"/>
			</xsl:when>
			<xsl:when test="$BrokerName = 'BTIG'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="0"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//Comparision">
				
				<xsl:if test="number(COL8) and normalize-space(COL5)!='U.S. DOLLARS'">
					<!--<xsl:if test="number(COL8) and normalize-space(COL15)='JETS' and normalize-space(COL5)!='U.S. DOLLARS' and (normalize-space(COL7)!='Assign Buy' and contains(normalize-space(COL7),'Exercise Buy')!='true')">-->
				<!--<xsl:if test="number(COL8) and normalize-space(COL5)!='U.S. DOLLARS' and (normalize-space(COL15)!='JEFF' or COL15!='')">-->

					<PositionMaster>

						<xsl:variable name="varPBName">
							<xsl:value-of select="'Morcom'"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name = "PB_Currency_NAME" >
							<xsl:value-of select="normalize-space(COL24)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--   Fund -->
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="normalize-space(COL17)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:value-of select="normalize-space(COL16)"/>
						</xsl:variable>

						<xsl:variable name="varCUSIP">
							<xsl:value-of select="COL20"/>
						</xsl:variable>

						<xsl:variable name="varRIC">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varBloomberg">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="varOSISymbol">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varEquitySymbol">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varOptionExpiry">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varPBSymbol">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="CompanyName">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="varDescription">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varNetPosition">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="varCostBasis">
							<xsl:value-of select="COL26"/>
						</xsl:variable>

						<xsl:variable name="varFXConversionMethodOperator">
							<xsl:value-of select="'D'"/>
						</xsl:variable>

						<xsl:variable name="varFXRate">
							<xsl:value-of select="COL36"/>
						</xsl:variable>

						<xsl:variable name="varCommission">
							<xsl:value-of select="COL27"/>
						</xsl:variable>

						<xsl:variable name="varFees">
							<xsl:value-of select="COL29"/>
						</xsl:variable>

						<xsl:variable name="varMiscFees">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varClearingFee">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varTransactionLevy">
							<xsl:value-of select="COL29"/>
						</xsl:variable>

						<xsl:variable name="varSecFees">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL28"/>
						</xsl:variable>


						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<TradeDate>
							<xsl:value-of select="substring-before($varPositionStartDate,' ')"/>
						</TradeDate>


						<!--<xsl:choose>
							<xsl:when test="$varAssetType = 'OPTC' or $varAssetType = 'OPTP'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat($varOSISymbol,'U')"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="varSuffix">
									<xsl:call-template name="GetSuffix">
										<xsl:with-param name="Suffix" select="COL6"/>
									</xsl:call-template>
								</xsl:variable>

								<Symbol>
									<xsl:choose>
										<xsl:when test="$PRANA_Symbol_NAME != ''">
											<xsl:value-of select="$PRANA_Symbol_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="COL24= 'USD'">
													<xsl:value-of select="$varEquitySymbol"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>								
							</xsl:otherwise>
						</xsl:choose>-->


						<xsl:choose>
							<xsl:when test="$PRANA_Symbol_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_Symbol_NAME"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:when test="$varAssetType = 'OPTC' or $varAssetType = 'OPTP'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat($varOSISymbol,'U')"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:when test="COL24= 'USD' and $varAssetType != 'OPTC' and $varAssetType != 'OPTP'">
								<Symbol>
									<xsl:value-of select="$varEquitySymbol"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:when test="COL24 != 'USD' and $varAssetType != 'OPTC' and $varAssetType != 'OPTP'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select="COL4"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>

						<FXRate>
							<xsl:choose>
								<xsl:when test ="boolean(number($varFXRate))">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<CUSIPSymbol>
							<xsl:value-of select="COL20"/>
						</CUSIPSymbol>

						<SEDOLSymbol>
							<xsl:value-of select="COL4"/>
						</SEDOLSymbol>

						<ISINSymbol>
							<xsl:value-of select="COL21"/>
						</ISINSymbol>

											
						<xsl:variable name="PB_CountnerParty" select="normalize-space(COL15)"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'CON']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>



						<FXConversionMethodOperator>
							<xsl:value-of select ="$varFXConversionMethodOperator"/>
						</FXConversionMethodOperator>

						<!--<CUSIP>
							<xsl:value-of select="$varCUSIP"/>
						</CUSIP>

						<RIC>
							<xsl:value-of select="$varRIC"/>
						</RIC>

						<Bloomberg>
							<xsl:value-of select="$varBloomberg"/>
						</Bloomberg>-->

						<!--<SEDOL>
							<xsl:value-of select="$varSEDOL"/>
						</SEDOL>-->

						<PBSymbol>
							<xsl:value-of select="$varPBSymbol"/>
						</PBSymbol>

						<CompanyName>
							<xsl:value-of select="$CompanyName"/>
						</CompanyName>

						<PBAssetType>
							<xsl:value-of select="$varAssetType"/>
						</PBAssetType>

						<!--QUANTITY-->

						<xsl:choose>
							<xsl:when test="$varNetPosition &lt; 0">
								<Quantity>
									<xsl:value-of select="$varNetPosition * (-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="$varNetPosition &gt; 0">
								<Quantity>
									<xsl:value-of select="$varNetPosition"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!--Side-->

						<Side>
							<xsl:choose>
								<xsl:when test="COL7 = 'Buy' and substring($varAssetType,1,3)= 'OPT'">
									<xsl:value-of select="'Buy to Open'"/>
								</xsl:when>
								<xsl:when test="COL7 = 'Sell' and substring($varAssetType,1,3)= 'OPT'">
									<xsl:value-of select="'Sell to Close'"/>
								</xsl:when>

								<xsl:when test="COL7 = 'Sell Short' and substring($varAssetType,1,3)= 'OPT'">
									<xsl:value-of select="'Sell to Open'"/>
								</xsl:when>
								<xsl:when test="COL7 = 'Buy'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="COL7 = 'Sell'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="COL7 = 'Sell Short'">
									<xsl:value-of select="'Sell Short'"/>
								</xsl:when>
								<xsl:when test="COL7 = 'Cover Short'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<AvgPX>
							<xsl:choose>
								<xsl:when test ="boolean(number($varCostBasis))">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<!--<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$PRANA_CounterPartyID">
									<xsl:value-of select="$PRANA_CounterPartyID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>-->

						<Fees>
							<xsl:choose>
								<xsl:when test="$varFees &gt; 0">
									<xsl:value-of select="$varFees"/>
								</xsl:when>
								<xsl:when test="$varFees &lt; 0">
									<xsl:value-of select="$varFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<MiscFees>
							<xsl:choose>
								<xsl:when test="$varMiscFees &gt; 0">
									<xsl:value-of select="$varMiscFees"/>
								</xsl:when>
								<xsl:when test="$varMiscFees &lt; 0">
									<xsl:value-of select="$varMiscFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MiscFees>

						<xsl:variable name="TotalCommission">
							<xsl:value-of select="number($varCommission) + number($varStampDuty) + number($varFees)"/>
						</xsl:variable>

						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$TotalCommission &gt; 0">
									<xsl:value-of select="$TotalCommission"/>
								</xsl:when>
								<xsl:when test="$TotalCommission &lt; 0">
									<xsl:value-of select="$TotalCommission*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>

						<xsl:variable name="NetNotionalValueBase">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>
						
						<xsl:variable name="NetNotionalValue">
							<xsl:value-of select="number(COL32)"/>
						</xsl:variable>



						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="GrossNotionalValue">
							<xsl:value-of select="number(COL31)"/>
						</xsl:variable>



						<GrossNotionalValue>
							<xsl:choose>
								<xsl:when test="$GrossNotionalValue &gt; 0">
									<xsl:value-of select="$GrossNotionalValue"/>
								</xsl:when>
								<xsl:when test="$GrossNotionalValue &lt; 0">
									<xsl:value-of select="$GrossNotionalValue*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</GrossNotionalValue>

						<xsl:variable name="GrossNotionalValueBase">
							<xsl:value-of select="number(COL30)"/>
						</xsl:variable>



						<GrossNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$GrossNotionalValueBase &gt; 0">
									<xsl:value-of select="$GrossNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$GrossNotionalValueBase &lt; 0">
									<xsl:value-of select="$GrossNotionalValueBase*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</GrossNotionalValueBase>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="$varStampDuty &gt; 0">
									<xsl:value-of select="$varStampDuty"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &lt; 0">
									<xsl:value-of select="$varStampDuty*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

						<!--<SMRequest>

							<xsl:choose>
								<xsl:when test="COL24!='USD'">
									<xsl:value-of select="'true'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'false'"/>
									</xsl:otherwise>

								</xsl:choose>

						</SMRequest>-->
						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

						<!--<CounterParty>

				  <xsl:value-of select="'COL15'"/>
					 
			  </CounterParty>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
