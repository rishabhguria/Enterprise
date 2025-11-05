<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxlotState>
					<xsl:value-of select="'TaxlotState'"/>
				</TaxlotState>

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

				<Ticker>
					<xsl:value-of select="'Ticker'"/>
				</Ticker>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>

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

				<SettlementCurrency>
					<xsl:value-of select="'SettlementCurrency'"/>
				</SettlementCurrency>

				<SettlementExchangeRate>
					<xsl:value-of select="'SettlementExchangeRate'"/>
				</SettlementExchangeRate>

				<TradeId>
					<xsl:value-of select="'TradeId'"/>
				</TradeId>

				<AllocationId>
					<xsl:value-of select="'AllocationId'"/>
				</AllocationId>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:if test="Asset='Equity' and (Side='Buy' or Side='Sell') and FundName='Clover SMA: Hatteras'">

					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<!--for system internal use-->

						<TaxlotState>
							<xsl:value-of select="TaxlotState"/>
						</TaxlotState>

						<Account>
							<xsl:value-of select="'Acct #19-3543'"/>
						</Account>

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="SettlementDate"/>
						</SettlementDate>

						<Transaction>
							<xsl:choose>
								<xsl:when test="Side='Buy'">
									<xsl:value-of select="'BY'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'SL'"/>
								</xsl:otherwise>
							</xsl:choose>
						</Transaction>

						<InstrumentID>
							<xsl:value-of select="Symbol"/>
						</InstrumentID>

						<Ticker>
							<xsl:value-of select="Symbol"/>
						</Ticker>

						<CUSIP>
							<xsl:value-of select="CUSIP"/>
						</CUSIP>

						<SEDOL>
							<xsl:value-of select="SEDOL"/>
						</SEDOL>

						<ISIN>
							<xsl:value-of select="ISIN"/>
						</ISIN>

						<AssetType>
							<xsl:value-of select="'EQTY'"/>
						</AssetType>

						<SecurityDescription>
							<xsl:value-of select="FullSecurityName"/>
						</SecurityDescription>

						<Shares>
							<xsl:value-of select="AllocatedQty"/>
						</Shares>

						<AvgPrice>
							<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
						</AvgPrice>

						<PriceCurrency>
							<xsl:value-of select="CurrencySymbol"/>
						</PriceCurrency>

						<xsl:variable name="PB_NAME" select="'Hatteras'"/>

						<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

						<xsl:variable name="PB_COUNTERPARTY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<Broker>
							<xsl:choose>
								<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
									<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Broker>

						<CommissionType>
							<xsl:value-of select="'R'"/>
						</CommissionType>

						<xsl:variable name="TotalCommission">
							<xsl:choose>
								<xsl:when test ="number(number(NetAmount)-number(GrossAmount))">
									<xsl:value-of select="format-number(number(NetAmount)-number(GrossAmount),'0.##')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ="$TotalCommission &gt; 0">
									<xsl:value-of select="format-number(($TotalCommission div (AllocatedQty*AssetMultiplier)),'0.##')"/>
								</xsl:when>
								<xsl:when test="$TotalCommission &lt; 0">
									<xsl:value-of select="format-number((($TotalCommission*-1) div (AllocatedQty*AssetMultiplier)),'0.##')"/>
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
							<xsl:value-of select="NetAmount+SoftCommissionCharged*$SideMultiplier"/>
						</xsl:variable>

						<NetAmount>
							<xsl:value-of select="format-number($varNetAmount,'0.##')"/>
						</NetAmount>

						<OtherFeesAmount>
							<xsl:value-of select="format-number(TransactionLevy+ClearingBrokerFee+OtherBrokerFee+ClearingFee+TaxOnCommissions+MiscFees+OccFee+OrfFee+StampDuty,'0.##')"/>
						</OtherFeesAmount>

						<SECFeeAmount>
							<xsl:value-of select="format-number(StampDuty,'0.##')"/>
						</SECFeeAmount>

						<SettlementCurrency>
							<xsl:value-of select="CurrencySymbol"/>
						</SettlementCurrency>

						<SettlementExchangeRate>
							<xsl:value-of select="'1'"/>
						</SettlementExchangeRate>

						<TradeId>
							<xsl:value-of select="TradeRefID"/>
						</TradeId>

						<AllocationId>
							<xsl:value-of select="EntityID"/>
						</AllocationId>

						<Comments>
							<xsl:value-of select="''"/>
						</Comments>

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>

				</xsl:if>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>