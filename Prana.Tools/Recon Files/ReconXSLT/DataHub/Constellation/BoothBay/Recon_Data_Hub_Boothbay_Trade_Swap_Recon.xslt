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

	<xsl:template name="MonthName">
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
			<xsl:when test="$Month='Jul'">
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
						<xsl:with-param name="Number" select="COL15"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and contains(COL8,'TRD')">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'UBS AG - London'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL13"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="''"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="COL12"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>

									<xsl:when test="$varSEDOL!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>

								</xsl:choose>
							</Symbol>

							<SEDOL>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varSEDOL!=''">
										<xsl:value-of select="$varSEDOL"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>

								</xsl:choose>
							</SEDOL>


							<xsl:variable name="PB_FUND_NAME" select="concat(COL1,' ','swap')"/>
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

							<MasterFund>
								<xsl:value-of select ="'/SWAP'"/>
								<!--<xsl:value-of select ="normalize-space(substring-after(COL1,'/'))"/>-->
							</MasterFund>

							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL20"/>
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
									<xsl:with-param name="Number" select="COL21"/>
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



							<xsl:variable name="varSecFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<SecFee>
								<xsl:choose>
									<xsl:when test="$varSecFee &gt; 0">
										<xsl:value-of select="$varSecFee"/>
									</xsl:when>
									<xsl:when test="$varSecFee &lt; 0">
										<xsl:value-of select="$varSecFee * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecFee>
							<xsl:variable name="varFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<Fees>
								<xsl:choose>
									<xsl:when test="$varFee &gt; 0">
										<xsl:value-of select="$varFee"/>
									</xsl:when>
									<xsl:when test="$varFee &lt; 0">
										<xsl:value-of select="$varFee * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Fees>

							<xsl:variable name="ClearingFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<ClearingFee>
								<xsl:choose>
									<xsl:when test="$ClearingFee &gt; 0">
										<xsl:value-of select="$ClearingFee"/>
									</xsl:when>
									<xsl:when test="$ClearingFee &lt; 0">
										<xsl:value-of select="$ClearingFee * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</ClearingFee>


							<xsl:variable name="AUECFee1">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<AUECFee1>
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
							</AUECFee1>

							<xsl:variable name="AUECFee2">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<AUECFee2>
								<xsl:choose>
									<xsl:when test="$AUECFee2 &gt; 0">
										<xsl:value-of select="$AUECFee2"/>
									</xsl:when>
									<xsl:when test="$AUECFee2 &lt; 0">
										<xsl:value-of select="$AUECFee2 * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AUECFee2>

							<xsl:variable name="StampDuty">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<StampDuty>
								<xsl:choose>
									<xsl:when test="$StampDuty &gt; 0">
										<xsl:value-of select="$StampDuty"/>
									</xsl:when>
									<xsl:when test="$StampDuty &lt; 0">
										<xsl:value-of select="$StampDuty * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</StampDuty>




							<xsl:variable name="GrossNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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



							<xsl:variable name="NetNaotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL22"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValue>
								<xsl:choose>
									<xsl:when test="$NetNaotionalValue &gt; 0">
										<xsl:value-of select="$NetNaotionalValue"/>
									</xsl:when>
									<xsl:when test="$NetNaotionalValue &lt; 0">
										<xsl:value-of select="$NetNaotionalValue * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValue>

							<xsl:variable name="NetNaotionalValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValueBase>
								<xsl:choose>
									<xsl:when test="$NetNaotionalValueBase &gt; 0">
										<xsl:value-of select="$NetNaotionalValueBase"/>
									</xsl:when>
									<xsl:when test="$NetNaotionalValueBase &lt; 0">
										<xsl:value-of select="$NetNaotionalValueBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValueBase>


							<xsl:variable name="TotalCommissionandFees">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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

							<xsl:variable name="TotalCommissionandFeesBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>

							<TotalCommissionandFeesBase>
								<xsl:choose>
									<xsl:when test="$TotalCommissionandFeesBase &gt; 0">
										<xsl:value-of select="$TotalCommissionandFeesBase"/>
									</xsl:when>
									<xsl:when test="$TotalCommissionandFeesBase &lt; 0">
										<xsl:value-of select="$TotalCommissionandFeesBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</TotalCommissionandFeesBase>


							<xsl:variable name="ClearingBrokerFeeBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<ClearingBrokerFeeBase>
								<xsl:choose>
									<xsl:when test="$ClearingBrokerFeeBase &gt; 0">
										<xsl:value-of select="$ClearingBrokerFeeBase"/>
									</xsl:when>
									<xsl:when test="$ClearingBrokerFeeBase &lt; 0">
										<xsl:value-of select="$ClearingBrokerFeeBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</ClearingBrokerFeeBase>


							<xsl:variable name="SoftCommission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<SoftCommission>
								<xsl:choose>
									<xsl:when test="$SoftCommission &gt; 0">
										<xsl:value-of select="$SoftCommission"/>
									</xsl:when>
									<xsl:when test="$SoftCommission &lt; 0">
										<xsl:value-of select="$SoftCommission * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SoftCommission>




							<xsl:variable name="TaxOnCommissions">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<TaxOnCommissions>
								<xsl:choose>
									<xsl:when test="$TaxOnCommissions &gt; 0">
										<xsl:value-of select="$TaxOnCommissions"/>
									</xsl:when>
									<xsl:when test="$TaxOnCommissions &lt; 0">
										<xsl:value-of select="$TaxOnCommissions * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</TaxOnCommissions>



							<xsl:variable name="UnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<UnitCost>
								<xsl:choose>
									<xsl:when test="$UnitCost &gt; 0">
										<xsl:value-of select="$UnitCost"/>
									</xsl:when>
									<xsl:when test="$UnitCost &lt; 0">
										<xsl:value-of select="$UnitCost * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</UnitCost>




							<BaseCurrency>
								<xsl:value-of select="''"/>
							</BaseCurrency>


							<SettlCurrency>
								<xsl:value-of select="''"/>
							</SettlCurrency>


							<xsl:variable name="SettlCurrFxRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<SettlCurrFxRate>
								<xsl:choose>
									<xsl:when test="number($SettlCurrFxRate)">
										<xsl:value-of select="$SettlCurrFxRate"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</SettlCurrFxRate>


							<xsl:variable name="SettlCurrAmt">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<SettlCurrAmt>

								<xsl:choose>
									<xsl:when test="number($SettlCurrAmt)">
										<xsl:value-of select="$SettlCurrAmt"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlCurrAmt>


							<xsl:variable name="SettlPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<SettlPrice>

								<xsl:choose>
									<xsl:when test="number($SettlPrice)">
										<xsl:value-of select="$SettlPrice"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlPrice>

							<xsl:variable name="MiscFees">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<MiscFees>
								<xsl:choose>
									<xsl:when test="$MiscFees &gt; 0">
										<xsl:value-of select="$MiscFees"/>
									</xsl:when>
									<xsl:when test="$MiscFees &lt; 0">
										<xsl:value-of select="$MiscFees * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MiscFees>
							<xsl:variable name="varMonth">
								<xsl:call-template name="MonthName">
									<xsl:with-param name="Month" select="substring-before(substring-after(COL5,'-'),'-')"/>
								</xsl:call-template>
								
							</xsl:variable>

							<xsl:variable name="varDay">
								<xsl:value-of select="substring-before(COL5,'-')"/>
							</xsl:variable>

							<xsl:variable name="varYear">
								<xsl:value-of select="concat(20,substring-after(substring-after(COL5,'-'),'-'))"/>
							</xsl:variable>


							<TradeDate>
								<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>



							<CurrencySymbol>
								<xsl:value-of select="''"/>
							</CurrencySymbol>



							<xsl:variable name="varSide">
								<xsl:value-of select="COL3"/>
							</xsl:variable>
							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select ="'Buy'"/>
									</xsl:when>

									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select ="'Sell'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>


							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>


							<!--<SMRequest>
								<xsl:value-of select="'true'"/>
							</SMRequest>-->

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>
							

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<SEDOL>
								<xsl:value-of select="''"/>
							</SEDOL>


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



							
							<SecFee>
								<xsl:value-of select="0"/>
							</SecFee>
							
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
								<xsl:value-of select="''"/>
							</BaseCurrency>


							<SettlCurrency>
								<xsl:value-of select="''"/>
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


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>



							<CurrencySymbol>
								<xsl:value-of select="''"/>
							</CurrencySymbol>



							<xsl:variable name="varSide">
								<xsl:value-of select="COL3"/>
							</xsl:variable>
							<Side>
								<xsl:value-of select="''"/>
							</Side>


							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>


							<SMRequest>
								<xsl:value-of select="'true'"/>
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


