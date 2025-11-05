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

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantity)">
				<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>


						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" >
							<xsl:value-of select="COL28"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						
						</Symbol>

						<CUSIPSymbol>
							<xsl:value-of select="COL24"/>
						</CUSIPSymbol>

						<ISINSymbol>
							<xsl:value-of select="COL25"/>
						</ISINSymbol>

						<OSI>
							<xsl:value-of select="COL27"/>
						</OSI>

						<SEDOLSymbol>
							<xsl:value-of select="COL26"/>
						</SEDOLSymbol>
					
					<BloombergSymbol>
						<xsl:value-of select="normalize-space(COL3)"/>
					</BloombergSymbol>

						<xsl:variable name="PB_FUND_NAME" select="COL6"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
								<xsl:when test="number($Quantity)">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
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



						<xsl:variable name="Side" select="COL9"/>

						<Side>
							<xsl:choose>
								<xsl:when test ="$Side='Sell'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>
								<xsl:when test ="$Side='Buy'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:when test ="$Side='Sell short'">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:when test ="$Side='Buy to Close'">
									<xsl:value-of select ="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<Asset>
							<xsl:value-of select="COL1"/>
						</Asset>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
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

						<xsl:variable name="StampDuty">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
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

						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
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
						</SecFee>

						<xsl:variable name="OtherBrokerFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="$OtherBrokerFees &gt; 0">
									<xsl:value-of select="$OtherBrokerFees"/>

								</xsl:when>
								<xsl:when test="$OtherBrokerFees &lt; 0">
									<xsl:value-of select="$OtherBrokerFees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="TransLevy">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>
						<TransactionLevy>
							<xsl:choose>
								<xsl:when test="$TransLevy &gt; 0">
									<xsl:value-of select="$TransLevy"/>
								</xsl:when>
								<xsl:when test="$TransLevy &lt; 0">
									<xsl:value-of select="$TransLevy * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>

						<xsl:variable name="OccFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL19"/>
							</xsl:call-template>
						</xsl:variable>
						<OccFee>
							<xsl:choose>
								<xsl:when test="$OccFee &gt; 0">
									<xsl:value-of select="$OccFee"/>

								</xsl:when>
								<xsl:when test="$OccFee &lt; 0">
									<xsl:value-of select="$OccFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</OccFee>
						<xsl:variable name="OrfFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL18"/>
							</xsl:call-template>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>

								</xsl:when>
								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</OrfFee>


						<xsl:variable name="AccruedInterest">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL22"/>
							</xsl:call-template>
						</xsl:variable>
						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="$AccruedInterest &gt; 0">
									<xsl:value-of select="$AccruedInterest"/>

								</xsl:when>
								<xsl:when test="$AccruedInterest &lt; 0">
									<xsl:value-of select="$AccruedInterest * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccruedInterest>



						<TradeDate>
							<xsl:value-of select="COL7"/>
						</TradeDate>

						<xsl:variable name="TaxOnCommissions">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
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

						<Multiplier>
							<xsl:value-of select="COL20"/>
						</Multiplier>

						<ClearingFee>
							<xsl:value-of select="''"/>
						</ClearingFee>

						<SettlementDate>
							<xsl:value-of select="COL8"/>
						</SettlementDate>
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<!-- 
								Variables to collect all the tax related fields of the trade 
						-->

						<xsl:variable name="Commission1">

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

						</xsl:variable>

						<xsl:variable name="OtherBrokerFees1">

							<xsl:choose>
								<xsl:when test="$OtherBrokerFees &gt; 0">
									<xsl:value-of select="$OtherBrokerFees"/>
								</xsl:when>
								<xsl:when test="$OtherBrokerFees &lt; 0">
									<xsl:value-of select="$OtherBrokerFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="StampDuty1">

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

						</xsl:variable>

						<xsl:variable name="SecFee1">

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

						</xsl:variable>



						<xsl:variable name="TransLevy1">

							<xsl:choose>
								<xsl:when test="$TransLevy &gt; 0">
									<xsl:value-of select="$TransLevy"/>
								</xsl:when>
								<xsl:when test="$TransLevy &lt; 0">
									<xsl:value-of select="$TransLevy * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="TaxOnCommissions1">

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

						</xsl:variable>

						<xsl:variable name="OrfFee1">
							<xsl:choose>

								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>
								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="OccFee1">

							<xsl:choose>
								<xsl:when test="$OccFee &gt; 0">
									<xsl:value-of select="$OccFee"/>
								</xsl:when>
								<xsl:when test="$OccFee &lt; 0">
									<xsl:value-of select="$OccFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="TotalCommissionandFees">
							<xsl:value-of select="$Commission1 + $OtherBrokerFees1 + $StampDuty1 + $SecFee1 + $TransLevy1 + $TaxOnCommissions1 + $OrfFee1 + $OccFee1"/>
						</xsl:variable>

						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$TotalCommissionandFees &gt; 0">
									<xsl:value-of select="$TotalCommissionandFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</TotalCommissionandFees>

					

						<CounterParty>
							<xsl:choose>
								<xsl:when test="normalize-space(COL23) != ''">
									<xsl:value-of select="normalize-space(COL23)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Undefined'"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterParty>

					<SMRequest>
						<xsl:value-of select="'false'"/>
					</SMRequest>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>