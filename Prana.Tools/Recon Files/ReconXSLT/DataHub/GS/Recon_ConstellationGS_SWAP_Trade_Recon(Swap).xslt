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

	<xsl:template name="MonthCodevar">
		<xsl:param name="Month"/>
		<xsl:param name="varPutCall"/>
		<xsl:if test="$varPutCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07' ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$varPutCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring(substring-after(normalize-space(COL4),' '),7,1),'P') or contains(substring(substring-after(normalize-space(COL4),' '),7,1),'C')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(normalize-space(COL4),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),1,2)"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL4),' '),8) div 1000,'#.00')"/>
			</xsl:variable>
			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodevar">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
					<xsl:with-param name="varPutCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MonthCodeForDate">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='Jul' ">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="'12'"/>
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
						<xsl:with-param name="Number" select="COL11"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Goldman Sachs'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL12"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_SUFFIX_NAME" select="COL15"/>

							<xsl:variable name="PRANA_SYMBOL_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolSuffixMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SYMBOL_SUFFIX_NAME]/@TickerSuffixCode"/>
							</xsl:variable>


							<xsl:variable name="varUnderlying" select="substring-before(COL12,'.')"/>

							<xsl:variable name ="varSymbol">								
										<xsl:value-of select="concat($varUnderlying,$PRANA_SYMBOL_SUFFIX_NAME,'/','SWAP')"/>									
							</xsl:variable>
							<Symbol>
								<xsl:choose>
									<!--<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>-->
									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<xsl:variable name="PB_FUND_NAME" select="COL1"/>

							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>

									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="$Quantity * -1"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL23"/>
								</xsl:call-template>
							</xsl:variable>

							<AvgPX>
								<xsl:choose>
									<xsl:when test="$AvgPrice &gt; 0">
										<xsl:value-of select="$AvgPrice"/>

									</xsl:when>
									<xsl:when test="$AvgPrice &lt; 0">
										<xsl:value-of select="$AvgPrice * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>

								</xsl:choose>
							</AvgPX>

							<xsl:variable name="Commission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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


							<xsl:variable name="Fees">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<MiscFees>
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
							</MiscFees>

							<xsl:variable name="Currency" select="''"/>
							<CurrencySymbol>
								<xsl:value-of select="$Currency"/>
							</CurrencySymbol>
							<xsl:variable name="GrossNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL25"/>
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

							<xsl:variable name="GrossNotionalValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<GrossNotionalValueBase>
								<xsl:choose>
									<xsl:when test="$GrossNotionalValueBase &gt; 0">
										<xsl:value-of select="$GrossNotionalValueBase"/>
									</xsl:when>
									<xsl:when test="$GrossNotionalValueBase &lt; 0">
										<xsl:value-of select="$GrossNotionalValueBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</GrossNotionalValueBase>

							<xsl:variable name="NetNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL26"/>
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


							<xsl:variable name="varNetNotionalValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL26"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValueBase>
								<xsl:choose>
									<xsl:when test="$varNetNotionalValueBase &gt; 0">
										<xsl:value-of select="$varNetNotionalValueBase"/>
									</xsl:when>
									<xsl:when test="$varNetNotionalValueBase &lt; 0">
										<xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValueBase>

							<xsl:variable name="TotalCommissionandFees">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="(COL31 + COL29)"/>
								</xsl:call-template>
							</xsl:variable>

							<TotalCommissionandFees>
								<xsl:choose>
									<xsl:when test="$TotalCommissionandFees &gt; 0">
										<xsl:value-of select="$TotalCommissionandFees"/>
									</xsl:when>
									<xsl:when test="$TotalCommissionandFees &lt; 0">
										<xsl:value-of select="$TotalCommissionandFees * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</TotalCommissionandFees>

							<xsl:variable name="varDay" select="substring-before(normalize-space(COL8),' ')"/>
							<xsl:variable name="varYear" select="substring-after(substring-after(normalize-space(COL8),' '),' ')"/>
							<xsl:variable name="varMonth">
								<xsl:call-template name="MonthCodeForDate">
									<xsl:with-param name="Month" select="substring-before(substring-after(normalize-space(COL8),' '),' ')"/>
								</xsl:call-template>
							</xsl:variable>

							<TradeDate>
								<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<xsl:variable name ="varSide1" select="COL4"/>
							<xsl:variable name ="varSide2" select="COL5"/>
							<Side>
								<xsl:choose>
									<xsl:when test="$varSide1='Buy' and $varSide2='Open'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$varSide1='Buy' and $varSide2='Close'">
										<xsl:value-of select="'Buy To Close'"/>
									</xsl:when>
									<xsl:when test="$varSide1='Sell' and $varSide2='Open'">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>
									<xsl:when test="$varSide1='Sell' and $varSide2='Close'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
								</xsl:choose>
							</Side>

							<xsl:variable name="FXRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
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

							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>


							<Commission>
								<xsl:value-of select="0"/>
							</Commission>


							<FxRate>
								<xsl:value-of select="0"/>
							</FxRate>


							<Fees>
								<xsl:value-of select="0"/>
							</Fees>


							<ClearingFee>
								<xsl:value-of select="0"/>
							</ClearingFee>


							<AUECFee1>
								<xsl:value-of select="0"/>
							</AUECFee1>


							<AUECFee2>
								<xsl:value-of select="0"/>
							</AUECFee2>


							<StampDuty>
								<xsl:value-of select="0"/>
							</StampDuty>


							<UnderlyingSymbol>
								<xsl:value-of select="''"/>
							</UnderlyingSymbol>

							<Bloomberg>
								<xsl:value-of select="''"/>
							</Bloomberg>

							<SEDOL>
								<xsl:value-of select="''"/>
							</SEDOL>

							<CUSIP>
								<xsl:value-of select="COL22"/>
							</CUSIP>

							<Asset>
								<xsl:value-of select="''"/>
							</Asset>


							<CounterParty>
								<xsl:value-of select="''"/>
							</CounterParty>


							<GrossNotionalValue>
								<xsl:value-of select="0"/>
							</GrossNotionalValue>

							<GrossNotionalValueBase>
								<xsl:value-of select="0"/>
							</GrossNotionalValueBase>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>

							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>

							<TotalCommissionandFees>
								<xsl:value-of select="0"/>
							</TotalCommissionandFees>

							<TotalCommissionandFeesBase>
								<xsl:value-of select="0"/>
							</TotalCommissionandFeesBase>

							<ClearingBrokerFeeBase>
								<xsl:value-of select="0"/>
							</ClearingBrokerFeeBase>

							<SoftCommission>
								<xsl:value-of select="0"/>
							</SoftCommission>

							<TaxOnCommissions>
								<xsl:value-of select="0"/>
							</TaxOnCommissions>


							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>

							<BaseCurrency>
								<xsl:value-of select="0"/>
							</BaseCurrency>

							<SettlCurrency>
								<xsl:value-of select="0"/>
							</SettlCurrency>

							<SettlCurrFxRate>
								<xsl:value-of select="0"/>
							</SettlCurrFxRate>


							<SettlCurrAmt>
								<xsl:value-of select="0"/>
							</SettlCurrAmt>

							<SettlPrice>
								<xsl:value-of select="0"/>
							</SettlPrice>

							<MiscFees>
								<xsl:value-of select="0"/>
							</MiscFees>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<OriginalPurchaseDate>
								<xsl:value-of select="''"/>
							</OriginalPurchaseDate>

							<SettlementDate>
								<xsl:value-of select="''"/>
							</SettlementDate>

							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<ProcessDate>
								<xsl:value-of select="''"/>
							</ProcessDate>

							<CurrencySymbol>
								<xsl:value-of select="''"/>
							</CurrencySymbol>

							<Side>
								<xsl:value-of select="''"/>
							</Side>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

							<SMRequest>
								<xsl:value-of select="''"/>
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


