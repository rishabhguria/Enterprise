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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL18,'/')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL9,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),2,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),4,2)"/>
			</xsl:variable>

			<!--<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),12,1)"/>
			</xsl:variable>-->

			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL9),' '),8),'##.##')"/>
			</xsl:variable>


			<!--<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
				-->
			<!--<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,1)"/>-->
			<!--
			</xsl:variable>-->

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

			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>-->

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$ExpiryMonth,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>



	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL2"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:choose>
				<xsl:when test="number($Quantity) and COL43 != 'Expire' and COL43 !='Exercise' and COL43 != 'Assignment' and COL26 = 'CLST' or COL26 = 'Marex'">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="normalize-space($Symbol)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL13"/>
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
								<xsl:with-param name="Number" select="COL3"/>
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
								<xsl:with-param name="Number" select="COL14"/>
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

						<xsl:variable name="FxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>
						<FxRate>
							<xsl:choose>
								<xsl:when test="number($FxRate)">
									<xsl:value-of select="$FxRate"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FxRate>

						<xsl:variable name="OtherBrokerFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="$OtherBrokerFee &gt; 0">
									<xsl:value-of select="$OtherBrokerFee"/>
								</xsl:when>
								<xsl:when test="$OtherBrokerFee &lt; 0">
									<xsl:value-of select="$OtherBrokerFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="ClearingFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
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
								<xsl:with-param name="Number" select="COL18"/>
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
								<xsl:with-param name="Number" select="COL19"/>
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
								<xsl:with-param name="Number" select="COL20"/>
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

					

						<UnderlyingSymbol>
							<xsl:value-of select="COL12"/>
						</UnderlyingSymbol>

						<Bloomberg>
							<xsl:value-of select="COL23"/>
						</Bloomberg>

						<SEDOL>
							<xsl:value-of select="COL21"/>
						</SEDOL>

						<CUSIP>
							<xsl:value-of select="COL22"/>
						</CUSIP>

						<Asset>
							<xsl:value-of select="COL25"/>
						</Asset>


						<!--<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
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
						
						<CounterParty>
							<xsl:value-of select="COL26"/>
						</CounterParty>



						<xsl:variable name="GrossNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL27"/>
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
								<xsl:with-param name="Number" select="COL28"/>
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
								<xsl:with-param name="Number" select="COL29"/>
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

						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
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


						<xsl:variable name="TotalCommissionandFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL31"/>
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
								<xsl:with-param name="Number" select="COL32"/>
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
						
						<!--<SecFee>	
								
						</SecFee>

						<OccFee>
							
							
						</OccFee>

						<OrfFee>
							
							
						</OrfFee>

						<SecFeeBase>
							
						</SecFeeBase>

						<OccFeeBase>
							
							
						</OccFeeBase>	
						
						<OrfFeeBase>
							
						</OrfFeeBase>-->
						<xsl:variable name="ClearingBrokerFeeBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL39"/>
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
								<xsl:with-param name="Number" select="COL39"/>
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

						<!--<SoftCommissionBase>
							
							
						</SoftCommissionBase>-->

				
							<xsl:variable name="TaxOnCommissions">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL39"/>
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
						
						
						<!--<Transition	>
														
							
						</Transition>-->

				


						<xsl:variable name="UnitCost">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL44"/>
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
							<xsl:value-of select="COL45"/>
						</BaseCurrency>


						<SettlCurrency>
							<xsl:value-of select="COL46"/>	
						</SettlCurrency>
						
						
						<xsl:variable name="SettlCurrFxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL44"/>
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
								<xsl:with-param name="Number" select="COL49"/>
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
								<xsl:with-param name="Number" select="COL49"/>
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
								<xsl:with-param name="Number" select="COL17"/>
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

						<TradeDate>
							<xsl:value-of select ="COL6"/>
						</TradeDate>

						<OriginalPurchaseDate>
							<xsl:value-of select ="COL5"/>
						</OriginalPurchaseDate>


						<SettlementDate>
							<xsl:value-of select ="COL8"/>
						</SettlementDate>


						<ExpirationDate>
							<xsl:value-of select ="COL9"/>
						</ExpirationDate>
						
						<ProcessDate>
							<xsl:value-of select ="COL7"/>
						</ProcessDate>


						<CurrencySymbol>
							<xsl:value-of select ="COL4"/>
						</CurrencySymbol>



						<xsl:variable name="Side">
							<xsl:value-of select="COL24"/>
						</xsl:variable>
						<Side>
							<xsl:choose>

								<xsl:when test ="$Side='Sell'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>
								<xsl:when test ="$Side='Buy'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

								<xsl:when test ="$Side='Buy to Close'">
									<xsl:value-of select ="'Buy to Close'"/>
								</xsl:when>
								
								<xsl:when test ="$Side='Sell short'">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>

								<xsl:when test ="$Side='Sell to Close'">
									<xsl:value-of select ="'Sell to Close'"/>
								</xsl:when>

                <xsl:when test ="$Side='Sell to Open'">
                  <xsl:value-of select ="'Sell to Open'"/>
                </xsl:when>
								
								<xsl:when test ="$Side='Buy to Open'">
									<xsl:value-of select ="'Buy to Open'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<CompanyName>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</CompanyName>
						
						<xsl:variable name="varOSI">
							<xsl:value-of select="normalize-space(COL48)"/>
						</xsl:variable>
						<OSI>
							<xsl:value-of select="$varOSI"/>
						</OSI>

					</PositionMaster>
				</xsl:when>

				<xsl:otherwise>
					<PositionMaster>

						<Symbol>
							<xsl:value-of select ="''"/>
						</Symbol>



						<FundName>

							<xsl:value-of select ="''"/>

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
							<xsl:value-of select="''"/>
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
							<xsl:value-of select ="''"/>
						</TradeDate>

						<OriginalPurchaseDate>
							<xsl:value-of select ="''"/>
						</OriginalPurchaseDate>


						<SettlementDate>
							<xsl:value-of select ="''"/>
						</SettlementDate>


						<ExpirationDate>
							<xsl:value-of select ="''"/>
						</ExpirationDate>

						<ProcessDate>
							<xsl:value-of select ="''"/>
						</ProcessDate>


						<CurrencySymbol>
							<xsl:value-of select ="''"/>
						</CurrencySymbol>




						<Side>

							<xsl:value-of select="''"/>

						</Side>

						<PrimeBroker>
							<xsl:value-of select="''"/>
						</PrimeBroker>

						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<CompanyName>
							<xsl:value-of select="''"/>
						</CompanyName>
						<OSI>
							<xsl:value-of select ="''"/>
						</OSI>

					</PositionMaster>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


