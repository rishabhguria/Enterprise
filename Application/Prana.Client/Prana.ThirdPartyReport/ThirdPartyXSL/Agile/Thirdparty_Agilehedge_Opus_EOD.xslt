<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<trddate>
					<xsl:value-of select ="'trddate'"/>
				</trddate>

				<setdate>
					<xsl:value-of select ="'setdate'"/>
				</setdate>

				<SYCODE>
					<xsl:value-of select ="'SYCODE'"/>
				</SYCODE>

				<TRXTYPE>
					<xsl:value-of select ="'TRXTYPE'"/>
				</TRXTYPE>

				<qty>
					<xsl:value-of select ="'qty'"/>
				</qty>

				<PRICE>
					<xsl:value-of select ="'PRICE'"/>
				</PRICE>

				<Commission>
					<xsl:value-of select ="'Commission'"/>
				</Commission>

				<SettlementCCY>
					<xsl:value-of select ="'Settlement CCY'"/>
				</SettlementCCY>

				<ContraParty>
					<xsl:value-of select ="'Contra Party'"/>
				</ContraParty>

				<Exchange>
					<xsl:value-of select ="'Exchange'"/>
				</Exchange>

				<CLRBRKRACCT>
					<xsl:value-of select ="'CLRBRKRACCT'"/>
				</CLRBRKRACCT>

				<SettleFXRate>
					<xsl:value-of select ="'Settle FX Rate'"/>
				</SettleFXRate>

				<evreference>
					<xsl:value-of select ="'evreference'"/>
				</evreference>

				<CBFee>
					<xsl:value-of select ="'CB Fee'"/>	
				</CBFee>

				<ExFee>
					<xsl:value-of select ="'Ex Fee'"/>
				</ExFee>

				<Interest>
					<xsl:value-of select ="'Interest'"/>
				</Interest>

				<Ofee>
					<xsl:value-of select ="'Ofee'"/>
				</Ofee>

				<SecFee>
					<xsl:value-of select ="'Sec Fee'"/>
				</SecFee>

				<NetProceeds>
					<xsl:value-of select ="'Net Proceeds'"/>
				</NetProceeds>

				<PositionCCY>
					<xsl:value-of select ="'Position CCY'"/>
				</PositionCCY>

				<PosFXRate>
					<xsl:value-of select ="'Pos FX Rate'"/>
				</PosFXRate>

				<Blank>
					<xsl:value-of select ="'Blank'"/>
				</Blank>

				<strategy>
					<xsl:value-of select ="'strategy'"/>
				</strategy>

				<FNID>
					<xsl:value-of select ="'FNID'"/>
				</FNID>

				<CPS>
					<xsl:value-of select ="'CPS'"/>
				</CPS>

				<bips>
					<xsl:value-of select ="'bips'"/>
				</bips>

				<Status>
					<xsl:value-of select ="'Status'"/>
				</Status>

				<bareference>
					<xsl:value-of select ="'ba reference'"/>
				</bareference>

				<OwnerTrader>
					<xsl:value-of select ="'Owner/Trader'"/>
				</OwnerTrader>

				<SoftCommPct>
					<xsl:value-of select ="'Soft Comm Pct'"/>
				</SoftCommPct>

				<DealDescription>
					<xsl:value-of select ="'Deal Description'"/>
				</DealDescription>

				<DealRate>
					<xsl:value-of select ="'Deal Rate'"/>
				</DealRate>

				<Giveupbrokercode>
					<xsl:value-of select ="'Give-up broke rcode'"/>
				</Giveupbrokercode>

				<Giveupcmmsntypecode>
					<xsl:value-of select ="'Give-up cmmsn type code'"/>
				</Giveupcmmsntypecode>

				<GiveUpCommRate>
					<xsl:value-of select ="'GiveUp Comm Rate'"/>
				</GiveUpCommRate>

				<GiveUpCommAmt>
					<xsl:value-of select ="'GiveUp Comm Amt'"/>
				</GiveUpCommAmt>

				<GovtFees>
					<xsl:value-of select ="'Govt Fees'"/>
				</GovtFees>

				<Remarks>
					<xsl:value-of select ="'Remarks'"/>
				</Remarks>

				<EVType>
					<xsl:value-of select ="'EV Type'"/>
				</EVType>

				<TermDate>
					<xsl:value-of select ="'Term Date'"/>
				</TermDate>

				<ExcludeOtherFeesfromNetProceeds>
					<xsl:value-of select ="'Exclude Other Fees from Net Proceeds'"/>
				</ExcludeOtherFeesfromNetProceeds>

				<ExcludeOtherFeesfromBrCash>
					<xsl:value-of select ="'Exclude Other Fees from BrCash'"/>
				</ExcludeOtherFeesfromBrCash>

				<ExcludeCommissionfromProceeds>
					<xsl:value-of select ="'Exclude Commission from Proceeds'"/>
				</ExcludeCommissionfromProceeds>

				<GiveUpBrokerCommPostingRule>
					<xsl:value-of select ="'Give-Up Broker Comm Posting Rule'"/>
				</GiveUpBrokerCommPostingRule>

				<CommTypeCode>
					<xsl:value-of select ="'Comm Type Code'"/>
				</CommTypeCode>

				<Route>
					<xsl:value-of select ="'Route'"/>
				</Route>

				<UploadStatus>
					<xsl:value-of select ="'Upload Status'"/>
				</UploadStatus>

				<PairOffMethod>
					<xsl:value-of select ="'Pair Off Method'"/>
				</PairOffMethod>

				<UDCNamesValues>
					<xsl:value-of select ="'UDC Names Values'"/>
				</UDCNamesValues>

				<TargetSettlement>
					<xsl:value-of select ="'Target Settlement'"/>
				</TargetSettlement>

				<ContractType>
					<xsl:value-of select ="'Contract Type'"/>
				</ContractType>

				<!-- system inetrnal use-->
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>


			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Columbus LP' or AccountName='Columbus QP']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<trddate>
						<xsl:value-of select ="TradeDate"/>
					</trddate>

					<setdate>
						<xsl:value-of select ="TradeDate"/>
					</setdate>

					<xsl:variable name="varSymbol">
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="normalize-space(OSIOptionSymbol)"/>
							</xsl:when>
							<xsl:when test="SEDOL != '' and SEDOL != '*'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP != '' and CUSIP != '*'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="ISIN != '' and ISIN != '*'">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="concat(Symbol,' ',CurrencySymbol)"/>
							</xsl:when>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SYCODE>
						<xsl:value-of select ="$varSymbol"/>
					</SYCODE>

					<TRXTYPE>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="Side='Buy to Open' or Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell to Open'">
										<xsl:value-of select="'SHT'"/>
									</xsl:when>


									<xsl:when test="Side='Sell to Close' or Side='Sell'">
										<xsl:value-of select="'SEL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'CVS'"/>
									</xsl:when>

								</xsl:choose>
							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'SEL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'CVS'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'SHT'"/>
									</xsl:when>
								</xsl:choose>


							</xsl:otherwise>
						</xsl:choose>
					</TRXTYPE>

					<qty>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select ="format-number(AllocatedQty,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</qty>

					<xsl:variable name="SideMultiplier">
						<xsl:choose>
							<xsl:when test="SideID = '1'  or SideID = 'A' or SideID = 'B' or SideID = '3' ">
								<xsl:value-of select ="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="-1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					

					<xsl:variable name="Commission" select="(CommissionCharged + SoftCommissionCharged)"/>

					<xsl:variable name="OtherFees" select="(OtherBrokerFees + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions+ MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee)"/>

					<xsl:variable name="CalcNetAmt">
						<xsl:value-of select="(AllocatedQty * AvgPrice * AssetMultiplier) + (($Commission + $OtherFees) * $SideMultiplier)"/>
					</xsl:variable>

					<xsl:variable name="NetPrice">
						<xsl:value-of select="$CalcNetAmt div (AllocatedQty * AssetMultiplier)"/>
					</xsl:variable>
					
					<!--<xsl:variable name="NetPrice">
						<xsl:value-of select="NetAmount div (AllocatedQty * AssetMultiplier)"/>
					</xsl:variable>-->

					<PRICE>
						<xsl:choose>
							<xsl:when test="number($NetPrice)">
								<xsl:value-of select ="format-number($NetPrice,'0.########')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PRICE>
					
					
					<Commission>
						<xsl:value-of select ="''"/>
					</Commission>

					<SettlementCCY>
						<xsl:value-of select ="SettlCurrency"/>
					</SettlementCCY>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<!--<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='MS']/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@MLPBroker"/>
					</xsl:variable>-->

					<xsl:variable name="varBROKER">
						<xsl:choose>
							<xsl:when test="$PRANA_BROKER_NAME != ''">
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<ContraParty>
						<xsl:value-of select ="''"/>
					</ContraParty>

					<Exchange>
						<xsl:value-of select ="''"/>
					</Exchange>

					<CLRBRKRACCT>
						<!--<xsl:value-of select ="concat('CLIENT',' ','-',' ',$varBROKER)"/>-->
						<xsl:choose>
							<xsl:when test="AccountName='Columbus LP'">
								<xsl:value-of select="'UBS - CLP'"/>
							</xsl:when>
							<xsl:when test="AccountName='Columbus QP'">
								<xsl:value-of select="'UBS - CQP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountName"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</CLRBRKRACCT>

					<SettleFXRate>
						<!--<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							--><!--<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>--><!--
							--><!--<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>--><!--
						</xsl:choose>-->
						<xsl:value-of select="FXRate_Taxlot"/>
					</SettleFXRate>

					<evreference>
						<!--<xsl:value-of select="concat(concat('=&quot;',EntityID),'&quot;')"/>-->
						<xsl:value-of select="concat('A',EntityID)"/>
					</evreference>

					<CBFee>
						<xsl:value-of select ="''"/>
					</CBFee>

					<ExFee>
						<xsl:value-of select ="''"/>
					</ExFee>

					<Interest>
						<xsl:value-of select ="''"/>
					</Interest>

					<Ofee>
						<xsl:value-of select ="''"/>
					</Ofee>

					<SecFee>
						<xsl:value-of select ="''"/>
					</SecFee>

					<NetProceeds>
						<xsl:value-of select ="''"/>
					</NetProceeds>

					<PositionCCY>
						<xsl:value-of select ="CurrencySymbol"/>
					</PositionCCY>

					<PosFXRate>
						<!--<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="FXRate_Taxlot"/>
					</PosFXRate>

					<Blank>
						<xsl:value-of select ="''"/>
					</Blank>

					<strategy>
						<xsl:value-of select ="'A-A'"/>
					</strategy>

					<FNID>
						<xsl:value-of select ="''"/>
					</FNID>

					<CPS>
						<xsl:value-of select ="''"/>
					</CPS>

					<bips>
						<xsl:value-of select ="''"/>
					</bips>

					<Status>
						<xsl:value-of select ="'NEW'"/>
					</Status>

					<bareference>
						<xsl:value-of select="concat('A',EntityID)"/>
					</bareference>

					<OwnerTrader>
						<xsl:value-of select ="''"/>
					</OwnerTrader>

					<SoftCommPct>
						<xsl:value-of select ="''"/>
					</SoftCommPct>

					<DealDescription>
						<xsl:value-of select ="''"/>
					</DealDescription>

					<DealRate>
						<xsl:value-of select ="'0'"/>
					</DealRate>

					<Giveupbrokercode>
						<xsl:value-of select ="''"/>
					</Giveupbrokercode>

					<Giveupcmmsntypecode>
						<xsl:value-of select ="''"/>
					</Giveupcmmsntypecode>

					<GiveUpCommRate>
						<xsl:value-of select ="'0'"/>
					</GiveUpCommRate>

					<GiveUpCommAmt>
						<xsl:value-of select ="'0'"/>
					</GiveUpCommAmt>

					<GovtFees>
						<xsl:value-of select ="'0'"/>
					</GovtFees>

					<Remarks>
						<xsl:value-of select="concat('A',EntityID)"/>
					</Remarks>

					<EVType>
						<xsl:value-of select ="'TRD'"/>
					</EVType>

					<TermDate>
						<xsl:value-of select ="''"/>
					</TermDate>

					<ExcludeOtherFeesfromNetProceeds>
						<xsl:value-of select ="''"/>
					</ExcludeOtherFeesfromNetProceeds>

					<ExcludeOtherFeesfromBrCash>
						<xsl:value-of select ="''"/>
					</ExcludeOtherFeesfromBrCash>

					<ExcludeCommissionfromProceeds>
						<xsl:value-of select ="''"/>
					</ExcludeCommissionfromProceeds>

					<GiveUpBrokerCommPostingRule>
						<xsl:value-of select ="''"/>
					</GiveUpBrokerCommPostingRule>

					<CommTypeCode>
						<xsl:value-of select ="'FLAT'"/>
					</CommTypeCode>

					<Route>
						<xsl:value-of select ="''"/>
					</Route>

					<UploadStatus>
						<xsl:value-of select ="''"/>
					</UploadStatus>

					<PairOffMethod>
						<xsl:value-of select ="''"/>
					</PairOffMethod>

					<UDCNamesValues>
						<xsl:value-of select ="''"/>
					</UDCNamesValues>

					<TargetSettlement>
						<xsl:value-of select ="''"/>
					</TargetSettlement>

					<ContractType>
						<xsl:value-of select ="''"/>
					</ContractType>

					<!-- system inetrnal use-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
