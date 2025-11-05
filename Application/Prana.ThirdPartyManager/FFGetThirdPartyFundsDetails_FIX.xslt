<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<AvgPrice>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="AvgPrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AvgPrice>

					<Currency>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Currency>

					<Shares>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="CumQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Shares>

					<OrderSideTagValue>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:choose>
									<xsl:when test ="Side = 'Buy' or Side = 'Buy to Open'">
										<xsl:value-of  select="'A'"/>
									</xsl:when>
									<xsl:when test ="Side = 'Buy to Cover' or Side = 'Buy to Close'">
										<xsl:value-of  select="'B'"/>
									</xsl:when>
									<xsl:when test ="Side = 'Sell' or Side = 'Sell to Close'">
										<xsl:value-of  select="'2'"/>
									</xsl:when>
									<xsl:when test ="Side = 'Sell to Open'">
										<xsl:value-of  select="'C'"/>
									</xsl:when>
									<xsl:when test ="Side = 'Sell short'">
										<xsl:value-of  select="'5'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</OrderSideTagValue>

					<Symbol>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<TransactionTime>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="TradeDateTime"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TransactionTime>

					<xsl:variable name="varSettlDate">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="SettlementDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varSettlementDate">
						<xsl:value-of select="concat(substring-after(substring-after($varSettlDate,'/'),'/'),substring-before($varSettlDate,'/'),substring-before(substring-after($varSettlDate,'/'),'/'))"/>
					</xsl:variable>

					<FutSettDate>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="$varSettlementDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</FutSettDate>

					<AllocID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Amended'">										
								<xsl:value-of select="concat(GroupID,'R')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="GroupID"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocID>

					<xsl:variable name="varTransactionStateTx">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="0"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="1"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="2"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<AllocTransType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="$varTransactionStateTx"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocTransType>

					<xsl:variable name="varTradeDateFormat">
						<xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="TradeDate">
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varTradeDate">
						<xsl:value-of select="concat(substring-after(substring-after($varTradeDateFormat,'/'),'/'),substring-before($varTradeDateFormat,'/'),substring-before(substring-after($varTradeDateFormat,'/'),'/'))"/>
					</xsl:variable>

					<TradeDate>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="$varTradeDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeDate>

					<SecurityDesc>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="CompanyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityDesc>


					<xsl:variable name="varTotalCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="varOtherFees">
						<xsl:value-of select="(OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
					</xsl:variable>

					<xsl:variable name="varTotalCommissionAndFee">
						<xsl:value-of select ="($varTotalCommission + $varOtherFees)"/>
					</xsl:variable>

					<xsl:variable name="varSideMulteplier">
						<xsl:choose>
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="-1"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varGroupNetamount">
						<xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) + ($varTotalCommissionAndFee * $varSideMulteplier)"/>
					</xsl:variable>

					<NetMoney>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="$varGroupNetamount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetMoney>

					<SecurityType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="Asset"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<xsl:variable name="varGrossTradeAmt">
						<xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier)"/>
					</xsl:variable>

					<GrossTradeAmt>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="$varGrossTradeAmt"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</GrossTradeAmt>

					<PriceType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PriceType>
					
					<xsl:variable name="varPutOrCall">
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="PutOrCall='PUT'">
										<xsl:value-of select="0"/>
									</xsl:when>
									<xsl:when test="PutOrCall='CALL'">
										<xsl:value-of select="1"/>
									</xsl:when>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PutOrCall>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="$varPutOrCall"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PutOrCall>

					<ContractMultiplier>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="AssetMultiplier"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ContractMultiplier>

					<QtyType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</QtyType>

					<NoPartyIDs>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NoPartyIDs>
					
					<AllocNoOrdersType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocNoOrdersType>

					<NoOrders>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NoOrders>

					<ClOrderID>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="ClOrderID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ClOrderID>

					<OrderID>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="OrderID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</OrderID>

					<CusipSymbol>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CusipSymbol>

					<ISINSymbol>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ISINSymbol>

					<SEDOLSymbol>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SEDOLSymbol>

					<!-- RefAllocID-->
					<RefAllocID>
						<xsl:choose>
							<xsl:when test="TaxLotState='Amended' and GroupOrTaxlotType='Group'" >										
								<xsl:value-of select="GroupID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</RefAllocID>

					<AllocCancReplaceReason>
						<xsl:choose>
							<xsl:when test="TaxLotState='Amended' and GroupOrTaxlotType='Group'" >										
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocCancReplaceReason>

					<AllocType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocType>

					<AllocAccount>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountName"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocAccount>

					<AllocShares>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="TaxLotQty"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocShares>

					<IndividualAllocID>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Level1AllocationID"/>
							</xsl:otherwise>
						</xsl:choose>
					</IndividualAllocID>

					<ProcessCode>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ProcessCode>

					<NoNestedPartyIDs>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NoNestedPartyIDs>

					<Commission>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="$varTotalCommission"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

					<CommType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="3"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommType>

					<CommCurrency>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommCurrency>

					<AllocAvgPx>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Group'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AvgPrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocAvgPx>

					<xsl:variable name="varAllocNetamount">
						<xsl:value-of select="(TaxLotQty * AvgPrice * AssetMultiplier) + ($varTotalCommissionAndFee * $varSideMulteplier)"/>
					</xsl:variable>

					<AllocNetMoney>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="$varAllocNetamount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AllocNetMoney>

					<NoMiscFees>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NoMiscFees>

					<MiscFeeAmt>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="$varOtherFees"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</MiscFeeAmt>

					<MiscFeeType>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="7"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</MiscFeeType>

					<MiscFeeBasis>
						<xsl:choose>
							<xsl:when test="GroupOrTaxlotType='Taxlot'">
								<xsl:value-of select="0"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</MiscFeeBasis>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
