<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<Account>
					<xsl:value-of select="'Account'"/>
				</Account>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>
				</SettlementDate>

				<Transaction>
					<xsl:value-of select="'Transaction'"/>
				</Transaction>

				<InstrumentID>
					<xsl:value-of select="'InstrumentID'"/>
				</InstrumentID>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<AssetType>
					<xsl:value-of select="'AssetType'"/>
				</AssetType>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<Shares>
					<xsl:value-of select="'Shares'"/>
				</Shares>

				<AvgPrice>
					<xsl:value-of select="'AvgPrice'"/>
				</AvgPrice>

				<PriceCurrency>
					<xsl:value-of select="'PriceCurrency'"/>
				</PriceCurrency>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<CommissionType>
					<xsl:value-of select="'CommissionType'"/>
				</CommissionType>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<TotalCommission>
					<xsl:value-of select="'TotalCommission'"/>
				</TotalCommission>

				<NetAmount>
					<xsl:value-of select="'NetAmount'"/>
				</NetAmount>

				<OtherFeesAmount>
					<xsl:value-of select="'OtherFeesAmount'"/>
				</OtherFeesAmount>

				<SECFeeAmount>
					<xsl:value-of select="'SECFeeAmount'"/>
				</SECFeeAmount>

				<TradeId>
					<xsl:value-of select="'Trade Id'"/>
				</TradeId>

				<AllocationId>
					<xsl:value-of select="'Allocation Id'"/>
				</AllocationId>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:if test="FundName='Clover Street'">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="'Jadwin'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="FundName"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<Account>
						<xsl:choose>
							<xsl:when test ="$PB_FUND_CODE!=''">
								<xsl:value-of select="$PB_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Account>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>

					<Transaction>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'BY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'SL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Transaction>

					<InstrumentID>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</InstrumentID>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<AssetType>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'EQTY'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPTN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AssetType>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<Shares>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">

								<xsl:choose>
									<xsl:when test="AllocatedQty &gt; 0">
										<xsl:value-of select="AllocatedQty"/>
									</xsl:when>
									<xsl:when test="AllocatedQty &lt; 0">
										<xsl:value-of select="AllocatedQty * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Shares>

					<AvgPrice>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</AvgPrice>

					<PriceCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</PriceCurrency>

					<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>

					<CommissionType>
						<xsl:value-of select="'R'"/>
					</CommissionType>

					<!--<Commission>
						<xsl:choose>
							<xsl:when test="number(CommissionCharged)">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>						
					</Commission>

					<xsl:variable name="TotComm">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<TotalCommission>
						<xsl:choose>
							<xsl:when test="number($TotComm)">
								<xsl:value-of select="$TotComm"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</TotalCommission>

					<NetAmount>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>						
					</NetAmount>

					<xsl:variable name="OtherFee" select="OtherBrokerFee + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee"/>

					<OtherFeesAmount>
						<xsl:choose>
							<xsl:when test="number($OtherFee)">
								<xsl:value-of select="$OtherFee"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</OtherFeesAmount>-->


					<Commission>
						<xsl:choose>
							<xsl:when test ="CommissionCharged &gt; 0">
								<xsl:value-of select="format-number(((CommissionCharged) div (AllocatedQty*AssetMultiplier)),'0.##')"/>
							</xsl:when>
							<xsl:when test="CommissionCharged &lt; 0">
								<xsl:value-of select="format-number(((CommissionCharged*-1) div (AllocatedQty*AssetMultiplier)),'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

					<TotalCommission>
						<!--<xsl:choose>
								<xsl:when test ="$TotalCommission &gt; 0">
									<xsl:value-of select="$TotalCommission"/>
								</xsl:when>
								<xsl:when test="$TotalCommission &lt; 0">
									<xsl:value-of select="$TotalCommission*-1"/>
								</xsl:when>
								<xsl:otherwise>-->
						<xsl:value-of select="format-number(CommissionCharged,'0.##')"/>
						<!--</xsl:otherwise>
							</xsl:choose>-->
					</TotalCommission>

					<xsl:variable name ="SideMultiplier">
						<xsl:choose>
							<xsl:when test="SideTag='1' or SideTag='B' or SideTag='A' or SideTag='3' or SideTag='8' or SideTag='E' or SideTag='9'">
								<xsl:value-of select ="-1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<xsl:variable name ="varNetAmount">
						<xsl:choose>
							<xsl:when test="(Asset='EquityOption' or Asset='Equity') and CounterParty='JEFF'">
								<xsl:value-of select="format-number(NetAmount+(OccFee+SoftCommissionCharged)*$SideMultiplier,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(NetAmount - ((10-SoftCommissionCharged)*$SideMultiplier),'0.##')"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<NetAmount>
						<xsl:value-of select="$varNetAmount"/>
					</NetAmount>

					<OtherFeesAmount>
						<xsl:choose>
							<xsl:when test="(Asset='EquityOption' or Asset='Equity') and CounterParty='JEFF'">
								<xsl:value-of select="format-number(TransactionLevy+ClearingBrokerFee+OtherBrokerFee+ClearingFee+TaxOnCommissions+MiscFees+OrfFee+StampDuty,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(TransactionLevy+ClearingBrokerFee+OtherBrokerFee+ClearingFee+TaxOnCommissions+MiscFees+OccFee+OrfFee+StampDuty+10,'0.##')"/>
							</xsl:otherwise>
						</xsl:choose>
					</OtherFeesAmount>
					
					<SECFeeAmount>
						<xsl:choose>
							<xsl:when test="number(StampDuty)">
								<xsl:value-of select="StampDuty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SECFeeAmount>

					<TradeId>
						<xsl:value-of select="TradeRefID"/>
					</TradeId>

					<AllocationId>
						<xsl:value-of select="EntityID"/>
					</AllocationId>					

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
				</xsl:if>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
