<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public int Now(string date)
		{
		DateTime d1 = DateTime.Parse(date);
		DateTime d2 = DateTime.Today;

		int result = DateTime.Compare(d1,d2);
		return result;
		}

	</msxsl:script>



	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Resonate Core']">
				<!--<if	test="CounterParty !='CCMB' and not(contains(PRANA_FUND_NAME, 'KH'))">-->
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<trddate>
						<xsl:value-of select="TradeDate"/>
					</trddate>

					<setdate>
						<xsl:value-of select="SettlementDate"/>
					</setdate>

					<SYCODE>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							
							<xsl:when test="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SYCODE>

					<TRXTYPE>
						<xsl:choose>
							<xsl:when test="Side='Buy' ">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' ">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' ">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CVS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Open'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Open'">
								<xsl:value-of select="'SHT'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close'">
								<xsl:value-of select="'SEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRXTYPE>

					<qty>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="format-number(AllocatedQty,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</qty>

					<PRICE>
						<xsl:value-of select="format-number(AveragePrice,'0.########')"/>
					</PRICE>
					
					<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged + TaxOnCommissions"/>
					</xsl:variable>
					<Commission>
						<xsl:value-of select="$COMM"/>
					</Commission>

					<SettlementCCY>
						<xsl:value-of select="SettlCurrency"/>
					</SettlementCCY>
					
					<xsl:variable name="PB_NAME" select="'Opus'"/>

					<!--<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<ContraParty>
						<xsl:value-of select="$Broker"/>
					</ContraParty>-->

					<ContraParty>
						<xsl:value-of select="'MS'"/>
					</ContraParty>

					<xsl:value-of select="'Resonate - MS'"/>

					<!--<xsl:variable name = "PRANA_EXCHANGE_NAME">
						<xsl:value-of select="Exchange"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_EXCHANGE_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE_NAME]/@PBExchangeName"/>
					</xsl:variable>

					<Exchange>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_EXCHANGE_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_EXCHANGE_CODE"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Exchange>-->

					<Exchange>
						<xsl:value-of select="''"/>
					</Exchange>
					
					<CLRBRKRACCT>
						<xsl:value-of select="'Resonate - MS'"/>
					</CLRBRKRACCT>

					<SettleFXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)!=0">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:when test="number(FXRate_Taxlot)=0">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettleFXRate>

					<evreference>
						<xsl:value-of select="PBUniqueID"/>
					</evreference>

					<CBFee>
						<xsl:value-of select="''"/>
					</CBFee>

					<ExFee>
						<xsl:value-of select="''"/>
					</ExFee>

					<Interest>
						<xsl:value-of select="''"/>
					</Interest>

					<Ofee>
						<xsl:value-of select="OrfFee"/>
					</Ofee>

					<SecFee>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number(SecFee)">
										<xsl:value-of select="SecFee"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</SecFee>

					<!--<NetProceeds>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
						
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetProceeds>-->

					<NetProceeds>
						<xsl:value-of select="''"/>
					</NetProceeds>

					<PositionCCY>
						<xsl:value-of select="CurrencySymbol"/>
					</PositionCCY>

					<PosFXRate>
						<xsl:choose>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
										
					</PosFXRate>

					<Blank>
						<xsl:value-of select="''"/>
					</Blank>

					<strategy>
						<xsl:value-of select="'A-A'"/>
					</strategy>

					<FNID>
						<xsl:value-of select="''"/>
					</FNID>

					<CPS>
						<xsl:value-of select="''"/>
					</CPS>

					<bips>
						<xsl:value-of select="''"/>
					</bips>

					<Status>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Status>

					<bareference>
						<xsl:value-of select="PBUniqueID"/>
					</bareference>

					<OwnerTrader>
						<xsl:value-of select="''"/>
					</OwnerTrader>

					<SoftCommPct>
						<xsl:value-of select="''"/>
					</SoftCommPct>

					<DealDescription>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="'Resonate Equity Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</DealDescription>

					<DealRate>
						<xsl:value-of select="0"/>
					</DealRate>

					<Giveupbrokercode>
						<xsl:value-of select="''"/>
					</Giveupbrokercode>

					<Giveupcmmsntypecode>
						<xsl:value-of select="''"/>
					</Giveupcmmsntypecode>

					<GiveUpCommRate>
						<xsl:value-of select="0"/>
					</GiveUpCommRate>

					<GiveUpCommAmt>
						<xsl:value-of select="0"/>
					</GiveUpCommAmt>

					<GovtFees>
						<xsl:value-of select="0"/>
					</GovtFees>

					<Remarks>
						<xsl:value-of select="PBUniqueID"/>
					</Remarks> 	
					
					<EVType>
						<xsl:value-of select="'TRD'"/>
					</EVType>

					<TermDate>
						<xsl:value-of select="''"/>
					</TermDate>

					<ExcludeOtherFeesfromNetProceeds>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromNetProceeds>

					<ExcludeOtherFeesfromBrCash>
						<xsl:value-of select="''"/>
					</ExcludeOtherFeesfromBrCash>

					<ExcludeCommissionfromProceeds>
						<xsl:value-of select="''"/>
					</ExcludeCommissionfromProceeds>

					<GiveUpBrokerCommPostingRule>
						<xsl:value-of select="''"/>
					</GiveUpBrokerCommPostingRule>

					<CommTypeCode>
						<xsl:value-of select="'FLAT'"/>
					</CommTypeCode>

					<Route>
						<xsl:value-of select="''"/>
					</Route>

					<UploadStatus>
						<xsl:value-of select="''"/>
					</UploadStatus>

					<PairOffMethod>
						<xsl:value-of select="''"/>
					</PairOffMethod>

					<UDCNamesValues>
						<xsl:value-of select="''"/>
					</UDCNamesValues>

					<TargetSettlement>
						<xsl:value-of select="''"/>
					</TargetSettlement>

					<ContractType>
						<xsl:value-of select="''"/>
					</ContractType>
					
					<StrategyName>
						<xsl:value-of select="Strategy"/>
					</StrategyName>
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
				<!--</if>-->
			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>