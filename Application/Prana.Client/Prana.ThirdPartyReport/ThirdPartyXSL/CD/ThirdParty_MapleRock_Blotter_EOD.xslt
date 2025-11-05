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

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<BuySell>
					<xsl:value-of select="'B/S'"/>
				</BuySell>

				<TickerSymbol>
					<xsl:value-of select="'Ticker Symbol'"/>
				</TickerSymbol>

				<CusipSymbol>
					<xsl:value-of select="'Cusip Symbol'"/>
				</CusipSymbol>

				<SecurityDesc>
					<xsl:value-of select="'Security Description Name'"/>
				</SecurityDesc>

				<TradeQuantity>
					<xsl:value-of select="'Trade Quantity'"/>
				</TradeQuantity>

				<AllocatedQuantity>
					<xsl:value-of select="'Allocated Quantity'"/>
				</AllocatedQuantity>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<SECFees>
					<xsl:value-of select="'SEC Fees'"/>
				</SECFees>

				<ORFFees>
					<xsl:value-of select="'ORF Fees'"/>
				</ORFFees>

				<OCCFees>
					<xsl:value-of select="'OCC Fees'"/>
				</OCCFees>

				<AllOtherFees>
					<xsl:value-of select="'All Other Fees'"/>
				</AllOtherFees>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<Account>
					<xsl:value-of select="'Allocations'"/>
				</Account>

				<Time>
					<xsl:value-of select="'Time'"/>
				</Time>

				<Confirm>
					<xsl:value-of select="'Confirm'"/>
				</Confirm>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
		
					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<!--for system internal use-->
						<BuySell>
							<xsl:value-of select="substring(Side,1,1)"/>
						</BuySell>

						<TickerSymbol>
							<xsl:value-of select="Symbol"/>
						</TickerSymbol>

						<CusipSymbol>
							<xsl:value-of select="CUSIP"/>
						</CusipSymbol>

						<SecurityDesc>
							<xsl:value-of select="FullSecurityName"/>
						</SecurityDesc>

						<TradeQuantity>
							<xsl:value-of select="ExecutedQty"/>
						</TradeQuantity>

						<AllocatedQuantity>
							<xsl:value-of select="AllocatedQty"/>
						</AllocatedQuantity>

						<Price>
							<xsl:choose>
								<xsl:when test="number(AveragePrice)">
									<xsl:value-of select="AveragePrice"/>		
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</Price>

						<Commission>
							<xsl:choose>
								<xsl:when test="number(CommissionCharged)">
									<xsl:value-of select="CommissionCharged"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<SECFees>
							<xsl:choose>
								<xsl:when test="number(StampDuty)">
									<xsl:value-of select="StampDuty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SECFees>

						<ORFFees>
							<xsl:choose>
								<xsl:when test="number(OrfFee)">
									<xsl:value-of select="OrfFee"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ORFFees>

						<OCCFees>
							<xsl:choose>
								<xsl:when test="number(OccFee)">
									<xsl:value-of select="OccFee"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OCCFees>

						<AllOtherFees>
							<xsl:choose>
								<xsl:when test="number(TransactionLevy+ClearingBrokerFee+OtherBrokerFee+ClearingFee+TaxOnCommissions+MiscFees)">
									<xsl:value-of select="TransactionLevy+ClearingBrokerFee+OtherBrokerFee+ClearingFee+TaxOnCommissions+MiscFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							</AllOtherFees>

						<Broker>
							<xsl:value-of select="CounterParty"/>
						</Broker>

						<Account>
							<xsl:value-of select="AccountName"/>
						</Account>

						<Time>
							<xsl:value-of select="substring-after(TradeDateTime,' ')"/>
						</Time>

						<Confirm>
							<xsl:value-of select="''"/>
						</Confirm>

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
			
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>