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


				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>


				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<Account>
					<xsl:value-of select="'Account Name'"/>
				</Account>
				
				<TickerSymbol>
					<xsl:value-of select="'Ticker'"/>
				</TickerSymbol>

				<SecurityDesc>
					<xsl:value-of select="'Security Description Name'"/>
				</SecurityDesc>

				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>

				<Asset>
					<xsl:value-of select="'Asset'"/>
				</Asset>

				<BuySell>
					<xsl:value-of select="'Side'"/>
				</BuySell>

				<TradeQuantity>
					<xsl:value-of select="'Trade Quantity'"/>
				</TradeQuantity>

				<!--<AllocatedQuantity>
					<xsl:value-of select="'Allocated Quantity'"/>
				</AllocatedQuantity>-->

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<!--<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>-->

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

				<!--<AccruedInterest>
					<xsl:value-of select="'Accrued Interest'"/>
				</AccruedInterest>-->
				
				<CusipSymbol>
					<xsl:value-of select="'Cusip Symbol'"/>
				</CusipSymbol>

				<NetAmountLocal>
					<xsl:value-of select="'Net Amount (Local)'"/>
				</NetAmountLocal>

				<FXRate>
					<xsl:value-of select="'FX Rate'"/>
				</FXRate>

				<NetAmountBase>
					<xsl:value-of select="'Net Amount (Base)'"/>
				</NetAmountBase>

				<!--<CmsnPerShareRate>
					<xsl:value-of select="'Commission in cents Per share'"/>
				</CmsnPerShareRate>-->
							
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>				

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='UBS: LP-6500012/690012' or AccountName='Context BH Financial Opportunities']">

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

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>
					
					<!--<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>-->

					<BuySell>
						<xsl:choose>
							<xsl:when test ="Side='Sell short'">
								<xsl:value-of select ="'Sell short'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell'">
								<xsl:value-of select ="'Sell'"/>
							</xsl:when>
							<xsl:when test ="Side='Buy'">
								<xsl:value-of select ="'Buy'"/>
							</xsl:when>
							<xsl:when test ="Side='Buy to Close'">
								<xsl:value-of select ="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell to Close'">
								<xsl:value-of select ="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test ="Side='Buy to Open'">
								<xsl:value-of select ="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell to Open'">
								<xsl:value-of select ="'Sell to Open'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(Side,1,1)"/>
							</xsl:otherwise>
						</xsl:choose>
					</BuySell>

					<TickerSymbol>
						<xsl:value-of select="Symbol"/>
					</TickerSymbol>

					<CusipSymbol>
						<xsl:value-of select="CUSIP"/>
					</CusipSymbol>

					<SecurityDesc>
						<xsl:choose>
							<xsl:when test="contains(FullSecurityName,',')">
								<xsl:value-of select="translate(FullSecurityName,',','')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="FullSecurityName"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityDesc>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<Asset>
						<xsl:value-of select="Asset"/>
					</Asset>

					<TradeQuantity>
						<xsl:value-of select="ExecutedQty"/>
					</TradeQuantity>

					<!--<AllocatedQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</AllocatedQuantity>-->

					<Account>
						<xsl:value-of select="AccountName"/>
					</Account>

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
					
					<!--<CmsnPerShareRate>
						<xsl:value-of select="number(CommissionCharged div ExecutedQty)"/>
					</CmsnPerShareRate>-->
					
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
							<xsl:when test="number(SecFee)">
								<xsl:value-of select="SecFee"/>
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


					<!--<AccruedInterest>
						<xsl:value-of select="number(AccruedInterest)"/>
					</AccruedInterest>-->
					
					<NetAmountLocal>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetAmountLocal>

					<FXRate>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</FXRate>

					<NetAmountBase>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="number(NetAmount*FXRate_Taxlot)"/>
									</xsl:when>
									<xsl:when test="FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="number(NetAmount div FXRate_Taxlot)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="number(NetAmount)"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:choose>
									<xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
										<xsl:value-of select="number(NetAmount * ForexRate)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="number(NetAmount div ForexRate)"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="number(NetAmount)"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetAmountBase>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
					

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>