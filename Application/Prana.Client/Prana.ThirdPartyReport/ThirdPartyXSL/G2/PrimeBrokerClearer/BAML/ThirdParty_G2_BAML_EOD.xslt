<?xml version="1.0" encoding="UTF-8"?>


<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>




	



	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
<xsl:variable name="Year">
		<xsl:value-of select="substring-after(substring-after(Date,'/'),'/')"/>
	</xsl:variable>

	<xsl:variable name="Month">
		<xsl:choose>
			<xsl:when test="string-length(substring-before(Date,'/'))=1">
				<xsl:value-of select="concat('0',substring-before(Date,'/'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring-before(Date,'/')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<xsl:variable name ="Day">
		<xsl:choose>
			<xsl:when test="string-length(substring-before(substring-after(Date,'/'),'/'))=1">
				<xsl:value-of select="concat('0',substring-before(substring-after(Date,'/'),'/'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring-before(substring-after(Date,'/'),'/')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
		<ThirdPartyFlatFileDetailCollection>

			<!--<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<PBAcctNo>
					<xsl:value-of select="'Pb Acct. No.'"/>
				</PBAcctNo>

				<PBType>
					<xsl:value-of select="'PB Type'"/>
				</PBType>

				<RecordType>
					<xsl:value-of select="'Record Type'"/>
				</RecordType>

				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>

				<ClientTransID>
					<xsl:value-of select="'Client Trans. Id.'"/>
				</ClientTransID>

				<ClientBlockId>
					<xsl:value-of select="'Client Block Id'"/>
				</ClientBlockId>

				<ClientOrgTransId>
					<xsl:value-of select="'Client Org. Trans. Id'"/>
				</ClientOrgTransId>

				<AssetType>
					<xsl:value-of select="'Asset Type'"/>
				</AssetType>

				<ProductIdType>
					<xsl:value-of select="'Product Id Type'"/>
				</ProductIdType>

				<ProductId>
					<xsl:value-of select="'Product Id'"/>
				</ProductId>

				<TradingCountry>
					<xsl:value-of select="'Country of Trading'"/>
				</TradingCountry>

				<ClientProdDesc>
					<xsl:value-of select="'Client Prod. Desc.'"/>
				</ClientProdDesc>

				<ExecBroker>
					<xsl:value-of select="'Exec. Broker'"/>
				</ExecBroker>

				<TradeDate>
					<xsl:value-of select="'Trade Dt.'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'Contractual Sett. Dt.'"/>
				</SettlementDate>

				<SpotDate>
					<xsl:value-of select="'Spot Dt.'"/>
				</SpotDate>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<IssueCurr>
					<xsl:value-of select="'Issue Curr.'"/>
				</IssueCurr>

				<SettCurr>
					<xsl:value-of select="'Sett. Curr.'"/>
				</SettCurr>

				<CostBasisFXRate>
					<xsl:value-of select="'Cost Basis FX Rt.'"/>
				</CostBasisFXRate>

				<Quantity>
					<xsl:value-of select="'Qty'"/>
				</Quantity>

				<Commission>
					<xsl:value-of select="'Commission Amt.'"/>
				</Commission>

				<CommissionRate>
					<xsl:value-of select="'Commission Rt.'"/>
				</CommissionRate>

				<CommissionType>
					<xsl:value-of select="'Commission Type'"/>
				</CommissionType>

				<SecFee>
					<xsl:value-of select="'SEC Fee'"/>
				</SecFee>

				<FeeType1>
					<xsl:value-of select="'Fee Type 1'"/>
				</FeeType1>

				<FeeAmt1>
					<xsl:value-of select="'Fee Amt. 1'"/>
				</FeeAmt1>

				<FeeType2>
					<xsl:value-of select="'Fee Type 2'"/>
				</FeeType2>

				<FeeAmt2>
					<xsl:value-of select="'Fee Amt. 2'"/>
				</FeeAmt2>

				<FeeType3>
					<xsl:value-of select="'Fee Type 3'"/>
				</FeeType3>

				<FeeAmt3>
					<xsl:value-of select="'Fee Amt. 3'"/>
				</FeeAmt3>

				<FeeType4>
					<xsl:value-of select="'Fee Type 4'"/>
				</FeeType4>

				<FeeAmt4>
					<xsl:value-of select="'Fee Amt. 4'"/>
				</FeeAmt4>

				<FeeType5>
					<xsl:value-of select="'Fee Type 5'"/>
				</FeeType5>

				<FeeAmt5>
					<xsl:value-of select="'Fee Amt. 5'"/>
				</FeeAmt5>

				<AccInt>
					<xsl:value-of select="'Acc. Int.'"/>
				</AccInt>

				<NetAmt>
					<xsl:value-of select="'Net Amt.'"/>
				</NetAmt>

				<OptType>
					<xsl:value-of select="'Opt. Type'"/>
				</OptType>

				<StrikePrice>
					<xsl:value-of select="'Strike Price'"/>
				</StrikePrice>

				<ExpDate>
					<xsl:value-of select="'Exp Dt.'"/>
				</ExpDate>

				<ClientProdType>
					<xsl:value-of select="'Client Prod. Type'"/>
				</ClientProdType>

				<ClientProdId>
					<xsl:value-of select="'Client Prod. Id'"/>
				</ClientProdId>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<BlotterCode>
					<xsl:value-of select="'Blotter Code'"/>
				</BlotterCode>

				<ExecutingService>
					<xsl:value-of select="'ExecutingService'"/>
				</ExecutingService>

				<ExchangeCode>
					<xsl:value-of select="'Exchange Code'"/>
				</ExchangeCode>

				<SettlementCode>
					<xsl:value-of select="'Settlement Code'"/>
				</SettlementCode>

				<TaxlotCloseout>
					<xsl:value-of select="'Tax Lot Closeout'"/>
				</TaxlotCloseout>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>-->

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'true'"/>
					</FileFooter>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<!--for system use only-->
					<!--<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>-->

					<!--for system internal use-->
					<PBAcctNo>
						<xsl:choose>
							<xsl:when test ="AccountName='BAML LP'">
								<xsl:value-of select ="'LP'"/>
							</xsl:when>
							<xsl:when test ="AccountName='BAML QP'">
								<xsl:value-of select ="'QP'"/>
							</xsl:when>
						</xsl:choose>
						<xsl:value-of select="''"/>
					</PBAcctNo>

					<TradingUnit>
						<xsl:value-of select="''"/>
					</TradingUnit>

					<TradingSubUnit>
						<xsl:value-of select="''"/>
					</TradingSubUnit>

					<PBType>
						<xsl:value-of select="''"/>
					</PBType>

					<RecordType>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select="'R'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'N'"/>
							</xsl:otherwise>
						</xsl:choose>
					</RecordType>

					<TransactionType>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<xsl:value-of select="'BY'"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close'">
									<xsl:value-of select="'SL'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side='Sell to Open'">
									<xsl:value-of select="'SS'"/>
								</xsl:when>
								<xsl:when test="Side='Buy to Close'">
									<xsl:value-of select="'CS'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionType>

						<ClientTransID>
							<xsl:value-of select="EntityID"/>
						</ClientTransID>

						



						<ClientBlockId>
							<xsl:value-of select="concat('00000',concat(concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/')),PBUniqueID))"/>
						</ClientBlockId>

						<ClientOrgTransId>
							<xsl:value-of select="EntityID"/>
						</ClientOrgTransId>

						<!--..............Futures, FutureOptions??........-->

						<AssetType>
							<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="'OP'"/>
								</xsl:when>
								<xsl:when test="Asset='FixedIncome'">
									<xsl:value-of select="'BO'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'EQ'"/>
								</xsl:otherwise>
							</xsl:choose>
						</AssetType>

						<ProductIdType>
							<!--<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="'OCCKEY'"/>
								</xsl:when>
								<xsl:when test="CUSIP!=''">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>
								<xsl:when test="SEDOL!=''">-->
									<xsl:value-of select="'SEDOL'"/>
								<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'TICKER'"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</ProductIdType>

						<ProductId>
							<!--<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="OSIOptionSymbol"/>
								</xsl:when>
								<xsl:when test="CUSIP!=''">
									<xsl:value-of select="CUSIP"/>
								</xsl:when>
								<xsl:when test="SEDOL!=''">-->
									<xsl:value-of select="SEDOL"/>
								<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Symbol"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</ProductId>

						<xsl:variable name="PB_NAME" select="'BAML'"/>

						<xsl:variable name="PRANA_COUNTRY_CODE" select="Country"/>
						
						<xsl:variable name="PB_COUNTRY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/CountryCodeMapping.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$PRANA_COUNTRY_CODE]/@PBCountryName"/>
						</xsl:variable>

						<TradingCountry>
							<xsl:choose>
								<xsl:when test="$PB_COUNTRY_NAME!=''">
									<xsl:value-of select="$PB_COUNTRY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Country"/>
								</xsl:otherwise>
							</xsl:choose>
						</TradingCountry>

						<ClientProdDesc>
							<xsl:value-of select="FullSecurityName"/>
						</ClientProdDesc>

						<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

						<xsl:variable name="PB_COUNTERPARTY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<ExecBroker>
							<!--<xsl:choose>
								<xsl:when test="$PRANA_COUNTERPARTY_NAME='GSCO'">
									<xsl:value-of select="'GSEC'"/>
								</xsl:when>
								<xsl:otherwise>-->
									<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
								<!--</xsl:otherwise>
							</xsl:choose>-->
						</ExecBroker>

						<TradeDate>
							<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'), substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'), substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
						</SettlementDate>

						<SpotDate>
							<xsl:value-of select="''"/>
						</SpotDate>

						<Price>							
							<xsl:value-of select="format-number(AveragePrice,'0.######')"/>
						</Price>

						<IssueCurr>
							<xsl:value-of select="CurrencySymbol"/>
						</IssueCurr>

						<SettCurr>
							<xsl:value-of select="CurrencySymbol"/>
						</SettCurr>

						<CostBasisFXRate>
							<xsl:value-of select="FXRate_Taxlot"/>
						</CostBasisFXRate>

						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<CommissionCharged>
							<xsl:choose>
								<xsl:when test="CommissionCharged &gt; 0">
									<xsl:value-of select="CommissionCharged"/>
								</xsl:when>
								<xsl:when test="CommissionCharged &lt; 0">
									<xsl:value-of select="CommissionCharged * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</CommissionCharged>

						<CommissionRate>
							<xsl:value-of select="'0'"/>
						</CommissionRate>

						<CommissionType>
							<xsl:value-of select="'A'"/>
						</CommissionType>

						<SecFee>
							<xsl:value-of select="SecFee"/>
						</SecFee>

						<FeeType1>
							<xsl:value-of select="'STAMP'"/>
						</FeeType1>

						<FeeAmt1>
							<xsl:value-of select="format-number(StampDuty,'0.######')"/>
						</FeeAmt1>

						<FeeType2>
							<xsl:value-of select="'LEVY'"/>
						</FeeType2>

						<FeeAmt2>
							<xsl:value-of select="format-number(TransactionLevy,'0.######')"/>
						</FeeAmt2>

						<FeeType3>
							<xsl:value-of select="'TICKET'"/>
						</FeeType3>

						<FeeAmt3>
							<xsl:value-of select="''"/>
						</FeeAmt3>

						<FeeType4>
							<xsl:value-of select="'TAXES'"/>
						</FeeType4>

						<FeeAmt4>
							<xsl:value-of select="format-number(TaxOnCommissions,'0.######')"/>
						</FeeAmt4>

						<FeeType5>
							<xsl:value-of select="'GST'"/>
						</FeeType5>

						<FeeAmt5>
							<xsl:value-of select="''"/>
						</FeeAmt5>

						<AccInt>
							<xsl:value-of select="AccruedInterest"/>
						</AccInt>

						<NetAmt>
							<xsl:value-of select="NetAmount"/>
						</NetAmt>

						<OptType>
							<xsl:choose>
								<xsl:when test="PutOrCall='Call'">
									<xsl:value-of select="'CL'"/>
								</xsl:when>
								<xsl:when test="PutOrCall='Put'">
									<xsl:value-of select="'PT'"/>
								</xsl:when>
							</xsl:choose>
						</OptType>

						<StrikePrice>
							<xsl:choose>
								<xsl:when test ="number(StrikePrice)">
									<xsl:value-of select="StrikePrice"/>		
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>							
						</StrikePrice>

						<ExpDate>
							<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'), substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExpDate>

						<ClientProdType>
							<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="'OSIOPTIONSYMBOL'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'TICKER'"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClientProdType>

						<ClientProdId>
							<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="OSIOptionSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClientProdId>

						<Comments>
							<xsl:value-of select="''"/>
						</Comments>

						<BlotterCode>
							<xsl:value-of select="''"/>
						</BlotterCode>

						<ExecutingService>
							<xsl:value-of select="''"/>
						</ExecutingService>
						
						<xsl:variable name="PRANA_EXCHANGE_CODE" select="Exchange"/>

						<xsl:variable name="PB_EXCHANGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE_CODE]/@PBExchangeName"/>
						</xsl:variable>

						<ExchangeCode>
							<!--<xsl:choose>
								<xsl:when test="$PB_EXCHANGE_CODE!=''">
									<xsl:value-of select="$PB_EXCHANGE_CODE"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Exchange"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select ="''"/>
						</ExchangeCode>

						<SettlementCode>
							<xsl:value-of select="''"/>
						</SettlementCode>

						<TaxlotCloseout>
							<xsl:value-of select="''"/>
						</TaxlotCloseout>

						<SpecialHandling>
							<xsl:value-of select="''"/>
						</SpecialHandling>

						<BOMMarker>
							<xsl:value-of select="''"/>
						</BOMMarker>

						<InternationalTransferFlag>
							<xsl:value-of select="''"/>
						</InternationalTransferFlag>

						<StampAmountCurrency>
							<xsl:value-of select="''"/>
						</StampAmountCurrency>

						<StampAmount>
							<xsl:value-of select="''"/>
						</StampAmount>

						<StampConsiderationCurrency>
							<xsl:value-of select="''"/>
						</StampConsiderationCurrency>

						<StampConsideration>
							<xsl:value-of select="''"/>
						</StampConsideration>

						<TransactionStampStatus>
							<xsl:value-of select="''"/>
						</TransactionStampStatus>

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>