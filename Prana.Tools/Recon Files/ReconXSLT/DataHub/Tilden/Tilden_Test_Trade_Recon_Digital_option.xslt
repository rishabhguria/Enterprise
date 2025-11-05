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
		<xsl:variable name="Spece">' '</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'�',''),$SingleQuote,''),$Spece,''))"/>
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
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)and COL2='FX Digital Option'">
				
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'QAPB1'"/>
						</xsl:variable>


						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="translate(COL3,'�','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" >
							<xsl:value-of select="translate(COL3,'�','')"/>
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
						<xsl:variable name="Symbol1" >
							<xsl:value-of select="translate(COL16,'�','')"/>
						</xsl:variable>

						<Masterfund>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Masterfund>
						

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



						<xsl:variable name="Side" select="COL4"/>

						<Side>
							<xsl:choose>
								<xsl:when test ="$Side='Sell'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>
								<xsl:when test ="$Side='BUY'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:when test ="$Side='Sell short'">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:when test ="$Side='Buy to Close'">
									<xsl:value-of select ="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Side"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>
						<xsl:variable name="varComments" >
							<xsl:value-of select="translate(COL2,'�','')"/>
						</xsl:variable>

						<Comments>
							<xsl:value-of select="$varComments"/>
						</Comments>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>
						<SoftCommission>
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
						</SoftCommission>


						<TradeDate>
							<xsl:value-of select="COL5"/>
						</TradeDate>

						<xsl:variable name="ClearingFeeBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>
						<ClearingFeeBase>
							<xsl:choose>
								<xsl:when test="$ClearingFeeBase &gt; 0">
									<xsl:value-of select="$ClearingFeeBase"/>

								</xsl:when>
								<xsl:when test="$ClearingFeeBase &lt; 0">
									<xsl:value-of select="$ClearingFeeBase * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</ClearingFeeBase>

						<xsl:variable name="ClearingBrokerFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL19"/>
							</xsl:call-template>
						</xsl:variable>
						<ClearingBrokerFee>
							<xsl:choose>
								<xsl:when test="$ClearingBrokerFee &gt; 0">
									<xsl:value-of select="$ClearingBrokerFee"/>

								</xsl:when>
								<xsl:when test="$ClearingBrokerFee &lt; 0">
									<xsl:value-of select="$ClearingBrokerFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</ClearingBrokerFee>

						<SettlementDate>
							<xsl:value-of select="COL6"/>
						</SettlementDate>

						<CounterParty>
							<xsl:choose>
								<xsl:when test="normalize-space(COL15) != ''">
									<xsl:value-of select="normalize-space(COL15)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Undefined'"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterParty>

						<xsl:variable name="varNetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
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

						<xsl:variable name="varNetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValue &gt; 0">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValue &lt; 0">
									<xsl:value-of select="$varNetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetNotionalValue>

						<ExpirationDate>
							<xsl:value-of select="COL10"/>
						</ExpirationDate>

						<ProcessDate>
							<xsl:value-of select="COL10"/>
						</ProcessDate>

						<OriginalpurchaseDate>
							<xsl:value-of select="COL10"/>
						</OriginalpurchaseDate>

						<OSI>
							<xsl:value-of select="COL12"/>
						</OSI>

						<xsl:variable name="varCoupon">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<Coupon>
							<xsl:choose>
								<xsl:when test="number($varCoupon)">
									<xsl:value-of select="$varCoupon"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Coupon>

						<CurrencySymbol>
							<xsl:value-of select="COL18"/>
						</CurrencySymbol>
						
						<xsl:variable name="UnderlyingSymbol" >
							<xsl:value-of select="translate(COL22,'�','')"/>
						</xsl:variable>
						<UnderlyingSymbol>
							<xsl:value-of select="$UnderlyingSymbol"/>
						</UnderlyingSymbol>

						<xsl:variable name="PutCall" >
							<xsl:value-of select="translate(COL23,'�','')"/>
						</xsl:variable>
						<PutOrCall>
							<xsl:value-of select="$PutCall"/>
						</PutOrCall>
						<xsl:variable name="CompanyName" >
							<xsl:value-of select="translate(COL24,'�','')"/>
						</xsl:variable>
						<CompanyName>
							<xsl:value-of select="$CompanyName"/>
						</CompanyName>
						
						<xsl:variable name="varFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL25"/>
							</xsl:call-template>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="number($varFees)">
									<xsl:value-of select="$varFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>
						
						<xsl:variable name="Idco" >
							<xsl:value-of select="translate(COL26,'�','')"/>
						</xsl:variable>
						<Idco>
							<xsl:value-of select="$Idco"/>
						</Idco>
						<xsl:variable name="PrimeBroker" >
							<xsl:value-of select="translate(COL27,'�','')"/>
						</xsl:variable>
						<PrimeBroker>
							<xsl:value-of select="$PrimeBroker"/>
						</PrimeBroker>
						<xsl:variable name="SEDOL" >
							<xsl:value-of select="translate(COL28,'�','')"/>
						</xsl:variable>
						<SEDOL>
							<xsl:value-of select="$SEDOL"/>
						</SEDOL>
						<xsl:variable name="Bloomberg" >
							<xsl:value-of select="translate(COL29,'�','')"/>
						</xsl:variable>
						<Bloomberg>
							<xsl:value-of select="$Bloomberg"/>
						</Bloomberg>
						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="number($varCommission)">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>
						<xsl:variable name="varClearingFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL31"/>
							</xsl:call-template>
						</xsl:variable>
						<ClearingFees>
							<xsl:choose>
								<xsl:when test="number($varClearingFees)">
									<xsl:value-of select="$varClearingFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingFees>
						<xsl:variable name="varAUECFees1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>
						<AUECFees1>
							<xsl:choose>
							<xsl:when test="number($varAUECFees1)">
								<xsl:value-of select="$varAUECFees1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
							</xsl:choose>
						</AUECFees1>
						<xsl:variable name="varAUECFees2">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33"/>
							</xsl:call-template>
						</xsl:variable>
						<AUECFees1>
							<xsl:choose>
								<xsl:when test="number($varAUECFees2)">
									<xsl:value-of select="$varAUECFees2"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AUECFees1>
						<xsl:variable name="varGrossNotional">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL34"/>
							</xsl:call-template>
						</xsl:variable>
						<GrossNotional>
							<xsl:choose>
								<xsl:when test="number($varGrossNotional)">
									<xsl:value-of select="$varGrossNotional"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</GrossNotional>
						<xsl:variable name="varMultiplier">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL35"/>
							</xsl:call-template>
						</xsl:variable>
						<Multiplier>
							<xsl:choose>
								<xsl:when test="number($varMultiplier)">
									<xsl:value-of select="$varMultiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Multiplier>
						<xsl:variable name="varSettleCurrency" >
							<xsl:value-of select="translate(COL36,'�','')"/>
						</xsl:variable>
						<SettlCurrency>
							<xsl:value-of select="$varSettleCurrency"/>
						</SettlCurrency>
						<xsl:variable name="varBaseCurrency" >
							<xsl:value-of select="translate(COL37,'�','')"/>
						</xsl:variable>
						<BaseCurrency>
							<xsl:value-of select="$varBaseCurrency"/>
						</BaseCurrency>
						<xsl:variable name="varLotID">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL38"/>
							</xsl:call-template>
						</xsl:variable>
						<LotID>
							<xsl:choose>
								<xsl:when test="number($varMultiplier)">
									<xsl:value-of select="$varMultiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</LotID>
						
					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>