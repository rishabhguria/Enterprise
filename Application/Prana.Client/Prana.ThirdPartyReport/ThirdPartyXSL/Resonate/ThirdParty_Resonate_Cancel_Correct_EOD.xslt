<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


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

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:variable name="NetAmount" select="(OrderQty * AvgPrice) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>

				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileFooter>
								<xsl:value-of select="'false'"/>
							</FileFooter>

							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<trddate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</trddate>


							<setdate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
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
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="format-number(OrderQty,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</qty>

							<PRICE>
								<xsl:value-of select="format-number(AvgPrice,'0.########')"/>
							</PRICE>

							<xsl:variable name="COMM">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
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
								<xsl:value-of select="SecFee"/>
							</SecFee>

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
										<xsl:value-of select ="'SENT'"/>
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
								<xsl:value-of select="''"/>
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

							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<FileFooter>
									<xsl:value-of select="'false'"/>
								</FileFooter>

								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>

								<xsl:variable name="OldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="OldSettleDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<trddate>
									<xsl:value-of select="concat(substring-before($OldTradeDate,'/'),'/',substring-before(substring-after($OldTradeDate,'/'),'/'),'/',substring-after(substring-after($OldTradeDate,'/'),'/'))"/>
								</trddate>


								<setdate>
									<xsl:value-of select="concat(substring-before($OldSettleDate,'/'),'/',substring-before(substring-after($OldSettleDate,'/'),'/'),'/',substring-after(substring-after($OldSettleDate,'/'),'/'))"/>
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
										<xsl:when test="OldSide='Buy' ">
											<xsl:value-of select="'BUY'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell' ">
											<xsl:value-of select="'SEL'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short' ">
											<xsl:value-of select="'SHT'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'CVS'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Open'">
											<xsl:value-of select="'BUY'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell to Open'">
											<xsl:value-of select="'SHT'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell to Close'">
											<xsl:value-of select="'SEL'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</TRXTYPE>

								<qty>
									<xsl:choose>
										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="format-number(OldExecutedQuantity,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</qty>


								<PRICE>
									<xsl:value-of select="format-number(OldAvgPrice,'0.########')"/>
								</PRICE>

								<xsl:variable name="COMM">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>
								<Commission>
									<xsl:value-of select="$COMM"/>
								</Commission>

								<SettlementCCY>
									<xsl:value-of select="OldSettlCurrency"/>
								</SettlementCCY>


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
										<xsl:when test="number(OldSettlCurrFxRate)">
											<xsl:value-of select="OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="1"/>
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
									<xsl:value-of select="OldOrfFee"/>
								</Ofee>

								<SecFee>
									<xsl:value-of select="OldSecFee"/>
								</SecFee>

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
									<xsl:value-of select="'Delete'"/>
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
									<xsl:value-of select="''"/>
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

								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>

						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileFooter>
								<xsl:value-of select="'false'"/>
							</FileFooter>

							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<trddate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</trddate>

							<setdate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
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
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="format-number(OrderQty,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</qty>

							<PRICE>
								<xsl:value-of select="format-number(AvgPrice,'0.########')"/>
							</PRICE>

							<xsl:variable name="COMM">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
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
								<xsl:value-of select="SecFee"/>
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

								<xsl:value-of select ="'NEW'"/>

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
								<xsl:value-of select="''"/>
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

							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:otherwise>
				</xsl:choose>


			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>